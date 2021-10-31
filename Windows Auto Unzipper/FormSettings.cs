using Microsoft.Win32;
using System;
using System.Windows.Forms;
using Windows_Auto_Unzipper.Properties;

namespace Windows_Auto_Unzipper
{
    /// <summary>
    /// Manages the settings form/window
    /// </summary>
    public partial class FormSettings : System.Windows.Forms.Form
    {
        private UnzipperContext context;

        public FormSettings(ApplicationContext context)
        {
            this.context = (UnzipperContext)context;
            this.InitializeComponent();

        }

        /// <summary>
        /// Loads all saved settings
        /// </summary>
        private void LoadSettings()
        {
            this.labelTargetDirectory.Text = this.context.GetTargetFolder();
            this.toolTipTargetDirectory.SetToolTip(this.labelTargetDirectory, this.labelTargetDirectory.Text);
            this.comboBoxStartMode.Text = Settings.Default.StartMode;
            this.checkBoxAutoLaunch.Checked = Settings.Default.AutoLaunch;
            this.checkBoxAutoDelete.Checked = Settings.Default.AutoDelete;
            
        }

        /// <summary>
        /// Interupts the form resize event and minimizes it to the system try instead.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Resize(Object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        /// <summary>
        /// Opens the settings window when the system tray icon is double clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            this.LoadSettings();
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// Interupts the window/program close event and minimize it to the system tray insted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(Object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        /// <summary>
        /// Load saved settings when the form is being loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(Object sender, EventArgs e)
        {
            this.LoadSettings();
        }


        private void btnChangeDirectory_Click(Object sender, EventArgs e)
        {
            if (this.directoryBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                this.labelTargetDirectory.Text = this.directoryBrowserDialog.SelectedPath;
                this.toolTipTargetDirectory.SetToolTip(this.labelTargetDirectory, this.directoryBrowserDialog.SelectedPath);
            }
        }

        /// <summary>
        /// Save the chosen settings when the done button is clicked, and close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDone_Click(Object sender, EventArgs e)
        {
            //Save changes to target location
            Settings.Default.TargetFolder = this.labelTargetDirectory.Text;
            this.context.SetTargetDirectory(this.labelTargetDirectory.Text);

            //Save changes to start mode
            String selected = this.comboBoxStartMode.Items[this.comboBoxStartMode.SelectedIndex].ToString();
            Settings.Default.StartMode = selected;

            //Save changes to auto launch option
            bool autoLaunch = this.checkBoxAutoLaunch.Checked;
            //Update registry if autolaunch option changed
            if (autoLaunch != Settings.Default.AutoLaunch)
            {
                
                if (autoLaunch)
                {
                    RegistryHelper.EnableAutoRun();
                }
                else
                {
                    RegistryHelper.DisableAutoRun();
                }
            }
            Settings.Default.AutoLaunch = this.checkBoxAutoLaunch.Checked;

            //Save changes to auto delete
            Settings.Default.AutoDelete = checkBoxAutoDelete.Checked;

            Settings.Default.Save();

            this.Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Hide the settings window when the cancel button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(Object sender, EventArgs e)
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
        }

        /// <summary>
        /// Load settings when the form/window becomes visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSettings_VisibleChanged(Object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.LoadSettings();
            }
        }
    }
}
