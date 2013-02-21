using System;
using System.Collections.Generic;

using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.DataAccess
{
    public interface IDataProvider
    {
        void GetProjectsAsync(Action<List<Project>> callback);

        void GetBuildConfigsAsync(Action<List<BuildConfig>> callback);

        void GetBuildsInBuildConfigAsync(BuildConfig buildConfig, Action<List<Build>> callback);

        void GetArtifactsInBuildAsync(Build build, Action<List<string>> callback);

        void GetMostRecentBuildInBuildConfigAsync(BuildConfig buildConfig, Action<Build> callback);
    }
}