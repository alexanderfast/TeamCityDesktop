using System;
using System.ComponentModel;
using TeamCityDesktop.Background;

namespace TeamCityDesktop.ViewModel
{
    internal class ProgressDialogViewModel : WindowViewModelBase
    {
        private readonly IBackgroundTask backgroundTask;
        private bool disposed;
        private double progressPercentage;
        private object userState;

        public ProgressDialogViewModel(IBackgroundTask backgroundTask)
        {
            if (backgroundTask == null)
            {
                throw new ArgumentNullException("backgroundTask");
            }
            this.backgroundTask = backgroundTask;
            backgroundTask.ProgressChanged += WorkerProgressChanged;
            backgroundTask.TaskCompleted += WorkerCompleted;
        }

        public double ProgressPercentage
        {
            get { return progressPercentage; }
            set
            {
                if (value != progressPercentage)
                {
                    progressPercentage = value;
                    OnPropertyChanged("ProgressPercentage");
                }
            }
        }

        public object UserState
        {
            get { return userState; }
            set
            {
                if (value != userState)
                {
                    userState = value;
                    OnPropertyChanged("UserState");
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    backgroundTask.ProgressChanged -= WorkerProgressChanged;
                    backgroundTask.TaskCompleted -= WorkerCompleted;
                }
                disposed = true;
                base.Dispose(disposing);
            }
        }

        public void CancelTask()
        {
            Title = "Cancelling...";
            backgroundTask.Cancel();
        }

        private void WorkerCompleted(object sender, TaskCompletedEventArgs eventArgs)
        {
            OnRequestClose();
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs eventArgs)
        {
            ProgressPercentage = eventArgs.ProgressPercentage;
            UserState = eventArgs.UserState;
        }
    }
}
