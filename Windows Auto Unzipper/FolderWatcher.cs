using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows_Auto_Unzipper.Properties;

namespace Windows_Auto_Unzipper
{
    class FolderWatcher : IDisposable
    {
        private UnzipperContext context;
        private FileSystemWatcher watcher;

        public FolderWatcher(UnzipperContext context)
        {
            this.context = context;
            this.watcher = new FileSystemWatcher(@context.GetTargetFolder());
            this.watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;

            this.watcher.Created += this.OnCreated;
            this.watcher.Error += OnError;

            this.watcher.Filter = "*.zip";
            this.watcher.IncludeSubdirectories = false;
        }

        public void SetTargetDirectory(String targetDirectory)
        {
            this.watcher.Path = targetDirectory;
        }

        public bool Start()
        {
            if (!String.IsNullOrEmpty(this.watcher.Path))
            {
                this.watcher.EnableRaisingEvents = true;
                Settings.Default.LastRunningMode = "Running";
                Settings.Default.Save();
                return true;
            }
            return false;
        }

        public void Stop()
        {
            this.watcher.EnableRaisingEvents = false;
            Settings.Default.LastRunningMode = "Stopped";
            Settings.Default.Save();
        }

        public bool IsRunning()
        {
            return !String.IsNullOrEmpty(this.watcher.Path) && this.watcher.EnableRaisingEvents;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            String extractDir = this.context.GetTargetFolder() + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.context.GetTargetFolder() + "\\" + e.Name);
            Task unzipTask = Task.Run(() => Unzipper.UnzipAndDelete(e.FullPath, extractDir));
        }


        private static void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Debug.WriteLine($"Message: {ex.Message}");
                Debug.WriteLine("Stacktrace:");
                Debug.WriteLine(ex.StackTrace);
                Debug.WriteLine("");
                PrintException(ex.InnerException);
            }
        }

        public void Dispose()
        {
            this.watcher.Dispose();
        }
    }
}

