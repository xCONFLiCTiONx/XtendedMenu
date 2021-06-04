using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static XtendedMenu.SendMessage;

namespace XtendedMenu
{
    internal class Installation
    {
        private static readonly string website = "https://github.com/xCONFLiCTiONx/XtendedMenu";

        internal static void InstallerClass(string args)
        {
            // Installer
            if (args == "-install" || args == "-i")
            {
                if (Main.IsElevated)
                {
                    try
                    {
                        Install(GetAssembly.AssemblyInformation("directory"));
                    }
                    catch (Exception ex)
                    {
                        MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                }
                else
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
            // Uninstaller
            if (args == "-uninstall" || args == "-u")
            {
                if (Main.IsElevated)
                {
                    try
                    {
                        Uninstall();
                    }
                    catch (Exception ex)
                    {
                        MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(0);
                    }
                }
                else
                {
                    try
                    {
                        StartProcess.StartInfo(GetAssembly.AssemblyInformation("filelocation"), "-uninstall", false, true);
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

        internal static void Install(string location)
        {
            try
            {
                SetCustomKeys("SOFTWARE\\XtendedMenu\\Settings\\AllFiles");
                SetCustomKeys("SOFTWARE\\XtendedMenu\\Settings\\Directories");
                SetCustomKeys("SOFTWARE\\XtendedMenu\\Settings\\Background");


                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;

                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\XtendedMenu");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\XtendedMenu\\Version.txt", version);

                RegistryKey XtendedMenuSettings = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings");
                RegistryKey InstallInfo = Registry.LocalMachine.CreateSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\XtendedMenu");

                StartProcess.StartInfo(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe", "\"" + location + "\\XtendedMenu.dll" + "\"" + " -codebase", true, true, true);

                // Adds Information to Uninstall - Change to 32 bit for compiling x86
                InstallInfo.SetValue("InstallLocation", "\"" + location + "\"", RegistryValueKind.String);
                InstallInfo.SetValue("InstallFileLocation", "\"" + location + @"\XtendedMenu.exe" + "\"", RegistryValueKind.String);
                InstallInfo.SetValue("UninstallString", "\"" + location + @"\XtendedMenu.exe" + "\"" + " -uninstall", RegistryValueKind.String);
                InstallInfo.SetValue("DisplayIcon", location + @"\XtendedMenu.exe", RegistryValueKind.String);
                InstallInfo.SetValue("Publisher", "xCONFLiCTiONx", RegistryValueKind.String);
                InstallInfo.SetValue("HelpLink", website, RegistryValueKind.String);
                InstallInfo.SetValue("DisplayName", "XtendedMenu", RegistryValueKind.String);
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
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        internal static void Uninstall()
        {
            try
            {
                DialogResult dialog1 = MessageForm("Would you like to uninstall XtendedMenu?", "XtendedMenu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog1 == DialogResult.No)
                {
                    Environment.Exit(0);
                }

                RegistryKey RegistrySoftware = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                RegistryKey UninstallInfo = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", true);
                StartProcess.StartInfo(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe", "-unregister " + "\"" + GetAssembly.AssemblyInformation("directory") + "\\XtendedMenu.dll" + "\"", true, true, true);

                UninstallInfo.DeleteSubKeyTree("XtendedMenu", false);
                RegistrySoftware.DeleteSubKey("XtendedMenu\\Settings\\AllFiles", false);
                RegistrySoftware.DeleteSubKey("XtendedMenu\\Settings\\Directories", false);
                RegistrySoftware.DeleteSubKey("XtendedMenu\\Settings\\Background", false);
                RegistrySoftware.DeleteSubKey("XtendedMenu\\Settings", false);
                RegistrySoftware.DeleteSubKey("XtendedMenu", false);

                // Restart Explorer
                DialogResult dialog = MessageForm("Uninstall is complete!" + Environment.NewLine +
                    "Explorer must be restarted to complete the uninstallation." + Environment.NewLine +
                    "Would you like to restart Explorer now? ", "XtendedMenu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private static void SetCustomKeys(string RegistryLocation)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryLocation))
            {
                string[] CustomNameRange = { };
                List<string> CustomNameList = new List<string>();
                CustomNameList.AddRange(CustomNameRange);
                string[] CustomNameArray = CustomNameList.ToArray();
                key.SetValue("CustomName", CustomNameArray, RegistryValueKind.MultiString);


                string[] CustomProcessRange = { };
                List<string> CustomProcessList = new List<string>();
                CustomProcessList.AddRange(CustomProcessRange);
                string[] CustomProcessArray = CustomProcessList.ToArray();
                key.SetValue("CustomProcess", CustomProcessArray, RegistryValueKind.MultiString);


                string[] CustomArgumentsRange = { };
                List<string> CustomArgumentsList = new List<string>();
                CustomArgumentsList.AddRange(CustomArgumentsRange);
                string[] CustomArgumentsArray = CustomArgumentsList.ToArray();
                key.SetValue("CustomArguments", CustomArgumentsArray, RegistryValueKind.MultiString);


                string[] CustomDirectoryRange = { };
                List<string> CustomDirectoryList = new List<string>();
                CustomDirectoryList.AddRange(CustomDirectoryRange);
                string[] CustomDirectoryArray = CustomDirectoryList.ToArray();
                key.SetValue("CustomDirectory", CustomDirectoryArray, RegistryValueKind.MultiString);


                string[] CustomIconRange = { };
                List<string> CustomIconList = new List<string>();
                CustomIconList.AddRange(CustomIconRange);
                string[] CustomIconArray = CustomIconList.ToArray();
                key.SetValue("CustomIcon", CustomIconArray, RegistryValueKind.MultiString);


                string[] RunAsAdminRange = { };
                List<string> RunAsAdminList = new List<string>();
                RunAsAdminList.AddRange(RunAsAdminRange);
                string[] RunAsAdminArray = RunAsAdminList.ToArray();
                key.SetValue("RunAsAdmin", RunAsAdminArray, RegistryValueKind.MultiString);
            }
        }
    }
}
