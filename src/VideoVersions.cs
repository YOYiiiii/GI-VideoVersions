using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GI_VideoVersions
{
    public class VideoVersionInfo(
        string version, bool encAudio, ulong? key = null,
        List<string>? videos = null, List<VideoVersionInfo>? videoGroups = null)
    {
        public static readonly List<string> VideoFiles = [];

        public bool EncAudio { get; init; } = encAudio;

        public ulong? Key { get; set; } = key;

        public string Version { get; init; } = version;

        public List<string>? Videos { get; init; } = videos;

        public List<VideoVersionInfo>? VideoGroups { get; init; } = videoGroups;

        public void MergeFrom(VideoVersionInfo other)
        {
            if (Videos?.Count > 0 && other.Videos?.Count > 0)
                Videos.AddRange(other.Videos
                    .Where(n => !Videos.Contains(n)));
            if (VideoGroups?.Count > 0 && other.VideoGroups?.Count > 0)
            {
                foreach (var group in other.VideoGroups)
                {
                    var g = VideoGroups.Find(n => n.Version == group.Version);
                    if (g is null) VideoGroups.Add(group);
                    else g.MergeFrom(group);
                }
            }
        }

        public void TrimVideos()
        {
            Videos?.RemoveAll(n => !VideoFiles.Contains(n));
            VideoGroups?.ForEach(g => g.TrimVideos());
            VideoGroups?.RemoveAll(g => (g.Videos?.Count ?? 0) == 0);
        }

        public void SortVideos()
        {
            Videos?.Sort();
            VideoGroups?.ForEach(g => g.SortVideos());
        }
    }

    public class VideoVersions(List<VideoVersionInfo>? list = null)
    {
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            WriteIndented = true,
            IndentCharacter = '\t',
            IndentSize = 1,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };

        public List<VideoVersionInfo> List { get; init; } = list ?? [];

        public static void InitVideoFiles(Process process)
        {
            var fileName = NativeHelper.GetProcessExecutablePath((uint)process.Id);
            string dictory = Path.GetDirectoryName(fileName)!;
            string name = Path.GetFileNameWithoutExtension(fileName);
            string path = $@"{dictory}\{name}_Data\StreamingAssets\VideoAssets\StandaloneWindows64\";
            VideoVersionInfo.VideoFiles.AddRange(Directory
                .GetFiles(path, "*.usm")
                .Select(n => Path.GetFileNameWithoutExtension(n)));
        }

        public void TrimVersions()
        {
            foreach (var version in List)
                version.TrimVideos();
            List.RemoveAll(n =>
                (n.Videos?.Count ?? 0) == 0 &&
                (n.VideoGroups?.Count ?? 0) == 0);
        }

        public void SortVersions()
        {
            List.Sort((a, b) =>
            {
                if (a.Version == "common")
                    return -1;
                if (b.Version == "common")
                    return 1;
                return a.Version.CompareTo(b.Version);
            });

            foreach (var version in List)
            {
                version.VideoGroups?.Sort((a, b) =>
                {
                    if (int.TryParse(a.Version, out var versionA) &&
                        int.TryParse(b.Version, out var versionB))
                        return versionA.CompareTo(versionB);
                    return a.Version.CompareTo(b.Version);
                });
                version.SortVideos();
            }
        }

        public void MergeTagKeys(Dictionary<string, ulong> dict)
        {
            foreach (var item in dict)
            {
                foreach (var version in List)
                {
                    if (version.Version == item.Key)
                        version.Key = item.Value;
                    else if (version.VideoGroups != null)
                    {
                        foreach (var group in version.VideoGroups)
                        {
                            if (group.Version == item.Key)
                                group.Key = item.Value;
                        }
                    }
                }
            }
        }

        public void MergeFrom(VideoVersions other)
        {
            foreach (var version in other.List)
            {
                var v = List.Find(n => n.Version == version.Version);
                if (v is null) List.Add(version);
                else v.MergeFrom(version);
            }
            SortVersions();
        }

        public static VideoVersions FromJson(ReadOnlySpan<byte> json)
            => JsonSerializer.Deserialize<VideoVersions>(json, jsonOptions)!;

        public static VideoVersions FromJson(string json)
            => JsonSerializer.Deserialize<VideoVersions>(json, jsonOptions)!;

        public string ToJson()
            => JsonSerializer.Serialize(this, jsonOptions);
    }
}
