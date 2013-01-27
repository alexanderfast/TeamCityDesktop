using System;
using System.Threading;
using System.Xml.Serialization;
using TeamCitySharp;

namespace TeamCityDesktop.ViewModel
{
    /// <summary>
    /// This view model wraps a TeamCityClient and handles async updates.
    /// </summary>
    public abstract class ClientResponseViewModel : ViewModelBase, IDisposable
    {
        private TeamCityClient client;
        private bool isLoading;
        private Thread thread;

        protected ClientResponseViewModel(TeamCityClient client = null)
        {
            this.client = client;
        }

        [XmlIgnore]
        public TeamCityClient Client
        {
            get { return client; }
            set
            {
                if (value != client)
                {
                    client = value;
                    OnPropertyChanged("Client");
                }
            }
        }

        [XmlIgnore]
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (value != isLoading)
                {
                    isLoading = value;
                    OnPropertyChanged("IsLoading");
                }
            }
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            if (thread != null)
            {
                thread.Abort();
                thread.Join();
                thread = null;
            }
        }

        #endregion

        public void RefreshAsync()
        {
            if (thread != null)
            {
                return; // already updating
            }
            thread = new Thread(Refresh);
            thread.Start();
        }

        protected abstract void Refresh();
    }
}
