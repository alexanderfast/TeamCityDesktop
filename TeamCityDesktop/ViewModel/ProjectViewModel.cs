using System;

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
        private bool successful;

        public ProjectViewModel(Project project, IDataProvider dataProvider)
        {
            if (project == null)
            {
                throw new ArgumentNullException("project");
            }
            this.project = project;
            this.dataProvider = dataProvider;
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
    }
}
