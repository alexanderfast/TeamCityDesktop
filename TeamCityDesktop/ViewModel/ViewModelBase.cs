using System;
using System.ComponentModel;
using System.Diagnostics;

namespace TeamCityDesktop.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        protected void OnPropertyChanged(string property)
        {
            VerifyPropertyName(property);
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        ~ViewModelBase()
        {
            Dispose(false);
            string msg = string.Format("{0} ({1}) Finalized", GetType().Name, GetHashCode());
            Debug.WriteLine(msg);
        }
    }
}
