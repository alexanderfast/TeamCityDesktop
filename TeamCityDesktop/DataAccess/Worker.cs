using System.Threading;

namespace TeamCityDesktop.DataAccess
{
    /// <summary>
    /// A wrapper around the <see cref="ThreadPool"/> to allow either
    /// synchronous or asynchronous transparently.
    /// </summary>
    public class Worker : IWorker
    {
        public bool IsAsync { get; set; }

        public void QueueWork(WaitCallback callback, object state = null)
        {
            if (IsAsync)
            {
                ThreadPool.QueueUserWorkItem(callback, state);
            }
            else
            {
                callback(state);
            }
        }
    }
}
