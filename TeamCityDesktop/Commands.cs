using System.Windows.Input;

namespace TeamCityDesktop
{
    public static class Commands
    {
        // Connect to a TeamCity instance.
        public static readonly RoutedCommand Connect = new RoutedCommand();

        // Download artifacts from a selected build
        public static readonly RoutedCommand DownloadArtifacts = new RoutedCommand();

        // Download all artifacts from a selected build
        public static readonly RoutedCommand DownloadAllArtifacts = new RoutedCommand();
    }
}
