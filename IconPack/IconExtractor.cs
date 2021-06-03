using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.API;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using System.Drawing;
using TAFactory.Utilities;

namespace TAFactory.IconPack
{
    /// <summary>
    /// Get icon resources (RT_GROUP_ICON and RT_ICON) from an executable module (either a .dll or an .exe file).
    /// </summary>
    public class IconExtractor : IDisposable
    {
        #region Public Propreties
        private string _fileName;
        /// <summary>
        /// A fully quallified name of the executable module.
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            private set { _fileName = value; }
        }

        private IntPtr _moduleHandle;
        /// <summary>
        /// Gets the module handle.
        /// </summary>
        public IntPtr ModuleHandle
        {
            get { return _moduleHandle; }
            private set { _moduleHandle = value; }
        }

        private List<ResourceName> _iconNamesList;
        /// <summary>
        /// Gets a list of icons resource names RT_GROUP_ICON;
        /// </summary>
        public List<ResourceName> IconNamesList
        {
            get { return _iconNamesList; }
            private set { _iconNamesList = value; }
        }

        /// <summary>
        /// Gets number of RT_GROUP_ICON found in the executable module.
        /// </summary>
        public int IconCount
        {
            get { return this.IconNamesList.Count; }
        }
        #endregion

        #region Private Properties
        private Dictionary<int, Icon> _iconCache;
        /// <summary>
        /// Gets or sets the RT_GROUP_ICON cache.
        /// </summary>
        private Dictionary<int, Icon> IconCache
        {
            get { return _iconCache; }
            set { _iconCache = value; }
        }
        #endregion

