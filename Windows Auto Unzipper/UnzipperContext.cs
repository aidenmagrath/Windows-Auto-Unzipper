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

        public UnzipperContext()
        {
            this.trayIcon = new NotifyIcon();
            this.trayIcon.Icon = Windows_Auto_Unzipper.Properties.Resources.icon;
            this.trayIcon.Visible = true;
            this.trayIcon.DoubleClick += (sender, e) => this.ShowSettings();

            if (String.IsNullOrEmpty(Settings.Default.TargetFolder))
            {
                Settings.Default.TargetFolder = UserFolders.GetPath(UserFolder.Downloads);
                Settings.Default.Save();
            }

            this.SetTargetDirectory(Settings.Default.TargetFolder);

            if (Settings.Default.StartMode == "Running" || (Settings.Default.StartMode == "Remember from last session" && Settings.Default.LastRunningMode == "Running"))
            {
                this.folderWatcher.Start();
            }


            this.InitializeContextMenu();

            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (Settings.Default.AutoLaunch)
            {
                if (rkApp.GetValue(Application.ProductName) == null)
                {
                    rkApp.SetValue(Application.ProductName, Application.ExecutablePath);
                }
            }
            else
            {
                if (rkApp.GetValue(Application.ProductName) != null)
                {
                    rkApp.DeleteValue(Application.ProductName, false);
                }
            }
        }

        public void ShowSettings()
        {
            if (Application.OpenForms.OfType<FormSettings>().Any())
            {
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
                this.settingsForm = new FormSettings(this);
                this.settingsForm.Show();
            }
        }


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

        public void Close()
        {
            this.folderWatcher.Dispose();
            this.trayIcon.Dispose();
            Application.Exit();
        }

        public String GetTargetFolder()
        {
            return this.targetDirectory;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (this.folderWatcher != null)
            {
                this.folderWatcher.Dispose();
            }
        }

        private void InitializeContextMenu()
        {
            this.menu = new ContextMenuStrip();
            this.menu.Opening += new CancelEventHandler(this.menu_Openining);
            this.menu.SuspendLayout();

            this.menuItemSettings = new ToolStripMenuItem();
            this.menuItemSettings.Name = "menuItemSettings";
            this.menuItemSettings.Text = "Settings";
            this.menuItemSettings.Click += (sender, e) => this.ShowSettings();

            this.menuItemExit = new ToolStripMenuItem();
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += (sender, e) => this.Close();

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

            this.menu.Items.AddRange(new ToolStripItem[] { this.menuItemToggleRunning, this.menuItemSettings, this.menuItemExit });

            this.menu.ResumeLayout(false);
            this.trayIcon.ContextMenuStrip = this.menu;

        }

    }
}
