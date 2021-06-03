
namespace XtendedMenu
{
    partial class CustomEntries
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomEntries));
            this.AllFilesEntryBox = new System.Windows.Forms.ComboBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.ProcessBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.ProcessBrowseButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ArgumentsBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DirectoryBox = new System.Windows.Forms.TextBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.AllFilesCB = new System.Windows.Forms.CheckBox();
            this.DirectoriesCB = new System.Windows.Forms.CheckBox();
            this.BackgroundCB = new System.Windows.Forms.CheckBox();
            this.IconBrowseButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.IconBox = new System.Windows.Forms.TextBox();
            this.DirectoryBrowseButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.adminBox = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.DirectoriesEntryBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.BackgroundEntryBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // AllFilesEntryBox
            // 
            this.AllFilesEntryBox.Location = new System.Drawing.Point(79, 181);
            this.AllFilesEntryBox.Name = "AllFilesEntryBox";
            this.AllFilesEntryBox.Size = new System.Drawing.Size(323, 21);
            this.AllFilesEntryBox.TabIndex = 12;
            this.AllFilesEntryBox.SelectedIndexChanged += new System.EventHandler(this.EntryBox_SelectedIndexChanged);
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(80, 12);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(322, 20);
            this.NameBox.TabIndex = 0;
            // 
            // ProcessBox
            // 
            this.ProcessBox.Location = new System.Drawing.Point(80, 38);
            this.ProcessBox.Name = "ProcessBox";
            this.ProcessBox.Size = new System.Drawing.Size(228, 20);
            this.ProcessBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Entry Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Process";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "All Files";
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(218, 262);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(89, 23);
            this.RemoveButton.TabIndex = 15;
            this.RemoveButton.Text = "Remove Entry";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // ProcessBrowseButton
            // 
            this.ProcessBrowseButton.Location = new System.Drawing.Point(313, 37);
            this.ProcessBrowseButton.Name = "ProcessBrowseButton";
            this.ProcessBrowseButton.Size = new System.Drawing.Size(89, 23);
            this.ProcessBrowseButton.TabIndex = 2;
            this.ProcessBrowseButton.Text = "Browse";
            this.ProcessBrowseButton.UseVisualStyleBackColor = true;
            this.ProcessBrowseButton.Click += new System.EventHandler(this.ProcessBrowseButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Arguments";
            // 
            // ArgumentsBox
            // 
            this.ArgumentsBox.Location = new System.Drawing.Point(80, 64);
            this.ArgumentsBox.Name = "ArgumentsBox";
            this.ArgumentsBox.Size = new System.Drawing.Size(228, 20);
            this.ArgumentsBox.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Directory";
            // 
            // DirectoryBox
            // 
            this.DirectoryBox.Location = new System.Drawing.Point(80, 90);
            this.DirectoryBox.Name = "DirectoryBox";
            this.DirectoryBox.Size = new System.Drawing.Size(228, 20);
            this.DirectoryBox.TabIndex = 4;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(313, 262);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(89, 23);
            this.AddButton.TabIndex = 14;
            this.AddButton.Text = "Add Entry";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // AllFilesCB
            // 
            this.AllFilesCB.AutoSize = true;
            this.AllFilesCB.Checked = true;
            this.AllFilesCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AllFilesCB.Location = new System.Drawing.Point(170, 149);
            this.AllFilesCB.Name = "AllFilesCB";
            this.AllFilesCB.Size = new System.Drawing.Size(61, 17);
            this.AllFilesCB.TabIndex = 8;
            this.AllFilesCB.Text = "All Files";
            this.AllFilesCB.UseVisualStyleBackColor = true;
            // 
            // DirectoriesCB
            // 
            this.DirectoriesCB.AutoSize = true;
            this.DirectoriesCB.Checked = true;
            this.DirectoriesCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DirectoriesCB.Location = new System.Drawing.Point(237, 149);
            this.DirectoriesCB.Name = "DirectoriesCB";
            this.DirectoriesCB.Size = new System.Drawing.Size(76, 17);
            this.DirectoriesCB.TabIndex = 10;
            this.DirectoriesCB.Text = "Directories";
            this.DirectoriesCB.UseVisualStyleBackColor = true;
            // 
            // BackgroundCB
            // 
            this.BackgroundCB.AutoSize = true;
            this.BackgroundCB.Checked = true;
            this.BackgroundCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BackgroundCB.Location = new System.Drawing.Point(319, 149);
            this.BackgroundCB.Name = "BackgroundCB";
            this.BackgroundCB.Size = new System.Drawing.Size(84, 17);
            this.BackgroundCB.TabIndex = 11;
            this.BackgroundCB.Text = "Background";
            this.BackgroundCB.UseVisualStyleBackColor = true;
            // 
            // IconBrowseButton
            // 
            this.IconBrowseButton.Location = new System.Drawing.Point(314, 116);
            this.IconBrowseButton.Name = "IconBrowseButton";
            this.IconBrowseButton.Size = new System.Drawing.Size(89, 23);
            this.IconBrowseButton.TabIndex = 7;
            this.IconBrowseButton.Text = "Browse";
            this.IconBrowseButton.UseVisualStyleBackColor = true;
            this.IconBrowseButton.Click += new System.EventHandler(this.IconBrowseButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(49, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Icon";
            // 
            // IconBox
            // 
            this.IconBox.Location = new System.Drawing.Point(80, 119);
            this.IconBox.Name = "IconBox";
            this.IconBox.Size = new System.Drawing.Size(228, 20);
            this.IconBox.TabIndex = 6;
            // 
            // DirectoryBrowseButton
            // 
            this.DirectoryBrowseButton.Location = new System.Drawing.Point(314, 90);
            this.DirectoryBrowseButton.Name = "DirectoryBrowseButton";
            this.DirectoryBrowseButton.Size = new System.Drawing.Size(89, 23);
            this.DirectoryBrowseButton.TabIndex = 5;
            this.DirectoryBrowseButton.Text = "Browse";
            this.DirectoryBrowseButton.UseVisualStyleBackColor = true;
            this.DirectoryBrowseButton.Click += new System.EventHandler(this.DirectoryBrowseButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(123, 262);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(89, 23);
            this.ClearButton.TabIndex = 16;
            this.ClearButton.Text = "Clear All";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // adminBox
            // 
            this.adminBox.AutoSize = true;
            this.adminBox.Location = new System.Drawing.Point(314, 66);
            this.adminBox.Name = "adminBox";
            this.adminBox.Size = new System.Drawing.Size(92, 17);
            this.adminBox.TabIndex = 13;
            this.adminBox.Text = "Run as Admin";
            this.adminBox.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 211);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Directories";
            // 
            // DirectoriesEntryBox
            // 
            this.DirectoriesEntryBox.Location = new System.Drawing.Point(79, 208);
            this.DirectoriesEntryBox.Name = "DirectoriesEntryBox";
            this.DirectoriesEntryBox.Size = new System.Drawing.Size(323, 21);
            this.DirectoriesEntryBox.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 238);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Background";
            // 
            // BackgroundEntryBox
            // 
            this.BackgroundEntryBox.Location = new System.Drawing.Point(79, 235);
            this.BackgroundEntryBox.Name = "BackgroundEntryBox";
            this.BackgroundEntryBox.Size = new System.Drawing.Size(323, 21);
            this.BackgroundEntryBox.TabIndex = 24;
            // 
            // CustomEntries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 298);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.BackgroundEntryBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.DirectoriesEntryBox);
            this.Controls.Add(this.adminBox);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.DirectoryBrowseButton);
            this.Controls.Add(this.IconBrowseButton);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.IconBox);
            this.Controls.Add(this.BackgroundCB);
            this.Controls.Add(this.DirectoriesCB);
            this.Controls.Add(this.AllFilesCB);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DirectoryBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ArgumentsBox);
            this.Controls.Add(this.ProcessBrowseButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProcessBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.AllFilesEntryBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CustomEntries";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox AllFilesEntryBox;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.TextBox ProcessBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button ProcessBrowseButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ArgumentsBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DirectoryBox;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.CheckBox AllFilesCB;
        private System.Windows.Forms.CheckBox DirectoriesCB;
        private System.Windows.Forms.CheckBox BackgroundCB;
        private System.Windows.Forms.Button IconBrowseButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox IconBox;
        private System.Windows.Forms.Button DirectoryBrowseButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.CheckBox adminBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox DirectoriesEntryBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox BackgroundEntryBox;
    }
}