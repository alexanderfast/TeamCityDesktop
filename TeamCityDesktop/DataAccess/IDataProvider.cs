using System.Collections.Generic;
using TeamCityDesktop.Model;
using TeamCityDesktop.ViewModel;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.DataAccess
{
    public interface IDataProvider
    {
        IEnumerable<ProjectViewModel> GetProjects();
        IEnumerable<BuildConfigViewModel> GetBuildConfigs();
        IEnumerable<BuildConfigViewModel> GetBuildConfigsByProject(Project project);
        IEnumerable<BuildViewModel> GetBuildsInBuildConfig(BuildConfig buildConfig);
        IEnumerable<ArtifactModel> GetArtifactsInBuild(Build build);
        BuildViewModel GetMostRecentBuildInBuildConfig(BuildConfig buildConfig);
    }
}