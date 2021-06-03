using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TAFactory.IconPack;

namespace XtendedMenu
{
    public partial class CustomEntries : Form
    {
        public CustomEntries()
        {
            InitializeComponent();

            PopulateEntryBoxes();
        }

        private void PopulateEntryBoxes()
        {
            AllFilesEntryBox.Items.Clear();
            DirectoriesEntryBox.Items.Clear();
            BackgroundEntryBox.Items.Clear();

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\XtendedMenu\\Settings\\AllFiles", true))
            {
                if (key.GetValue("CustomName") != null)
                {
                    if (key.GetValue("CustomName") is string)
                    {
                        AllFilesEntryBox.Items.Add((string)key.GetValue("CustomName"));
                    }
                    else
                    {
                        AllFilesEntryBox.Items.AddRange((string[])key.GetValue("CustomName"));
                    }
                }
            }
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\XtendedMenu\\Settings\\Directories", true))
            {
                if (key.GetValue("CustomName") != null)
                {
                    if (key.GetValue("CustomName") is string)
                    {
                        DirectoriesEntryBox.Items.Add((string)key.GetValue("CustomName"));
                    }
                    else
                    {
                        DirectoriesEntryBox.Items.AddRange((string[])key.GetValue("CustomName"));
                    }
                }
            }
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\XtendedMenu\\Settings\\Background", true))
            {
                if (key.GetValue("CustomName") != null)
                {
                    if (key.GetValue("CustomName") is string)
                    {
                        BackgroundEntryBox.Items.Add((string)key.GetValue("CustomName"));
                    }
                    else
                    {
                        BackgroundEntryBox.Items.AddRange((string[])key.GetValue("CustomName"));
                    }
                }
            }
        }

