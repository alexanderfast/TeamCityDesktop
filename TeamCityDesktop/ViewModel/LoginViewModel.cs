namespace TeamCityDesktop.ViewModel
{
    /// <summary>
    /// This view is shown if there are no servers.
    /// Also doubles as a welcome screen.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private ServerViewModel server;

        /// <summary>
        /// The server to configure.
        /// </summary>
        public ServerViewModel Server
        {
            get { return server; }
            set
            {
                if (value != server)
                {
                    server = value;
                    OnPropertyChanged("Server");
                }
            }
        }
    }
}
