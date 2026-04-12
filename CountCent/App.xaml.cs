namespace CountCent
{
    public partial class App : Application
    {
        // designate as initial page for our application
        public App(MainPage mainPage)
        {
            InitializeComponent();

            UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}