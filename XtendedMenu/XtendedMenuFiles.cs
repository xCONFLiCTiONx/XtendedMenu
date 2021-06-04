using Microsoft.Win32;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

[assembly: CLSCompliant(false)]
namespace XtendedMenu
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFiles)]
    [DisplayName("XtendedMenu")]
    public class XtendedMenuFiles : SharpContextMenu
    {
        private static readonly RegistryKey XtendedMenuSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\XtendedMenu\\Settings", true);
        private ContextMenuStrip Menu;
        private ToolStripMenuItem XtendedMenuMenu, Attributes, OpenNotepad, CopyName, CopyPath, CopyPathURL, CopyLONGPath, SymLink, BlockFirewall, TakeOwnership;
        private ToolStripMenuItem AttributesMenu, HiddenAttributes, SystemAttributes, ReadOnlyAttributes;

        protected override bool CanShowMenu()
        {
            return true;
        }
        protected override ContextMenuStrip CreateMenu()
        {
            try
            {
                CheckUserSettings();
                // Main Menu
                using (Menu = new ContextMenuStrip())
                {
                    Menu.Name = "XtendedMenuFiles";

                    using (XtendedMenuMenu = new ToolStripMenuItem())
                    {
                        XtendedMenuMenu.Name = "XtendedMenuMenu";

                        // Attributes
                        using (Attributes = new ToolStripMenuItem())
                        {
                            Attributes.Name = "Attributes";

                            Attributes.Text = "Attributes";
                            // AttributesMenu
                            using (AttributesMenu = new ToolStripMenuItem())
                            {
                                AttributesMenu.Text = "Attributes Menu";
                                AttributesMenu.Name = "AttributesMenu";
                            }
                            // Get : Set Attributes
                            string[] SelectedPath = SelectedItemPaths.Cast<string>().ToArray();
                            if (SelectedPath.Length > 1)
                            {
                                foreach (string item in SelectedPath)
                                {
                                    try
                                    {
                                        AttributesInfo.GetFileAttributes(item);
                                    }
                                    catch (Exception ex)
                                    {
                                        StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    AttributesInfo.GetFileAttributes(SelectedPath.ToStringArray(false));
                                }
                                catch (Exception ex)
                                {
                                    StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
                                }
                            }
                            SetFileAttributes();
                        }
                        // OpenNotepad
                        using (OpenNotepad = new ToolStripMenuItem())
                        {
                            OpenNotepad.Text = "Open with text editor";
                            OpenNotepad.Name = "OpenNotepad";
                        }
                        // CopyName
                        using (CopyName = new ToolStripMenuItem())
                        {
                            CopyName.Text = "Copy Name";
                            CopyName.Name = "CopyName";
                        }
                        // CopyPath
                        using (CopyPath = new ToolStripMenuItem())
                        {
                            CopyPath.Text = "Copy Path";
                            CopyPath.Name = "CopyPath";
                        }
                        // CopyPathURL
                        using (CopyPathURL = new ToolStripMenuItem())
                        {
                            CopyPathURL.Text = "Copy Path as URL";
                            CopyPathURL.Name = "CopyPathURL";
                        }
                        // CopyLONGPath
                        using (CopyLONGPath = new ToolStripMenuItem())
                        {
                            CopyLONGPath.Text = "Copy Long Path";
                            CopyLONGPath.Name = "CopyLONGPath";
                        }
                        // SymLink
                        using (SymLink = new ToolStripMenuItem())
                        {
                            SymLink.Text = "Create Symbolic Link (Junction)";
                            SymLink.Name = "SymLink";
                        }
                        // BlockFirewall
                        using (BlockFirewall = new ToolStripMenuItem())
                        {
                            BlockFirewall.Text = "Block with Firewall";
                            BlockFirewall.Name = "BlockFirewall";
                        }
                        // TakeOwnership
                        using (TakeOwnership = new ToolStripMenuItem())
                        {
                            TakeOwnership.Text = "Take Ownership";
                            TakeOwnership.Name = "TakeOwnership";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }

            MenuDeveloper();

            return Menu;
        }

        private static void CheckUserSettings()
        {
            try
            {
                if (XtendedMenuSettings == null)
                {
                    RegistryKey Software = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                    Software.CreateSubKey("XtendedMenu");
                    RegistryKey SoftwareXtendedMenu = Registry.CurrentUser.OpenSubKey("SOFTWARE\\XtendedMenu", true);
                    SoftwareXtendedMenu.CreateSubKey("Settings");

                    SetRegistryItems.SetItems();
                }
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void MenuDeveloper()
        {
            try
            {
                // Main Menu
                Menu.Items.Add(XtendedMenuMenu);
                XtendedMenuMenu.Text = "XtendedMenu";

                // Icons
                XtendedMenuMenu.Image = Properties.Resources.MAIN_ICON.ToBitmap();
                Attributes.Image = Properties.Resources.FileAttributes.ToBitmap();
                AttributesMenu.Image = Properties.Resources.MAIN_ICON.ToBitmap();
                OpenNotepad.Image = Properties.Resources.notepad.ToBitmap();
                CopyPath.Image = Properties.Resources.CopyPath.ToBitmap();
                CopyPathURL.Image = Properties.Resources.CopyPath.ToBitmap();
                CopyLONGPath.Image = Properties.Resources.CopyPath.ToBitmap();
                CopyName.Image = Properties.Resources.CopyName.ToBitmap();
                SymLink.Image = Properties.Resources.SymLink.ToBitmap();
                BlockFirewall.Image = Properties.Resources.Firewall.ToBitmap();
                TakeOwnership.Image = Properties.Resources.TakeOwnership.ToBitmap();

                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                AddMenuItems(array);
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }

            CustomEntries();

            try
            {
                // Subscriptions
                AttributesMenu.Click += (sender, args) => AttributesMenuMethod();
                OpenNotepad.Click += (sender, args) => OpenNotepadMethod();
                CopyPath.Click += (sender, args) => CopyPathMethod();
                CopyPathURL.Click += (sender, args) => CopyPathURLMethod();
                CopyLONGPath.Click += (sender, args) => CopyLONGPathMethod();
                CopyName.Click += (sender, args) => CopyNameMethod();
                HiddenAttributes.Click += (sender, args) => HiddenAttributesMethod();
                SystemAttributes.Click += (sender, args) => SystemAttributesMethod();
                ReadOnlyAttributes.Click += (sender, args) => ReadOnlyAttributesMethod();
                SymLink.Click += (sender, args) => SymLinkMethod();
                BlockFirewall.Click += (sender, args) => BlockFirewallMethod();
                TakeOwnership.Click += (sender, args) => TakeOwnershipMethod();
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }

        private void CustomEntries()
        {
            try
            {
                // Custom Entries
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings\\AllFiles"))
                {
                    int index = 0;
                    List<string> CustomNameList = new List<string>();
                    CustomNameList.AddRange((string[])key.GetValue("CustomName"));
                    string[] CustomNameArray = CustomNameList.ToArray();
                    foreach (string value in CustomNameArray)
                    {
                        ToolStripMenuItem CustomMenuItem = new ToolStripMenuItem();

                        using (CustomMenuItem = new ToolStripMenuItem())
                        {
                            CustomMenuItem.Text = value;
                            CustomMenuItem.Name = index.ToString();
                        }

                        List<string> CustomIconList = new List<string>();
                        CustomIconList.AddRange((string[])key.GetValue("CustomIcon"));
                        string[] IconListArray = CustomIconList.ToArray();

                        if (!string.IsNullOrEmpty(IconListArray[index]))
                        {
                            if (File.Exists(IconListArray[index]))
                            {
                                CustomMenuItem.Image = Image.FromFile(IconListArray[index]);
                            }
                        }

                        Menu.Items.Add(CustomMenuItem);

                        CustomMenuItem.Click += CustomMenuItem_Click;
                        XtendedMenuMenu.DropDownItems.Add(CustomMenuItem);
                        index++;
                    }
                }
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }

        // Add Menu Items
        private void AddMenuItems(string[] array)
        {
            try
            {
                // Disabler
                object AttributesFiles = XtendedMenuSettings.GetValue("AttributesFiles");
                if (AttributesFiles != null)
                {
                    if (AttributesFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(Attributes);
                        Attributes.DropDownItems.Add(AttributesMenu);
                        Attributes.DropDownItems.Add(new ToolStripSeparator());
                        Attributes.DropDownItems.Add(HiddenAttributes);
                        Attributes.DropDownItems.Add(SystemAttributes);
                        Attributes.DropDownItems.Add(ReadOnlyAttributes);
                    }
                }
                object OpenNotepadFiles = XtendedMenuSettings.GetValue("OpenNotepadFiles");
                if (OpenNotepadFiles != null)
                {
                    if (OpenNotepadFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(OpenNotepad);
                    }
                }
                object CopyNameFiles = XtendedMenuSettings.GetValue("CopyNameFiles");
                if (CopyNameFiles != null)
                {
                    if (CopyNameFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(CopyName);
                    }
                }
                object CopyPathFiles = XtendedMenuSettings.GetValue("CopyPathFiles");
                if (CopyPathFiles != null)
                {
                    if (CopyPathFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(CopyPath);
                    }
                }
                object CopyURLFiles = XtendedMenuSettings.GetValue("CopyURLFiles");
                if (CopyURLFiles != null)
                {
                    if (CopyURLFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(CopyPathURL);
                    }
                }
                object CopyLONGPathFiles = XtendedMenuSettings.GetValue("CopyLONGPathFiles");
                if (CopyLONGPathFiles != null)
                {
                    if (CopyLONGPathFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(CopyLONGPath);
                    }
                }
                object SymlinkFiles = XtendedMenuSettings.GetValue("SymlinkFiles");
                if (SymlinkFiles != null)
                {
                    if (SymlinkFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(SymLink);
                    }
                }
                object BlockWithFirewallFiles = XtendedMenuSettings.GetValue("BlockWithFirewallFiles");
                if (BlockWithFirewallFiles != null)
                {
                    if (BlockWithFirewallFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(BlockFirewall);
                    }
                }
                object TakeOwnershipFiles = XtendedMenuSettings.GetValue("TakeOwnershipFiles");
                if (TakeOwnershipFiles != null)
                {
                    if (TakeOwnershipFiles.ToString() == "1")
                    {
                        XtendedMenuMenu.DropDownItems.Add(TakeOwnership);
                    }
                }

                bool AllDisabled = true;
                foreach (ToolStripMenuItem item in XtendedMenuMenu.DropDownItems)
                {
                    if (item != null)
                    {
                        AllDisabled = false;
                    }
                }
                if (AllDisabled)
                {
                    Menu.Dispose();
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        // Set File Attributes
        private void SetFileAttributes()
        {
            try
            {
                using (HiddenAttributes = new ToolStripMenuItem())
                {
                    HiddenAttributes.Text = "Set Hidden Attribute";
                    HiddenAttributes.Name = "HiddenAttributes";
                }
                if (AttributesInfo.hidden)
                {
                    HiddenAttributes.Image = Properties.Resources.AttributesShow.ToBitmap();
                }
                // SystemAttributes
                using (SystemAttributes = new ToolStripMenuItem())
                {
                    SystemAttributes.Text = "Set System Attribute";
                    SystemAttributes.Name = "SystemAttributes";
                }
                if (AttributesInfo.system)
                {
                    SystemAttributes.Image = Properties.Resources.AttributesShow.ToBitmap();
                }
                // ReadOnlyAttributes
                using (ReadOnlyAttributes = new ToolStripMenuItem())
                {
                    ReadOnlyAttributes.Text = "Set Read-only Attribute";
                    ReadOnlyAttributes.Name = "ReadOnlyAttributes";
                }
                if (AttributesInfo.readOnly)
                {
                    ReadOnlyAttributes.Image = Properties.Resources.AttributesShow.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }

        // Methods
        public static bool HasExecutable(string path)
        {
            string executable = FindExecutable(path);
            return !string.IsNullOrEmpty(executable);
        }

        private static string FindExecutable(string path)
        {
            StringBuilder executable = new StringBuilder(1024);
            FindExecutable(path, string.Empty, executable);
            return executable.ToString();
        }

        [DllImport("shell32.dll", EntryPoint = "FindExecutable")]
        private static extern long FindExecutable(string lpFile, string lpDirectory, StringBuilder lpResult);
        private void OpenNotepadMethod()
        {
            try
            {
                string textFile = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\XtendedMenu\\Version.txt";
                if (!File.Exists(textFile))
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                    string version = fvi.FileVersion;

                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\XtendedMenu");
                    File.WriteAllText(textFile, version);
                }
                if (HasExecutable(textFile))
                {
                    foreach (string filePath in SelectedItemPaths)
                    {
                        StartProcess.StartInfo(FindExecutable(textFile), "\"" + filePath + "\"");
                    }
                }
                else
                {
                    foreach (string filePath in SelectedItemPaths)
                    {
                        StartProcess.StartInfo("Notepad.exe", "\"" + filePath + "\"");
                    }
                }
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void BlockFirewallMethod()
        {
            try
            {
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + array.ToStringArray() + "\" " + "-firewallfiles", false, true);
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void CopyNameMethod()
        {
            try
            {
                Clipboard.Clear();
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                Clipboard.SetText(array.ToStringArray(true));
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void CopyPathMethod()
        {
            try
            {
                Clipboard.Clear();
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                Clipboard.SetText(array.ToStringArray(false));
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void CopyPathURLMethod()
        {
            try
            {
                Clipboard.Clear();
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                Clipboard.SetText(new Uri(array.ToStringArray(false)).AbsoluteUri);
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void CopyLONGPathMethod()
        {
            try
            {
                Clipboard.Clear();
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                Clipboard.SetText(@"\\?\" + array.ToStringArray(false));
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void AttributesMenuMethod()
        {
            try
            {
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + array.ToStringArray(false) + "\" " + "-attributesmenu");
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void HiddenAttributesMethod()
        {
            try
            {
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                foreach (string item in array)
                {
                    FileAttributes attributes = File.GetAttributes(item);
                    if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.Hidden);
                        File.SetAttributes(item, attributes);
                    }
                    else
                    {
                        File.SetAttributes(item, File.GetAttributes(item) | FileAttributes.Hidden);
                    }
                }
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "-refresh");
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void SystemAttributesMethod()
        {
            try
            {
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                foreach (string item in array)
                {
                    FileAttributes attributes = File.GetAttributes(item);
                    if ((attributes & FileAttributes.System) == FileAttributes.System)
                    {
                        attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.System);
                        File.SetAttributes(item, attributes);
                    }
                    else
                    {
                        File.SetAttributes(item, File.GetAttributes(item) | FileAttributes.System);
                    }
                }
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "-refresh");
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void ReadOnlyAttributesMethod()
        {
            try
            {
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                foreach (string item in array)
                {
                    FileAttributes attributes = File.GetAttributes(item);
                    if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.ReadOnly);
                        File.SetAttributes(item, attributes);
                    }
                    else
                    {
                        File.SetAttributes(item, File.GetAttributes(item) | FileAttributes.ReadOnly);
                    }
                }
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void SymLinkMethod()
        {
            try
            {
                foreach (string file in SelectedItemPaths)
                {
                    try
                    {
                        using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                        {
                            ofd.Description = Path.GetFileName(file);
                            if (ofd.ShowDialog() == DialogResult.OK)
                            {
                                string PathName = ofd.SelectedPath + @"\" + Path.GetFileName(file);
                                StartProcess.StartInfo("cmd.exe", "/c mklink " + "\"" + PathName + "\"" + " " + "\"" + file + "\"", true, true, true);
                            }
                        }
                    }
                    catch (System.ComponentModel.Win32Exception ex)
                    {
                        StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
                    }
                }
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void TakeOwnershipMethod()
        {
            try
            {
                string[] array = SelectedItemPaths.Cast<string>().ToArray();
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + array.ToStringArray(false) + "\"" + " -ownership", false, true);
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }

        private void CustomMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] array = SelectedItemPaths.Cast<string>().ToArray();

                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings\\AllFiles"))
                {
                    int itemName = Convert.ToInt32(((ToolStripMenuItem)sender).Name);

                    List<string> CustomProcessList = new List<string>();
                    CustomProcessList.AddRange((string[])key.GetValue("CustomProcess"));
                    string[] CustomProcessArray = CustomProcessList.ToArray();

                    List<string> CustomArgumentsList = new List<string>();
                    CustomArgumentsList.AddRange((string[])key.GetValue("CustomArguments"));
                    string[] CustomArgumentsArray = CustomArgumentsList.ToArray();

                    List<string> CustomDirectoryList = new List<string>();
                    CustomDirectoryList.AddRange((string[])key.GetValue("CustomDirectory"));
                    string[] CustomDirectoryArray = CustomDirectoryList.ToArray();

                    List<string> RunAsAdminList = new List<string>();
                    RunAsAdminList.AddRange((string[])key.GetValue("RunAsAdmin"));
                    string[] RunAsAdminArray = RunAsAdminList.ToArray();

                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = CustomProcessArray[itemName];
                        process.StartInfo.Arguments = CustomArgumentsArray[itemName] + " " + "\"" + array.ToStringArray(false) + "\"";
                        process.StartInfo.WorkingDirectory = CustomDirectoryArray[itemName];
                        if (RunAsAdminArray[itemName] == "True")
                        {
                            process.StartInfo.Verb = "runas";
                        }
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
    }
}
