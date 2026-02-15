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

            UpdateItemsSource();
        }


        // Add DataPoint
        private void Button_Clicked(object sender, EventArgs e)
        {
            // Get Entry value
            // Check not null
            // Add to the list
            // Update the items source with the list

            //string dataPoint = ent__main.SelectedItem as string;
        }

        

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

        private void btn__Save_Clicked(object sender, EventArgs e)
        {
            // convert to a string, then write to a file
            string toWrite = string.Empty;
            foreach (var item in dataPoints)
            {
                toWrite += item.ToString();
            }
            

            File.WriteAllText(Path.Combine($"{FileSystem.AppDataDirectory}", "test.txt"), toWrite);
        }
    }
}
