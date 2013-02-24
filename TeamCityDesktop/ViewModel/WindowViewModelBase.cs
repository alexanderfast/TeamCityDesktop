using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamCityDesktop.ViewModel
{
    class WindowViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Make a request to the view to close the window.
        /// </summary>
        public event EventHandler RequestClose;

        private string title;

        /// <summary>
        /// The title of the window.
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                if (value != title)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        protected virtual void OnRequestClose()
        {
            if (RequestClose != null)
            {
                RequestClose(this, new EventArgs());
            }
        }
    }
}
