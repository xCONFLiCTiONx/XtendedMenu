using System.Runtime.InteropServices;
using BYTE = System.Byte;
using DWORD = System.Int32;
using WORD = System.Int16;

namespace TAFactory.IconPack
{
    /// <summary>
    /// Presents an Icon Directory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 6)]
    public struct IconDir
    {
        public WORD Reserved;   // Reserved (must be 0)
        public WORD Type;       // Resource Type (1 for icons)
        public WORD Count;      // How many images?

        /// <summary>
        /// Converts the current TAFactory.IconPack.IconDir into TAFactory.IconPack.GroupIconDir.
        /// </summary>
        /// <returns>TAFactory.IconPack.GroupIconDir</returns>
        public GroupIconDir ToGroupIconDir()
        {
            GroupIconDir grpDir = new GroupIconDir
            {
                Reserved = Reserved,
                Type = Type,
                Count = Count
            };
            return grpDir;
        }
    }

    /// <summary>
    /// Presents an Icon Directory Entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public struct IconDirEntry
    {
        public BYTE Width;          // Width, in pixels, of the image
        public BYTE Height;         // Height, in pixels, of the image
        public BYTE ColorCount;     // Number of colors in image (0 if >=8bpp)
        public BYTE Reserved;       // Reserved ( must be 0)
        public WORD Planes;         // Color Planes
        public WORD BitCount;       // Bits per pixel
        public DWORD BytesInRes;     // How many bytes in this resource?
        public DWORD ImageOffset;    // Where in the file is this image?

        /// <summary>
        /// Converts the current TAFactory.IconPack.IconDirEntry into TAFactory.IconPack.GroupIconDirEntry.
        /// </summary>
        /// <param name="id">The resource identifier.</param>
        /// <returns>TAFactory.IconPack.GroupIconDirEntry</returns>
        public GroupIconDirEntry ToGroupIconDirEntry(int id)
        {
            GroupIconDirEntry grpEntry = new GroupIconDirEntry
            {
                Width = Width,
                Height = Height,
                ColorCount = ColorCount,
                Reserved = Reserved,
                Planes = Planes,
                BitCount = BitCount,
                BytesInRes = BytesInRes,
                ID = (short)id
            };
            return grpEntry;
        }
    }

    /// <summary>
    /// Presents a Group Icon Directory.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 6)]
    public struct GroupIconDir
    {
        public WORD Reserved;   // Reserved (must be 0)
        public WORD Type;       // Resource Type (1 for icons)
        public WORD Count;      // How many images?

        /// <summary>
        /// Converts the current TAFactory.IconPack.GroupIconDir into TAFactory.IconPack.IconDir.
        /// </summary>
        /// <returns>TAFactory.IconPack.IconDir</returns>
        public IconDir ToIconDir()
        {
            IconDir dir = new IconDir
            {
                Reserved = Reserved,
                Type = Type,
                Count = Count
            };
            return dir;
        }
    }

    /// <summary>
    /// Presents a Group Icon Directory Entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 14)]
    public struct GroupIconDirEntry
    {
        public BYTE Width;          // Width, in pixels, of the image
        public BYTE Height;         // Height, in pixels, of the image
        public BYTE ColorCount;     // Number of colors in image (0 if >=8bpp)
        public BYTE Reserved;       // Reserved ( must be 0)
        public WORD Planes;         // Color Planes
        public WORD BitCount;       // Bits per pixel
        public DWORD BytesInRes;     // How many bytes in this resource?
        public WORD ID;             // the ID

        /// <summary>
        /// Converts the current TAFactory.IconPack.GroupIconDirEntry into TAFactory.IconPack.IconDirEntry.
        /// </summary>
        /// <param name="id">The resource identifier.</param>
        /// <returns>TAFactory.IconPack.IconDirEntry</returns>
        public IconDirEntry ToIconDirEntry(int imageOffiset)
        {
            IconDirEntry entry = new IconDirEntry
            {
                Width = Width,
                Height = Height,
                ColorCount = ColorCount,
                Reserved = Reserved,
                Planes = Planes,
                BitCount = BitCount,
                BytesInRes = BytesInRes,
                ImageOffset = imageOffiset
            };
            return entry;
        }
    }
}
