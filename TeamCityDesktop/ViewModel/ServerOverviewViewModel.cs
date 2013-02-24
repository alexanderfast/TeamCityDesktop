using System;
using System.ComponentModel;

using TeamCityDesktop.DataAccess;

namespace TeamCityDesktop.ViewModel
{
    public class ServerOverviewViewModel : ViewModelBase
    {
        private readonly ProjectsViewModel projects;
        private readonly IArtifactDownloader downloader;
        private readonly IFolderSelector folderSelector;
        private ArtifactsViewModel artifacts;
        private bool disposed;

        public ServerOverviewViewModel(
            IDataProvider dataProvider,
            IArtifactDownloader downloader,
            IFolderSelector folderSelector = null)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }
            if (downloader == null)
            {
                throw new ArgumentNullException("downloader");
            }
            projects = new ProjectsViewModel(dataProvider);
            projects.PropertyChanged += ProjectPropertyChanged;
            projects.LoadItems();
            this.downloader = downloader;
            this.folderSelector = folderSelector;
        }

        public ProjectsViewModel Projects
        {
            get { return projects; }
        }

        public ArtifactsViewModel Artifacts
        {
            get { return artifacts; }
            private set
            {
                artifacts = value;
                OnPropertyChanged("Artifacts");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                    projects.PropertyChanged -= ProjectPropertyChanged;
                    projects.Dispose();
                    artifacts.Dispose();
                }
            }
        }

        private void ProjectPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("SelectedBuild".Equals(e.PropertyName))
            {
                Artifacts = new ArtifactsViewModel(
                    downloader, folderSelector, projects.SelectedBuild);
                Artifacts.LoadItems();
            }
        }
    }
}
