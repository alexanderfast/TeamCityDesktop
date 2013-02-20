using System.Windows.Controls;
using TeamCityDesktop.Model;

namespace TeamCityDesktop.Controls
{
    public sealed partial class Login : UserControl
    {
        public Login(ServerCredentialsModel serverCredentials = null)
        {
            ServerCredentials = serverCredentials ?? new ServerCredentialsModel();
            DataContext = ServerCredentials;
            InitializeComponent();
        }

        public ServerCredentialsModel ServerCredentials { get; private set; }
    }
}
