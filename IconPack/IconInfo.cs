using Microsoft.API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using TAFactory.Utilities;

namespace TAFactory.IconPack
{
    /// <summary>
    /// Provides information about a givin icon.
    /// This class cannot be inherited.
    /// </summary>
    [Serializable]
    public class IconInfo
    {
        #region ReadOnly
        public static int SizeOfIconDir = Marshal.SizeOf(typeof(IconDir));
        public static int SizeOfIconDirEntry = Marshal.SizeOf(typeof(IconDirEntry));
        public static int SizeOfGroupIconDir = Marshal.SizeOf(typeof(GroupIconDir));
        public static int SizeOfGroupIconDirEntry = Marshal.SizeOf(typeof(GroupIconDirEntry));
        #endregion

        #region Properties
        private Icon _sourceIcon;
        /// <summary>
        /// Gets the source System.Drawing.Icon.
        /// </summary>
        public Icon SourceIcon
        {
            get => _sourceIcon;
            private set => _sourceIcon = value;
        }

        private string _fileName = null;
        /// <summary>
        /// Gets the icon's file name. 
        /// </summary>
        public string FileName
        {
            get => _fileName;
            private set => _fileName = value;
        }

        private List<Icon> _images;
        /// <summary>
        /// Gets a list System.Drawing.Icon that presents the icon contained images.
        /// </summary>
        public List<Icon> Images
        {
            get => _images;
            private set => _images = value;
        }

        /// <summary>
        /// Get whether the icon contain more than one image or not.
        /// </summary>
        public bool IsMultiIcon => (Images.Count > 1);

        private int _bestFitIconIndex;
        /// <summary>
        /// Gets icon index that best fits to screen resolution.
        /// </summary>
        public int BestFitIconIndex
        {
            get => _bestFitIconIndex;
            private set => _bestFitIconIndex = value;
        }

        private int _width;
        /// <summary>
        /// Gets icon width.
        /// </summary>
        public int Width
        {
            get => _width;
            private set => _width = value;
        }

        private int _height;
        /// <summary>
        /// Gets icon height.
        /// </summary>
        public int Height
        {
            get => _height;
            private set => _height = value;
        }

        private int _colorCount;
        /// <summary>
        /// Gets number of colors in icon (0 if >=8bpp).
        /// </summary>
        public int ColorCount
        {
            get => _colorCount;
            private set => _colorCount = value;
        }

        private int _planes;
        /// <summary>
        /// Gets icon color planes.
        /// </summary>
        public int Planes
        {
            get => _planes;
            private set => _planes = value;
        }

        private int _bitCount;
        /// <summary>
        /// Gets icon bits per pixel (0 if < 8bpp).
        /// </summary>
        public int BitCount
        {
            get => _bitCount;
            private set => _bitCount = value;
        }

        /// <summary>
        /// Gets icon bits per pixel.
        /// </summary>
        public int ColorDepth
        {
            get
            {
                if (BitCount != 0)
                {
                    return BitCount;
                }

                if (ColorCount == 0)
                {
                    return 0;
                }

                return (int)Math.Log(ColorCount, 2);
            }
        }
        #endregion

        #region Icon Headers Properties
        private IconDir _iconDir;
        /// <summary>
        /// Gets the TAFactory.IconPack.IconDir of the icon.
        /// </summary>
        public IconDir IconDir
        {
            get => _iconDir;
            private set => _iconDir = value;
        }

        private GroupIconDir _groupIconDir;
        /// <summary>
        /// Gets the TAFactory.IconPack.GroupIconDir of the icon.
        /// </summary>
        public GroupIconDir GroupIconDir
        {
            get => _groupIconDir;
            private set => _groupIconDir = value;
        }

        private List<IconDirEntry> _iconDirEntries;
        /// <summary>
        /// Gets a list of TAFactory.IconPack.IconDirEntry of the icon.
        /// </summary>
        public List<IconDirEntry> IconDirEntries
        {
            get => _iconDirEntries;
            private set => _iconDirEntries = value;
        }

        private List<GroupIconDirEntry> _groupIconDirEntries;
        /// <summary>
        /// Gets a list of TAFactory.IconPack.GroupIconDirEntry of the icon.
        /// </summary>
        public List<GroupIconDirEntry> GroupIconDirEntries
        {
            get => _groupIconDirEntries;
            private set => _groupIconDirEntries = value;
        }

        private List<byte[]> _rawData;
        /// <summary>
        /// Gets a list of raw data for each icon image.
        /// </summary>
        public List<byte[]> RawData
        {
            get => _rawData;
            private set => _rawData = value;
        }

