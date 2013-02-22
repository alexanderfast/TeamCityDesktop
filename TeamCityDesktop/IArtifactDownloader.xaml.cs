using System.Collections.Generic;

using TeamCityDesktop.Model;

namespace TeamCityDesktop
{
    public interface IArtifactDownloader
    {
        void DownloadAsync(IEnumerable<ArtifactModel> artifacts);
    }
}
