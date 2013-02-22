using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Extensions;

using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class ProjectViewModel : AsyncCollectionViewModel<BuildConfigViewModel>
    {
        private readonly IDataProvider dataProvider;
        private readonly Project project;
        private bool isExpanded;
        private bool loaded;
        private bool successful;

        public ProjectViewModel(Project project, IDataProvider dataProvider)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            this.project = project;
            this.dataProvider = dataProvider;

            Collection.CollectionChanged += CollectionChanged;
        }

        /// <summary>
        /// A project is successful if all bulid configs are successful.
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

        public Project Project
        {
            get { return project; }
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
            dataProvider.GetBuildConfigsByProject(project, viewModels =>
                {
                    Collection.DispatcherAddRange(viewModels);
                    IsLoading = false;
                });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Collection.Clear();
                Collection.CollectionChanged -= CollectionChanged;
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IsSuccessful = Collection.All(x => x.IsSuccessful);

            if (e.OldItems != null)
            {
                foreach (BuildConfigViewModel oldItem in e.OldItems.OfType<BuildConfigViewModel>())
                {
                    oldItem.PropertyChanged -= BuildConfigViewModelPropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (BuildConfigViewModel newItem in e.NewItems.OfType<BuildConfigViewModel>())
                {
                    newItem.PropertyChanged += BuildConfigViewModelPropertyChanged;
                }
            }
        }

        // Let the child with a selected item be the selected item
        private void BuildConfigViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("SelectedItem".Equals(e.PropertyName))
            {
                SelectedItem = sender as BuildConfigViewModel;
            }
        }
    }
}
