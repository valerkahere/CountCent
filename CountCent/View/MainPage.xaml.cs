using CountCent.Model;
using CountCent.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace CountCent
{
    public partial class MainPage : ContentPage
    {
        // create db service
        private readonly LocalDbService _LocalDbService;

        private int _editDataPointId;

        // assign to constructor
        public MainPage(LocalDbService localDbService)
        {
            InitializeComponent();

            // instantiate it
            _LocalDbService = localDbService;

            Task.Run(async () => clc__mainScreen.ItemsSource = await _LocalDbService.GetDataPoints());
        }


        // This will store data throughout the program
        static List<DataPoint> dataPoints = new List<DataPoint>();

        // behaviour immediately prior to application becoming visible
        //protected override void OnAppearing()
        //{
        //    // Get today's date, converts to date, to string (short date), and display it
        //    lbl_day.Text = DateTime.Today.ToString("d");

        //    // Populate with dummy (or existing) data
        //    Random random = new Random();
        //    for (int i = 0; i < 3; i++)
        //    {
        //        dataPoints.Add(
        //            new DataPoint(
        //                    random.Next(100, 100000),
        //                    DateTime.UtcNow
        //                )
        //            );
        //    }

        //    UpdateItemsSource();
        //}


        private void ent__main_Completed(object sender, EventArgs e)
        {

            // To ensure you get the current value, cast the sender object to an Entry within the handler. This is more reliable than using the name ent__main directly: 

            if (sender is Entry entry)
            {
                // returns left if not null, otherwise returns right
                string amount = entry.Text ?? string.Empty;
                decimal amountConverted = Convert.ToDecimal(amount);

                dataPoints.Add(
                    new DataPoint(amountConverted)
                    );

                UpdateItemsSource();
            }
        }

        private void UpdateItemsSource()
        {
            clc__mainScreen.ItemsSource = null;
            clc__mainScreen.ItemsSource = dataPoints;
        }

        private void btn__ExportToFile_Clicked(object sender, EventArgs e)
        {
            // Use "yyyy-MM-dd" for easy sorting in file explorers
            string dateString = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");

            string filePath = Path.Combine($"{FileSystem.AppDataDirectory}", $"CountCent Export {dateString}.csv");
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
