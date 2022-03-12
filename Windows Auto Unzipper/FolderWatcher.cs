using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows_Auto_Unzipper.Properties;

namespace Windows_Auto_Unzipper
{
    /// <summary>
    /// Uses FileSystemWatcher to watch for new zip files added to a specified directory
    /// </summary>
    class FolderWatcher : IDisposable
    {
        private UnzipperContext context;
        private FileSystemWatcher watcher;

        /// <summary>
        /// Initializes the FileSystemWatcher
        /// </summary>
        /// <param name="context">Reference to the applications context</param>
        public FolderWatcher(UnzipperContext context)
        {
            this.context = context;

            //Create the FileSystemWatcher
            this.watcher = new FileSystemWatcher(@context.GetTargetFolder());
            this.watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;

            //Set event handlers for FileSystemWatcger
            this.watcher.Created += this.OnCreated;
            this.watcher.Error += OnError;

            this.watcher.Filter = "*.zip";
            this.watcher.IncludeSubdirectories = false;
        }

        /// <summary>
        /// Set which folder the FileSytemWatcher is watching
        /// </summary>
        /// <param name="targetDirectory">Path to the directory</param>
        public void SetTargetDirectory(String targetDirectory)
        {
            this.watcher.Path = targetDirectory;
        }

        /// <summary>
        /// Starts the FileSystemWatcher if a directory has been set
        /// </summary>
        /// <returns>Returns false if no directory has been set</returns>
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

        /// <summary>
        /// Stop the FileSystemWatcher
        /// </summary>
        public void Stop()
        {
            this.watcher.EnableRaisingEvents = false;
            Settings.Default.LastRunningMode = "Stopped";
            Settings.Default.Save();
        }

        /// <summary>
        /// Check if the FileSystemWatcher is running
        /// </summary>
        /// <returns>Returns true if running</returns>
        public bool IsRunning()
        {
            return !String.IsNullOrEmpty(this.watcher.Path) && this.watcher.EnableRaisingEvents;
        }

        /// <summary>
        /// Event handler that is called when a new zip file is added to the directory that is being watched and extracts it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            //Get the path to the watchedDirectory+file name
            String extractDir = this.context.GetTargetFolder() + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.context.GetTargetFolder() + "\\" + e.Name);
            //Unzip the file on a seperate thread
            Task unzipTask = Task.Run(() => Unzipper.Unzip(e.FullPath, extractDir, Settings.Default.AutoDelete));
        }


        //Event handler that is called when an error occurs with the FileSystemWatcher
        private static void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }

        private static void PrintException(Exception ex)
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

        /// <summary>
        /// Dispose of the FileSystemWatcher
        /// </summary>
        public void Dispose()
        {
            this.watcher.Dispose();
        }
    }
}