        private byte[] _resourceRawData;
        /// <summary>
        /// Gets the icon raw data as a resource data.
        /// </summary>
        public byte[] ResourceRawData
        {
            get => _resourceRawData;
            set => _resourceRawData = value;
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Intializes a new instance of TAFactory.IconPack.IconInfo which contains the information about the givin icon.
        /// </summary>
        /// <param name="icon">A System.Drawing.Icon object to retrieve the information about.</param>
        public IconInfo(Icon icon)
        {
            FileName = null;
            LoadIconInfo(icon);
        }

        /// <summary>
        /// Intializes a new instance of TAFactory.IconPack.IconInfo which contains the information about the icon in the givin file.
        /// </summary>
        /// <param name="fileName">A fully qualified name of the icon file, it can contain environment variables.</param>
        public IconInfo(string fileName)
        {
            FileName = FileName;
            LoadIconInfo(new Icon(fileName));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex()
        {
            int iconIndex = 0;
            IntPtr resBits = Marshal.AllocHGlobal(ResourceRawData.Length);
            Marshal.Copy(ResourceRawData, 0, resBits, ResourceRawData.Length);
            try { iconIndex = Win32.LookupIconIdFromDirectory(resBits, true); }
            finally { Marshal.FreeHGlobal(resBits); }

            return iconIndex;
        }
        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex(Size desiredSize)
        {
            return GetBestFitIconIndex(desiredSize, false);
        }
        /// <summary>
        /// Gets the index of the icon that best fits the current display device.
        /// </summary>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <param name="isMonochrome">Specifies whether to get the monochrome icon or the colored one.</param>
        /// <returns>The icon index.</returns>
        public int GetBestFitIconIndex(Size desiredSize, bool isMonochrome)
        {
            int iconIndex = 0;
            LookupIconIdFromDirectoryExFlags flags = LookupIconIdFromDirectoryExFlags.LR_DEFAULTCOLOR;
            if (isMonochrome)
            {
                flags = LookupIconIdFromDirectoryExFlags.LR_MONOCHROME;
            }

            IntPtr resBits = Marshal.AllocHGlobal(ResourceRawData.Length);
            Marshal.Copy(ResourceRawData, 0, resBits, ResourceRawData.Length);
            try { iconIndex = Win32.LookupIconIdFromDirectoryEx(resBits, true, desiredSize.Width, desiredSize.Height, flags); }
            finally { Marshal.FreeHGlobal(resBits); }

            return iconIndex;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Loads the icon information from the givin icon into class members.
        /// </summary>
        /// <param name="icon">A System.Drawing.Icon object to retrieve the information about.</param>
        private void LoadIconInfo(Icon icon)
        {
            SourceIcon = icon ?? throw new ArgumentNullException("icon");
            MemoryStream inputStream = new MemoryStream();
            SourceIcon.Save(inputStream);

            inputStream.Seek(0, SeekOrigin.Begin);
            IconDir dir = Utility.ReadStructure<IconDir>(inputStream);

            IconDir = dir;
            GroupIconDir = dir.ToGroupIconDir();

            Images = new List<Icon>(dir.Count);
            IconDirEntries = new List<IconDirEntry>(dir.Count);
            GroupIconDirEntries = new List<GroupIconDirEntry>(dir.Count);
            RawData = new List<byte[]>(dir.Count);

            IconDir newDir = dir;
            newDir.Count = 1;
            for (int i = 0; i < dir.Count; i++)
            {
                inputStream.Seek(SizeOfIconDir + i * SizeOfIconDirEntry, SeekOrigin.Begin);

                IconDirEntry entry = Utility.ReadStructure<IconDirEntry>(inputStream);

                IconDirEntries.Add(entry);
                GroupIconDirEntries.Add(entry.ToGroupIconDirEntry(i));

                byte[] content = new byte[entry.BytesInRes];
                inputStream.Seek(entry.ImageOffset, SeekOrigin.Begin);
                inputStream.Read(content, 0, content.Length);
                RawData.Add(content);

                IconDirEntry newEntry = entry;
                newEntry.ImageOffset = SizeOfIconDir + SizeOfIconDirEntry;

                MemoryStream outputStream = new MemoryStream();
                Utility.WriteStructure<IconDir>(outputStream, newDir);
                Utility.WriteStructure<IconDirEntry>(outputStream, newEntry);
                outputStream.Write(content, 0, content.Length);

                outputStream.Seek(0, SeekOrigin.Begin);
                Icon newIcon = new Icon(outputStream);
                outputStream.Close();

                Images.Add(newIcon);
                if (dir.Count == 1)
                {
                    BestFitIconIndex = 0;

                    Width = entry.Width;
                    Height = entry.Height;
                    ColorCount = entry.ColorCount;
                    Planes = entry.Planes;
                    BitCount = entry.BitCount;
                }
            }
            inputStream.Close();
            ResourceRawData = GetIconResourceData();

            if (dir.Count > 1)
            {
                BestFitIconIndex = GetBestFitIconIndex();

                Width = IconDirEntries[BestFitIconIndex].Width;
                Height = IconDirEntries[BestFitIconIndex].Height;
                ColorCount = IconDirEntries[BestFitIconIndex].ColorCount;
                Planes = IconDirEntries[BestFitIconIndex].Planes;
                BitCount = IconDirEntries[BestFitIconIndex].BitCount;
            }

        }
        /// <summary>
        /// Returns the icon's raw data as a resource data.
        /// </summary>
        /// <returns>The icon's raw as a resource data.</returns>
        private byte[] GetIconResourceData()
        {
            MemoryStream outputStream = new MemoryStream();
            Utility.WriteStructure<GroupIconDir>(outputStream, GroupIconDir);
            foreach (GroupIconDirEntry entry in GroupIconDirEntries)
            {
                Utility.WriteStructure<GroupIconDirEntry>(outputStream, entry);
            }

            return outputStream.ToArray();
        }
        #endregion
    }
}
