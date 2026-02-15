using CountCent.Model;

namespace CountCent
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        // This will store data throughout the program
        List<DataPoint> dataPoints = new List<DataPoint>();

        // behaviour immediately prior to application becoming visible
        protected override void OnAppearing()
        {
            // Get today's date, converts to date, to string (short date), and display it
            lbl_day.Text = DateTime.Today.ToString("d");

            // Populate with dummy (or existing) data
            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                dataPoints.Add(
                    new DataPoint(
                            random.Next(100, 100000),
                            DateTime.Now
                        )
                    );
            }

            clc__mainScreen.ItemsSource = dataPoints;
        }

    }
}
