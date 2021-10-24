using Microsoft.Win32;
using System;
using System.Windows.Forms;
using Windows_Auto_Unzipper.Properties;

namespace Windows_Auto_Unzipper
{
    public partial class FormSettings : System.Windows.Forms.Form
    {
        private UnzipperContext context;

        public FormSettings(ApplicationContext context)
        {
            this.context = (UnzipperContext)context;
            this.InitializeComponent();

        }

        private void LoadSettings()
        {
            this.labelTargetFolder.Text = this.context.GetTargetFolder();
            this.comboBoxStartMode.Text = Settings.Default.StartMode;
            this.checkBoxAutoLaunch.Checked = Settings.Default.AutoLaunch;
        }
        private void Form1_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            this.LoadSettings();
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void Form1_Load(Object sender, EventArgs e)
        {
            this.LoadSettings();
        }

        private void btnChangeFolder_Click(Object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.labelTargetFolder.Text = this.folderBrowserDialog1.SelectedPath;
                this.toolTip1.SetToolTip(this.labelTargetFolder, this.folderBrowserDialog1.SelectedPath);
            }
        }

        private void btnDone_Click(Object sender, EventArgs e)
        {
            //Save changes to target location
            Settings.Default.TargetFolder = this.labelTargetFolder.Text;
            this.context.SetTargetFolder(this.labelTargetFolder.Text);

            //Save changes to start mode
            String selected = this.comboBoxStartMode.Items[this.comboBoxStartMode.SelectedIndex].ToString();
            Settings.Default.StartMode = selected;

            //Save changes to auto launch option
            bool autoLaunch = this.checkBoxAutoLaunch.Checked;
            //Update registry if autolaunch option changed
            if (autoLaunch != Settings.Default.AutoLaunch)
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (this.checkBoxAutoLaunch.Checked)
                {
                    rkApp.SetValue(Application.ProductName, Application.ExecutablePath);
                }
                else
                {
                    rkApp.DeleteValue(Application.ProductName, false);
                }
            }
            Settings.Default.AutoLaunch = this.checkBoxAutoLaunch.Checked;

            Settings.Default.Save();

            this.Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnCancel_Click(Object sender, EventArgs e)
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormSettings_VisibleChanged(Object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.LoadSettings();
            }
        }
    }
}
