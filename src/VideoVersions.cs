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
    public class VideoVersionInfo
    {
        public static readonly List<string> VideoFiles = [];

        private List<string> _videos = [];
        private List<VideoVersionInfo> _videoGroups = [];

        public required string Version { get; init; }
        public ulong? Key { get; set; } = null;
        public bool EncAudio { get; init; }

        public List<string>? Videos
        {
            get { return _videos.Count == 0 ? null : _videos; }
            init { if (value is not null) _videos = value; }
        }

        public List<VideoVersionInfo>? VideoGroups
        {
            get { return _videoGroups.Count == 0 ? null : _videoGroups; }
            init { if (value is not null) _videoGroups = value; }
        }

        public static void InitVideoFiles(Process process)
        {
            var fileName = NativeHelper.GetProcessExecutablePath((uint)process.Id);
            string dictory = Path.GetDirectoryName(fileName)!;
            string name = Path.GetFileNameWithoutExtension(fileName);
            string path = $@"{dictory}\{name}_Data\StreamingAssets\VideoAssets\StandaloneWindows64\";
            VideoFiles.AddRange(Directory
                .GetFiles(path, "*.usm")
                .Select(n => Path.GetFileNameWithoutExtension(n))
                .Except(VideoFiles));
        }

        public void MergeFrom(VideoVersionInfo other)
        {
            if (this == other)
                throw new ArgumentException("VideoVersionInfo cannot merge from self!");
            if (Version != other.Version)
                throw new ArgumentException("VideoVersionInfo cannot merge from different version!");

            if (Key is null or 0) Key = other.Key;
            _videos.AddRange(other._videos.Except(_videos));

            _videoGroups.AddRange(other._videoGroups);
            var groups = _videoGroups.GroupBy(n => n.Version);
            foreach (var dups in groups.Where(g => g.Count() > 1))
            {
                foreach (var v in dups.Skip(1))
                {
                    dups.First().MergeFrom(v);
                    _videoGroups.Remove(v);
                }
            }
        }

        public bool MergeTagKey(string version, ulong key)
        {
            if (Version == version)
            {
                Key = key;
                return true;
            }
            foreach (var group in _videoGroups)
                if (group.MergeTagKey(version, key))
                    return true;
            return false;
        }

        public void TrimVideos()
        {
            _videos.RemoveAll(n => !VideoFiles.Contains(n));
            _videoGroups.ForEach(g => g.TrimVideos());
            _videoGroups.RemoveAll(g =>
                g._videos.Count == 0 &&
                g._videoGroups.Count == 0);
        }

        public void SortVideos()
        {
            _videos.Sort();
            _videoGroups.ForEach(g => g.SortVideos());
        }
    }

    public class VideoVersions
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

        public List<VideoVersionInfo> List { get; init; } = [];

        public void TrimVersions()
        {
            foreach (var info in List)
                info.TrimVideos();
            List.RemoveAll(n =>
                n.Videos?.Count is null or 0 &&
                n.VideoGroups?.Count is null or 0);
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

            foreach (var info in List)
            {
                info.VideoGroups?.Sort((a, b) =>
                {
                    if (int.TryParse(a.Version, out var x) &&
                        int.TryParse(b.Version, out var y))
                        return x.CompareTo(y);
                    return a.Version.CompareTo(b.Version);
                });
                info.SortVideos();
            }
        }

        public void MergeFrom(VideoVersions other)
        {
            if (this == other)
                throw new ArgumentException("VideoVersions cannot merge from self!");

            List.AddRange(other.List);
            var groups = List.GroupBy(n => n.Version);
            foreach (var dups in groups.Where(g => g.Count() > 1))
            {
                foreach (var v in dups.Skip(1))
                {
                    dups.First().MergeFrom(v);
                    List.Remove(v);
                }
            }
        }

        public void MergeTagKey(string version, ulong key)
        {
            foreach (var info in List)
                if (info.MergeTagKey(version, key))
                    return;
        }

        public static VideoVersions FromJson(ReadOnlySpan<byte> json)
            => JsonSerializer.Deserialize<VideoVersions>(json, jsonOptions)!;

        public static VideoVersions FromJson(string json)
            => JsonSerializer.Deserialize<VideoVersions>(json, jsonOptions)!;

        public string ToJson()
            => JsonSerializer.Serialize(this, jsonOptions);
    }
}
