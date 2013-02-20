using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TeamCityDesktop.Model;
using TeamCityDesktop.ViewModel;
using TeamCitySharp;

namespace TeamCityDesktop
{
    public class ArtifactDownloader : BackgroundWorker
    {
        private readonly List<ArtifactModel> artifacts;
        private readonly TeamCityClient client;
        private readonly string targetPath;

        public ArtifactDownloader(TeamCityClient client, string targetPath, IEnumerable<ArtifactModel> artifacts)
        {
            this.client = client;
            this.targetPath = targetPath;
            this.artifacts = new List<ArtifactModel>(artifacts);
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = false;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            base.OnDoWork(e);

            for (int i = 0; i < artifacts.Count; i++)
            {
                ArtifactModel artifact = artifacts[i];
                string filename = ArtifactUrlToFilename(artifact.DownloadUrl);

                double progress = (i / (double)artifacts.Count) * 100;
                OnProgressChanged(new ProgressChangedEventArgs((int)progress, filename));

                string fullPath = Path.Combine(targetPath, filename);
                if (!File.Exists(fullPath))
                {
                    client.DownloadArtifact(artifact.DownloadUrl, s => MoveFile(s, fullPath));
                }
            }
            OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(null, null, false));
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
