using System;
using System.ComponentModel;
using TeamCityDesktop.Model;

namespace TeamCityDesktop.ViewModel
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly DelegateCommand connectCommand;
        private readonly CommandViewModel connectCommandViewModel;
        private readonly ServerCredentialsModel serverCredentials;
        private bool disposed;

        public LoginViewModel(
            ServerCredentialsModel serverCredentials,
            Action connectCallback)
        {
            this.serverCredentials = serverCredentials;
            serverCredentials.PropertyChanged += ServerCredentialsPropertyChanged;
            connectCommand = new DelegateCommand(
                connectCallback,
                serverCredentials.IsValid);
            connectCommandViewModel = new CommandViewModel("_Login", connectCommand);
        }

        public ServerCredentialsModel ServerCredentials
        {
            get { return serverCredentials; }
        }

        public CommandViewModel ConnectCommand
        {
            get { return connectCommandViewModel; }
        }

        private void ServerCredentialsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            connectCommand.OnCanExecuteChanged();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    serverCredentials.PropertyChanged -= ServerCredentialsPropertyChanged;
                }
                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}
