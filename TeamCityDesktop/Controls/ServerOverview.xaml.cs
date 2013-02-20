using System.Windows.Controls;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop.Controls
{
    public sealed partial class ServerOverview : UserControl
    {
        public ServerOverview(ServerOverviewViewModel viewModel = null)
        {
            DataContext = viewModel ?? new ServerOverviewViewModel();
            InitializeComponent();
        }
    }
}
