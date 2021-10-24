using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            InitializeComponent();

        }

        private void LoadSettings() {
            labelTargetFolder.Text = this.context.GetTargetFolder();
            comboBoxStartMode.Text = Settings.Default.StartMode;
            checkBoxAutoLaunch.Checked = Settings.Default.AutoLaunch;
        }
        private void Form1_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            LoadSettings();
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            Hide();
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void Form1_Load(Object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void btnChangeFolder_Click(Object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                labelTargetFolder.Text = folderBrowserDialog1.SelectedPath;
                toolTip1.SetToolTip(labelTargetFolder, folderBrowserDialog1.SelectedPath);
            }
        }

        private void btnDone_Click(Object sender, EventArgs e)
        {
            //Save changes to target location
            Settings.Default.TargetFolder = labelTargetFolder.Text;
            context.SetTargetFolder(labelTargetFolder.Text);

            //Save changes to start mode
            String selected = comboBoxStartMode.Items[comboBoxStartMode.SelectedIndex].ToString();
            Settings.Default.StartMode = selected;

            //Save changes to auto launch option
            bool autoLaunch = checkBoxAutoLaunch.Checked;
            //Update registry if autolaunch option changed
            if (autoLaunch != Settings.Default.AutoLaunch) {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (checkBoxAutoLaunch.Checked)
                {
                    rkApp.SetValue(Application.ProductName, Application.ExecutablePath);
                }
                else
                {
                    rkApp.DeleteValue(Application.ProductName, false);
                }
            }
            Settings.Default.AutoLaunch = checkBoxAutoLaunch.Checked;
            
            Settings.Default.Save();

            Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnCancel_Click(Object sender, EventArgs e)
        {
            Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        private void FormSettings_VisibleChanged(Object sender, EventArgs e)
        {
            if (this.Visible) {
                LoadSettings();
            }
        }
    }
}
