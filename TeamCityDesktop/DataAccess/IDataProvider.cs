using System;
using System.Collections.Generic;
using TeamCityDesktop.Model;
using TeamCityDesktop.ViewModel;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.DataAccess
{
    public interface IDataProvider
    {
        void GetProjects(Action<IEnumerable<ProjectViewModel>> callback);
        void GetBuildConfigs(Action<IEnumerable<BuildConfigViewModel>> callback);
        void GetBuildConfigsByProject(Project project, Action<IEnumerable<BuildConfigViewModel>> callback);
        void GetBuildsInBuildConfig(BuildConfig buildConfig, Action<IEnumerable<BuildViewModel>> callback);
        void GetArtifactsInBuild(Build build, Action<IEnumerable<ArtifactModel>> callback);
        void GetMostRecentBuildInBuildConfig(BuildConfig buildConfig, Action<BuildViewModel> callback);
    }
}