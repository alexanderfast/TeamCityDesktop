using System;
using System.Collections.Generic;
using System.Linq;

using TeamCitySharp;
using TeamCitySharp.DomainEntities;
using TeamCitySharp.Locators;

namespace TeamCityDesktop.Tests.Mocks
{
    public class MockedTeamCityClient : ITeamCityClient
    {
        private List<Project> projects = new List<Project>();
        private List<BuildConfig> buildConfigs = new List<BuildConfig>();
        private List<Build> builds = new List<Build>();

        public MockedTeamCityClient()
        {
            var project = new Project
                {
                    Href = "/httpAuth/app/rest/projects/id:project6",
                    Id = "project6",
                    Name = "MyProject"
                };
            projects.Add(project);

            var buildConfig = new BuildConfig
                {
                    Id = "bt18",
                    Name = "MyBuildConfig",
                    Href = "/httpAuth/app/rest/buildTypes/id:bt18",
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    WebUrl = "http://localhost/viewType.html?buildTypeId=bt18"
                };
            buildConfigs.Add(buildConfig);

            var build = new Build
                {
                    Id = "2200",
                    Number = "build_1.0.2200",
                    Status = "SUCCESS",
                    BuildTypeId = buildConfig.Id,
                    Href = "/httpAuth/app/rest/builds/id:2200",
                    WebUrl = "http://localhost/viewLog.html?buildId=2200&amp;buildTypeId=bt18",
                    StartDate = new DateTime(2013, 02, 19, 16, 3, 1),
                    FinishDate = DateTime.MinValue
                };
            builds.Add(build);
        }

        public List<Project> Projects
        {
            get { return projects; }
        }

        public List<BuildConfig> BuildConfigs
        {
            get { return buildConfigs; }
        }

        public List<Build> Builds
        {
            get { return builds; }
        }

        #region Implementation of ITeamCityClient
        public void Connect(string userName, string password, bool actAsGuest)
        {
            throw new NotImplementedException();
        }

        public bool Authenticate()
        {
            throw new NotImplementedException();
        }

        public List<Project> AllProjects()
        {
            return projects;
        }

        public Project ProjectByName(string projectLocatorName)
        {
            return projects.FirstOrDefault(x => x.Name.Equals(projectLocatorName));
        }

        public Project ProjectById(string projectLocatorId)
        {
            return projects.FirstOrDefault(x => x.Id.Equals(projectLocatorId));
        }

        public Project ProjectDetails(Project project)
        {
            throw new NotImplementedException();
        }

        public Server ServerInfo()
        {
            throw new NotImplementedException();
        }

        public List<Plugin> AllServerPlugins()
        {
            throw new NotImplementedException();
        }

        public List<Agent> AllAgents()
        {
            throw new NotImplementedException();
        }

        public Build LastBuildByAgent(string agentName)
        {
            throw new NotImplementedException();
        }

        public List<VcsRoot> AllVcsRoots()
        {
            throw new NotImplementedException();
        }

        public VcsRoot VcsRootById(string vcsRootId)
        {
            throw new NotImplementedException();
        }

        public List<User> AllUsers()
        {
            throw new NotImplementedException();
        }

        public List<Role> AllRolesByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public List<Group> AllGroupsByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public List<Group> AllUserGroups()
        {
            throw new NotImplementedException();
        }

        public List<User> AllUsersByUserGroup(string userGroupName)
        {
            throw new NotImplementedException();
        }

        public List<Role> AllUserRolesByUserGroup(string userGroupName)
        {
            throw new NotImplementedException();
        }

        public List<Change> AllChanges()
        {
            throw new NotImplementedException();
        }

        public Change ChangeDetailsByChangeId(string id)
        {
            throw new NotImplementedException();
        }

        public Change LastChangeDetailByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public List<Change> ChangeDetailsByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public List<BuildConfig> AllBuildConfigs()
        {
            throw new NotImplementedException();
        }

        public BuildConfig BuildConfigByConfigurationName(string buildConfigName)
        {
            throw new NotImplementedException();
        }

        public BuildConfig BuildConfigByConfigurationId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public BuildConfig BuildConfigByProjectNameAndConfigurationName(string projectName, string buildConfigName)
        {
            throw new NotImplementedException();
        }

