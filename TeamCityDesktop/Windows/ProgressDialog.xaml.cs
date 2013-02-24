using System;
using System.ComponentModel;
using System.Windows;
using TeamCityDesktop.ViewModel;

namespace TeamCityDesktop.Windows
{
    public sealed partial class ProgressDialog : Window
    {
        public ProgressDialog()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = e.OldValue as ProgressDialogViewModel;
            if (viewModel != null)
            {
                viewModel.RequestClose -= HandleCloseRequest;
                viewModel.Dispose();
            }
            viewModel = e.NewValue as ProgressDialogViewModel;
            if (viewModel != null)
            {
                viewModel.RequestClose += HandleCloseRequest;
            }
        }

        private void HandleCloseRequest(object sender, EventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke((Action)(() => HandleCloseRequest(sender, e)));
            }
            else
            {
                DataContext = null;
                Close();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var viewModel = DataContext as ProgressDialogViewModel;
            if (viewModel == null)
            {
                // without a view model close as normal
                base.OnClosing(e);
            }
            else
            {
                // otherwise request the view model to cancel and abort closing
                viewModel.CancelTask();
                e.Cancel = true;
            }
        }
    }
}
