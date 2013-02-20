using System;
using TeamCityDesktop.ViewModel;
using TeamCitySharp;

namespace TeamCityDesktop.Model
{
    public class ServerCredentialsModel : ViewModelBase
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Guest { get; set; }

        public bool IsValid()
        {
            Uri o;
            if (!Uri.TryCreate(Url, UriKind.RelativeOrAbsolute, out o))
            {
                return false;
            }
            if (Guest)
            {
                return true;
            }
            if (string.IsNullOrEmpty(Username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }
            return true;
        }

        public TeamCityClient CreateClient()
        {
            var client = new TeamCityClient(Url);
            client.Connect(Username, Password, Guest);
            return client;
        }
    }
}
