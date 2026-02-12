namespace CountCent.Models
{
    internal class Note
    {
        public string Filename { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Note(string filename)
        {
            // if file exists at - then fill a note with details
            // C:\\Users\\username\\AppData\\Local\\User Name\\com.companyname.mauiapp1\\Data\\FILENAME
            if (File.Exists(Path.Combine(FileSystem.AppDataDirectory, filename)))
            {
                Filename = filename;

                // If file already exists
                // Include its contents into the Model
                Text = File.ReadAllText(filename); // if empty will be just "" (string.Empty)

                // Update the Model with the date the file was created
                Date = File.GetCreationTime(filename);
            }

        }
    }
}
