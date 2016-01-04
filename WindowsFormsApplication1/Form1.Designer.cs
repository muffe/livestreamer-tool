namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.Stream = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.qualityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourcebestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.middleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qualityToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.middleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Stream});
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.Location = new System.Drawing.Point(12, 27);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1094, 666);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // Stream
            // 
            this.Stream.Text = "Streams";
            this.Stream.Width = 50000;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.qualityToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1118, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // qualityToolStripMenuItem
            // 
            this.qualityToolStripMenuItem.Checked = true;
            this.qualityToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.qualityToolStripMenuItem.Name = "qualityToolStripMenuItem";
            this.qualityToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.qualityToolStripMenuItem.Text = "Quality";
            // 
            // sourcebestToolStripMenuItem
            // 
            this.sourcebestToolStripMenuItem.Checked = true;
            this.sourcebestToolStripMenuItem.CheckOnClick = true;
            this.sourcebestToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sourcebestToolStripMenuItem.Name = "sourcebestToolStripMenuItem";
            this.sourcebestToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sourcebestToolStripMenuItem.Text = "Source (best)";
            // 
            // highToolStripMenuItem
            // 
            this.highToolStripMenuItem.CheckOnClick = true;
            this.highToolStripMenuItem.Name = "highToolStripMenuItem";
            this.highToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.highToolStripMenuItem.Text = "High";
            // 
            // middleToolStripMenuItem
            // 
            this.middleToolStripMenuItem.Name = "middleToolStripMenuItem";
            this.middleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.middleToolStripMenuItem.Text = "Middle";
            // 
            // lowToolStripMenuItem
            // 
            this.lowToolStripMenuItem.Name = "lowToolStripMenuItem";
            this.lowToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lowToolStripMenuItem.Text = "Low";
            // 
            // qualityToolStripMenuItem1
            // 
            this.qualityToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sourceToolStripMenuItem,
            this.highToolStripMenuItem1,
            this.middleToolStripMenuItem1,
            this.lowToolStripMenuItem1});
            this.qualityToolStripMenuItem1.Name = "qualityToolStripMenuItem1";
            this.qualityToolStripMenuItem1.Size = new System.Drawing.Size(57, 20);
            this.qualityToolStripMenuItem1.Text = "Quality";
            // 
            // sourceToolStripMenuItem
            // 
            this.sourceToolStripMenuItem.Checked = true;
            this.sourceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sourceToolStripMenuItem.Name = "sourceToolStripMenuItem";
            this.sourceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.sourceToolStripMenuItem.Text = "Source (best)";
            this.sourceToolStripMenuItem.Click += new System.EventHandler(this.sourceToolStripMenuItem_Click);
            // 
            // highToolStripMenuItem1
            // 
            this.highToolStripMenuItem1.Name = "highToolStripMenuItem1";
            this.highToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.highToolStripMenuItem1.Text = "High";
            this.highToolStripMenuItem1.Click += new System.EventHandler(this.highToolStripMenuItem1_Click);
            // 
            // middleToolStripMenuItem1
            // 
            this.middleToolStripMenuItem1.Name = "middleToolStripMenuItem1";
            this.middleToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.middleToolStripMenuItem1.Text = "Middle";
            this.middleToolStripMenuItem1.Click += new System.EventHandler(this.middleToolStripMenuItem1_Click);
            // 
            // lowToolStripMenuItem1
            // 
            this.lowToolStripMenuItem1.Name = "lowToolStripMenuItem1";
            this.lowToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.lowToolStripMenuItem1.Text = "Low";
            this.lowToolStripMenuItem1.Click += new System.EventHandler(this.lowToolStripMenuItem1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(76, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(301, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "Choose a game (or just start typing)";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(986, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Refresh streams";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 705);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Twitch.tv livestreamer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Stream;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem qualityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sourcebestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem middleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qualityToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sourceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem middleToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem lowToolStripMenuItem1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
    }
}

