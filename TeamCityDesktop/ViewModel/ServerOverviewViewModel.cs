using System.Collections.Generic;
using System.Windows.Forms;
using TeamCityDesktop.Windows;
using TeamCitySharp;
using Application = System.Windows.Application;

namespace TeamCityDesktop.ViewModel
{
    public class ServerOverviewViewModel : ViewModelBase
    {
        private ServerResponseViewModel response;
        private BuildViewModel selectedBuild;
        private BuildConfigViewModel selectedBuildConfig;
        private ServerViewModel server;

        public ServerViewModel Server
        {
            get { return server; }
            private set
            {
                if (value != server)
                {
                    server = value;
                    OnPropertyChanged("Server");
                }
            }
        }

        public ServerResponseViewModel Response
        {
            get { return response; }
            set
            {
                if (value != response)
                {
                    response = value;
                    OnPropertyChanged("Response");
                }
            }
        }

        public BuildConfigViewModel SelectedBuildConfig
        {
            get { return selectedBuildConfig; }
            set
            {
                if (value != selectedBuildConfig)
                {
                    selectedBuildConfig = value;
                    OnPropertyChanged("SelectedBuildConfig");
                }
            }
        }

        public BuildViewModel SelectedBuild
        {
            get { return selectedBuild; }
            set
            {
                if (value != selectedBuild)
                {
                    selectedBuild = value;
                    OnPropertyChanged("SelectedBuild");
                }
            }
        }

        public void Connect(ServerViewModel newServer)
        {
            Server = newServer;
            Response = new ServerResponseViewModel(newServer);
        }

        public void DownloadArtifacts(List<ArtifactViewModel> artifactViewModels)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                TeamCityClient client = server.CreateClient();
                var downloader = new ArtifactDownloader(client, dialog.SelectedPath, artifactViewModels);
                downloader.RunWorkerAsync();
                new ProgressDialog(downloader)
                    {
                        Owner = Application.Current.MainWindow,
                        Title = "Downloading artifacts..."
                    }.ShowDialog();
            }
        }
    }
}
