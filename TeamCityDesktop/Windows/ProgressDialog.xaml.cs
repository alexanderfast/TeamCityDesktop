using System;
using System.ComponentModel;
using System.Windows;

namespace TeamCityDesktop.Windows
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog : Window, INotifyPropertyChanged
    {
        private readonly BackgroundWorker worker;
        private double progressPercentage;
        private object userState;

        public ProgressDialog(BackgroundWorker worker = null)
        {
            this.worker = worker;
            if (worker != null)
            {
                worker.ProgressChanged += WorkerProgressChanged;
                worker.RunWorkerCompleted += WorkerCompleted;
            }
            DataContext = this;
            InitializeComponent();
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            worker.ProgressChanged -= WorkerProgressChanged;
            worker.RunWorkerCompleted -= WorkerCompleted;
        }

        private void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke((Action)Close);
            }
            else
            {
                Close();
            }
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressPercentage = e.ProgressPercentage;
            UserState = e.UserState;
        }

        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
