using System.Collections.Generic;
using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Extensions;
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

        public override void LoadItems()
        {
            if ("N/A".Equals(build.Number))
            {
                return;
            }
            IsLoading = true;
            dataProvider.GetArtifactsInBuild(build, viewModels =>
                {
                    Collection.DispatcherAddRange(viewModels);
                    IsLoading = false;
                });
        }
    }
}
