using System.Diagnostics;
using CountCent.Models;

namespace CountCent.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            // Connect the Model to the View manually
            // so LearnMore_Clicked method works
            BindingContext = new About();
        }

        private async void LearnMore_Clicked(object sender, EventArgs e)
        {
            // Navigate to the specified URL in the system browser
            // what is bindingContext type?
            Debug.WriteLine(BindingContext.GetType().ToString());
            Debug.WriteLine(BindingContext.ToString());

            if (BindingContext is Models.About about)
            {
                await Launcher.Default.OpenAsync(about.MoreInfoUrl);
            }
        }
    }

}

