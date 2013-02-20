using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace TeamCityDesktop.ViewModel
{
    public class ProjectsViewModel : AsyncCollectionViewModel<ProjectViewModel>
    {
        public ProjectsViewModel()
        {
            Collection.CollectionChanged += CollectionChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Collection.Clear();
                Collection.CollectionChanged -= CollectionChanged;
            }
        }

        public override void LoadCollectionAsync()
        {
            RequestManager.Instance.GetProjectsAsync(
                projects => DispatcherUpdateCollection(projects.Select(x => new ProjectViewModel(x))));
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
