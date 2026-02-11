namespace CountCent.Views
{
    public partial class Settings : ContentPage
    {
        // Represents the name of the file
        string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");
        public Settings()
        {
            InitializeComponent();

            // If file already exists
            // Include its contents into Editor
            if (File.Exists(_fileName))
            {
                TextEditor.Text = File.ReadAllText(_fileName);
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            // Save the file
            File.WriteAllText(_fileName, TextEditor.Text);
        }

        private void DeleteButton_Clicked(Object sender, EventArgs e)
        {
            // Delete the file
            if (File.Exists(_fileName))
            {
                File.Delete(_fileName);
            }

            TextEditor.Text = string.Empty;

        }
    }
}
