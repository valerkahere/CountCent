using CountCent.Models;
using Microsoft.Maui.Storage;

namespace CountCent.Views
{
    public partial class NotePage : ContentPage
    {
        // Represents the name of the file
        string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");
        public NotePage()
        {
            InitializeComponent();

            // If file already exists
            // Include its contents into Editor
            string appDataPath = FileSystem.AppDataDirectory;
            string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

            LoadNotePage(Path.Combine(appDataPath, randomFileName));

           
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

        private void LoadNotePage(string filename)
        {
            Note noteModel = new Note();
            noteModel.Filename = filename;
            // If file already exists
            // Include its contents into the Model
            if (File.Exists(filename))
            {
                noteModel.Text = File.ReadAllText(filename);

                // Update the Model with the date the file was created
                noteModel.Date = File.GetCreationTime(filename);
            }

            BindingContext = noteModel;
        }
    }
}
