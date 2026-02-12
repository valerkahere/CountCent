using CountCent.Models;
using Microsoft.Maui.Storage;

namespace CountCent.Views
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class NotePage : ContentPage
    {
        static string appDataPath = FileSystem.AppDataDirectory;

        // with get; gives CS8652
        public string ItemId
        {
            set { LoadNote(value); }
        }


        public NotePage()
        {
            InitializeComponent();

            // If file already exists
            // Include its contents into Editor
            
            string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";

            LoadNote(Path.Combine(appDataPath, randomFileName));

           
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is Models.Note note)
            {
                note.Filename = $"{Path.GetRandomFileName()}.notes.txt";
                File.WriteAllText(note.Filename, TextEditor.Text);
            }
                
            // navigate to previous page
            await Shell.Current.GoToAsync("..");
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (BindingContext is Models.Note note)
            {
                // Delete the file.
                if (File.Exists(note.Filename))
                {
                    File.Delete(note.Filename);
                }
            }

            await Shell.Current.GoToAsync("..");
        }

        private void LoadNote(string filename)
        {
            Note noteModel = new Note(filename);

            BindingContext = noteModel;
        }
    }
}
