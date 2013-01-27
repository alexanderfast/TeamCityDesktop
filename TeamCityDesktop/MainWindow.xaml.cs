using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using TeamCityDesktop.Controls;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly SettingsViewModel settings;
        private object activity;

        public MainWindow()
        {
            //MaxHeight = SystemParameters.PrimaryScreenHeight - 50;

            // for demo purposes always show the login screen
            var login = new Login();
            activity = login;

            settings = SettingsViewModel.Load();
            ServerViewModel server;
            if (settings.Servers.Count == 0)
            {
                // if no servers have been saved, create some default setting
                server = new ServerViewModel();
                settings.Servers.Add(server);

                login.ViewModel.Server.ServerUrl = "localhost:8080";
                login.ViewModel.Server.Username = "username";
                login.ViewModel.Server.Password = "password";
            }
            else
            {
                server = settings.Servers[0];
            }
            login.ViewModel.Server = server;
            activity = login;

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

        private void ConnectCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var login = activity as Login;
            e.CanExecute = login == null
                ? false
                : login.ViewModel.Server.IsValid;
        }

        private void ConnectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var login = activity as Login;
            if (login != null)
            {
                // save the credential info
                settings.Save();

                var overview = new ServerOverview();
                Activity = overview;
                overview.ViewModel.Connect(login.ViewModel.Server);

                // maximize the window
                WindowState = WindowState.Maximized;
                MaxWidth = SystemParameters.WorkArea.Width;
                MaxHeight = SystemParameters.WorkArea.Height;
            }
        }
    }
}
