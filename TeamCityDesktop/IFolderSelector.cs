using System.ComponentModel;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop
{
    public interface IFolderSelector : INotifyPropertyChanged
    {
        CommandViewModel SelectFolder { get; }

        string SelectedFolder { get; }
    }
}
