using System.Collections.Generic;
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

        public bool IsSuccessful
        {
            get { return "SUCCESS".Equals(build.Status); }
        }

        public override IEnumerable<ArtifactModel> LoadItems()
        {
            if ("N/A".Equals(build.Number))
            {
                return new ArtifactModel[] {};
            }
            return dataProvider.GetArtifactsInBuild(build);
        }
    }
}
