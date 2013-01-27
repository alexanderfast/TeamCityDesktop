using System.Collections.Generic;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    /// <summary>
    /// This class exists mainly for cache a response from the server,
    /// since I cant assume that a TeamCity will be present during demo.
    /// </summary>
    public class ServerResponseViewModel : ViewModelBase
    {
        // if true forces read of a cached response
        private const bool ForceLocal = false;
        private readonly BuildConfigsViewModel buildConfigs;
        private readonly TeamCityClient client;
        private readonly ProjectsViewModel projects;
        private List<BuildViewModel> builds;
        private List<Change> changes;

        public ServerResponseViewModel(ServerViewModel server)
        {
            client = server.CreateClient();

            buildConfigs = new BuildConfigsViewModel(client);
            projects = new ProjectsViewModel(client);

            projects.RefreshAsync();
        }

        public BuildConfigsViewModel BuildConfigs
        {
            get { return buildConfigs; }
        }

        public ProjectsViewModel Projects
        {
            get { return projects; }
        }

        public List<BuildViewModel> Builds
        {
            get { return builds; }
            set
            {
                if (value != builds)
                {
                    builds = value;
                    OnPropertyChanged("Builds");
                }
            }
        }

        public List<Change> Changes
        {
            get { return changes; }
            set
            {
                if (value != changes)
                {
                    changes = value;
                    OnPropertyChanged("Changes");
                }
            }
        }
    }
}
