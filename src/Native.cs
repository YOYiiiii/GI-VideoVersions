using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VideoVersions
{
    internal static partial class Native
    {
        [Flags]
        public enum LoadLibraryFlags : uint
        {
            DontResolveDllReferences = 0x00000001
        }

        [LibraryImport("Kernel32.dll", EntryPoint = "LoadLibraryExW", StringMarshalling = StringMarshalling.Utf16)]
        public static partial nint LoadLibraryEx(string lpLibFileName, nint hFile, LoadLibraryFlags dwFlags);
    }

    internal static partial class NativeHelper
    {
        public static bool LoadLibraryDll(
            uint dwProcessId, string lpDllPath, out int errorCode)
        {
            ProcessStartInfo psi = new()
            {
                FileName = "loader.exe",
                Arguments = $"-p {dwProcessId} -d \"{Path.GetFullPath(lpDllPath)}\"",
                UseShellExecute = true,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            using var process = Process.Start(psi);
            if (process is null)
            {
                errorCode = Marshal.GetLastWin32Error();
                return false;
            }
            process.WaitForExit();
            errorCode = process.ExitCode;
            return errorCode == 0;
        }
    }
}
