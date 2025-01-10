using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Forms;

namespace GI_VideoVersions
{
    internal static class Program
    {
        static bool IsRunAsAdmin()
        {
            using WindowsIdentity wi = WindowsIdentity.GetCurrent();
            WindowsPrincipal wp = new(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            if (!IsRunAsAdmin())
            {
                Utils.ShowError(Config.LoadString("MsgRequireAdmin"));
                return;
            }

            Environment.CurrentDirectory = Path.GetDirectoryName(Environment.ProcessPath)!;
            nint hModule = Native.LoadLibraryEx(Config.DllName, 0,
                Native.LoadLibraryExFlags.DontResolveDllReferences);
            if (hModule == 0)
            {
                Utils.ShowError(string.Format(
                    Config.LoadString("MsgLoadDllFail")!, Config.DllName,
                    new Win32Exception(Marshal.GetLastWin32Error()).Message));
                return;
            }

            Application.Run(new MainForm());
        }
    }
}
