using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Microsoft.API
{
    [UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Auto)]
    public delegate bool EnumResNameProc(IntPtr hModule, ResourceTypes lpszType, IntPtr lpszName, IntPtr lParam);

    #region Enumurations
    [Flags]
    public enum LoadLibraryExFlags : int
    {
        DONT_RESOLVE_DLL_REFERENCES   = 0x00000001,
        LOAD_LIBRARY_AS_DATAFILE      = 0x00000002,
        LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
    }
    public enum GetLastErrorResult : int
    {
        ERROR_SUCCESS                 = 0,
        ERROR_FILE_NOT_FOUND          = 2,
        ERROR_BAD_EXE_FORMAT          = 193,
        ERROR_RESOURCE_TYPE_NOT_FOUND = 1813
    }
    public enum ResourceTypes : int
    {
        RT_ICON = 3,
        RT_GROUP_ICON = 14
    }
    public enum LookupIconIdFromDirectoryExFlags : int
    {
        LR_DEFAULTCOLOR = 0,
        LR_MONOCHROME   = 1
    }
    public enum LoadImageTypes : int
    {
        IMAGE_BITMAP = 0,
        IMAGE_ICON   = 1,
        IMAGE_CURSOR = 2
    }
    [Flags]
    public enum SHGetFileInfoFlags : int
    {
        Icon              = 0x000000100,     // get icon
        DisplayName       = 0x000000200,     // get display name
        TypeName          = 0x000000400,     // get type name
        Attributes        = 0x000000800,     // get attributes
        IconLocation      = 0x000001000,     // get icon location
        ExeType           = 0x000002000,     // return exe type
        SysIconIndex      = 0x000004000,     // get system icon index
        LinkOverlay       = 0x000008000,     // put a link overlay on icon
        Selected          = 0x000010000,     // show icon in selected state
        AttrSpecified     = 0x000020000,     // get only specified attributes
        LargeIcon         = 0x000000000,     // get large icon
        SmallIcon         = 0x000000001,     // get small icon
        OpenIcon          = 0x000000002,     // get open icon
        ShellIconSize     = 0x000000004,     // get shell size icon
        PIDL              = 0x000000008,     // pszPath is a pidl
        UseFileAttributes = 0x000000010      // use passed dwFileAttribute
    }
    #endregion

    #region Structures
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };
    #endregion

    public static class Win32
    {
        #region Constants
        public const int MAX_PATH = 260;
        #endregion

        #region Helper Functions
        public static bool IsIntResource(IntPtr lpszName)
        {
            return (((uint)lpszName >> 16) == 0);
        }
        #endregion

        #region API Functions
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, LoadLibraryExFlags dwFlags);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetModuleFileName(IntPtr hModule, StringBuilder lpFilename, int nSize);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool EnumResourceNames(IntPtr hModule, ResourceTypes lpszType, EnumResNameProc lpEnumFunc, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, ResourceTypes lpType);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern int SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern int LookupIconIdFromDirectory(IntPtr presbits, bool fIcon);

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern int LookupIconIdFromDirectoryEx(IntPtr presbits, bool fIcon, int cxDesired, int cyDesired, LookupIconIdFromDirectoryExFlags Flags);

        [DllImport("user32.dll", EntryPoint = "LoadImageW", SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr LoadImage(IntPtr hInstance, IntPtr lpszName, LoadImageTypes imageType, int cxDesired, int cyDesired, uint fuLoad);

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, SHGetFileInfoFlags uFlags);
        #endregion
    }
}
