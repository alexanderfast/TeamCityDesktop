namespace TeamCityDesktop.ViewModel
{
    public class ServerOverviewViewModel : ViewModelBase
    {
        private ProjectsViewModel projects;

        public ProjectsViewModel Projects
        {
            get
            {
                if (projects == null)
                {
                    projects = new ProjectsViewModel();
                    projects.LoadCollectionAsync();
                }
                return projects;
            }
        }
    }
}
