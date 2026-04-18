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
        private readonly CurrencyService _CurrencyService;
        static List<DataPoint> dataPoints = new List<DataPoint>();
        
        // Track current day view
        private DateTime _selectedDate = DateTime.Today;

        public MainPage(LocalDbService localDbService, CurrencyService currencyService)
        {
            InitializeComponent();
            _LocalDbService = localDbService;
            _CurrencyService = currencyService;

            // Load db data
            Task.Run(async () =>
            {
                var items = await _LocalDbService.GetDataPoints();
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    dataPoints = items;
                    UpdateDateLabel();
                    UpdateItemsSource();
                });
            });

            // Load API rates
            LoadExchangeRates();
        }

        private async void LoadExchangeRates()
        {
            // Base API assumes EUR. Fetch 1 EUR equiv.
            decimal usd = await _CurrencyService.ConvertAmountAsync(1m, "USD");
            decimal gbp = await _CurrencyService.ConvertAmountAsync(1m, "GBP");
            decimal jpy = await _CurrencyService.ConvertAmountAsync(1m, "JPY");
            decimal chf = await _CurrencyService.ConvertAmountAsync(1m, "CHF");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                lbl_usd.Text = $"USD {usd:F2}";
                lbl_gbp.Text = $"GBP {gbp:F2}";
                lbl_jpy.Text = $"JPY {jpy:F2}";
                lbl_chf.Text = $"CHF {chf:F2}";
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
        // DELETE LOGIC 

        private async void btn__DeleteEntry_Clicked(object sender, EventArgs e)
        {
            if (clc__mainScreen.SelectedItem is DataPoint dp)
            {
                // Call wrapper instead
                await TryProcessDelete(dp);
            }
            else
            {
                // Notify the user if no item is selected
                await DisplayAlert("No Selection", "Please tap an entry to select it before deleting.", "OK");
            }
        }

        private async void SwipeItem_Delete_Invoked(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is DataPoint dp)
            {
                // Call wrapper instead
                await TryProcessDelete(dp);
            }
        }

        // New confirmation wrapper
        private async Task TryProcessDelete(DataPoint dp)
        {
            // Check saved preference
            bool skipConfirm = Preferences.Default.Get("SkipDeleteConfirm", false);
            
            if (!skipConfirm)
            {
                // 3 options. Cancel is native bottom button.
                string action = await DisplayActionSheet("Confirm Delete", "Cancel", null, "Delete", "Delete (Don't ask again)");
                
                // Abort if cancel or tap outside
                if (action == "Cancel" || string.IsNullOrEmpty(action)) 
                    return;
                
                // Save preference if chosen
                if (action == "Delete (Don't ask again)")
                {
                    Preferences.Default.Set("SkipDeleteConfirm", true);
                }
            }

            // Proceed to existing logic
            await ProcessDelete(dp);
        }

        private async Task ProcessDelete(DataPoint dp)
        {
            // Remove from DB
            await _LocalDbService.Delete(dp);
            
            // Remove from local list
            dataPoints.Remove(dp);
            
            // Clear selection. Prevent crash.
            clc__mainScreen.SelectedItem = null;
            
            // Refresh UI list and totals
            UpdateItemsSource();
        }
    }
}
