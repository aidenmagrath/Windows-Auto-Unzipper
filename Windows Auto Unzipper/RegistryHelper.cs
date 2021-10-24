using Microsoft.Win32;
using System.Windows.Forms;

namespace Windows_Auto_Unzipper
{
    class RegistryHelper
    {
        public static void EnableAutoRun()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue(Application.ProductName, Application.ExecutablePath);
        }

        public static void DisableAutoRun()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.DeleteValue(Application.ProductName);
        }

        public static bool IsAutoRunEnabled()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            return IsAutoRunEnabled(key);
        }

        public static bool IsAutoRunEnabled(RegistryKey key)
        {
            return key.GetValue(Application.ProductName) != null;
        }
    }
}
