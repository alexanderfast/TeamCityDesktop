using System.Collections.Generic;
using TeamCityDesktop.Model;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    /// <summary>
    /// This class exists mainly for cache a response from the server,
    /// since I cant assume that a TeamCity will be present during demo.
    /// </summary>
    public class ServerViewModel : ViewModelBase
    {
        private readonly TeamCityClient client;
        private List<BuildViewModel> builds;
        private List<Change> changes;
        private ProjectsViewModel projects;

        public ServerViewModel(ServerCredentialsModel serverCredentials)
        {
            client = serverCredentials.CreateClient();

            //buildConfigs = new BuildConfigsViewModel(client);
            //projects = new ProjectsViewModel(client);

            //projects.LoadCollectionAsync();
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
