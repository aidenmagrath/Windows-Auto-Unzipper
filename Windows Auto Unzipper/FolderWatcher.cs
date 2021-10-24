﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows_Auto_Unzipper.Properties;

namespace Windows_Auto_Unzipper
{
    class FolderWatcher : IDisposable
    {
        private UnzipperContext context;
        private FileSystemWatcher watcher;
        private bool isWatching = false;

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

            this.watcher.Created += OnCreated;
            this.watcher.Error += OnError;

            this.watcher.Filter = "*.zip";
            this.watcher.IncludeSubdirectories = false;
        }

        public void SetTargetFolder(String targetFolder) {
            this.watcher.Path = @targetFolder;
        }

        public bool Start() {
            if (!String.IsNullOrEmpty(this.watcher.Path)) {
                this.watcher.EnableRaisingEvents = true;
                Settings.Default.LastRunningMode = "Running";
                Settings.Default.Save();
                return true;
            }
            return false;
        }

        public void Stop() {
            this.watcher.EnableRaisingEvents = false;
            Settings.Default.LastRunningMode = "Stopped";
            Settings.Default.Save();
        }

        public bool IsRunning() {
            return !String.IsNullOrEmpty(this.watcher.Path) && this.watcher.EnableRaisingEvents;
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            //Can possibly use e.Directory?
            string value = $"Created: {e.FullPath}";
            Debug.WriteLine(value);
            String extractDir = this.context.GetTargetFolder() + "\\" + System.IO.Path.GetFileNameWithoutExtension(this.context.GetTargetFolder() + "\\" + e.Name);
            Debug.WriteLine("ExtractDir: " + extractDir);

            Task unzipTask = Task.Run(() => Unzipper.UnzipAndDelete(e.FullPath, extractDir));
        }


        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

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
            watcher.Dispose();
        }
    }
}