        static string CapitalizeFirstLetter(string value)
        {
            value = value.ToLower();
            char[] array = value.ToCharArray();

            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        private void ProcessBrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "exe files (*.exe)|*.exe|All files (*.*)|*.*";
                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Program to run";
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;

                    DialogResult dr = openFileDialog.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        ProcessBox.Text = openFileDialog.FileName;
                        DirectoryBox.Text = Path.GetDirectoryName(openFileDialog.FileName);
                        NameBox.Text = CapitalizeFirstLetter(Path.GetFileNameWithoutExtension(openFileDialog.FileName));
                        GrabIcon(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddEntry(string RegistryLocation)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryLocation, true))
            {
                // CustomName
                if (key.GetValue("CustomName") == null)
                {
                    key.SetValue("CustomName", NameBox.Text, RegistryValueKind.String);
                }
                else
                {
                    if (key.GetValue("CustomName") is string)
                    {
                        if ((string)key.GetValue("CustomName") == NameBox.Text)
                        {
                            MessageBox.Show("This Name already exists.");
                            return;
                        }
                        string CustomName = (string)key.GetValue("CustomName");

                        var myList = new List<string>
                        {
                            CustomName,
                            NameBox.Text
                        };
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomName", newArray, RegistryValueKind.MultiString);
                    }
                    else
                    {
                        var myListCheck = new List<string>();
                        myListCheck.AddRange((string[])key.GetValue("CustomName"));
                        string[] newArrayCheck = myListCheck.ToArray();
                        foreach (string value in newArrayCheck)
                        {
                            if (value == NameBox.Text)
                            {
                                MessageBox.Show("This Name already exists.");
                                return;
                            }
                        }

                        string[] CustomName = (string[])key.GetValue("CustomName");

                        var myList = new List<string>();
                        myList.AddRange(CustomName);
                        myList.Add(NameBox.Text);
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomName", newArray, RegistryValueKind.MultiString);
                    }
                }
                // CustomArguments
                if (key.GetValue("CustomArguments") == null)
                {
                    key.SetValue("CustomArguments", ArgumentsBox.Text, RegistryValueKind.String);
                }
                else
                {
                    if (key.GetValue("CustomArguments") is string)
                    {
                        string CustomArguments = (string)key.GetValue("CustomArguments");

                        var myList = new List<string>
                        {
                            CustomArguments,
                            ArgumentsBox.Text
                        };
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomArguments", newArray, RegistryValueKind.MultiString);
                    }
                    else
                    {
                        string[] CustomArguments = (string[])key.GetValue("CustomArguments");

                        var myList = new List<string>();
                        myList.AddRange(CustomArguments);
                        myList.Add(ArgumentsBox.Text);
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomArguments", newArray, RegistryValueKind.MultiString);
                    }
                }
                // CustomDirectory
                if (key.GetValue("CustomDirectory") == null)
                {
                    key.SetValue("CustomDirectory", DirectoryBox.Text, RegistryValueKind.String);
                }
                else
                {
                    if (key.GetValue("CustomDirectory") is string)
                    {
                        string CustomDirectory = (string)key.GetValue("CustomDirectory");

                        var myList = new List<string>
                        {
                            CustomDirectory,
                            DirectoryBox.Text
                        };
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomDirectory", newArray, RegistryValueKind.MultiString);
                    }
                    else
                    {
                        string[] CustomDirectory = (string[])key.GetValue("CustomDirectory");

                        var myList = new List<string>();
                        myList.AddRange(CustomDirectory);
                        myList.Add(DirectoryBox.Text);
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomDirectory", newArray, RegistryValueKind.MultiString);
                    }
                }
                // CustomProcess
                if (key.GetValue("CustomProcess") == null)
                {
                    key.SetValue("CustomProcess", ProcessBox.Text, RegistryValueKind.String);
                }
                else
                {
                    if (key.GetValue("CustomProcess") is string)
                    {
                        string CustomProcess = (string)key.GetValue("CustomProcess");

                        var myList = new List<string>
                        {
                            CustomProcess,
                            ProcessBox.Text
                        };
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomProcess", newArray, RegistryValueKind.MultiString);
                    }
                    else
                    {
                        string[] CustomProcess = (string[])key.GetValue("CustomProcess");

                        var myList = new List<string>();
                        myList.AddRange(CustomProcess);
                        myList.Add(ProcessBox.Text);
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomProcess", newArray, RegistryValueKind.MultiString);
                    }
                }
                // CustomIcon
                if (key.GetValue("CustomIcon") == null)
                {
                    key.SetValue("CustomIcon", IconBox.Text, RegistryValueKind.String);
                }
                else
                {
                    if (key.GetValue("CustomIcon") is string)
                    {
                        string CustomIcon = (string)key.GetValue("CustomIcon");

                        var myList = new List<string>
                        {
                            CustomIcon,
                            IconBox.Text
                        };
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomIcon", newArray, RegistryValueKind.MultiString);
                    }
                    else
                    {
                        string[] CustomIcon = (string[])key.GetValue("CustomIcon");

                        var myList = new List<string>();
                        myList.AddRange(CustomIcon);
                        myList.Add(IconBox.Text);
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomIcon", newArray, RegistryValueKind.MultiString);
                    }
                }
                // CustomLocation
                string checkedLocations = string.Empty;
                if (AllFilesCB.Checked)
                {
                    checkedLocations += 1;
                }
                if (DirectoriesCB.Checked)
                {
                    checkedLocations += 3;
                }
                if (BackgroundCB.Checked)
                {
                    checkedLocations += 4;
                }
                if (key.GetValue("CustomLocation") == null)
                {
                    key.SetValue("CustomLocation", checkedLocations, RegistryValueKind.String);
                }
                else
                {
                    if (key.GetValue("CustomLocation") is string)
                    {
                        string CustomLocation = (string)key.GetValue("CustomLocation");

                        var myList = new List<string>
                        {
                            CustomLocation,
                            checkedLocations
                        };
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomLocation", newArray, RegistryValueKind.MultiString);
                    }
                    else
                    {
                        string[] CustomLocation = (string[])key.GetValue("CustomLocation");

                        var myList = new List<string>();
                        myList.AddRange(CustomLocation);
                        myList.Add(checkedLocations);
                        string[] newArray = myList.ToArray();

                        key.SetValue("CustomLocation", newArray, RegistryValueKind.MultiString);
                    }
                }

                // Admin
                if (key.GetValue("RunAsAdmin") == null)
                {
                    key.SetValue("RunAsAdmin", adminBox.Checked, RegistryValueKind.String);
                }
                else
                {
                    if (key.GetValue("RunAsAdmin") is string)
                    {
                        string RunAsAdmin = (string)key.GetValue("RunAsAdmin");

                        var myList = new List<string>
                        {
                            RunAsAdmin,
                            adminBox.Checked.ToString()
                        };
                        string[] newArray = myList.ToArray();

                        key.SetValue("RunAsAdmin", newArray, RegistryValueKind.MultiString);
                    }
                    else
                    {
                        string[] RunAsAdmin = (string[])key.GetValue("RunAsAdmin");

                        var myList = new List<string>();
                        myList.AddRange(RunAsAdmin);
                        myList.Add(adminBox.Checked.ToString());
                        string[] newArray = myList.ToArray();

                        key.SetValue("RunAsAdmin", newArray, RegistryValueKind.MultiString);
                    }
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AllFilesEntryBox.Text))
            {
                RemoveEntry("SOFTWARE\\XtendedMenu\\Settings\\AllFiles", AllFilesEntryBox);
            }
            if (!string.IsNullOrEmpty(DirectoriesEntryBox.Text))
            {
                RemoveEntry("SOFTWARE\\XtendedMenu\\Settings\\Directories", DirectoriesEntryBox);
            }
            if (!string.IsNullOrEmpty(BackgroundEntryBox.Text))
            {
                RemoveEntry("SOFTWARE\\XtendedMenu\\Settings\\Background", BackgroundEntryBox);
            }

