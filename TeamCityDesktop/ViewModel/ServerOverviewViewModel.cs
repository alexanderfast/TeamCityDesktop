using System;

using TeamCityDesktop.DataAccess;

namespace TeamCityDesktop.ViewModel
{
    public class ServerOverviewViewModel : ViewModelBase
    {
        private readonly ProjectsViewModel projects;

        public ServerOverviewViewModel(IDataProvider dataProvider)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            projects = new ProjectsViewModel(dataProvider);
            projects.LoadCollectionAsync();
        }

        public ProjectsViewModel Projects
        {
            get { return projects; }
        }
    }
}
