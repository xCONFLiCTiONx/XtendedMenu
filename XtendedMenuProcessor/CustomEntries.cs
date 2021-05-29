using System;
using System.IO;
using System.Windows.Forms;

namespace XtendedMenu
{
    public partial class CustomEntries : Form
    {
        public CustomEntries()
        {
            InitializeComponent();
        }

        private void ProcessBrowseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "exe files (*.txt)|*.exe|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Program to run";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;

                DialogResult dr = openFileDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    ProcessBox.Text = openFileDialog.FileName;
                    DirectoryBox.Text = Path.GetDirectoryName(openFileDialog.FileName);
                }
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CustomEntry.Add(NameBox.Text);
            Properties.Settings.Default.CustomEntryProcess.Add(ProcessBox.Text + ";" + ArgumentsBox.Text + ";" + DirectoryBox.Text);
            Properties.Settings.Default.CustonEntryIcon.Add(IconBox.Text);

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }

        private void IconBrowseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "ico files (*.ico)|*.exe|All files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Title = "Icon to show";
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;

                DialogResult dr = openFileDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    IconBox.Text = openFileDialog.FileName;
                }
            }
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
    }
}
