using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.API;
using System.Runtime.InteropServices;
using System.IO;
using TAFactory.Utilities;

namespace TAFactory.IconPack
{
    #region Enumuration
    //[Flags]
    public enum IconFlags : int
    {
        Icon              = 0x000000100,     // get icon
        LinkOverlay       = 0x000008000,     // put a link overlay on icon
        Selected          = 0x000010000,     // show icon in selected state
        LargeIcon         = 0x000000000,     // get large icon
        SmallIcon         = 0x000000001,     // get small icon
        OpenIcon          = 0x000000002,     // get open icon
        ShellIconSize     = 0x000000004,     // get shell size icon
    }
    #endregion

    /// <summary>
    /// Contains helper function to help dealing with System.Drawing.Icon.
    /// </summary>
    public static class IconHelper
    {
        #region Public Methods
        /// <summary>
        /// Returns TAFactory.IconPack.IconInfo object that holds the information about the icon.
        /// </summary>
        /// <param name="icon">System.Drawing.Icon to get the information about.</param>
        /// <returns>TAFactory.IconPack.IconInfo object that holds the information about the icon.</returns>
        public static IconInfo GetIconInfo(Icon icon)
        {
            return new IconInfo(icon);
        }
        /// <summary>
        /// Returns TAFactory.IconPack.IconInfo object that holds the information about the icon.
        /// </summary>
        /// <param name="icon">The icon file path.</param>
        /// <returns>TAFactory.IconPack.IconInfo object that holds the information about the icon.</returns>
        public static IconInfo GetIconInfo(string fileName)
        {
            return new IconInfo(fileName);
        }
        
        /// <summary>
        /// Extracts an icon from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <param name="iconIndex">The index of the icon in the executable module.</param>
        /// <returns>A System.Drawing.Icon extracted from the file at the specified index in case of an executable module.</returns>
        public static Icon ExtractIcon(string fileName, int iconIndex)
        {
            Icon icon = null;
            //Try to load the file as icon file.
            try { icon = new Icon(Environment.ExpandEnvironmentVariables(fileName)); }
            catch { }

            if (icon != null) //The file was an icon file, return the icon.
                return icon;

            //Load the file as an executable module.
            using (IconExtractor extractor = new IconExtractor(fileName))
            {
                return extractor.GetIconAt(iconIndex);
            }
        }
        /// <summary>
        /// Extracts all the icons from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <returns>
        /// A list of System.Drawing.Icon found in the file.
        /// If the file was an icon file, it will return a list containing a single icon.
        /// </returns>
        public static List<Icon> ExtractAllIcons(string fileName)
        {
            Icon icon = null;
            List<Icon> list = new List<Icon>();
            //Try to load the file as icon file.
            try { icon = new Icon(Environment.ExpandEnvironmentVariables(fileName)); }
            catch { }

            if (icon != null) //The file was an icon file.
            {
                list.Add(icon);
                return list;
            }

            //Load the file as an executable module.
            using (IconExtractor extractor = new IconExtractor(fileName))
            {
                for (int i = 0; i < extractor.IconCount; i++)
                {
                    list.Add(extractor.GetIconAt(i));
                }
            }
            return list;
        }
        
        /// <summary>
        /// Splits the group icon into a list of icons (the single icon file can contain a set of icons).
        /// </summary>
        /// <param name="icon">The System.Drawing.Icon need to be splitted.</param>
        /// <returns>List of System.Drawing.Icon.</returns>
        public static List<Icon> SplitGroupIcon(Icon icon)
        {
            IconInfo info = new IconInfo(icon);
            return info.Images;
        }

        /// <summary>
        /// Gets the System.Drawing.Icon that best fits the current display device.
        /// </summary>
        /// <param name="icon">System.Drawing.Icon to be searched.</param>
        /// <returns>System.Drawing.Icon that best fit the current display device.</returns>
        public static Icon GetBestFitIcon(Icon icon)
        {
            IconInfo info = new IconInfo(icon);
            int index = info.GetBestFitIconIndex();
            return info.Images[index];
        }
        /// <summary>
        /// Gets the System.Drawing.Icon that best fits the current display device.
        /// </summary>
        /// <param name="icon">System.Drawing.Icon to be searched.</param>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <returns>System.Drawing.Icon that best fit the current display device.</returns>
        public static Icon GetBestFitIcon(Icon icon, Size desiredSize)
        {
            IconInfo info = new IconInfo(icon);
            int index = info.GetBestFitIconIndex(desiredSize);
            return info.Images[index];
        }
        /// <summary>
        /// Gets the System.Drawing.Icon that best fits the current display device.
        /// </summary>
        /// <param name="icon">System.Drawing.Icon to be searched.</param>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <param name="isMonochrome">Specifies whether to get the monochrome icon or the colored one.</param>
        /// <returns>System.Drawing.Icon that best fit the current display device.</returns>
        public static Icon GetBestFitIcon(Icon icon, Size desiredSize, bool isMonochrome)
        {
            IconInfo info = new IconInfo(icon);
            int index = info.GetBestFitIconIndex(desiredSize, isMonochrome);
            return info.Images[index];
        }

