using System;
using System.Linq;

namespace TeamCityDesktop.Model
{
    public class ArtifactModel
    {
        // path is something like:
        // /repository/download/bt353/72/NuGet/bbv.Common.Async.Log4Net.7.0.12149.1635.nupkg
        public ArtifactModel(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(path);
            }
            string[] parts = path.Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
            BuildType = parts[2];
            BuildNumber = parts[3];
            DownloadUrl = path;
            Path = string.Join("/", parts.Skip(4));
        }

        public string BuildType { get; set; }
        public string BuildNumber { get; set; }
        public string DownloadUrl { get; set; }
        public string Path { get; set; }
    }
}
