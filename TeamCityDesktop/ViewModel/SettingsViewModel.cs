using System.Collections.ObjectModel;
using System.IO;

namespace TeamCityDesktop.ViewModel
{
    public class SettingsViewModel : SerializableViewModel<SettingsViewModel>
    {
        private const string DefaultFileName = "Settings.xml";

        public SettingsViewModel()
        {
            Servers = new ObservableCollection<ServerViewModel>();
        }

        public ObservableCollection<ServerViewModel> Servers { get; set; }

        /// <summary>
        /// Creates an empty settings file if none exist.
        /// </summary>
        public static SettingsViewModel Load()
        {
            if (!File.Exists(DefaultFileName))
            {
                Save(new SettingsViewModel(), DefaultFileName);
            }
            return Load(DefaultFileName);
        }

        public void Save()
        {
            Save(this, DefaultFileName);
        }
    }
}
