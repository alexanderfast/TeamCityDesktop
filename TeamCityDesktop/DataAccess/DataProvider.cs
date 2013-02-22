using System;
using System.Collections.Generic;
using System.Linq;
using TeamCityDesktop.Model;
using TeamCityDesktop.ViewModel;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCityDesktop.DataAccess
{
    public class DataProvider : IDataProvider
    {
        private readonly IWorker worker;
        private readonly ITeamCityClient client;

        public DataProvider(ITeamCityClient client, IWorker worker = null)
        {
            this.client = client;
            this.worker = worker ?? new Worker { IsAsync = true };
        }

        #region IDataProvider Members

        public void GetProjects(Action<IEnumerable<ProjectViewModel>> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            worker.QueueWork(delegate
                {
                    callback(client.AllProjects().Select(
                        x => new ProjectViewModel(x, this)));
                });
        }

        public void GetBuildConfigs(Action<IEnumerable<BuildConfigViewModel>> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            worker.QueueWork(delegate
                {
                    callback(client.AllBuildConfigs().Select(
                        x => new BuildConfigViewModel(x, this)));
                });
        }

        public void GetBuildConfigsByProject(Project project, Action<IEnumerable<BuildConfigViewModel>> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            worker.QueueWork(delegate
                {
                    callback(client.BuildConfigsByProjectId(project.Id).Select(
                        x => new BuildConfigViewModel(x, this)));
                });
        }

        public void GetBuildsInBuildConfig(BuildConfig buildConfig, Action<IEnumerable<BuildViewModel>> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            worker.QueueWork(delegate
                {
                    BuildLocator locator = BuildLocator.WithDimensions(
                        BuildTypeLocator.WithId(buildConfig.Id));
                    callback(client.BuildsByBuildLocator(locator).Select(
                        x => new BuildViewModel(x, this)));
                });
        }

        public void GetArtifactsInBuild(Build build, Action<IEnumerable<ArtifactModel>> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            if (!(client is TeamCityClient)) throw new InvalidOperationException("Not TeamCityClient");
            worker.QueueWork(delegate
                {
                    // TODO why is not this method in the interface?
                    callback(
                        ((TeamCityClient)client).ArtifactsByBuildConfigIdAndBuildNumber(build.BuildTypeId, build.Number).Select(
                        x => new ArtifactModel(x)));
                });
        }

        public void GetMostRecentBuildInBuildConfig(BuildConfig buildConfig, Action<BuildViewModel> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            worker.QueueWork(delegate
                {
                    BuildLocator locator = BuildLocator.WithDimensions(
                        BuildTypeLocator.WithId(buildConfig.Id), maxResults: 1);
                    callback(client.BuildsByBuildLocator(locator).Select(
                        x => new BuildViewModel(x, this)).FirstOrDefault());
                });
        }

        #endregion
    }
}
