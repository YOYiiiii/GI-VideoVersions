using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GI_VideoVersions
{
    internal static partial class Config
    {
        public enum LanguageType
        {
            en_us = 0,
            zh_cn = 1
        }

        private const string ConfigFile = "config.json";

        public const string DllName = "VideoVersions.dll";

        public static readonly string GenshinProcName
            = Path.GetFileNameWithoutExtension(
                Marshal.PtrToStringUTF8(GetGenshinProcName())!);

        public static readonly string GenshinVersion
            = Marshal.PtrToStringUTF8(GetGenshinVersion())!;

        public static readonly string PipeName
            = Marshal.PtrToStringUTF8(GetPipeName())!;

        public static LanguageType Language
        {
            get
            {
                try
                {
                    var json = File.ReadAllText(ConfigFile);
                    var obj = JsonDocument.Parse(json);
                    return Enum.Parse<LanguageType>(obj
                        .RootElement
                        .GetProperty("language")
                        .GetString()!);
                }
                catch { return LanguageType.en_us; }
            }
            set
            {
                try
                {
                    var obj = new JsonObject()
                    {
                        ["language"] = value.ToString()
                    };
                    var json = JsonSerializer.Serialize(obj);
                    File.WriteAllText(ConfigFile, json);
                }
                catch (Exception ex)
                {
                    Utils.ShowError(string.Format(
                        Config.LoadString("MsgSaveFileFail")!,
                        ConfigFile, ex.Message));
                }
            }
        }

        public static string? LoadString(string name)
            => new ResourceManager(
                $"GI_VideoVersions.res.lang.{Language}",
                Assembly.GetExecutingAssembly()
                ).GetString(name);

        [LibraryImport(DllName)]
        private static partial nint GetGenshinProcName();

        [LibraryImport(DllName)]
        private static partial nint GetGenshinVersion();

        [LibraryImport(DllName)]
        private static partial nint GetPipeName();
    }
}
