namespace CountCent
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // behaviour immediately prior to application becoming visible
        protected override void OnAppearing()
        {
            // Get today's date, converts to date, to string (short date), and display it
            DateTime today = DateTime.Today;
            lbl_day.Text = today.ToString("d");
        }

    }
}
