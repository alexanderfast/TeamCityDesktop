using System.Collections.Generic;
using System.Linq;
using TeamCityDesktop.Model;
using TeamCityDesktop.ViewModel;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCityDesktop.DataAccess
{
    internal class DataProvider : IDataProvider
    {
        private readonly TeamCityClient client;

        public DataProvider(ServerCredentialsModel credentials)
        {
            client = credentials.CreateClient();
        }

        #region IDataProvider Members

        public IEnumerable<ProjectViewModel> GetProjects()
        {
            return client.AllProjects().Select(x => new ProjectViewModel(x, this));
        }

        public IEnumerable<BuildConfigViewModel> GetBuildConfigs()
        {
            return client.AllBuildConfigs().Select(x => new BuildConfigViewModel(x, this));
        }

        public IEnumerable<BuildConfigViewModel> GetBuildConfigsByProject(Project project)
        {
            return client.BuildConfigsByProjectId(
                project.Id).Select(x => new BuildConfigViewModel(x, this));
        }

        public IEnumerable<BuildViewModel> GetBuildsInBuildConfig(BuildConfig buildConfig)
        {
            BuildLocator locator = BuildLocator.WithDimensions(
                BuildTypeLocator.WithId(buildConfig.Id));
            return client.BuildsByBuildLocator(
                locator).Select(x => new BuildViewModel(x, this));
        }

        public IEnumerable<ArtifactModel> GetArtifactsInBuild(Build build)
        {
            return client.ArtifactsByBuildConfigIdAndBuildNumber(
                build.BuildTypeId, build.Number).Select(x => new ArtifactModel(x));
        }

        public BuildViewModel GetMostRecentBuildInBuildConfig(BuildConfig buildConfig)
        {
            BuildLocator locator = BuildLocator.WithDimensions(
                BuildTypeLocator.WithId(buildConfig.Id), maxResults: 1);
            return client.BuildsByBuildLocator(
                locator).Select(x => new BuildViewModel(x, this)).FirstOrDefault();
        }

        #endregion
    }
}
