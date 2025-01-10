using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GI_VideoVersions
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Environment.CurrentDirectory = Path.GetDirectoryName(Environment.ProcessPath)!;

            nint hModule = Native.LoadLibraryEx(Config.DllName, 0,
                Native.LoadLibraryFlags.DontResolveDllReferences);
            if (hModule == 0) Utils.ShowError(string.Format(
                Config.LoadString("MsgLoadDllFail")!,
                Config.DllName, new Win32Exception(Marshal.GetLastWin32Error())));
            else Application.Run(new MainForm());
        }
    }
}
