using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using XtendedMenu.Properties;
using static XtendedMenu.SendMessage;

[assembly: CLSCompliant(true)]
namespace XtendedMenu
{
    public partial class Main : Form
    {
        public static string JunctionName = string.Empty;
        private static bool CountingFilesOperation = true;
        private static int FilesCount = 0;
        private static readonly string Operation = "Counting Files";
        private bool PauseOperation = false;
        private string MainFolderName;
        private static bool ThreadRunning = false;
        private bool NoMoreThreads = false;
        private static int current = 0;
        private static IEnumerable<string> RootPath;
        private bool KeepAlive = false;
        private readonly string CurrentUser = Environment.UserDomainName + "\\" + Environment.UserName;
        private bool NoErrors;
        private bool Ready = false;
        internal static bool IsElevated => WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);

        public Main(string[] args)
        {
            try
            {
                InitializeComponent();

                Shown += Main_Shown;

                RegistryKey InstallInfo = Registry.CurrentUser.OpenSubKey("SOFTWARE\\XtendedMenu\\Settings", false);

                if (InstallInfo == null)
                {
                    if (args.Length > 0)
                    {
                        if (args[0].ToLower() == "-install" || args[0].ToLower() == "-i" || args[0].ToLower() == "/install" || args[0].ToLower() == "/i")
                        {
                            Installation.InstallerClass("-install");
                        }
                        if (args[0].ToLower() == "-uninstall" || args[0].ToLower() == "-u" || args[0].ToLower() == "/uninstall" || args[0].ToLower() == "/u")
                        {
                            Installation.InstallerClass("-uninstall");
                        }
                    }
                    else
                    {
                        Installation.InstallerClass("-install");
                    }
                }
                else
                {
                    if (args.Length == 0)
                    {
                        using (new Settings())
                        {
                            new Settings().ShowDialog();
                        }
                    }
                    if (args.Length > 0)
                    {
                        if (args[0].ToLower() == "-install" || args[0].ToLower() == "-i" || args[0].ToLower() == "/install" || args[0].ToLower() == "/i")
                        {
                            Installation.InstallerClass("-install");
                        }
                        if (args[0].ToLower() == "-uninstall" || args[0].ToLower() == "-u" || args[0].ToLower() == "/uninstall" || args[0].ToLower() == "/u")
                        {
                            Installation.InstallerClass("-uninstall");
                        }
                        // Settings
                        if (args[0].ToLower() == "-settings" || args[0].ToLower() == "-s" || args[0].ToLower() == "/settings" || args[0].ToLower() == "/s")
                        {
                            using (new Settings())
                            {
                                new Settings().ShowDialog();
                            }
                        }
                        // Custom Entries
                        if (args[0].ToLower() == "-customentries" || args[0].ToLower() == "/customentries" || args[0].ToLower() == "-c" || args[0].ToLower() == "/c")
                        {
                            if (IsElevated)
                            {
                                using (new Settings("customEntries"))
                                {
                                    new Settings().ShowDialog();
                                }
                            }
                            else
                            {
                                using (Process proc = new Process())
                                {
                                    proc.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                                    proc.StartInfo.Arguments = "-CustomEntries";
                                    proc.StartInfo.UseShellExecute = true;
                                    proc.StartInfo.Verb = "runas";
                                    proc.Start();
                                };

                                Environment.Exit(0);
                            }
                        }
                        // Refresh Explorer
                        if (args[0].ToLower() == "-refresh" || args[0].ToLower() == "/refresh" || args[0].ToLower() == "-r" || args[0].ToLower() == "/r")
                        {
                            ExplorerRefresh.RefreshWindowsExplorer();
                        }
                    }
                    if (args.Length > 1)
                    {
                        ExecuteCommands(args);
                    }
                    if (!KeepAlive)
                    {
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Environment.Exit(0);
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            Ready = true;
        }

        private void ExecuteCommands(string[] args)
        {
            try
            {
                if (args[1].ToLower() == "-makelink")
                {
                    string[] selectPaths = args[0].Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    int items = 0;
                    string selectedItem = string.Empty;
                    List<string> Paths = new List<string>();
                    string PathName = string.Empty;
                    try
                    {
                        foreach (string selected in selectPaths)
                        {
                            selectedItem = selected;
                            items++;
                        }
                        if (items == 1)
                        {
                            using (new InputBox())
                            {
                                new InputBox(Path.GetFileName(selectedItem)).ShowDialog();
                            }

                            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                            {
                                ofd.Description = Path.GetFileName(selectedItem);
                                if (ofd.ShowDialog() == DialogResult.OK)
                                {
                                    PathName = ofd.SelectedPath + @"\" + JunctionName;
                                    StartProcess.StartInfo("cmd.exe", "/c mklink /J " + "\"" + PathName + "\"" + " " + "\"" + selectedItem + "\"", true, true, true);
                                    Paths.Add(PathName);
                                }
                            }
                            if (PathName != string.Empty)
                            {
                                StartProcess.StartInfo(PathName);
                            }
                        }
                        else
                        {
                            MessageForm("Please try again only selecting one directory at a time while creating junctions.", "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, false);
                        }
                    }
                    catch (Win32Exception ex)
                    {
                        MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (args[1].ToLower() == "-catchhandler")
                {
                    MessageLogging(args[0], MessageBoxIcon.Error);
                }
                if (args[1].ToLower() == "-attributesmenu")
                {
                    string[] array = args[0].Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    AttributesMenu menu = new AttributesMenu(array);
                    using (menu)
                    {
                        menu.ShowDialog();
                    }
                    Environment.Exit(0);
                }
                if (args[1].ToLower() == "-firewallfiles")
                {
                    try
                    {
                        KeepAlive = true;
                        NoErrors = true;
                        string[] array = args[0].Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        Thread thread = new Thread(() => MultiSelectFirewallFiles(array))
                        {
                            IsBackground = true
                        };
                        thread.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (args[1].ToLower() == "-firewallfolder")
                {
                    KeepAlive = true;
                    Text = "Blocking Files";
                    Thread thread = new Thread(() => FirewallDirectory(args[0], "-firewall"))
                    {
                        IsBackground = true
                    };
                    thread.Start();
                }
                if (args[1].ToLower() == "-ownership")
                {
                    string[] array = args[0].Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in array)
                    {
                        if (File.Exists(item))
                        {
                            StartProcess.StartInfo("cmd.exe", "/c takeown /f " + "\"" + item + "\"" + " /SKIPSL && icacls " + "\"" + item + "\"" + " /grant:r " + "\"" + CurrentUser + "\"" + ":F /t /l /c /q", false, true);
                        }
                        if (Directory.Exists(item))
                        {
                            StartProcess.StartInfo("cmd.exe", "/c takeown /f " + "\"" + item + "\"" + " /r /SKIPSL /d y && icacls " + "\"" + item + "\"" + " /grant:r " + "\"" + CurrentUser + "\"" + ":F /t /l /c /q", false, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private void MultiSelectFirewallFiles(string[] array)
        {
            try
            {
                // wait for the form to load
                while (!Ready)
                {
                    Thread.Sleep(200);
                    Application.DoEvents();
                }
                try
                {
                    Invoke(new Action(() =>
                    {
                        progressBar1.Enabled = true;

                        progressBar1.Style = ProgressBarStyle.Marquee;

                        PauseButton.Enabled = false;
                        StopButton.Enabled = false;

                        label3.Text = "Task:";

                        label2.Text = "Firewall";

                        label1.Text = "Add files to Windows Defender Firewall inbound and outbound rules.";
                    }));
                }
                catch (Exception ex)
                {
                    EasyLogger.Error(ex);
                }

                foreach (string item in array)
                {
                    try
                    {
                        MainFolderName = Path.GetFileName(item);

                        string path = Path.GetDirectoryName(item);
                        string title = Path.GetFileName(path) + " - " + Path.GetFileName(item);
                        string arguments = "advfirewall firewall add rule name=" + "\"" + MainFolderName + " - " + title + "\"" + " dir=out program=" + "\"" + item + "\"" + " action=block";

                        StartProcess.StartInfo("netsh.exe", arguments, true, true, true);

                        path = Path.GetDirectoryName(item);
                        title = Path.GetFileName(path) + " - " + Path.GetFileName(item);
                        arguments = "advfirewall firewall add rule name=" + "\"" + MainFolderName + " - " + title + "\"" + " dir=in program=" + "\"" + item + "\"" + " action=block";

                        StartProcess.StartInfo("netsh.exe", arguments, true, true, true);
                    }
                    catch (Exception ex)
                    {
                        MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        NoErrors = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NoErrors = false;
            }

            try
            {
                // wait for the form to load
                while (!Ready)
                {
                    Thread.Sleep(200);
                    Application.DoEvents();
                }
                try
                {
                    Invoke(new Action(() =>
                    {
                        Hide();
                        if (NoErrors)
                        {
                            DialogResult results = MessageForm("File(s) added to Windows Defender Firewall successfully." + Environment.NewLine +
                                "Would you like to view the results?", "Task Completed Successfully", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, false);
                            if (results == DialogResult.Yes)
                            {
                                StartProcess.StartInfo("wf.msc");
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    EasyLogger.Error(ex);
                }
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Environment.Exit(0);
        }


        private void FirewallDirectory(string args, string operation)
        {
            try
            {
                int threads = 0;
                int ThreadsCount = 0;
                string[] array = args.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                label1.Text = "Please wait while file count is enumerated...";
                foreach (string item in array)
                {
                    threads++;
                }
                foreach (string item in array)
                {
                    MainFolderName = Path.GetFileName(item);
                    // Firewall Operation
                    if (operation == "-firewall")
                    {
                        label1.Text = "Adding Files to Windows Firewall";
                        RootPath = Directory.EnumerateFiles(item, "*.*", SearchOption.AllDirectories)
                                 .Where(s => s.EndsWith(".exe", StringComparison.CurrentCulture) || s.EndsWith(".dll", StringComparison.CurrentCulture));
                        Thread thread = new Thread(() => AddToFirewall("-firewall"))
                        {
                            IsBackground = true
                        };
                        thread.Start();
                    }

                    ThreadsCount++;

                    ThreadRunning = true;
                    while (ThreadRunning && ThreadsCount != threads)
                    {
                        Thread.Sleep(250);
                    }
                }
                NoMoreThreads = true;
            }
            catch (Exception ex)
            {
                MessageForm(ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Source + Environment.NewLine + ex.GetBaseException() + Environment.NewLine + ex.TargetSite, "XtendedMenu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }
        private void EnumerateFiles()
        {
        TryAgain:;
            try
            {
                label2.Text = "Please wait while file count is enumerated...";
                if (RootPath != null)
                {
                    foreach (string file in RootPath)
                    {
                        while (PauseOperation)
                        {
                            Thread.Sleep(500);
                        }

                        FilesCount++;
                    }
                }

                CountingFilesOperation = false;
            }
            catch (Exception ex)
            {
                EasyLogger.Error(ex);
                goto TryAgain;
            }
        }
        private void AddToFirewall(string operation)
        {
        TryAgain:;
            try
            {
                int dotCount = 0;
                FilesCount = 0;
                Thread thread = new Thread(() => EnumerateFiles())
                {
                    IsBackground = true
                };
                thread.Start();
                progressBar1.Value = 0;
                current = 0;

                if (operation == "-firewall" && RootPath != null)
                {
                    while (CountingFilesOperation)
                    {
                        Thread.Sleep(500);

                        dotCount++;
                        if (dotCount == 2)
                        {
                            label1.Text = Operation;
                        }

                        if (dotCount == 4)
                        {
                            label1.Text = Operation + ".";
                        }

                        if (dotCount == 6)
                        {
                            label1.Text = Operation + "..";
                        }
                        if (dotCount == 8)
                        {
                            label1.Text = Operation + "...";
                            dotCount = 0;
                        }
                        Application.DoEvents();
                    }
                    label1.Text = "Adding Files to Windows Firewall (outbound)";
                    foreach (string item in RootPath)
                    {
                        try
                        {
                            while (PauseOperation)
                            {
                                Thread.Sleep(500);
                            }

                            current++;
                            string path = Path.GetDirectoryName(item);
                            string title = Path.GetFileName(path) + " - " + Path.GetFileName(item);
                            string arguments = "advfirewall firewall add rule name=" + "\"" + MainFolderName + " - " + title + "\"" + " dir=out program=" + "\"" + item + "\"" + " action=block";
                            progressBar1.Value = (current * 100) / FilesCount;
                            try
                            {
                                Text = "Blocking Files:  " + progressBar1.Value + "%";
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                EasyLogger.Error(ex);
                                continue;
                            }

                            string lastFolderName = Path.GetFileName(Path.GetDirectoryName(item));
                            label2.Text = "..\\" + item.Substring(item.IndexOf(lastFolderName));

                            Application.DoEvents();
                            StartProcess.StartInfo("netsh.exe", arguments, true, true, true);
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            EasyLogger.Error(ex);
                            continue;
                        }
                    }
                    current = 0;
                    progressBar1.Value = 0;
                    label1.Text = "Adding Files to Windows Firewall (inbound)";
                    foreach (string item in RootPath)
                    {
                        try
                        {
                            while (PauseOperation)
                            {
                                Thread.Sleep(500);
                            }

                            current++;
                            string path = Path.GetDirectoryName(item);
                            string title = Path.GetFileName(path) + " - " + Path.GetFileName(item);
                            string arguments = "advfirewall firewall add rule name=" + "\"" + MainFolderName + " - " + title + "\"" + " dir=in program=" + "\"" + item + "\"" + " action=block";
                            progressBar1.Value = (current * 100) / FilesCount;
                            try
                            {
                                Text = "Blocking Files:  " + progressBar1.Value + "%";
                            }
                            catch (ArgumentOutOfRangeException ex)
                            {
                                EasyLogger.Error(ex);
                                continue;
                            }

                            string lastFolderName = Path.GetFileName(Path.GetDirectoryName(item));
                            label2.Text = "..\\" + item.Substring(item.IndexOf(lastFolderName));

                            Application.DoEvents();
                            StartProcess.StartInfo("netsh.exe", arguments, true, true, true);
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            EasyLogger.Error(ex);
                            continue;
                        }
                    }
                    Thread.Sleep(1500);
                    if (current > 0 && NoMoreThreads)
                    {
                        Hide();

                        DialogResult results = MessageForm("All Files with extension exe and dll have been added to the Windows Defender Firewall." + Environment.NewLine +
                            "Would you like to view the results?", "Task Completed Successfully", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, false);
                        if (results == DialogResult.Yes)
                        {
                            StartProcess.StartInfo("wf.msc");
                        }
                    }
                    else if (current == 0)
                    {
                        Hide();

                        MessageForm("No .exe or .dll files were found in the given directory.", "Task Completed with Errors", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2, false);
                    }
                }
                ThreadRunning = false;
                if (NoMoreThreads)
                {
                    ExplorerRefresh.RefreshWindowsExplorer();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                EasyLogger.Error(ex);
                goto TryAgain;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            if (!PauseOperation)
            {
                PauseButton.Image = Resources.buttonContinue;
                PauseOperation = true;
            }
            else
            {
                PauseButton.Image = Resources.buttonPause;
                PauseOperation = false;
            }
        }
    }
}
