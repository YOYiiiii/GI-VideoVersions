using System.IO;
using System.Runtime.InteropServices;

namespace VideoVersions
{
    internal static partial class Config
    {
        public const string DllName = "VideoVersions.dll";

        public static readonly string GenshinProcName
            = Path.GetFileNameWithoutExtension(
                Marshal.PtrToStringUTF8(GetGenshinProcName())!);

        public static readonly string GenshinVersion
            = Marshal.PtrToStringUTF8(GetGenshinVersion())!;

        public static readonly string PipeName
            = Marshal.PtrToStringUTF8(GetPipeName())!;

        [LibraryImport(DllName)]
        private static partial nint GetGenshinProcName();

        [LibraryImport(DllName)]
        private static partial nint GetGenshinVersion();

        [LibraryImport(DllName)]
        private static partial nint GetPipeName();
    }
}
