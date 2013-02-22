using System.Collections.ObjectModel;

namespace TeamCityDesktop.ViewModel
{
    /// <summary>
    /// Wraps an ObservableCollection that is updated asynchronously.
    /// </summary>
    public abstract class AsyncCollectionViewModel<T> : ViewModelBase
    {
        private readonly ObservableCollection<T> collection =
            new ObservableCollection<T>();

        private bool isLoading;
        private T selectedItem;
        private bool isLoaded;

        public ObservableCollection<T> Collection
        {
            get { return collection; }
        }

        /// <summary>
        /// The currently selected item in the collection.
        /// </summary>
        public virtual T SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (!Equals(value, selectedItem))
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }

        /// <summary>
        /// True if the collection is currently being populated from another
        /// thread.
        /// </summary>
        public bool IsLoading
        {
            get { return isLoading; }
            protected set
            {
                if (value != isLoading)
                {
                    isLoading = value;
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        /// <summary>
        /// True if the collection has already been populated.
        /// </summary>
        public bool IsLoaded
        {
            get { return isLoaded; }
            protected set
            {
                if (value != isLoaded)
                {
                    isLoaded = value;
                    OnPropertyChanged("IsLoaded");
                }
            }
        }

        /// <summary>
        /// Populates the collection.
        /// </summary>
        public abstract void LoadItems();
    }
}