        /// <summary>
        /// Extracts an icon (that best fits the current display device) from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <param name="iconIndex">The index of the icon in the executable module.</param>
        /// <returns>A System.Drawing.Icon (that best fits the current display device) extracted from the file at the specified index in case of an executable module.</returns>
        public static Icon ExtractBestFitIcon(string fileName, int iconIndex)
        {
            Icon icon = ExtractIcon(fileName, iconIndex);
            return GetBestFitIcon(icon);
        }
        /// <summary>
        /// Extracts an icon (that best fits the current display device) from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <param name="iconIndex">The index of the icon in the executable module.</param>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <returns>A System.Drawing.Icon (that best fits the current display device) extracted from the file at the specified index in case of an executable module.</returns>
        public static Icon ExtractBestFitIcon(string fileName, int iconIndex, Size desiredSize)
        {
            Icon icon = ExtractIcon(fileName, iconIndex);
            return GetBestFitIcon(icon, desiredSize);
        }
        /// <summary>
        /// Extracts an icon (that best fits the current display device) from a givin icon file or an executable module (.dll or an .exe file).
        /// </summary>
        /// <param name="fileName">The path of the icon file or the executable module.</param>
        /// <param name="iconIndex">The index of the icon in the executable module.</param>
        /// <param name="desiredSize">Specifies the desired size of the icon.</param>
        /// <param name="isMonochrome">Specifies whether to get the monochrome icon or the colored one.</param>
        /// <returns>A System.Drawing.Icon (that best fits the current display device) extracted from the file at the specified index in case of an executable module.</returns>
        public static Icon ExtractBestFitIcon(string fileName, int iconIndex, Size desiredSize, bool isMonochrome)
        {
            Icon icon = ExtractIcon(fileName, iconIndex);
            return GetBestFitIcon(icon, desiredSize, isMonochrome);
        }

        /// <summary>
        /// Gets icon associated with the givin file.
        /// </summary>
        /// <param name="fileName">The file path (both absolute and relative paths are valid).</param>
        /// <param name="flags">Specifies which icon to be retrieved (Larg, Small, Selected, Link Overlay and Shell Size).</param>
        /// <returns>A System.Drawing.Icon associated with the givin file.</returns>
        public static Icon GetAssociatedIcon(string fileName, IconFlags flags)
        {
            flags |= IconFlags.Icon;
            SHFILEINFO fileInfo = new SHFILEINFO();
            IntPtr result = Win32.SHGetFileInfo(fileName, 0, ref fileInfo, (uint)Marshal.SizeOf(fileInfo), (SHGetFileInfoFlags) flags);

            if (fileInfo.hIcon == IntPtr.Zero)
                return null;

            return Icon.FromHandle(fileInfo.hIcon);
        }
        /// <summary>
        /// Gets large icon associated with the givin file.
        /// </summary>
        /// <param name="fileName">The file path (both absolute and relative paths are valid).</param>
        /// <returns>A System.Drawing.Icon associated with the givin file.</returns>
        public static Icon GetAssociatedLargeIcon(string fileName)
        {
            return GetAssociatedIcon(fileName, IconFlags.LargeIcon);
        }
        /// <summary>
        /// Gets small icon associated with the givin file.
        /// </summary>
        /// <param name="fileName">The file path (both absolute and relative paths are valid).</param>
        /// <returns>A System.Drawing.Icon associated with the givin file.</returns>
        public static Icon GetAssociatedSmallIcon(string fileName)
        {
            return GetAssociatedIcon(fileName, IconFlags.SmallIcon);
        }

        /// <summary>
        /// Merges a list of icons into one single icon.
        /// </summary>
        /// <param name="icons">The icons to be merged.</param>
        /// <returns>System.Drawing.Icon that contains all the images of the givin icons.</returns>
        public static Icon Merge(params Icon[] icons)
        {
            List<IconInfo> list = new List<IconInfo>(icons.Length);
            int numImages = 0;
            foreach (Icon icon in icons)
            {
                if (icon != null)
                {
                    IconInfo info = new IconInfo(icon);
                    list.Add(info);
                    numImages += info.Images.Count;
                }
            }
            if (list.Count == 0)
            {
                throw new ArgumentNullException("icons", "The icons list should contain at least one icon.");
            }

            //Write the icon to a stream.
            MemoryStream outputStream = new MemoryStream();
            int imageIndex = 0;
            int imageOffset = IconInfo.SizeOfIconDir + numImages * IconInfo.SizeOfIconDirEntry;
            for (int i = 0; i < list.Count; i++)
            {
                IconInfo iconInfo = list[i];
                //The firs image, we should write the icon header.
                if (i == 0)
                {
                    //Get the IconDir and update image count with the new count.
                    IconDir dir = iconInfo.IconDir;
                    dir.Count = (short)numImages;

                    //Write the IconDir header.
                    outputStream.Seek(0, SeekOrigin.Begin);
                    Utility.WriteStructure<IconDir>(outputStream, dir);
                }
                //For each image in the current icon, we should write the IconDirEntry and the image raw data.
                for (int j = 0; j < iconInfo.Images.Count; j++)
                {
                    //Get the IconDirEntry and update the ImageOffset to the new offset.
                    IconDirEntry entry = iconInfo.IconDirEntries[j];
                    entry.ImageOffset = imageOffset;

                    //Write the IconDirEntry to the stream.
                    outputStream.Seek(IconInfo.SizeOfIconDir + imageIndex * IconInfo.SizeOfIconDirEntry, SeekOrigin.Begin);
                    Utility.WriteStructure<IconDirEntry>(outputStream, entry);

                    //Write the image raw data.
                    outputStream.Seek(imageOffset, SeekOrigin.Begin);
                    outputStream.Write(iconInfo.RawData[j], 0, entry.BytesInRes);

                    //Update the imageIndex and the imageOffset
                    imageIndex++;
                    imageOffset += entry.BytesInRes;
                }
            }

            //Create the icon from the stream.
            outputStream.Seek(0, SeekOrigin.Begin);
            Icon resultIcon = new Icon(outputStream);
            outputStream.Close();

            return resultIcon;
        }
        #endregion
    }
}
