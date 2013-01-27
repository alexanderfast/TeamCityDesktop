using System;
using System.Xml.Serialization;
using TeamCitySharp;

namespace TeamCityDesktop.ViewModel
{
    /// <summary>
    /// Models a single server connection.
    /// </summary>
    public class ServerViewModel : ViewModelBase
    {
        private bool guest;
        private bool isValid;
        private string password;
        private string serverUrl;
        private string username;

        public string ServerUrl
        {
            get { return serverUrl; }
            set
            {
                if (value != serverUrl)
                {
                    serverUrl = value;
                    OnPropertyChanged("ServerUrl");
                    UpdateValid();
                }
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                if (value != username)
                {
                    username = value;
                    OnPropertyChanged("Username");
                    UpdateValid();
                }
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (value != password)
                {
                    password = value;
                    OnPropertyChanged("Password");
                    UpdateValid();
                }
            }
        }

        public bool Guest
        {
            get { return guest; }
            set
            {
                if (value != guest)
                {
                    guest = value;
                    OnPropertyChanged("Guest");
                    UpdateValid();
                }
            }
        }

        [XmlIgnore]
        public bool IsValid
        {
            get { return isValid; }
            private set
            {
                if (value != isValid)
                {
                    isValid = value;
                    OnPropertyChanged("IsValid");
                }
            }
        }

        private void UpdateValid()
        {
            Uri o;
            if (!Uri.TryCreate(serverUrl, UriKind.RelativeOrAbsolute, out o))
            {
                IsValid = false;
                return;
            }
            if (guest)
            {
                IsValid = true;
                return;
            }
            if (string.IsNullOrEmpty(username))
            {
                IsValid = false;
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                IsValid = false;
                return;
            }
            IsValid = true;
        }

        public TeamCityClient CreateClient()
        {
            var client = new TeamCityClient(serverUrl);
            client.Connect(username, password, guest);
            return client;
        }
    }
}
