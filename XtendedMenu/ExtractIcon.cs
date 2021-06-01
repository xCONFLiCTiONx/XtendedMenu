using System;
using System.Drawing;

namespace XtendedMenu
{
    public class ExtractIcon
    {
        public static Icon ExtractIconFromFilePath(string executablePath)
        {
            Icon result = (Icon)null;

            try
            {
                result = Icon.ExtractAssociatedIcon(executablePath);
            }
            catch (Exception)
            {
                EasyLogger.Error("Unable to extract the icon from the binary");
            }

            return result;
        }
    }
}