            if (string.IsNullOrEmpty(NameBox.Text))
            {
                MessageBox.Show("Make sure there is a Name before adding an entry.");
                return;
            }
            if (string.IsNullOrEmpty(ProcessBox.Text))
            {
                MessageBox.Show("Make sure there is a Process before adding an entry.");
                return;
            }

            if (AllFilesCB.Checked)
            {
                AddEntry("SOFTWARE\\XtendedMenu\\Settings\\AllFiles");
            }
            if (DirectoriesCB.Checked)
            {
                AddEntry("SOFTWARE\\XtendedMenu\\Settings\\Directories");
            }
            if (BackgroundCB.Checked)
            {
                AddEntry("SOFTWARE\\XtendedMenu\\Settings\\Background");
            }

            Clear();
            PopulateEntryBoxes();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear(string entryBox = null)
        {
            NameBox.Clear();
            ProcessBox.Clear();
            ArgumentsBox.Clear();
            DirectoryBox.Clear();
            IconBox.Clear();
            adminBox.Checked = false;

            AllFilesCB.Checked = true;
            DirectoriesCB.Checked = true;
            BackgroundCB.Checked = true;

            if (entryBox == "AllFilesEntryBox")
            {
                DirectoriesEntryBox.SelectedIndex = -1;
                BackgroundEntryBox.SelectedIndex = -1;
            }
            else if (entryBox == "DirectoriesEntryBox")
            {
                AllFilesEntryBox.SelectedIndex = -1;
                BackgroundEntryBox.SelectedIndex = -1;
            }
            else if (entryBox == "BackgroundEntryBox")
            {
                AllFilesEntryBox.SelectedIndex = -1;
                DirectoriesEntryBox.SelectedIndex = -1;
            }
            else
            {
                AllFilesEntryBox.SelectedIndex = -1;
                DirectoriesEntryBox.SelectedIndex = -1;
                BackgroundEntryBox.SelectedIndex = -1;
            }

            AddButton.Text = "Add Entry";

            NameBox.Select();
        }

        private void IconBrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(NameBox.Text))
                {
                    MessageBox.Show("Make sure there is a Name before adding an Icon.");
                    return;
                }
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "Icon (*.ico;*.exe;*.dll)|*.ico;*.exe;*.dll";

                    openFileDialog.RestoreDirectory = true;
                    openFileDialog.Title = "Icon to show";
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;