        #region Constructor/Destructor
        /// <summary>
        /// Initializes a new IconExtractor and loads the executable module into the address space of the calling process.
        /// The executable module can be a .dll or an .exe file.
        /// The specified module can cause other modules to be mapped into the address space.
        /// </summary>
        /// <param name="fileName">The name of the executable module (either a .dll or an .exe file). The file name can contain environment variables (like %SystemRoot%).</param>
        public IconExtractor(string fileName)
        {
            LoadLibrary(fileName);
        }
        /// <summary>
        /// Destructs the IconExtractor object.
        /// </summary>
        ~IconExtractor()
        {
            Dispose();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets a System.Drawing.Icon that represents RT_GROUP_ICON at the givin index.
        /// </summary>
        /// <param name="index">The index of the RT_GROUP_ICON in the executable module.</param>
        /// <returns>Returns System.Drawing.Icon.</returns>
        public Icon GetIconAt(int index)
        {
            if (index < 0 || index >= this.IconCount)
            {
                if (this.IconCount > 0)
                    throw new ArgumentOutOfRangeException("index", index, "Index should be in the range (0-" + this.IconCount.ToString() + ").");
                else
                    throw new ArgumentOutOfRangeException("index", index, "No icons in the list.");
            }

            if (!this.IconCache.ContainsKey(index))
                this.IconCache[index] = GetIconFromLib(index);

            return this.IconCache[index];
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// This function maps a specified executable module into the address space of the calling process.
        /// The executable module can be a .dll or an .exe file.
        /// The specified module can cause other modules to be mapped into the address space.
        /// </summary>
        /// <param name="fileName">The name of the executable module (either a .dll or an .exe file). The file name can contain environment variables (like %SystemRoot%).</param>
        private void LoadLibrary(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            this.FileName = Environment.ExpandEnvironmentVariables(fileName);
            //Load the executable module into memory using LoadLibraryEx API.
            this.ModuleHandle = Win32.LoadLibraryEx(Environment.ExpandEnvironmentVariables(this.FileName), IntPtr.Zero, LoadLibraryExFlags.LOAD_LIBRARY_AS_DATAFILE);
            if (this.ModuleHandle == IntPtr.Zero)
            {
                int errorNum = Marshal.GetLastWin32Error();
                switch ((GetLastErrorResult)errorNum)
                {
                    case GetLastErrorResult.ERROR_FILE_NOT_FOUND:
                        throw new FileNotFoundException("File not found.", this.FileName);
                    case GetLastErrorResult.ERROR_BAD_EXE_FORMAT:
                        throw new ArgumentException("The file '" + this.FileName + "' is not a valid win32 executable or dll.");
                    default:
                        throw new Win32Exception(errorNum);
                }
            }
            
            this.IconNamesList = new List<ResourceName>();
            this.IconCache = new Dictionary<int, Icon>();

            //Enumurate the resource names of RT_GROUP_ICON by calling EnumResourcesCallBack function for each resource of that type.
            Win32.EnumResourceNames(this.ModuleHandle, ResourceTypes.RT_GROUP_ICON, EnumResourcesCallBack, IntPtr.Zero);
        }
        /// <summary>
        /// The callback function that is being called for each resource (RT_GROUP_ICON, RT_ICON) in the executable module.
        /// The function stores the resource name of type RT_GROUP_ICON into the GroupIconsList and 
        /// stores the resource name of type RT_ICON into the IconsList.
        /// </summary>
        /// <param name="hModule">The module handle.</param>
        /// <param name="lpszType">Specifies the type of the resource being enumurated (RT_GROUP_ICON, RT_ICON).</param>
        /// <param name="lpszName">Specifies the name of the resource being enumurated. For more ifnormation, see the Remarks section.</param>
        /// <param name="lParam">Specifies the application defined parameter passed to the EnumResourceNames function.</param>
        /// <returns>This callback function return true to continue enumuration.</returns>
        /// <remarks>
        /// If the high bit of lpszName is not set (=0), lpszName specifies the integer identifier of the givin resource.
        /// Otherwise, it is a pointer to a null terminated string.
        /// If the first character of the string is a pound sign (#), the remaining characters represent a decimal number that specifies the integer identifier of the resource. For example, the string "#258" represents the identifier 258.
        /// #define IS_INTRESOURCE(_r) ((((ULONG_PTR)(_r)) >> 16) == 0)
        /// </remarks>
        private bool EnumResourcesCallBack(IntPtr hModule, ResourceTypes lpszType, IntPtr lpszName, IntPtr lParam)
        {
            switch (lpszType)
            {
                case ResourceTypes.RT_GROUP_ICON:
                    this.IconNamesList.Add(new ResourceName(lpszName));
                    break;
                default:
                    break;
            }

            return true;
        }
        /// <summary>
        /// Gets a System.Drawing.Icon that represents RT_GROUP_ICON at the givin index from the executable module.
        /// </summary>
        /// <param name="index">The index of the RT_GROUP_ICON in the executable module.</param>
        /// <returns>Returns System.Drawing.Icon.</returns>
        private Icon GetIconFromLib(int index)
        {
            byte[] resourceData = GetResourceData(this.ModuleHandle, this.IconNamesList[index], ResourceTypes.RT_GROUP_ICON);
            //Convert the resouce into an .ico file image.
            using (MemoryStream inputStream = new MemoryStream(resourceData))
            using (MemoryStream destStream = new MemoryStream())
            {
                //Read the GroupIconDir header.
                GroupIconDir grpDir = Utility.ReadStructure<GroupIconDir>(inputStream);

                int numEntries = grpDir.Count;
                int iconImageOffset = IconInfo.SizeOfIconDir + numEntries * IconInfo.SizeOfIconDirEntry;

                //Write the IconDir header.
                Utility.WriteStructure<IconDir>(destStream, grpDir.ToIconDir());
                for (int i = 0; i < numEntries; i++)
                {
                    //Read the GroupIconDirEntry.
                    GroupIconDirEntry grpEntry = Utility.ReadStructure<GroupIconDirEntry>(inputStream);

                    //Write the IconDirEntry.
                    destStream.Seek(IconInfo.SizeOfIconDir + i * IconInfo.SizeOfIconDirEntry, SeekOrigin.Begin);
                    Utility.WriteStructure<IconDirEntry>(destStream, grpEntry.ToIconDirEntry(iconImageOffset));

                    //Get the icon image raw data and write it to the stream.
                    byte[] imgBuf = GetResourceData(this.ModuleHandle, grpEntry.ID, ResourceTypes.RT_ICON);
                    destStream.Seek(iconImageOffset, SeekOrigin.Begin);
                    destStream.Write(imgBuf, 0, imgBuf.Length);
                    
                    //Append the iconImageOffset.
                    iconImageOffset += imgBuf.Length;
                }
                destStream.Seek(0, SeekOrigin.Begin);
                return new Icon(destStream);
            }
        }
        /// <summary>
        /// Extracts the raw data of the resource from the module.
        /// </summary>
        /// <param name="hModule">The module handle.</param>
        /// <param name="resrouceName">The name of the resource.</param>
        /// <param name="resourceType">The type of the resource.</param>
        /// <returns>The resource raw data.</returns>
        private static byte[] GetResourceData(IntPtr hModule, ResourceName resourceName, ResourceTypes resourceType)
        {
            //Find the resource in the module.
            IntPtr hResInfo = IntPtr.Zero;
            try { hResInfo = Win32.FindResource(hModule, resourceName.Value, resourceType); }
            finally { resourceName.Free(); }
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Load the resource.
            IntPtr hResData = Win32.LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Lock the resource to read data.
            IntPtr hGlobal = Win32.LockResource(hResData);
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Get the resource size.
            int resSize = Win32.SizeofResource(hModule, hResInfo);
            if (resSize == 0)
            {
                throw new Win32Exception();
            }
            //Allocate the requested size.
            byte[] buf = new byte[resSize];
            //Copy the resource data into our buffer.
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }
        /// <summary>
        /// Extracts the raw data of the resource from the module.
        /// </summary>
        /// <param name="hModule">The module handle.</param>
        /// <param name="resrouceName">The identifier of the resource.</param>
        /// <param name="resourceType">The type of the resource.</param>
        /// <returns>The resource raw data.</returns>
        private static byte[] GetResourceData(IntPtr hModule, int resourceId, ResourceTypes resourceType)
        {
            //Find the resource in the module.
            IntPtr hResInfo = Win32.FindResource(hModule, (IntPtr) resourceId, resourceType); 
            if (hResInfo == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Load the resource.
            IntPtr hResData = Win32.LoadResource(hModule, hResInfo);
            if (hResData == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Lock the resource to read data.
            IntPtr hGlobal = Win32.LockResource(hResData);
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            //Get the resource size.
            int resSize = Win32.SizeofResource(hModule, hResInfo);
            if (resSize == 0)
            {
                throw new Win32Exception();
            }
            //Allocate the requested size.
            byte[] buf = new byte[resSize];
            //Copy the resource data into our buffer.
            Marshal.Copy(hGlobal, buf, 0, buf.Length);

            return buf;
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases the resources of that object.
        /// </summary>
        public void Dispose()
        {
            if (this.ModuleHandle != IntPtr.Zero)
            {
                try { Win32.FreeLibrary(this.ModuleHandle); }
                catch { }
                this.ModuleHandle = IntPtr.Zero;
            }
            if (this.IconNamesList != null)
                this.IconNamesList.Clear();
        }
        #endregion
    }
}
