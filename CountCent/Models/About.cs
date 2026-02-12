namespace CountCent.Models
{
    internal class About
    {
        // Here Lambda operator means
        // This is read-only property that returns this value
        public string Title => AppInfo.Name;
        public string Version => AppInfo.VersionString;
        public string MoreInfoUrl => "https://www.valerkahere.com/";
        public string Message => "This app is written in XAML and C# with .NET MAUI.";
    }
}