                    DialogResult dr = openFileDialog.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        GrabIcon(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GrabIcon(string fileName)
        {
            string IconPath = AppDomain.CurrentDomain.BaseDirectory + "ICONS\\";
            Directory.CreateDirectory(IconPath);

            if (Path.GetExtension(fileName) == ".exe" || Path.GetExtension(fileName) == ".dll")
            {
                Icon ico = IconHelper.ExtractBestFitIcon(fileName, 0, SystemInformation.SmallIconSize);
                using (FileStream fs = new FileStream(IconPath + NameBox.Text + ".ico", FileMode.Create))
                {
                    ico.Save(fs);
                }
            }
            else
            {
                File.Copy(fileName, IconPath + NameBox.Text + ".ico", true);
            }

            IconBox.Text = IconPath + NameBox.Text + ".ico";

        }

        private void DirectoryBrowseButton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult dr = folderBrowserDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    DirectoryBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void EntryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear("AllFilesEntryBox");

            if (!string.IsNullOrEmpty(AllFilesEntryBox.Text))
            {
                AddButton.Text = "Update Entry";
            }
            else
            {
                AddButton.Text = "Add Entry";
            }
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings\\AllFiles"))
            {
                int index = 0;
                var CustomNameList = new List<string>();
                CustomNameList.AddRange((string[])key.GetValue("CustomName"));
                string[] CustomNameArray = CustomNameList.ToArray();

                foreach (string value in CustomNameArray)
                {
                    if (value == (string)AllFilesEntryBox.SelectedItem)
                    {
                        NameBox.Text = (string)AllFilesEntryBox.SelectedItem;

                        var CustomProcessList = new List<string>();
                        CustomProcessList.AddRange((string[])key.GetValue("CustomProcess"));
                        ProcessBox.Text = CustomProcessList[index];

                        var CustomArgumentsList = new List<string>();
                        CustomArgumentsList.AddRange((string[])key.GetValue("CustomArguments"));
                        string[] CustomArgumentsArray = CustomArgumentsList.ToArray();
                        ArgumentsBox.Text = CustomArgumentsArray[index];

                        var CustomDirectoryList = new List<string>();
                        CustomDirectoryList.AddRange((string[])key.GetValue("CustomDirectory"));
                        string[] CustomDirectoryArray = CustomDirectoryList.ToArray();
                        DirectoryBox.Text = CustomDirectoryArray[index];

                        var CustomIconList = new List<string>();
                        CustomIconList.AddRange((string[])key.GetValue("CustomIcon"));
                        string[] CustomIconArray = CustomIconList.ToArray();
                        IconBox.Text = CustomIconArray[index];
                    }

                    index++;
                }
            }
        }

        private void DirectoriesEntryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear("DirectoriesEntryBox");

