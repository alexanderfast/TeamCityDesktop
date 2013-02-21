using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
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
        private object activity;
        private IDataProvider dataProvider;
        private ServerCredentialsModel serverCredentials;

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
                var login = new Login(newCredentials);
                activity = login;
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
            serverCredentials = credentials;
            Activity = new ServerOverview
                {
                    DataContext = new ServerOverviewViewModel(new DataProvider(credentials))
                };
        }

        private void ConnectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var login = activity as Login;
            e.CanExecute = login == null
                ? false
                : login.ServerCredentials.IsValid();
        }

        private void ConnectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var login = activity as Login;
            if (login != null)
            {
                var serializer = new Serializer<List<ServerCredentialsModel>>();

                // save the credential info
                List<ServerCredentialsModel> credentials = File.Exists(ServerFile)
                    ? serializer.Load(ServerFile)
                    : new List<ServerCredentialsModel>();
                credentials.Add(login.ServerCredentials);
                serializer.Save(credentials, ServerFile);

                ShowServerOverview(login.ServerCredentials);
            }
        }

        private void DownloadArtifactsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter is IList<ArtifactModel> && ((IList)e.Parameter).Count > 0)
            {
                e.CanExecute = true;
            }
        }

        private void DownloadArtifactsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TeamCityClient client = serverCredentials.CreateClient();
                var downloader = new ArtifactDownloader(client, dialog.SelectedPath, ((IList<ArtifactModel>)e.Parameter));
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
