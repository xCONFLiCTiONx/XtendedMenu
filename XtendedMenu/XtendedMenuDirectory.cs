using Microsoft.Win32;
using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using XtendedMenu.Properties;

namespace XtendedMenu
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.Directory)]
    [DisplayName("XtendedMenu")]
    public class XtendedMenuDirectory : SharpContextMenu
    {
        private static readonly RegistryKey ExplorerAdvanced = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced", true);
        private readonly CultureInfo culture = CultureInfo.CurrentCulture;
        private ContextMenuStrip Menu;
        private ToolStripMenuItem XtendedMenuMenu, BlockFirewall, CopyName, CopyPath, CopyPathURL, CopyLONGPath, Attributes, SymLink, TakeOwnership;
        private ToolStripMenuItem AttributesMenu, HiddenAttributes, SystemAttributes, ReadOnlyAttributes, ShowHidden, HideHidden, ShowSystem, HideSystem;

        protected override bool CanShowMenu()
        {
            return true;
        }
        // Create the Menu
        protected override ContextMenuStrip CreateMenu()
        {
            CheckUserSettings();
            // Main Menu
            using (Menu = new ContextMenuStrip())
            {
                Menu.Name = "XtendedMenuDirectory";

                using (XtendedMenuMenu = new ToolStripMenuItem())
                {
                    XtendedMenuMenu.Name = "XtendedMenuMenu";

                    // BlockFirewall
                    using (BlockFirewall = new ToolStripMenuItem())
                    {
                        BlockFirewall.Text = "Block all with Firewall";
                        BlockFirewall.Name = "BlockFirewall";
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
                    // Attributes
                    using (Attributes = new ToolStripMenuItem())
                    {
                        Attributes.Text = "Attributes";
                        Attributes.Name = "Attributes";

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
                    // SymLink
                    using (SymLink = new ToolStripMenuItem())
                    {
                        SymLink.Text = "Create Symbolic Link (Junction)";
                        SymLink.Name = "SymLink";
                    }
                    // TakeOwnership
                    using (TakeOwnership = new ToolStripMenuItem())
                    {
                        TakeOwnership.Text = "Take Ownership";
                        TakeOwnership.Name = "TakeOwnership";
                    }
                }
            }
            MenuDeveloper();

            return Menu;
        }
        private static void CheckUserSettings()
        {
            RegistryKey XtendedMenuSettings = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings");
            if (XtendedMenuSettings == null)
            {
                SetRegistryItems.SetItems();
            }
        }
        private void MenuDeveloper()
        {
            // Main Menu
            Menu.Items.Add(XtendedMenuMenu);
            XtendedMenuMenu.Text = "XtendedMenu";

            // Icons
            XtendedMenuMenu.Image = Resources.MAIN_ICON.ToBitmap();
            BlockFirewall.Image = Resources.Firewall.ToBitmap();
            CopyName.Image = Resources.CopyName.ToBitmap();
            CopyPath.Image = Resources.CopyPath.ToBitmap();
            CopyPathURL.Image = Resources.CopyPath.ToBitmap();
            CopyLONGPath.Image = Resources.CopyPath.ToBitmap();
            Attributes.Image = Resources.FileAttributes.ToBitmap();
            AttributesMenu.Image = Resources.MAIN_ICON.ToBitmap();
            SymLink.Image = Resources.SymLink.ToBitmap();
            TakeOwnership.Image = Resources.TakeOwnership.ToBitmap();

            AddMenuItems();

            CustomEntries();

            // Subscriptions
            BlockFirewall.Click += (sender, args) => BlockFirewallMethod();
            CopyName.Click += (sender, args) => CopyNameMethod();
            CopyPath.Click += (sender, args) => CopyPathMethod();
            CopyPathURL.Click += (sender, args) => CopyPathURLMethod();
            CopyLONGPath.Click += (sender, args) => CopyLONGPathMethod();
            AttributesMenu.Click += (sender, args) => AttributesMenuMethod();
            HiddenAttributes.Click += (sender, args) => HiddenAttributesMethod();
            SystemAttributes.Click += (sender, args) => SystemAttributesMethod();
            ReadOnlyAttributes.Click += (sender, args) => ReadOnlyAttributesMethod();
            ShowHidden.Click += (sender, args) => ShowHiddenMethod();
            HideHidden.Click += (sender, args) => HideHiddenMethod();
            ShowSystem.Click += (sender, args) => ShowSystemMethod();
            HideSystem.Click += (sender, args) => HideSystemMethod();
            SymLink.Click += (sender, args) => SymLinkMethod();
            TakeOwnership.Click += (sender, args) => TakeOwnershipMethod();
        }

        private void CustomEntries()
        {
            try
            {
                // Custom Entries
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings\\Directories"))
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
        private void AddMenuItems()
        {
            RegistryKey XtendedMenuSettings = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings");
            object BlockWithFirewallDirectory = XtendedMenuSettings.GetValue("BlockWithFirewallDirectory");
            if (BlockWithFirewallDirectory != null)
            {
                if (BlockWithFirewallDirectory.ToString() == "1")
                {
                    XtendedMenuMenu.DropDownItems.Add(BlockFirewall);
                }
            }
            object CopyNameDirectory = XtendedMenuSettings.GetValue("CopyNameDirectory");
            if (CopyNameDirectory != null)
            {
                if (CopyNameDirectory.ToString() == "1")
                {
                    XtendedMenuMenu.DropDownItems.Add(CopyName);
                }
            }
            object CopyPathDirectory = XtendedMenuSettings.GetValue("CopyPathDirectory");
            if (CopyPathDirectory != null)
            {
                if (CopyPathDirectory.ToString() == "1")
                {
                    XtendedMenuMenu.DropDownItems.Add(CopyPath);
                }
            }
            object CopyURLDirectory = XtendedMenuSettings.GetValue("CopyURLDirectory");
            if (CopyURLDirectory != null)
            {
                if (CopyURLDirectory.ToString() == "1")
                {
                    XtendedMenuMenu.DropDownItems.Add(CopyPathURL);
                }
            }
            object CopyLONGPathDirectory = XtendedMenuSettings.GetValue("CopyLONGPathDirectory");
            if (CopyLONGPathDirectory != null)
            {
                if (CopyLONGPathDirectory.ToString() == "1")
                {
                    XtendedMenuMenu.DropDownItems.Add(CopyLONGPath);
                }
            }
            object AttributesDirectory = XtendedMenuSettings.GetValue("AttributesDirectory");
            if (AttributesDirectory != null)
            {
                if (AttributesDirectory.ToString() == "1")
                {
                    XtendedMenuMenu.DropDownItems.Add(Attributes);
                    Attributes.DropDownItems.Add(AttributesMenu);
                    Attributes.DropDownItems.Add(new ToolStripSeparator());
                    Attributes.DropDownItems.Add(HiddenAttributes);
                    Attributes.DropDownItems.Add(SystemAttributes);
                    Attributes.DropDownItems.Add(ReadOnlyAttributes);
                    Attributes.DropDownItems.Add(new ToolStripSeparator());
                    Attributes.DropDownItems.Add(ShowHidden);
                    Attributes.DropDownItems.Add(HideHidden);
                    Attributes.DropDownItems.Add(ShowSystem);
                    Attributes.DropDownItems.Add(HideSystem);
                }
            }
            object SymlinkDirectory = XtendedMenuSettings.GetValue("SymlinkDirectory");
            if (SymlinkDirectory != null)
            {
                if (SymlinkDirectory.ToString() == "1")
                {
                    XtendedMenuMenu.DropDownItems.Add(SymLink);
                }
            }
            object TakeOwnershipDirectory = XtendedMenuSettings.GetValue("TakeOwnershipDirectory");
            if (TakeOwnershipDirectory != null)
            {
                if (TakeOwnershipDirectory.ToString() == "1")
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
        // Set File Attributes
        private void SetFileAttributes()
        {
            using (HiddenAttributes = new ToolStripMenuItem())
            {
                HiddenAttributes.Text = "Set Hidden Attribute";
                HiddenAttributes.Name = "HiddenAttributes";
            }
            if (AttributesInfo.hidden)
            {
                HiddenAttributes.Image = Resources.AttributesShow.ToBitmap();
            }
            // SystemAttributes
            using (SystemAttributes = new ToolStripMenuItem())
            {
                SystemAttributes.Text = "Set Attributes";
                SystemAttributes.Name = "SystemAttributes";
            }
            if (AttributesInfo.system)
            {
                SystemAttributes.Image = Resources.AttributesShow.ToBitmap();
            }
            // ReadOnlyAttributes
            using (ReadOnlyAttributes = new ToolStripMenuItem())
            {
                ReadOnlyAttributes.Text = "Set Read-only Attribute";
                ReadOnlyAttributes.Name = "ReadOnlyAttributes";
            }
            if (AttributesInfo.readOnly)
            {
                ReadOnlyAttributes.Image = Resources.AttributesShow.ToBitmap();
            }
            SetInternalAttributes();
        }
        private void SetInternalAttributes()
        {
            using (ShowHidden = new ToolStripMenuItem())
            {
                ShowHidden.Text = "Show Hidden";
                ShowHidden.Name = "ShowHidden";
            }
            using (HideHidden = new ToolStripMenuItem())
            {
                HideHidden.Text = "Hide Hidden";
                HideHidden.Name = "HideHidden";
            }
            if (AttributesInfo.HiddenFilesShowing)
            {
                ShowHidden.Image = Resources.AttributesShow.ToBitmap();
            }
            if (!AttributesInfo.HiddenFilesShowing)
            {
                HideHidden.Image = Resources.AttributesHide.ToBitmap();
            }
            using (ShowSystem = new ToolStripMenuItem())
            {
                ShowSystem.Text = "Show System";
                ShowSystem.Name = "ShowSystem";
            }
            using (HideSystem = new ToolStripMenuItem())
            {
                HideSystem.Text = "Hide System";
                HideSystem.Name = "HideSystem";
            }
            if (AttributesInfo.SystemFilesShowing)
            {
                ShowSystem.Image = Resources.AttributesShow.ToBitmap();
            }
            if (!AttributesInfo.SystemFilesShowing)
            {
                HideSystem.Image = Resources.AttributesHide.ToBitmap();
            }
        }
        // Methods
        private void BlockFirewallMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + array.ToStringArray(false) + "\" " + "-firewallfolder", false, true);
        }
        private void CopyNameMethod()
        {
            Clipboard.Clear();
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            Clipboard.SetText(array.ToStringArray(true));
        }
        private void CopyPathMethod()
        {
            Clipboard.Clear();
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            Clipboard.SetText(array.ToStringArray(false));
        }
        private void CopyPathURLMethod()
        {
            Clipboard.Clear();
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            try
            {
                Clipboard.SetText(new Uri(array.ToStringArray(false)).AbsoluteUri);
            }
            catch (Exception ex)
            {
                StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite + "\"" + " -catchhandler");
            }
        }
        private void CopyLONGPathMethod()
        {
            Clipboard.Clear();
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            Clipboard.SetText(@"\\?\" + array.ToStringArray(false));
        }
        private void AttributesMenuMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + array.ToStringArray(false) + "\" " + "-attributesmenu");
        }
        private void SystemAttributesMethod()
        {
            foreach (string file in SelectedItemPaths)
            {
                FileAttributes attributes = File.GetAttributes(file);
                if ((attributes & FileAttributes.System) == FileAttributes.System)
                {
                    attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.System);
                    File.SetAttributes(file, attributes);
                }
                else
                {
                    File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.System);
                }
            }
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "-refresh");
        }
        private void ReadOnlyAttributesMethod()
        {
            foreach (string file in SelectedItemPaths)
            {
                FileAttributes attributes = File.GetAttributes(file);
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.ReadOnly);
                    File.SetAttributes(file, attributes);
                }
                else
                {
                    File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.ReadOnly);
                }
            }
        }
        private void ShowHiddenMethod()
        {
            ExplorerAdvanced.SetValue("Hidden", 1.ToString(culture), RegistryValueKind.DWord);
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "-refresh");
        }
        private void HideHiddenMethod()
        {
            ExplorerAdvanced.SetValue("Hidden", 2.ToString(culture), RegistryValueKind.DWord);
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "-refresh");
        }
        private void ShowSystemMethod()
        {
            ExplorerAdvanced.SetValue("ShowSuperHidden", 1.ToString(culture), RegistryValueKind.DWord);
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "-refresh");
        }
        private void HideSystemMethod()
        {
            ExplorerAdvanced.SetValue("ShowSuperHidden", 2.ToString(culture), RegistryValueKind.DWord);
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "-refresh");
        }
        private void HiddenAttributesMethod()
        {
            foreach (string file in SelectedItemPaths)
            {
                FileAttributes attributes = File.GetAttributes(file);
                if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    attributes = AttributesInfo.RemoveAttribute(attributes, FileAttributes.Hidden);
                    File.SetAttributes(file, attributes);
                }
                else
                {
                    File.SetAttributes(file, File.GetAttributes(file) | FileAttributes.Hidden);
                }
            }
        }
        private void SymLinkMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + array.ToStringArray(false) + "\" " + "-makelink", false, true);
        }
        private void TakeOwnershipMethod()
        {
            string[] array = SelectedItemPaths.Cast<string>().ToArray();
            StartProcess.StartInfo(AttributesInfo.GetAssembly.AssemblyInformation("directory") + @"\XtendedMenu.exe", "\"" + array.ToStringArray(false) + "\" " + "-ownership", false, true);
        }

        private void CustomMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] array = SelectedItemPaths.Cast<string>().ToArray();

                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings\\Directories"))
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
