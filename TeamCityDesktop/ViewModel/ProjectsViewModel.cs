﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TeamCityDesktop.DataAccess;

namespace TeamCityDesktop.ViewModel
{
    public class ProjectsViewModel : AsyncCollectionViewModel<ProjectViewModel>
    {
        private readonly IDataProvider dataProvider;

        public ProjectsViewModel(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
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

        public override IEnumerable<ProjectViewModel> LoadItems()
        {
            return dataProvider.GetProjects();
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
