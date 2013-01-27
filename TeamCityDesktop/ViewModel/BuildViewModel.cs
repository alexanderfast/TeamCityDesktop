using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TeamCitySharp.DomainEntities;

namespace TeamCityDesktop.ViewModel
{
    public class BuildViewModel : TreeViewModelBase
    {
        private ArtifactCollectionViewModel artifacts;
        private Build build;
        private bool refreshed;

        public Build Build
        {
            get { return build; }
            set
            {
                if (value != build)
                {
                    build = value;
                    OnPropertyChanged("Build");
                }
            }
        }

        public ArtifactCollectionViewModel Artifacts
        {
            get { return artifacts; }
            set
            {
                if (value != artifacts)
                {
                    artifacts = value;
                    OnPropertyChanged("Artifacts");
                }
            }
        }

        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                if (value != base.IsSelected)
                {
                    base.IsSelected = value;
                    if (!refreshed)
                    {
                        refreshed = true;
                        RefreshAsync();
                    }
                }
            }
        }

        protected override void Refresh()
        {
            IsLoading = true;
            try
            {
                ArtifactCollectionViewModel cached = new Cache(build).Load();
                List<string> urls = cached.ArtifactUrls;
                if (!cached.IsEmpty && urls.Count == 0)
                {
                    urls = Client.ArtifactsByBuildConfigIdAndBuildNumber(build.BuildTypeId, build.Number);
                    if (urls.Count == 0)
                    {
                        new Cache(build).Save();
                    }
                }
                if (urls.Count > 0)
                {
                    Application.Current.Dispatcher.BeginInvoke(
                        (Action)(() =>
                            {
                                cached = new ArtifactCollectionViewModel {ArtifactUrls = urls};
                                cached.WrapModels();
                                new Cache(build, cached).Save();
                                Artifacts = cached;
                                CommandManager.InvalidateRequerySuggested();
                            }));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.GetType().Name);
            }
            IsLoading = false;
        }

        #region Nested type: Cache

        private class Cache : GenericCache<ArtifactCollectionViewModel>
        {
            private static readonly ArtifactCollectionViewModel EmptyArtifacts =
                new ArtifactCollectionViewModel {IsEmpty = true};

            public Cache(Build parent, ArtifactCollectionViewModel artifacts = null)
                : base(
                    Path.Combine("Cache", string.Format("{0}_{1}_artifacts.xml", parent.BuildTypeId, parent.Number)),
                    artifacts ?? EmptyArtifacts)
            {
            }
        }

        #endregion
    }
}
