using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace TeamCityDesktop.ViewModel
{
    /// <summary>
    /// Wraps an ObservableCollection that is updated asynchronously.
    /// </summary>
    public abstract class AsyncCollectionViewModel<T> : ViewModelBase
    {
        private readonly ObservableCollection<T> collection =
            new ObservableCollection<T>();

        private bool isLoading = true;
        private T selectedItem;

        public ObservableCollection<T> Collection
        {
            get { return collection; }
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
            protected set
            {
                if (value != isLoading)
                {
                    isLoading = value;
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        public abstract void LoadCollectionAsync();

        /// <summary>
        /// Helper method to enforce that the objects are added to the collection on the GUI thread.
        /// </summary>
        /// <param name="objects">The objects to add.</param>
        protected void DispatcherUpdateCollection(IEnumerable<T> objects)
        {
            if (!Application.Current.CheckAccess())
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => DispatcherUpdateCollection(objects)));
                return;
            }
            Collection.Clear();
            foreach (T o in objects)
            {
                Collection.Add(o);
            }
            IsLoading = false;
        }
    }
}
