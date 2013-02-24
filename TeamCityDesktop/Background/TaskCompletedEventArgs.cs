using System;

namespace TeamCityDesktop.Background
{
    public class TaskCompletedEventArgs
    {
        public TaskCompletedEventArgs(Exception error, bool cancelled)
        {
            Error = error;
            Cancelled = cancelled;
        }

        public Exception Error { get; private set; }

        public bool Cancelled { get; private set; }
    }
}
