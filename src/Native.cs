using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace GI_VideoVersions
{
    public static unsafe partial class Native
    {
        public const int MaxPath = 260;

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            SuspendResume = 0x00000800,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [Flags]
        public enum LoadLibraryExFlags : uint
        {
            DontResolveDllReferences = 0x00000001,
            LoadLibraryAsDatafile = 0x00000002,
            LoadLibraryWithAlteredSearchPath = 0x00000008,
            LoadIgnoreCodeAuthzLevel = 0x00000010,
            LoadLibraryAsImageResource = 0x00000020,
            LoadLibraryAsDatafileExclusive = 0x00000040,
            LoadLibraryRequireSignedTarget = 0x00000080,
            LoadLibrarySearchDllLoadDir = 0x00000100,
            LoadLibrarySearchApplicationDir = 0x00000200,
            LoadLibrarySearchUserDirs = 0x00000400,
            LoadLibrarySearchSystem32 = 0x00000800,
            LoadLibrarySearchDefaultDirs = 0x00001000,
            LoadLibrarySafeCurrentDirs = 0x00002000,
        }

        [LibraryImport("Kernel32.dll")]
        public static partial nint OpenProcess(
            ProcessAccessFlags dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
            uint dwProcessId);

        [LibraryImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool CloseHandle(nint hObject);

        [LibraryImport("Kernel32.dll", EntryPoint = "LoadLibraryExW", StringMarshalling = StringMarshalling.Utf16)]
        public static partial nint LoadLibraryEx(
            string lpLibFileName, nint hFile, LoadLibraryExFlags dwFlags);

        [LibraryImport("KernelBase.dll", EntryPoint = "K32GetModuleFileNameExW", StringMarshalling = StringMarshalling.Utf16)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool GetModuleFileNameEx(
            nint hProcess, nint hModule, char* lpFileName, uint nSize);
    }

    internal static unsafe partial class NativeHelper
    {
        public static string GetProcessExecutablePath(uint dwProcessId)
        {
            nint hProcess = Native.OpenProcess(
                Native.ProcessAccessFlags.QueryInformation |
                Native.ProcessAccessFlags.VirtualMemoryRead,
                false, dwProcessId);
            if (hProcess == 0) Utils.ThrowLastError();

            char* lpFilename = stackalloc char[Native.MaxPath];
            if (!Native.GetModuleFileNameEx(hProcess, 0, lpFilename, Native.MaxPath))
                Utils.ThrowLastError();
            return new string(lpFilename);
        }

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
