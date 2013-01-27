using System.Collections.ObjectModel;

namespace TeamCityDesktop.ViewModel
{
    public abstract class TreeViewModelBase : ClientResponseViewModel
    {
        private readonly ObservableCollection<ViewModelBase> children =
            new ObservableCollection<ViewModelBase>();

        private bool isExpanded;
        private bool isSelected;

        public virtual bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        public virtual bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public ObservableCollection<ViewModelBase> Children
        {
            get { return children; }
        }
    }
}
