using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class ProjectsViewModel : ClientResponseViewModel
    {
        private readonly TeamCityClient client;

        private readonly ObservableCollection<ProjectViewModel> projects =
            new ObservableCollection<ProjectViewModel>();

        public ProjectsViewModel(TeamCityClient client)
        {
            this.client = client;
            RefreshAsync();
        }

        public ObservableCollection<ProjectViewModel> Projects
        {
            get { return projects; }
        }

        protected override void Refresh()
        {
            IsLoading = true;
            try
            {
                List<Project> cached = new Cache().Load();
                if (cached.Count == 0)
                {
                    cached = client.AllProjects();
                    new Cache(cached).Save();
                }
                Application.Current.Dispatcher.BeginInvoke(
                    (Action)(() => WrapModels(cached)));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name);
            }
            IsLoading = false;
        }

        private void WrapModels(IEnumerable<Project> projectsToAdd)
        {
            foreach (Project project in projectsToAdd)
            {
                projects.Add(new ProjectViewModel(client) {Project = project});
            }
        }

        #region Nested type: Cache

        private class Cache : GenericCache<List<Project>>
        {
            private const string Filename = @"Cache\Projects.xml";

            public Cache(List<Project> projects = null) : base(Filename, projects)
            {
            }
        }

        #endregion
    }
}
