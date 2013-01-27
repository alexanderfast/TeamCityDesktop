using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class BuildConfigViewModel : TreeViewModelBase
    {
        private readonly ObservableCollection<BuildViewModel> builds =
            new ObservableCollection<BuildViewModel>();

        private readonly ICollectionView collectionView;
        private BuildConfig buildConfig;
        private bool refreshed;

        public BuildConfigViewModel()
        {
            collectionView = CollectionViewSource.GetDefaultView(builds);
            ((ListCollectionView)collectionView).CustomSort = new SortByDate();
        }

        public BuildConfig BuildConfig
        {
            get { return buildConfig; }
            set
            {
                if (value != buildConfig)
                {
                    buildConfig = value;
                    OnPropertyChanged("BuildConfig");
                }
            }
        }

        public ICollectionView Builds
        {
            get { return collectionView; }
        }

        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                if (value != base.IsSelected)
                {
                    base.IsSelected = value;
                    if (!refreshed)
                    {
                        refreshed = true;
                        RefreshAsync();
                    }
                }
            }
        }

        protected override void Refresh()
        {
            IsLoading = true;
            try
            {
                List<Build> cached = new Cache(buildConfig).Load();
                if (cached.Count == 0)
                {
                    cached.AddRange(Client.ErrorBuildsByBuildConfigId(buildConfig.Id));
                    cached.AddRange(Client.FailedBuildsByBuildConfigId(buildConfig.Id));
                    cached.AddRange(Client.SuccessfulBuildsByBuildConfigId(buildConfig.Id));
                    new Cache(buildConfig, cached).Save();
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

        private void WrapModels(IEnumerable<Build> buildConfigs)
        {
            // dont add to children since we want builds in a separate view
            foreach (Build build in buildConfigs)
            {
                builds.Add(new BuildViewModel
                    {
                        Build = build,
                        Client = Client
                    });
            }
        }

        #region Nested type: Cache

        private class Cache : GenericCache<List<Build>>
        {
            public Cache(BuildConfig parent, List<Build> builds = null)
                : base(Path.Combine("Cache", parent.Id + "_builds.xml"), builds)
            {
            }
        }

        #endregion

        #region Nested type: SortByDate

        public class SortByDate : IComparer
        {
            public SortByDate()
            {
                Ascending = true;
            }

            public bool Ascending { get; set; }

            #region IComparer Members

            public int Compare(object x, object y)
            {
                if (x is BuildViewModel && y is BuildViewModel)
                {
                    DateTime date1 = ((BuildViewModel)x).Build.StartDate;
                    DateTime date2 = ((BuildViewModel)y).Build.StartDate;
                    return Ascending ? date2.CompareTo(date1) : date1.CompareTo(date2);
                }
                return -1;
            }

            #endregion
        }

        #endregion
    }
}
