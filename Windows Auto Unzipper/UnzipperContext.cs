using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Windows_Auto_Unzipper.Properties;

namespace Windows_Auto_Unzipper
{
    class UnzipperContext : ApplicationContext
    {
        private Form settingsForm;
        private NotifyIcon trayIcon;
        private FolderWatcher folderWatcher;

        private ContextMenuStrip menu;
        private ToolStripMenuItem menuItemToggleRunning;
        private ToolStripMenuItem menuItemSettings;
        private ToolStripMenuItem menuItemExit;

        private string targetDirectory = UserFolders.GetPath(UserFolder.Downloads);

        /// <summary>
        /// Initialize the program
        /// </summary>
        public UnzipperContext()
        {
            //Create the tray icon
            this.trayIcon = new NotifyIcon();
            this.trayIcon.Icon = Windows_Auto_Unzipper.Properties.Resources.icon_128x128;
            this.trayIcon.Text = "Auto Unzipper";
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += (sender, e) => this.ShowSettings();

            //Initialize the target folder the the users downloads folder on first run
            if (String.IsNullOrEmpty(Settings.Default.TargetFolder))
            {
                Settings.Default.TargetFolder = UserFolders.GetPath(UserFolder.Downloads);
                Settings.Default.Save();
            }

            this.SetTargetDirectory(Settings.Default.TargetFolder);

            //Start the folder watcher depending on the start mode
            if (Settings.Default.StartMode == "Running" || (Settings.Default.StartMode == "Remember from last session" && Settings.Default.LastRunningMode == "Running"))
            {
                this.folderWatcher.Start();
            }


            //Setup the right-click  menu
            this.InitializeContextMenu();

            //Enable auto-run based on saved settings
            if (Settings.Default.AutoLaunch)
            {
                RegistryHelper.EnableAutoRun();
            }
            else
            {
                RegistryHelper.DisableAutoRun();
            }
        }

        /// <summary>
        /// Opens the settings window
        /// </summary>
        public void ShowSettings()
        {
            if (Application.OpenForms.OfType<FormSettings>().Any())
            {
                //If windows form has already been created, show it and bring it to the front

                if (this.settingsForm == null)
                {
                    this.settingsForm = Application.OpenForms.OfType<FormSettings>().First();
                }

                this.settingsForm.Show();
                this.settingsForm.Visible = true;
                this.settingsForm.WindowState = FormWindowState.Normal;
                this.settingsForm.BringToFront();

            }
            else
            {
                //Create the windows form for the first time
                this.settingsForm = new FormSettings(this);
                this.settingsForm.Show();
            }
        }

        /// <summary>
        /// Event handler for when the right-click system tray menu is opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menu_Openining(Object sender, CancelEventArgs e)
        {
            if (this.folderWatcher.IsRunning())
            {
                this.menuItemToggleRunning.Text = "Stop";
            }
            else
            {
                this.menuItemToggleRunning.Text = "Start";
            }
        }

        /// <summary>
        /// Event handler for when the Running/Stopped menu item in the right-click menu is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemToggleRunning_Click(Object sender, EventArgs e)
        {
            if (this.folderWatcher.IsRunning())
            {
                this.folderWatcher.Stop();
                this.menuItemToggleRunning.Text = "Start";
                Settings.Default.LastRunningMode = "Stopped";
            }
            else if (this.folderWatcher.Start())
            {
                this.menuItemToggleRunning.Text = "Stop";
                Settings.Default.LastRunningMode = "Running";
            }
            Settings.Default.Save();
        }

        /// <summary>
        /// Sets the directory that will be watched for new zip files
        /// </summary>
        /// <param name="location"></param>
        public void SetTargetDirectory(String location)
        {
            this.targetDirectory = location;

            if (this.folderWatcher != null)
            {
                this.folderWatcher.SetTargetDirectory(location);
            }
            else
            {
                this.folderWatcher = new FolderWatcher(this);
            }
        }

        /// <summary>
        /// Get the path to the directory that the file watcher is watching for new zip files
        /// </summary>
        /// <returns></returns>
        public String GetTargetFolder()
        {
            return this.targetDirectory;
        }

        // Close/stop the program
        public void Close()
        {
            this.folderWatcher.Dispose();
            this.trayIcon.Dispose();
            Application.Exit();
        }

       /// <summary>
       /// Event handler that is called when the application exits. Used to cleanly dispose disposables
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (this.folderWatcher != null)
            {
                this.folderWatcher.Dispose();
            }
        }

        /// <summary>
        /// Initialize the system tray right-click menu
        /// </summary>
        private void InitializeContextMenu()
        {
            //Create menu
            this.menu = new ContextMenuStrip();
            this.menu.Opening += new CancelEventHandler(this.menu_Openining);
            this.menu.SuspendLayout();

            //Settings button
            this.menuItemSettings = new ToolStripMenuItem();
            this.menuItemSettings.Name = "menuItemSettings";
            this.menuItemSettings.Text = "Settings";
            this.menuItemSettings.Click += (sender, e) => this.ShowSettings();

            //Exit button
            this.menuItemExit = new ToolStripMenuItem();
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += (sender, e) => this.Close();

            //Running/Stopped button
            this.menuItemToggleRunning = new ToolStripMenuItem();
            this.menuItemToggleRunning.Name = "menuItemToggleRunnning";
            this.menuItemToggleRunning.Click += new EventHandler(this.menuItemToggleRunning_Click);
            if (this.folderWatcher.IsRunning())
            {
                this.menuItemToggleRunning.Text = "Stop";
            }
            else
            {
                this.menuItemToggleRunning.Text = "Start";
            }

            //Add the menu items to the menu
            this.menu.Items.AddRange(new ToolStripItem[] { this.menuItemToggleRunning, this.menuItemSettings, this.menuItemExit });

            this.menu.ResumeLayout(false);
            this.trayIcon.ContextMenuStrip = this.menu;

        }

    }
}
