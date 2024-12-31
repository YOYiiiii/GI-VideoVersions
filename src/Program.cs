namespace VideoVersions
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Environment.CurrentDirectory = Path.GetDirectoryName(Environment.ProcessPath)!;

            nint hModule = Native.LoadLibraryEx(Config.DllName, 0,
                Native.LoadLibraryFlags.DontResolveDllReferences);
            if (hModule != 0) Application.Run(new MainForm());
            else Utils.ShowError("Failed to load library: " + Config.DllName);
        }
    }
}
