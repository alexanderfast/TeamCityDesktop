using System;
using System.Collections.Generic;
using System.Windows;
using TeamCitySharp;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class BuildConfigsViewModel : ClientResponseViewModel
    {
        private readonly TeamCityClient client;
        private List<BuildConfig> buildConfigs;

        public BuildConfigsViewModel(TeamCityClient client)
        {
            this.client = client;
        }

        public List<BuildConfig> BuildConfigs
        {
            get { return buildConfigs; }
            set
            {
                if (value != buildConfigs)
                {
                    buildConfigs = value;
                    OnPropertyChanged("BuildConfigs");
                }
            }
        }

        protected override void Refresh()
        {
            IsLoading = true;
            try
            {
                BuildConfigs = client.AllBuildConfigs();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name);
            }
            IsLoading = false;
        }
    }
}
