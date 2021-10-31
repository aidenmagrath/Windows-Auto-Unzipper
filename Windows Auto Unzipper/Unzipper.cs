using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Threading;

namespace Windows_Auto_Unzipper
{
    /// <summary>
    /// Used to unzip/extract zip files
    /// </summary>
    class Unzipper
    {
        /// <summary>
        /// Unzips/extracts a zip file to a directory and optionally deletes the zip file
        /// </summary>
        /// <param name="fullPath">The location of the source archive</param>
        /// <param name="extractDir">The location the archive will be extracted to</param>
        /// <param name="deleteWhenDone">Should the source archive be deleted after it has been unzipped</param>
        /// <returns>Returns true if successful</returns>
        public static bool Unzip(string fullPath, string extractDir, bool deleteWhenDone)
        {
            if (IsFileClosed(fullPath, true))
            {
                Directory.CreateDirectory(extractDir);
                ZipFile.ExtractToDirectory(fullPath, extractDir);
                
                if (deleteWhenDone) {
                    File.Delete(fullPath);
                }
                
                RefreshWindowsExplorer();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if a file is closed
        /// </summary>
        /// <param name="filepath">The path of the file to check</param>
        /// <param name="wait">If true, wait for a short delay and try again</param>
        /// <returns>Returns true if file is closed</returns>
        public static bool IsFileClosed(string filepath, bool wait)
        {
            bool fileClosed = false;
            int retries = 20;
            const int delay = 500; // Max time spent here = retries*delay milliseconds

            if (!File.Exists(filepath))
                return false;

            do
            {
                try
                {
                    // Attempts to open then close the file in RW mode, denying other users to place any locks.
                    FileStream fs = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    fs.Close();
                    fileClosed = true; // success
                }
                catch (IOException) { }

                if (!wait) break;

                retries--;

                if (!fileClosed)
                    Thread.Sleep(delay);
            }
            while (!fileClosed && retries > 0);

            return fileClosed;
        }

        /// <summary>
        /// Refresh any open windows explorer windows to show changes in directory
        /// </summary>
        private static void RefreshWindowsExplorer()
        {
            SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
        
    }
}
