using System.Collections.ObjectModel;

namespace CountCent.Models
{
    internal class AllNotes
    {
        // Must be public for XAML to see it
        public ObservableCollection<Note> Notes { get; set; } = new ObservableCollection<Note>();

        public AllNotes() => LoadNotes();

        public void LoadNotes()
        {
            Notes.Clear();

            // Get the folder where the notes are stored
            string appDataPath = FileSystem.AppDataDirectory;

            // Use LINQ to load the *.notes.txt 
            // Use IEnumerable so behaves like array
            IEnumerable<Note> notes = Directory

                // Select the files names from the dir
                .EnumerateFiles(appDataPath, "*.notes.txt")

                // Each file name is used to create a new Note
                .Select(filename => new Note(filename))

                // With the final collection of notes, order them by date
                .OrderBy(note => note.Date);

            // Add each note from ENUMERABLE into the ObservableCollection
            foreach (Note note in notes)
            {
                Notes.Add(note);
            }

        }
    }
}
