using System.Linq;

using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Model;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class BuildViewModel : AsyncCollectionViewModel<ArtifactModel>
    {
        private readonly Build build;
        private readonly IDataProvider dataProvider;

        public BuildViewModel(Build build, IDataProvider dataProvider)
        {
            this.build = build;
            this.dataProvider = dataProvider;
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
            dataProvider.GetArtifactsInBuildAsync(build,
                artifacts => DispatcherUpdateCollection(artifacts.Select(x => new ArtifactModel(x))));
        }
    }
}
