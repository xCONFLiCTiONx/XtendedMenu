using Microsoft.Win32;
using System.IO;

namespace XtendedMenu
{
    internal class Shortcut
    {
        internal static void Create(string shortcutFolder, string InstallInfo)
        {
            IWshRuntimeLibrary.WshShell shellClass = new IWshRuntimeLibrary.WshShell();
            //Create Shortcut for Application Settings
            string settingsLink = Path.Combine(shortcutFolder, "XtendedMenu Settings.lnk");
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shellClass.CreateShortcut(settingsLink);

            string FileLocationInfo = "SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\XtendedMenu";
            RegistryKey XtendedMenuKey = Registry.LocalMachine.OpenSubKey(FileLocationInfo, false);

            string fileLocation = (string)XtendedMenuKey.GetValue("InstallFileLocation");

            if (!string.IsNullOrEmpty(fileLocation))
            {
                shortcut.TargetPath = fileLocation;
                shortcut.Description = "XtendedMenu Settings";
                shortcut.Save();
            }
        }
    }
}
