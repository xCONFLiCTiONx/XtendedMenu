using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using XtendedMenu.Properties;
using static XtendedMenu.SendMessage;

namespace XtendedMenu
{
    internal class Installation
    {
        private static readonly string website = "https://github.com/xCONFLiCTiONx/XtendedMenu";

        internal static void Install(string location)
        {
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;

                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\XtendedMenu");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\XtendedMenu\\Version.txt", version);

                RegistryKey XtendedMenuSettings = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings");
                RegistryKey InstallInfo = null;
                if (ArchitectureCheck.ProcessorIs64Bit())
                {
                    RegistryKey RegUninstallKey64 = Registry.LocalMachine.CreateSubKey(Resources.RegUninstallKey64String);
                    InstallInfo = RegUninstallKey64;
                }
                if (!ArchitectureCheck.ProcessorIs64Bit())
                {
                    RegistryKey RegUninstallKey32 = Registry.LocalMachine.CreateSubKey(Resources.RegUninstallKey32String);
                    InstallInfo = RegUninstallKey32;
                }
                StartProcess.StartInfo(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe", "\"" + location + "\\XtendedMenu.dll" + "\"" + " -codebase", true, true, true);

                // Adds Information to Uninstall - Change to 32 bit for compiling x86
                InstallInfo.SetValue("InstallLocation", "\"" + location + "\"", RegistryValueKind.String);
                InstallInfo.SetValue("InstallFileLocation", "\"" + location + @"\XtendedMenu.exe" + "\"", RegistryValueKind.String);
                InstallInfo.SetValue("UninstallString", "\"" + location + @"\XtendedMenu.exe" + "\"" + " -uninstall", RegistryValueKind.String);
                InstallInfo.SetValue("DisplayIcon", location + @"\XtendedMenu.exe", RegistryValueKind.String);
                InstallInfo.SetValue("Publisher", "xCONFLiCTiONx", RegistryValueKind.String);
                InstallInfo.SetValue("HelpLink", website, RegistryValueKind.String);
                InstallInfo.SetValue("DisplayName", Resources.XtendedMenu, RegistryValueKind.String);
                InstallInfo.SetValue("DisplayVersion", GetAssembly.AssemblyInformation("version"), RegistryValueKind.String);
                /* User Settings */

                SetRegistryItems.SetItems();

                // Create Shorcut in All Users Start Menu Programs
                StringBuilder allUserProfile = new StringBuilder(260);
                NativeMethods.SHGetSpecialFolderPath(IntPtr.Zero, allUserProfile, NativeMethods.CSIDL_COMMON_STARTMENU, false);

                string programs_path = Path.Combine(allUserProfile.ToString(), "Programs");

                string shortcutFolder = Path.Combine(programs_path, @"XtendedMenu");
                if (!Directory.Exists(shortcutFolder))
                {
                    Directory.CreateDirectory(shortcutFolder);
                }

                using (new Settings())
                {
                    new Settings().ShowDialog();
                }

                Shortcut.Create(shortcutFolder, InstallInfo.ToString());

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace, Resources.XtendedMenu, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        internal static void Uninstall()
        {
            try
            {
                RegistryKey RegUninstallKey64 = Registry.LocalMachine.OpenSubKey(Resources.Uninstall64Bit, true);
                RegistryKey RegUninstallKey32 = Registry.LocalMachine.OpenSubKey(Resources.Uninstall32Bit, true);
                RegistryKey RegistrySoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                RegistryKey UninstallInfo = RegUninstallKey64;
                if (ArchitectureCheck.ProcessorIs64Bit())
                {
                    UninstallInfo = RegUninstallKey64;
                }
                if (!ArchitectureCheck.ProcessorIs64Bit())
                {
                    UninstallInfo = RegUninstallKey32;
                }
                StartProcess.StartInfo(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe", "-unregister " + "\"" + GetAssembly.AssemblyInformation("directory") + "\\XtendedMenu.dll" + "\"", true, true, true);

                UninstallInfo.DeleteSubKeyTree("XtendedMenu", false);
                RegistrySoftware.DeleteSubKey("XtendedMenu\\Settings", false);
                RegistrySoftware.DeleteSubKey("XtendedMenu", false);

                // Restart Explorer
                DialogResult dialog = MessageForm(Resources.UninstallComplete, Resources.XtendedMenu, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    foreach (Process proc in Process.GetProcessesByName("explorer"))
                    {
                        proc.Kill();

                        proc.WaitForExit();
                    }
                }

                try
                {
                    // Delete shortcut
                    StringBuilder allUserProfile = new StringBuilder(260);
                    NativeMethods.SHGetSpecialFolderPath(IntPtr.Zero, allUserProfile, NativeMethods.CSIDL_COMMON_STARTMENU, false);
                    string programs_path = Path.Combine(allUserProfile.ToString(), "Programs");
                    string shortcutFolder = Path.Combine(programs_path, @"XtendedMenu");
                    foreach (string file in Directory.GetFiles(shortcutFolder))
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    Directory.Delete(shortcutFolder, true);
                }
                catch (Exception ex)
                {
                    EasyLogger.Error(ex);
                    MessageBox.Show("Some errors may have occured. Please check the log for details.", "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                try
                {
                    File.Copy(GetAssembly.AssemblyInformation("directory") + @"\Deleter.exe", Path.GetTempPath() + "Deleter.exe", true);

                    using (Process p = new Process())
                    {
                        p.StartInfo.Arguments = "\"" + GetAssembly.AssemblyInformation("directory") + "\"";
                        p.StartInfo.FileName = Path.GetTempPath() + @"\Deleter.exe";
                        p.Start();
                    }
                }
                catch (Exception ex)
                {
                    EasyLogger.Error(ex);
                    MessageBox.Show("Some errors may have occured. Please check the log for details.", "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Some errors may have occured. Please check the log for details.", "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Environment.Exit(0);
        }

        internal static void InstallerElevated()
        {
            try
            {
                RegistryKey InstallInfo = null;
                RegistryKey RegUninstallKey64 = Registry.LocalMachine.OpenSubKey(Resources.RegUninstallKey64String, true);
                RegistryKey RegUninstallKey32 = Registry.LocalMachine.OpenSubKey(Resources.RegUninstallKey32String, true);
                if (ArchitectureCheck.ProcessorIs64Bit())
                {
                    InstallInfo = RegUninstallKey64;
                }
                else
                {
                    InstallInfo = RegUninstallKey32;
                }
                if (InstallInfo == null)
                {
                    Install(GetAssembly.AssemblyInformation("directory"));
                }
                else
                {
                    DialogResult results = MessageForm(Resources.UninstallQuestion + Resources.XtendedMenu + Resources.UninstallNotice, Resources.XtendedMenu, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (results == DialogResult.Yes)
                    {
                        Uninstall();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }
        internal static void InstallerUnelevated()
        {
            try
            {
                StartProcess.StartInfo(GetAssembly.AssemblyInformation("filelocation"), "-install", false, true);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Environment.Exit(0);
            }
        }
    }
}
