using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TeamCityDesktop.ViewModel
{
    public class ArtifactCollectionViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ArtifactViewModel> artifacts =
            new ObservableCollection<ArtifactViewModel>();

        private List<string> artifactUrls = new List<string>();
        private bool isEmpty;

        /// <summary>
        /// Not all builds publish artifacts.
        /// This property serves the purpose of being able to serialize an empty collection.
        /// That way an empty collection can be cached so the server isn't spammed with requests.
        /// </summary>
        public bool IsEmpty
        {
            get { return isEmpty; }
            set
            {
                if (value != isEmpty)
                {
                    isEmpty = value;
                    OnPropertyChanged("IsEmpty");
                }
            }
        }

        public List<string> ArtifactUrls
        {
            get { return artifactUrls; }
            set
            {
                if (value != artifactUrls)
                {
                    artifactUrls = value;
                    OnPropertyChanged("ArtifactUrls");
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<ArtifactViewModel> Artifacts
        {
            get { return artifacts; }
        }

        internal void WrapModels()
        {
            foreach (string url in artifactUrls)
            {
                artifacts.Add(new ArtifactViewModel(url));
            }
        }
    }
}
