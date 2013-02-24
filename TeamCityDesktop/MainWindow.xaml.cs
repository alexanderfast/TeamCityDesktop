using System.Windows;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }
    }
}
