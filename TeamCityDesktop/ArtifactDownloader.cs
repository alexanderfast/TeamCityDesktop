using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TeamCityDesktop.Background;
using TeamCityDesktop.Model;
using TeamCityDesktop.ViewModel;
using TeamCitySharp;

namespace TeamCityDesktop
{
    public class ArtifactDownloader : IBackgroundTask
    {
        public event ProgressChangedEventHandler ProgressChanged;
        public event TaskCompletedEventHandler TaskCompleted;

        private readonly ArtifactModel[] artifacts;
        private readonly TeamCityClient client;
        private readonly string targetPath;
        private volatile bool cancel;

        public ArtifactDownloader(TeamCityClient client, string targetPath, IEnumerable<ArtifactModel> artifacts)
        {
            this.client = client;
            this.targetPath = targetPath;
            this.artifacts = artifacts.ToArray();
        }

        public void Cancel()
        {
            cancel = true;
        }

        public void RunSynchronously()
        {
            try
            {
                for (int i = 0; i < artifacts.Length; i++)
                {
                    if (cancel)
                    {
                        OnTaskCompleted(null, true);
                        return;
                    }

                    ArtifactModel artifact = artifacts[i];
                    string filename = ArtifactUrlToFilename(artifact.DownloadUrl);

                    double progress = (i / (double)artifacts.Length) * 100;
                    OnProgressChanged((int)progress, filename);

                    string fullPath = Path.Combine(targetPath, filename);
                    if (!File.Exists(fullPath))
                    {
                        client.DownloadArtifact(artifact.DownloadUrl, s => MoveFile(s, fullPath));
                    }
                }
                OnTaskCompleted(null, false);
            }
            catch (Exception e)
            {
                OnTaskCompleted(e, false);
            }
        }

        private void OnProgressChanged(int progressPercentage, object userState)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new ProgressChangedEventArgs(progressPercentage, userState));
            }
        }

        private void OnTaskCompleted(Exception exception, bool cancelled)
        {
            if (TaskCompleted != null)
            {
                TaskCompleted(this, new TaskCompletedEventArgs(exception, cancelled));
            }
        }

        private static string ArtifactUrlToFilename(string artifactUrl)
        {
            return string.Join(
                Path.DirectorySeparatorChar.ToString(),
                artifactUrl.Split(new[] {'/'}, 6).Last().Split(new[] {'/'}));
        }

        private static void MoveFile(string source, string destination)
        {
            string directoryName = Path.GetDirectoryName(destination);
            if (directoryName != null && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            File.Move(source, destination);
        }
    }
}
