using System.Threading;

namespace TeamCityDesktop.DataAccess
{
    public interface IWorker
    {
        bool IsAsync { get; }

        void QueueWork(WaitCallback callback, object state = null);
    }
}
