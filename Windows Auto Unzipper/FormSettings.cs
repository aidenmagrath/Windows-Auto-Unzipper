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



        private void Form1_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
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
            labelTargetFolder.Text = this.context.GetTargetFolder();
            comboBoxStartMode.Text = Settings.Default.StartMode;
            checkBoxAutoLaunch.Checked = Settings.Default.AutoLaunch;
        }

        private void btnChangeFolder_Click(Object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                String location = folderBrowserDialog1.SelectedPath;
                labelTargetFolder.Text = location;
                toolTip1.SetToolTip(labelTargetFolder, location);
                Settings.Default.TargetFolder = location;
                Settings.Default.Save();
                context.SetTargetFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        private void comboBoxStartMode_SelectedIndexChanged(Object sender, EventArgs e)
        {
            String selected = comboBoxStartMode.Items[comboBoxStartMode.SelectedIndex].ToString();
            Settings.Default.StartMode = selected;
            Settings.Default.Save();
        }

        private void checkBoxAutoLaunch_CheckedChanged(Object sender, EventArgs e)
        {
            Settings.Default.AutoLaunch = checkBoxAutoLaunch.Checked;
            Settings.Default.Save();
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
    }
}
