using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

using TeamCityDesktop.Model;

namespace TeamCityDesktop.ViewModel
{
    public class ArtifactsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ArtifactViewModel> artifacts =
            new ObservableCollection<ArtifactViewModel>();

        private readonly IArtifactDownloader downloader;
        private readonly BuildViewModel buildViewModel;
        private bool disposed;
        private DelegateCommand downloadSelected;
        private DelegateCommand downloadAll;

        public ArtifactsViewModel(
            IArtifactDownloader downloader, BuildViewModel buildViewModel)
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
            this.buildViewModel = buildViewModel;
            buildViewModel.LoadItems();
            buildViewModel.Collection.CollectionChanged +=
                BuildViewModelCollectionChanged;

            downloadSelected = new DelegateCommand(
                    DownloadSelected,
                    () => artifacts.Count(x => x.IsSelected) > 0);

            downloadAll = new DelegateCommand(
                    DownloadAll,
                    () => artifacts.Count > 0);

            Commands = new[]
            {
                new CommandViewModel("Download Selected", downloadSelected),
                new CommandViewModel("Download All", downloadAll)
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                    foreach (var viewModel in artifacts)
                    {
                        viewModel.PropertyChanged -= ArtifactViewModelPropertyChanged;
                    }
                    artifacts.Clear();
                }
            }
        }

        public BuildViewModel Build
        {
            get { return buildViewModel; }
        }

        public IEnumerable<CommandViewModel> Commands { get; private set; }

        public ObservableCollection<ArtifactViewModel> Artifacts
        {
            get { return artifacts; }
        }

        private void BuildViewModelCollectionChanged(
            object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var model in e.NewItems.OfType<ArtifactModel>())
                {
                    var viewModel = new ArtifactViewModel(model);
                    viewModel.PropertyChanged += ArtifactViewModelPropertyChanged;
                    artifacts.Add(viewModel);
                }
                downloadAll.OnCanExecuteChanged();
            }
            // TODO doesnt support removal, but BuildViewModel never removes artifacts
        }

        private void ArtifactViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ("IsSelected".Equals(e.PropertyName))
            {
                downloadSelected.OnCanExecuteChanged();
            }
        }

        private void DownloadAll()
        {
            downloader.DownloadAsync(artifacts.Select(x => x.Model));
        }

        private void DownloadSelected()
        {
            downloader.DownloadAsync(artifacts.Where(x => x.IsSelected).Select(x => x.Model));
        }
    }
}
