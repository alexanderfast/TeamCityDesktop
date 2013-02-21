using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using TeamCityDesktop.Model;

using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

using File = System.IO.File;

namespace TeamCityDesktop.DataAccess
{
    public class DataProvider : IDataProvider
    {
        private const string CacheFolder = "Cache";

        // BuildConfigId to Build cache lookup
        private readonly Dictionary<string, GenericCache<List<Build>>> buildCache =
            new Dictionary<string, GenericCache<List<Build>>>();

        private readonly GenericCache<List<BuildConfig>> buildConfigsCache =
            new GenericCache<List<BuildConfig>>(Path.Combine(CacheFolder, "BuildConfigs.xml"));

        private readonly GenericCache<List<Project>> projectCache =
            new GenericCache<List<Project>>(Path.Combine(CacheFolder, "Projects.xml"));

        private readonly TeamCityClient teamCityClient;

        public DataProvider(ServerCredentialsModel credentials)
        {
            teamCityClient = credentials.CreateClient();
        }

        public void GetProjectsAsync(Action<List<Project>> callback)
        {
            new Thread(() =>
                {
                    List<Project> projects = SynchronizedCache(projectCache, teamCityClient.AllProjects);
                    if (callback != null) callback(projects);
                }).Start();
        }

        public void GetBuildConfigsAsync(Action<List<BuildConfig>> callback)
        {
            new Thread(() =>
                {
                    List<BuildConfig> buildConfigs = SynchronizedCache(buildConfigsCache, teamCityClient.AllBuildConfigs);
                    if (callback != null) callback(buildConfigs);
                }).Start();
        }

        public void GetBuildsInBuildConfigAsync(BuildConfig buildConfig, Action<List<Build>> callback)
        {
            new Thread(() =>
                {
                    GenericCache<List<Build>> cache;
                    lock (buildCache)
                    {
                        if (buildCache.ContainsKey(buildConfig.Id))
                        {
                            cache = buildCache[buildConfig.Id];
                        }
                        else
                        {
                            cache = new GenericCache<List<Build>>(Path.Combine(CacheFolder,
                                string.Format("Builds_for_{0}.xml", buildConfig.Id)));
                            buildCache.Add(buildConfig.Id, cache);
                        }
                    }
                    BuildLocator locator = BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfig.Id));
                    List<Build> builds = SynchronizedCache(cache, () => teamCityClient.BuildsByBuildLocator(locator));
                    if (callback != null) callback(builds);
                }).Start();
        }

        public void GetArtifactsInBuildAsync(Build build, Action<List<string>> callback)
        {
            new Thread(() =>
                {
                    List<string> artifacts = teamCityClient.ArtifactsByBuildConfigIdAndBuildNumber(build.BuildTypeId,
                        build.Number);
                    if (callback != null) callback(artifacts);
                }).Start();
        }

        public void GetMostRecentBuildInBuildConfigAsync(BuildConfig buildConfig, Action<Build> callback)
        {
            new Thread(() =>
                {
                    BuildLocator locator = BuildLocator.WithDimensions(BuildTypeLocator.WithId(buildConfig.Id), maxResults: 1);
                    if (callback != null) callback(teamCityClient.BuildsByBuildLocator(locator).FirstOrDefault());
                }).Start();
        }

        private static T SynchronizedCache<T>(GenericCache<T> cache, Func<T> update) where T : class, new()
        {
            lock (cache)
            {
                if (File.Exists(cache.Filename))
                {
                    return cache.Load();
                }
                T result = update();
                cache.Save(result);
                return result;
            }
        }
    }
}
