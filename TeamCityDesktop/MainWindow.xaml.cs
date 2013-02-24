using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using TeamCityDesktop.Background;
using TeamCityDesktop.Controls;
using TeamCityDesktop.DataAccess;
using TeamCityDesktop.Model;
using TeamCityDesktop.ViewModel;
using TeamCityDesktop.Windows;
using TeamCitySharp;
using Application = System.Windows.Application;

namespace TeamCityDesktop
{
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string ServerFile = "Servers.xml";
        private readonly IFolderSelector folderSelector = new FolderSelector();
        private readonly Worker worker = new Worker {IsAsync = true};
        private object activity;
        private IArtifactDownloader artifactDownloader;

        public MainWindow()
        {
            List<ServerCredentialsModel> credentials = null;
            if (File.Exists(ServerFile))
            {
                credentials = new Serializer<List<ServerCredentialsModel>>().Load(ServerFile);
            }
            if (credentials == null || credentials.Count == 0)
            {
                // if no servers have been saved, create some default setting
                var newCredentials = new ServerCredentialsModel
                    {
                        Url = "teamcity.codebetter.com",
                        Guest = true
                    };
                activity = new Login
                    {
                        DataContext = new LoginViewModel(newCredentials, ConnectExecuted)
                    };
            }
            else
            {
                ShowServerOverview(credentials[0]);
            }
            InitializeComponent();
        }

        public object Activity
        {
            get { return activity; }
            set
            {
                if (activity is IDisposable)
                {
                    ((IDisposable)activity).Dispose();
                }
                activity = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Activity"));
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void ShowServerOverview(ServerCredentialsModel credentials)
        {
            artifactDownloader = new InteractiveArtifactDownloader(
                credentials.CreateClient(), worker);
            var overviewViewModel = new ServerOverviewViewModel(
                new DataProvider(
                    credentials.CreateClient(),
                    worker),
                artifactDownloader,
                folderSelector);
            Activity = new ServerOverview {DataContext = overviewViewModel};
        }

        private void ConnectExecuted()
        {
            var login = activity as Login;
            if (login != null)
            {
                var serializer = new Serializer<List<ServerCredentialsModel>>();

                // save the credential info
                var credentials = File.Exists(ServerFile)
                    ? serializer.Load(ServerFile)
                    : new List<ServerCredentialsModel>();
                var model = ((LoginViewModel)login.DataContext).ServerCredentials;
                credentials.Add(model);
                serializer.Save(credentials, ServerFile);

                ShowServerOverview(model);
            }
        }

        #region Nested type: FolderSelector

        /// <summary>
        /// Wraps the Windows Forms dialog to let the user select a folder.
        /// </summary>
        private class FolderSelector : ViewModelBase, IFolderSelector
        {
            private readonly CommandViewModel command;
            private string selectedFolder;

            public FolderSelector()
            {
                var delegateCommand = new DelegateCommand(() =>
                    {
                        var dialog = new FolderBrowserDialog {SelectedPath = selectedFolder};
                        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            SelectedFolder = dialog.SelectedPath;
                        }
                    });
                command = new CommandViewModel("Select Folder", delegateCommand);
                selectedFolder = Environment.CurrentDirectory;
            }

            #region Implementation of IFolderSelector

            public CommandViewModel SelectFolder
            {
                get { return command; }
            }

            public string SelectedFolder
            {
                get { return selectedFolder; }
                set
                {
                    if (value != selectedFolder)
                    {
                        selectedFolder = value;
                        OnPropertyChanged("SelectedFolder");
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: InteractiveArtifactDownloader

        /// <summary>
        /// Downloads artifacts asynchronously while displaying a progress dialog.
        /// </summary>
        private class InteractiveArtifactDownloader : IArtifactDownloader
        {
            private readonly TeamCityClient client;
            private readonly Worker worker;

            public InteractiveArtifactDownloader(TeamCityClient client, Worker worker)
            {
                if (client == null)
                {
                    throw new ArgumentNullException("client");
                }
                if (worker == null)
                {
                    throw new ArgumentNullException("worker");
                }
                this.client = client;
                this.worker = worker;
            }

            #region Implementation of IArtifactDownloader

            public void DownloadAsync(
                string targetFolder,
                IEnumerable<ArtifactModel> artifacts)
            {
                var downloader = new ArtifactDownloader(client, targetFolder, artifacts);
                new ProgressDialog
                    {
                        Owner = Application.Current.MainWindow,
                        DataContext = new ProgressDialogViewModel(downloader)
                            {
                                Title = "Downloading artifacts...",
                            }
                    }.Show();
                worker.QueueWork(delegate { downloader.RunSynchronously(); });
            }

            #endregion
        }

        #endregion
    }
}
