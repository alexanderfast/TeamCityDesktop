using System.Collections.Generic;

using TeamCityDesktop.Model;

namespace TeamCityDesktop
{
    public interface IArtifactDownloader
    {
        void DownloadAsync(string targetFolder, IEnumerable<ArtifactModel> artifacts);
    }
}
