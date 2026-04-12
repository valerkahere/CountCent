using CountCent.Model;
using CountCent.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CountCent
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _LocalDbService;
        static List<DataPoint> dataPoints = new List<DataPoint>();
        
        // Track current day view
        private DateTime _selectedDate = DateTime.Today;

        public MainPage(LocalDbService localDbService)
        {
            InitializeComponent();
            _LocalDbService = localDbService;

            // Load all. Filter to thread.
            Task.Run(async () => {
                var items = await _LocalDbService.GetDataPoints();
                MainThread.BeginInvokeOnMainThread(() => {
                    dataPoints = items;
                    UpdateDateLabel();
                    UpdateItemsSource();
                });
            });
        }

        private async void ent__main_Completed(object sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                string amount = entry.Text ?? string.Empty;

                if (!decimal.TryParse(amount, out decimal amountConverted))
                {
                    lbl_errorMsg.Text = "Invalid amount. Numbers only.";
                    lbl_errorMsg.IsVisible = true;
                    return;
                }

                lbl_errorMsg.IsVisible = false;

                // Bind new point to currently selected date + time now
                DateTime entryDate = _selectedDate.Date + DateTime.Now.TimeOfDay;
                DataPoint dataPoint = new DataPoint(amountConverted, entryDate);

                dataPoints.Add(dataPoint);
                await _LocalDbService.Create(dataPoint);
                
                // Clear input
                entry.Text = string.Empty;

                UpdateItemsSource();
            }
        }

        private void btn__PrevDay_Clicked(object sender, EventArgs e)
        {
            _selectedDate = _selectedDate.AddDays(-1);
            UpdateDateLabel();
            UpdateItemsSource();
        }

        private void btn__NextDay_Clicked(object sender, EventArgs e)
        {
            _selectedDate = _selectedDate.AddDays(1);
            UpdateDateLabel();
            UpdateItemsSource();
        }

        private void UpdateDateLabel()
        {
            lbl_day.Text = _selectedDate.Date == DateTime.Today.Date 
                ? "Today" 
                : _selectedDate.ToString("ddd, MMM dd, yyyy");
        }

        private void UpdateItemsSource()
        {
            clc__mainScreen.ItemsSource = null;
            
            // Filter global list for selected day only
            var dailyItems = dataPoints
                .Where(dp => dp.Date.Date == _selectedDate.Date)
                .OrderByDescending(dp => dp.Date)
                .ToList();

            clc__mainScreen.ItemsSource = dailyItems;

            // Calculate exact total for this day
            decimal dailyTotal = dailyItems.Sum(x => x.Amount);
            lbl_dailyTotal.Text = $"Daily Total: {dailyTotal:C}";
        }

        private void btn__ExportToFile_Clicked(object sender, EventArgs e)
        {
            string dateString = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
            string filePath = Path.Combine(FileSystem.AppDataDirectory, $"CountCent Export {dateString}.csv");
            WriteToCsv(filePath);
        }

        // Helper methods

        static void WriteToCsv(string filePath)
        {
            try
            {
                // in csv format
                // Using CsvHelper for:
                // Safety: Automatically escapes characters (e.g., if a user’s name is Doe, John, it wraps it in quotes so it doesn't break your columns).
                // Mapping: It can map your C# classes directly to CSV headers with one line of code.
                // Performance: It uses streams to handle millions of rows without crashing your app's memory.

                // not culture dependent
                // HasHeaderRecord - need a record in a file or not
                var configWrite = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true
                };

                // StreamWriter will automatically create the file if it doesn't exist
                using var writer = new StreamWriter(filePath);

                // Use a lowerCamelCase variable name to avoid conflicting with the CsvWriter class type
                using var csvWriter = new CsvWriter(writer, configWrite);
                csvWriter.WriteRecords(dataPoints);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
            }
        }


    }
}