            if (!string.IsNullOrEmpty(AllFilesEntryBox.Text))
            {
                AddButton.Text = "Update Entry";
            }
            else
            {
                AddButton.Text = "Add Entry";
            }
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings\\Directories"))
            {
                int index = 0;
                var CustomNameList = new List<string>();
                CustomNameList.AddRange((string[])key.GetValue("CustomName"));
                string[] CustomNameArray = CustomNameList.ToArray();

                foreach (string value in CustomNameArray)
                {
                    if (value == (string)AllFilesEntryBox.SelectedItem)
                    {
                        NameBox.Text = (string)AllFilesEntryBox.SelectedItem;

                        var CustomProcessList = new List<string>();
                        CustomProcessList.AddRange((string[])key.GetValue("CustomProcess"));
                        ProcessBox.Text = CustomProcessList[index];

                        var CustomArgumentsList = new List<string>();
                        CustomArgumentsList.AddRange((string[])key.GetValue("CustomArguments"));
                        string[] CustomArgumentsArray = CustomArgumentsList.ToArray();
                        ArgumentsBox.Text = CustomArgumentsArray[index];

                        var CustomDirectoryList = new List<string>();
                        CustomDirectoryList.AddRange((string[])key.GetValue("CustomDirectory"));
                        string[] CustomDirectoryArray = CustomDirectoryList.ToArray();
                        DirectoryBox.Text = CustomDirectoryArray[index];

                        var CustomIconList = new List<string>();
                        CustomIconList.AddRange((string[])key.GetValue("CustomIcon"));
                        string[] CustomIconArray = CustomIconList.ToArray();
                        IconBox.Text = CustomIconArray[index];
                    }

                    index++;
                }
            }
        }

        private void BackgroundEntryBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear("BackgroundEntryBox");

            if (!string.IsNullOrEmpty(AllFilesEntryBox.Text))
            {
                AddButton.Text = "Update Entry";
            }
            else
            {
                AddButton.Text = "Add Entry";
            }
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\XtendedMenu\\Settings\\Background"))
            {
                int index = 0;
                var CustomNameList = new List<string>();
                CustomNameList.AddRange((string[])key.GetValue("CustomName"));
                string[] CustomNameArray = CustomNameList.ToArray();

                foreach (string value in CustomNameArray)
                {
                    if (value == (string)AllFilesEntryBox.SelectedItem)
                    {
                        NameBox.Text = (string)AllFilesEntryBox.SelectedItem;

                        var CustomProcessList = new List<string>();
                        CustomProcessList.AddRange((string[])key.GetValue("CustomProcess"));
                        ProcessBox.Text = CustomProcessList[index];

                        var CustomArgumentsList = new List<string>();
                        CustomArgumentsList.AddRange((string[])key.GetValue("CustomArguments"));
                        string[] CustomArgumentsArray = CustomArgumentsList.ToArray();
                        ArgumentsBox.Text = CustomArgumentsArray[index];

                        var CustomDirectoryList = new List<string>();
                        CustomDirectoryList.AddRange((string[])key.GetValue("CustomDirectory"));
                        string[] CustomDirectoryArray = CustomDirectoryList.ToArray();
                        DirectoryBox.Text = CustomDirectoryArray[index];

                        var CustomIconList = new List<string>();
                        CustomIconList.AddRange((string[])key.GetValue("CustomIcon"));
                        string[] CustomIconArray = CustomIconList.ToArray();
                        IconBox.Text = CustomIconArray[index];
                    }

                    index++;
                }
            }
        }

        private void AllFilesRemoveButton_Click(object sender, EventArgs e)
        {
            RemoveEntry("SOFTWARE\\XtendedMenu\\Settings\\AllFiles", AllFilesEntryBox);
        }

        private void DirectoriesRemoveButton_Click(object sender, EventArgs e)
        {
            RemoveEntry("SOFTWARE\\XtendedMenu\\Settings\\Directories", DirectoriesEntryBox);
        }

        private void BackgroundRemoveButton_Click(object sender, EventArgs e)
        {
            RemoveEntry("SOFTWARE\\XtendedMenu\\Settings\\Background", BackgroundEntryBox);
        }

        private void RemoveEntry(string RegistryLocation, ComboBox comboBox)
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryLocation, true))
            {
                int index = 0;
                var CustomNameList = new List<string>();
                CustomNameList.AddRange((string[])key.GetValue("CustomName"));
                string[] CustomNameArray = CustomNameList.ToArray();

                foreach (string value in CustomNameArray)
                {
                    if (value == (string)comboBox.SelectedItem)
                    {
                        CustomNameArray = CustomNameArray.Where(w => w != value).ToArray();
                        key.SetValue("CustomName", CustomNameArray, RegistryValueKind.MultiString);


                        var CustomProcessList = new List<string>();
                        CustomProcessList.AddRange((string[])key.GetValue("CustomProcess"));
                        CustomProcessList.RemoveAt(index);
                        string[] CustomProcessArray = CustomProcessList.ToArray();
                        key.SetValue("CustomProcess", CustomProcessArray, RegistryValueKind.MultiString);

                        var CustomArgumentsList = new List<string>();
                        CustomArgumentsList.AddRange((string[])key.GetValue("CustomArguments"));
                        CustomArgumentsList.RemoveAt(index);
                        string[] CustomArgumentsArray = CustomArgumentsList.ToArray();
                        key.SetValue("CustomArguments", CustomArgumentsArray, RegistryValueKind.MultiString);


                        var CustomDirectoryList = new List<string>();
                        CustomDirectoryList.AddRange((string[])key.GetValue("CustomDirectory"));
                        CustomDirectoryList.RemoveAt(index);
                        string[] CustomDirectoryArray = CustomDirectoryList.ToArray();
                        key.SetValue("CustomDirectory", CustomDirectoryArray, RegistryValueKind.MultiString);


                        var CustomIconList = new List<string>();
                        CustomIconList.AddRange((string[])key.GetValue("CustomIcon"));
                        CustomIconList.RemoveAt(index);
                        string[] CustomIconArray = CustomIconList.ToArray();
                        key.SetValue("CustomIcon", CustomIconArray, RegistryValueKind.MultiString);


                        var CustomLocationList = new List<string>();
                        CustomLocationList.AddRange((string[])key.GetValue("CustomLocation"));
                        CustomLocationList.RemoveAt(index);
                        string[] CustomLocationArray = CustomLocationList.ToArray();
                        key.SetValue("CustomLocation", CustomLocationArray, RegistryValueKind.MultiString);


                        var RunAsAdminList = new List<string>();
                        RunAsAdminList.AddRange((string[])key.GetValue("RunAsAdmin"));
                        RunAsAdminList.RemoveAt(index);
                        string[] RunAsAdminArray = RunAsAdminList.ToArray();
                        key.SetValue("RunAsAdmin", RunAsAdminArray, RegistryValueKind.MultiString);
                    }

                    index++;
                }
            }

            Clear();
            PopulateEntryBoxes();
        }
    }
}
