using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using TeamCityDesktop.Extensions;

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
        private bool loaded;
        private T selectedItem;

        public ObservableCollection<T> Collection
        {
            get
            {
                if (!loaded)
                {
                    // populate collection on first request
                    loaded = true;
                    ThreadPool.QueueUserWorkItem(delegate
                        {
                            IsLoading = true;
                            var items = LoadItems();
                            collection.DispatcherAddRange(items);
                            IsLoading = false;
                        });
                }
                return collection;
            }
        }

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

        public bool IsLoading
        {
            get { return isLoading; }
            private set
            {
                if (value != isLoading)
                {
                    isLoading = value;
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        public abstract IEnumerable<T> LoadItems();
    }
}
