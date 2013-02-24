using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TeamCityDesktop.Model;

namespace TeamCityDesktop.ViewModel
{
    public class ArtifactsViewModel : AsyncCollectionViewModel<ArtifactViewModel>
    {
        private readonly BuildViewModel buildViewModel;
        private readonly DelegateCommand downloadAll;
        private readonly DelegateCommand downloadSelected;
        private readonly IArtifactDownloader downloader;
        private readonly IFolderSelector folderSelector;
        private bool disposed;

        public ArtifactsViewModel(
            IArtifactDownloader downloader,
            IFolderSelector folderSelector,
            BuildViewModel buildViewModel)
        {
            if (downloader == null)
            {
                throw new ArgumentNullException("downloader");
            }
            if (buildViewModel == null)
            {
                throw new ArgumentNullException("buildViewModel");
            }
            this.downloader = downloader;
            this.folderSelector = folderSelector;
            this.buildViewModel = buildViewModel;

            downloadSelected = new DelegateCommand(
                DownloadSelected,
                () => Collection.Count(x => x.IsSelected) > 0);

            downloadAll = new DelegateCommand(
                DownloadAll,
                () => Collection.Count > 0);

            Commands = new[]
                {
                    new CommandViewModel("Download Selected", downloadSelected),
                    new CommandViewModel("Download All", downloadAll)
                };
        }

        public BuildViewModel Build
        {
            get { return buildViewModel; }
        }

        public IFolderSelector FolderSelector
        {
            get { return folderSelector; }
        }

        public IEnumerable<CommandViewModel> Commands { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                    foreach (ArtifactViewModel viewModel in Collection)
                    {
                        viewModel.PropertyChanged -= ArtifactViewModelPropertyChanged;
                    }
                    Collection.Clear();
                }
            }
        }

        private void BuildViewModelCollectionChanged(
            object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ArtifactModel model in e.NewItems.OfType<ArtifactModel>())
                {
                    AddArtifact(model);
                }
                downloadAll.OnCanExecuteChanged();
            }
            // TODO doesnt support removal, but BuildViewModel never removes artifacts
        }

        private void AddArtifact(ArtifactModel model)
        {
            var viewModel = new ArtifactViewModel(model);
            viewModel.PropertyChanged += ArtifactViewModelPropertyChanged;
            Collection.Add(viewModel);
        }

        private void ArtifactViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("IsSelected".Equals(e.PropertyName))
            {
                downloadSelected.OnCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets the folder where downloaded artifacts should be placed.
        /// </summary>
        private string GetDownloadFolder()
        {
            if (folderSelector == null || !Directory.Exists(folderSelector.SelectedFolder))
            {
                return Environment.CurrentDirectory;
            }
            return folderSelector.SelectedFolder;
        }

        private void DownloadAll()
        {
            downloader.DownloadAsync(
                GetDownloadFolder(),
                Collection.Select(x => x.Model));
        }

        private void DownloadSelected()
        {
            downloader.DownloadAsync(
                GetDownloadFolder(),
                Collection.Where(x => x.IsSelected).Select(x => x.Model));
        }

        #region Overrides of AsyncCollectionViewModel<ArtifactViewModel>

        public override void LoadItems()
        {
            if (IsLoaded)
            {
                return;
            }

            // handle items already added
            foreach (ArtifactModel model in buildViewModel.Collection)
            {
                AddArtifact(model);
            }

            // listen to changes and request loading
            buildViewModel.Collection.CollectionChanged +=
                BuildViewModelCollectionChanged;
            buildViewModel.LoadItems();
        }

        #endregion
    }
}
