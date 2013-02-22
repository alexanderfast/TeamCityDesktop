using System;
using System.Collections.Generic;
using System.Threading;
using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Extensions;

using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class BuildConfigViewModel : AsyncCollectionViewModel<BuildViewModel>
    {
        private readonly BuildConfig buildConfig;
        private readonly IDataProvider dataProvider;
        private bool isExpanded;
        private bool successful;
        private readonly Worker worker = new Worker();

        public BuildConfigViewModel(BuildConfig buildConfig, IDataProvider dataProvider)
        {
            if (buildConfig == null)
            {
                throw new ArgumentNullException("buildConfig");
            }
            this.buildConfig = buildConfig;
            this.dataProvider = dataProvider;

            dataProvider.GetMostRecentBuildInBuildConfig(buildConfig,
                build => IsSuccessful = build != null && build.IsSuccessful);
        }

        public BuildConfig BuildConfig
        {
            get { return buildConfig; }
        }

        /// <summary>
        /// Is successful if and only if the latest build is successful.
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
                    LoadItems();
                    isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        public override void LoadItems()
        {
            IsLoading = true;
            dataProvider.GetBuildsInBuildConfig(buildConfig, viewModels =>
                {
                    Collection.DispatcherAddRange(viewModels);
                    IsLoading = false;
                });
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
