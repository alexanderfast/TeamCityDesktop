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
        private bool disposed;

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
                    foreach (var projectViewModel in Collection)
                    {
                        foreach (var buildConfigViewModel in projectViewModel.Collection)
                        {
                            if (buildConfigViewModel.SelectedItem != value)
                            {
                                buildConfigViewModel.SelectedItem = null;
                            }
                        }
                    }
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
                if (!disposed)
                {
                    disposed = true;
                    foreach (var projectViewModel in Collection)
                    {
                        foreach (var buildConfigViewModel in projectViewModel.Collection)
                        {
                            foreach (var buildViewModel in buildConfigViewModel.Collection)
                            {
                                buildViewModel.Collection.Clear();
                                buildViewModel.Dispose();
                            }
                            buildConfigViewModel.Collection.Clear();
                            buildConfigViewModel.Dispose();
                        }
                        projectViewModel.Collection.Clear();
                        projectViewModel.Dispose();
                    }
                    Collection.Clear();
                    Collection.CollectionChanged -= CollectionChanged;
                }
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (ProjectViewModel oldItem in e.OldItems.OfType<ProjectViewModel>())
                {
                    oldItem.Collection.CollectionChanged -= ProjectCollectionChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (ProjectViewModel newItem in e.NewItems.OfType<ProjectViewModel>())
                {
                    newItem.Collection.CollectionChanged += ProjectCollectionChanged;
                }
            }
        }

        private void ProjectCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (BuildConfigViewModel oldItem in e.OldItems.OfType<BuildConfigViewModel>())
                {
                    oldItem.PropertyChanged -= BuildConfigPropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (BuildConfigViewModel newItem in e.NewItems.OfType<BuildConfigViewModel>())
                {
                    newItem.PropertyChanged += BuildConfigPropertyChanged;
                }
            }
        }

        private void BuildConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var viewModel = sender as BuildConfigViewModel;
            if (viewModel == null)
            {
                return;
            }
            if ("SelectedItem".Equals(e.PropertyName) &&
                viewModel.SelectedItem != null)
            {
                SelectedBuild = viewModel.SelectedItem;
            }
        }
    }
}
