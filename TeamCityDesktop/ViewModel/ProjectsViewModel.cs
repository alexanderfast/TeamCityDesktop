using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Extensions;

namespace TeamCityDesktop.ViewModel
{
    public class ProjectsViewModel : AsyncCollectionViewModel<ProjectViewModel>
    {
        private readonly IDataProvider dataProvider;
        private BuildViewModel selectedBuild;

        public ProjectsViewModel(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            Collection.CollectionChanged += CollectionChanged;
        }

        /// <summary>
        /// The list of projects contains many build configs with many builds.
        /// This property enforces that only one build is selected among
        /// all the lists.
        /// </summary>
        public BuildViewModel SelectedBuild
        {
            get { return selectedBuild; }
            set
            {
                if (value != selectedBuild)
                {
                    selectedBuild = value;
                    OnPropertyChanged("SelectedBuild");
                }
            }
        }

        public override void LoadItems()
        {
            IsLoading = true;
            dataProvider.GetProjects(viewModels =>
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
            if (e.OldItems != null)
            {
                foreach (ProjectViewModel oldItem in e.OldItems.OfType<ProjectViewModel>())
                {
                    oldItem.PropertyChanged -= ProjectViewModelPropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (ProjectViewModel newItem in e.NewItems.OfType<ProjectViewModel>())
                {
                    newItem.PropertyChanged += ProjectViewModelPropertyChanged;
                }
            }
        }

        // Let the child with a selected item be the selected item
        private void ProjectViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("SelectedItem".Equals(e.PropertyName))
            {
                SelectedItem = sender as ProjectViewModel;
            }
        }
    }
}
