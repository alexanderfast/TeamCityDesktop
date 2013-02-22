using System;

using TeamCityDesktop.Model;

namespace TeamCityDesktop.ViewModel
{
    public class ArtifactViewModel : ViewModelBase
    {
        private readonly ArtifactModel model;
        private bool isSelected;

        public ArtifactViewModel(ArtifactModel model)
        {
            this.model = model;
        }

        public bool IsSelected
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

        public string Path
        {
            get { return model.Path; }
        }

        public ArtifactModel Model
        {
            get { return model; }
        }
    }
}