        public BuildConfig BuildConfigByProjectNameAndConfigurationId(string projectName, string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public BuildConfig BuildConfigByProjectIdAndConfigurationName(string projectId, string buildConfigName)
        {
            throw new NotImplementedException();
        }

        public BuildConfig BuildConfigByProjectIdAndConfigurationId(string projectId, string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public List<BuildConfig> BuildConfigsByProjectId(string projectId)
        {
            return new List<BuildConfig>(buildConfigs.Where(x => x.ProjectId.Equals(projectId)));
        }

        public List<BuildConfig> BuildConfigsByProjectName(string projectName)
        {
            throw new NotImplementedException();
        }

        public List<Build> SuccessfulBuildsByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public Build LastSuccessfulBuildByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public List<Build> FailedBuildsByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public Build LastFailedBuildByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public Build LastBuildByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public List<Build> ErrorBuildsByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public Build LastErrorBuildByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public List<Build> BuildConfigsByBuildConfigId(string buildConfigId)
        {
            throw new NotImplementedException();
        }

        public List<Build> BuildConfigsByConfigIdAndTag(string buildConfigId, string tag)
        {
            throw new NotImplementedException();
        }

        public List<Build> BuildsByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public List<Build> BuildsByBuildLocator(BuildLocator locator)
        {
            // TODO the build locator is complex
            var b = builds.Where(x => x.BuildTypeId.Equals(locator.BuildType.Id));
            if (locator.MaxResults.HasValue)
            {
                return new List<Build>(b.Take(locator.MaxResults.Value));
            }
            return new List<Build>(b);
        }

        public List<Build> AllBuildsSinceDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public List<Build> AllBuildsOfStatusSinceDate(DateTime date, BuildStatus buildStatus)
        {
            throw new NotImplementedException();
        }

        public List<Build> NonSuccessfulBuildsForUser(string userName)
        {
            throw new NotImplementedException();
        }

        public bool TriggerServerInstanceBackup(string fileName)
        {
            throw new NotImplementedException();
        }

        public bool CreateUser(string username, string name, string email, string password)
        {
            throw new NotImplementedException();
        }

        public bool AddPassword(string username, string password)
        {
            throw new NotImplementedException();
        }

        public T CallByUrl<T>(string urlPart)
        {
            throw new NotImplementedException();
        }

        public Project CreateProject(string projectName)
        {
            throw new NotImplementedException();
        }

        public void DeleteProject(string projectName)
        {
            throw new NotImplementedException();
        }

        public BuildConfig CreateConfiguration(string projectName, string configurationName)
        {
            throw new NotImplementedException();
        }

        public void SetConfigurationSetting(BuildTypeLocator locator, string settingName, string settingValue)
        {
            throw new NotImplementedException();
        }

        public VcsRoot AttachVcsRoot(BuildTypeLocator locator, VcsRoot vcsRoot)
        {
            throw new NotImplementedException();
        }

        public void SetVcsRootField(VcsRoot vcsRoot, VcsRootField field, object value)
        {
            throw new NotImplementedException();
        }

        public void PostRawArtifactDependency(BuildTypeLocator locator, string rawXml)
        {
            throw new NotImplementedException();
        }

        public void PostRawBuildStep(BuildTypeLocator locator, string rawXml)
        {
            throw new NotImplementedException();
        }

        public void PostRawBuildTrigger(BuildTypeLocator locator, string rawXml)
        {
            throw new NotImplementedException();
        }

        public void SetProjectParameter(string projectName, string settingName, string settingValue)
        {
            throw new NotImplementedException();
        }

        public void SetConfigurationParameter(BuildTypeLocator locator, string key, string value)
        {
            throw new NotImplementedException();
        }

        public void PostRawAgentRequirement(BuildTypeLocator locator, string rawXml)
        {
            throw new NotImplementedException();
        }

        public void DetachVcsRoot(BuildTypeLocator locator, string vcsRootId)
        {
            throw new NotImplementedException();
        }

        public void DeleteBuildStep(BuildTypeLocator locator, string buildStepId)
        {
            throw new NotImplementedException();
        }

        public void DeleteArtifactDependency(BuildTypeLocator locator, string artifactDependencyId)
        {
            throw new NotImplementedException();
        }

        public void DeleteAgentRequirement(BuildTypeLocator locator, string agentRequirementId)
        {
            throw new NotImplementedException();
        }

        public void DeleteParameter(BuildTypeLocator locator, string parameterName)
        {
            throw new NotImplementedException();
        }

        public void DeleteProjectParameter(string projectName, string parameterName)
        {
            throw new NotImplementedException();
        }

        public void DeleteBuildTrigger(BuildTypeLocator locator, string buildTriggerId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
