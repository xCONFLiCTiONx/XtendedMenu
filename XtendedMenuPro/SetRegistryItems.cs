using Microsoft.Win32;

namespace XtendedMenu
{
    internal class SetRegistryItems
    {
        private static readonly RegistryKey XtendedMenuSettings = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings");
        internal static void SetItems()
        {
            // All Files
            XtendedMenuSettings.SetValue("OpenNotepadFiles", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("BlockWithFirewallFiles", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CopyNameFiles", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CopyPathFiles", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CopyURLFiles", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CopyLONGPathFiles", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("AttributesFiles", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("SymlinkFiles", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("TakeOwnershipFiles", 0x00000001, RegistryValueKind.DWord);
            // Directories
            XtendedMenuSettings.SetValue("BlockWithFirewallDirectory", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CopyNameDirectory", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CopyPathDirectory", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CopyURLDirectory", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CopyLONGPathDirectory", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("AttributesDirectory", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("SymlinkDirectory", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("TakeOwnershipDirectory", 0x00000001, RegistryValueKind.DWord);
            // Directory Background
            XtendedMenuSettings.SetValue("AttributesDirectoryBack", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("CommandLinesDirectoryBack", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("FindWallpaperDirectoryBack", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("SystemFoldersDirectoryBack", 0x00000001, RegistryValueKind.DWord);
            XtendedMenuSettings.SetValue("PasteContentsDirectoryBack", 0x00000001, RegistryValueKind.DWord);
        }
    }
}
