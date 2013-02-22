using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

namespace TeamCityDesktop.Extensions
{
    internal static class ObservableCollectionExtensions
    {
        internal static void DispatcherAddRange<T>(
            this ObservableCollection<T> collection,
            IEnumerable<T> items,
            Dispatcher dispatcher = null)
        {
            if (items == null)
            {
                return;
            }
            if (dispatcher == null && Application.Current != null)
            {
                dispatcher = Application.Current.Dispatcher;
            }
            if (dispatcher != null && !dispatcher.CheckAccess())
            {
                dispatcher.BeginInvoke((Action)(
                    () => collection.DispatcherAddRange(items, dispatcher)));
                return;
            }
            collection.Clear();
            foreach (T o in items)
            {
                collection.Add(o);
            }
        }
    }
}
