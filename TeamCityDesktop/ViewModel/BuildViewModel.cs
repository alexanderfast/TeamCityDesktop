using System.Linq;
using TeamCityDesktop.Model;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class BuildViewModel : AsyncCollectionViewModel<ArtifactModel>
    {
        private readonly Build build;

        public BuildViewModel(Build build)
        {
            this.build = build;
        }

        public Build Build
        {
            get { return build; }
        }

        public override void LoadCollectionAsync()
        {
            if ("N/A".Equals(build.Number))
            {
                return;
            }
            RequestManager.Instance.GetArtifactsInBuildAsync(build,
                artifacts => DispatcherUpdateCollection(artifacts.Select(x => new ArtifactModel(x))));
        }
    }
}
