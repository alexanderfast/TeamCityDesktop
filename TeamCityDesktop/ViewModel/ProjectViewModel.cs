using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class ProjectViewModel : TreeViewModelBase
    {
        private Project project;
        private bool refreshed;

        public ProjectViewModel(TeamCityClient client)
        {
            Client = client;
            Children.Add(new LoadingViewModel());
        }

        public Project Project
        {
            get { return project; }
            set
            {
                if (value != project)
                {
                    project = value;
                    OnPropertyChanged("Project");
                }
            }
        }

        public override bool IsExpanded
        {
            get { return base.IsExpanded; }
            set
            {
                if (value != base.IsExpanded)
                {
                    if (!refreshed)
                    {
                        RefreshAsync();
                        refreshed = true;
                    }
                    base.IsExpanded = value;
                }
            }
        }

        protected override void Refresh()
        {
            IsLoading = true;
            try
            {
                List<BuildConfig> cached = new Cache(project).Load();
                if (cached.Count == 0)
                {
                    cached = Client.BuildConfigsByProjectId(project.Id);
                    new Cache(project, cached).Save();
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

        private void WrapModels(IEnumerable<BuildConfig> buildConfigs)
        {
            if (Children.Count == 1 && Children[0] is LoadingViewModel)
            {
                Children.Clear();
            }
            foreach (BuildConfig buildConfig in buildConfigs)
            {
                Children.Add(new BuildConfigViewModel
                    {
                        BuildConfig = buildConfig,
                        Client = Client
                    });
            }
        }

        #region Nested type: Cache

        private class Cache : GenericCache<List<BuildConfig>>
        {
            public Cache(Project parent, List<BuildConfig> buildConfigs = null)
                : base(Path.Combine("Cache", parent.Id + "_buildconfigs.xml"), buildConfigs)
            {
            }
        }

        #endregion
    }
}
