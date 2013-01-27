using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop.Controls
{
    public partial class ServerOverview : UserControl
    {
        public ServerOverview(ServerOverviewViewModel viewModel = null)
        {
            ViewModel = viewModel ?? new ServerOverviewViewModel();
            DataContext = ViewModel;
            InitializeComponent();
        }

        public ServerOverviewViewModel ViewModel { get; private set; }

        private void TreeViewSelectedItemChanged(
            object sender,
            RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is BuildConfigViewModel)
            {
                ViewModel.SelectedBuildConfig = (BuildConfigViewModel)e.NewValue;
            }
        }

        private void ListBoxSelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is BuildViewModel)
            {
                ViewModel.SelectedBuild = (BuildViewModel)e.AddedItems[0];
            }
            foreach (TreeViewModelBase treeViewItem in e.RemovedItems.OfType<TreeViewModelBase>())
            {
                treeViewItem.IsSelected = false;
            }
            foreach (TreeViewModelBase treeViewItem in e.AddedItems.OfType<TreeViewModelBase>())
            {
                treeViewItem.IsSelected = true;
            }
        }

        private void DownloadArtifactsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ArtifactsListBox.SelectedItems.Count > 0;
        }

        private void DownloadArtifactsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.DownloadArtifacts(ArtifactsListBox.SelectedItems.OfType<ArtifactViewModel>().ToList());
        }

        private void DownloadAllArtifactsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ArtifactsListBox.Items.Count > 0;
        }

        private void DownloadAllArtifactsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.DownloadArtifacts(ArtifactsListBox.Items.OfType<ArtifactViewModel>().ToList());
        }
    }
}
