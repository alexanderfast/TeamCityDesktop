using System.ComponentModel;

namespace TeamCityDesktop.Background
{
    public interface IBackgroundTask
    {
        event ProgressChangedEventHandler ProgressChanged;
        event TaskCompletedEventHandler TaskCompleted;
        void Cancel();
        void RunSynchronously();
    }
}
