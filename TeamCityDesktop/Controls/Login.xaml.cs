using System.Windows.Controls;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop.Controls
{
    public partial class Login : UserControl
    {
        public Login(LoginViewModel viewModel = null)
        {
            ViewModel = viewModel ?? new LoginViewModel();
            DataContext = ViewModel;
            InitializeComponent();
        }

        public LoginViewModel ViewModel { get; private set; }
    }
}
