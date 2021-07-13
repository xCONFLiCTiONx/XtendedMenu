using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace XtendedMenu
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            try
            {
                // Logging
                TimeSpan ts = DateTime.Now - File.GetLastAccessTime(EasyLogger.LogFile);
                if (ts.Days > 30)
                {
                    EasyLogger.BackupLogs(EasyLogger.LogFile);
                }
                EasyLogger.AddListener(EasyLogger.LogFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main(args));
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                EasyLogger.Error("Application.ThreadException: Base Exception: " + e.Exception.GetBaseException() + Environment.NewLine + "Exception Message: " + e.Exception.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                EasyLogger.Error("AppDomain.UnhandledException: Exception Object: " + e.ExceptionObject + Environment.NewLine + "Exception Object: " + ((Exception)e.ExceptionObject).Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
