using System;
using System.Linq;

using TeamCityDesktop.DataAccess;

using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class BuildConfigViewModel : AsyncCollectionViewModel<BuildViewModel>
    {
        private readonly BuildConfig buildConfig;
        private readonly IDataProvider dataProvider;
        private bool isExpanded;
        private bool loaded;
        private bool successful;

        public BuildConfigViewModel(BuildConfig buildConfig, IDataProvider dataProvider)
        {
            if (buildConfig == null)
            {
                throw new ArgumentNullException("buildConfig");
            }
            this.buildConfig = buildConfig;
            this.dataProvider = dataProvider;

            dataProvider.GetMostRecentBuildInBuildConfigAsync(
                buildConfig, x => IsSuccessful = "SUCCESS".Equals(x.Status));
        }

        public BuildConfig BuildConfig
        {
            get { return buildConfig; }
        }

        /// <summary>
        /// A BuildConfig is successful if the latest build is successful.
        /// </summary>
        public bool IsSuccessful
        {
            get { return successful; }
            private set
            {
                if (value != successful)
                {
                    successful = value;
                    OnPropertyChanged("IsSuccessful");
                }
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    if (!loaded)
                    {
                        loaded = true;
                        LoadCollectionAsync();
                    }
                    isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        public override BuildViewModel SelectedItem
        {
            get { return base.SelectedItem; }
            set
            {
                if (!Equals(value, base.SelectedItem))
                {
                    if (value != null)
                    {
                        value.LoadCollectionAsync();
                    }
                    base.SelectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        public override void LoadCollectionAsync()
        {
            dataProvider.GetBuildsInBuildConfigAsync(
                buildConfig,
                builds => DispatcherUpdateCollection(builds.Select(x => new BuildViewModel(x, dataProvider))));
        }

        //#region Nested type: SortByDate

        //public class SortByDate : IComparer
        //{
        //    public SortByDate()
        //    {
        //        Ascending = true;
        //    }

        //    public bool Ascending { get; set; }

        //    #region IComparer Members

        //    public int Compare(object x, object y)
        //    {
        //        if (x is BuildViewModel && y is BuildViewModel)
        //        {
        //            DateTime date1 = ((BuildViewModel)x).Build.StartDate;
        //            DateTime date2 = ((BuildViewModel)y).Build.StartDate;
        //            return Ascending ? date2.CompareTo(date1) : date1.CompareTo(date2);
        //        }
        //        return -1;
        //    }

        //    #endregion
        //}

        //#endregion
    }
}
