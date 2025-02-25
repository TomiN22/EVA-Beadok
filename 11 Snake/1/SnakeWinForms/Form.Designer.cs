namespace SnakeWinForms
{
    partial class Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            newGameToolStripMenuItem = new ToolStripMenuItem();
            level1ToolStripMenuItem = new ToolStripMenuItem();
            level2ToolStripMenuItem = new ToolStripMenuItem();
            level3ToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            pauseToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            TimeTextLabel = new ToolStripStatusLabel();
            TimeLabel = new ToolStripStatusLabel();
            EggsTextLabel = new ToolStripStatusLabel();
            EggsLabel = new ToolStripStatusLabel();
            timer1 = new System.Windows.Forms.Timer(components);
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1 = new OpenFileDialog();
            tableLayoutPanel1 = new TableLayoutPanel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem, pauseToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(882, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newGameToolStripMenuItem, exitToolStripMenuItem });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(60, 24);
            menuToolStripMenuItem.Text = "Menu";
            // 
            // newGameToolStripMenuItem
            // 
            newGameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { level1ToolStripMenuItem, level2ToolStripMenuItem, level3ToolStripMenuItem });
            newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            newGameToolStripMenuItem.Size = new Size(165, 26);
            newGameToolStripMenuItem.Text = "New Game";
            // 
            // level1ToolStripMenuItem
            // 
            level1ToolStripMenuItem.Name = "level1ToolStripMenuItem";
            level1ToolStripMenuItem.Size = new Size(134, 26);
            level1ToolStripMenuItem.Text = "Level1";
            // 
            // level2ToolStripMenuItem
            // 
            level2ToolStripMenuItem.Name = "level2ToolStripMenuItem";
            level2ToolStripMenuItem.Size = new Size(134, 26);
            level2ToolStripMenuItem.Text = "Level2";
            // 
            // level3ToolStripMenuItem
            // 
            level3ToolStripMenuItem.Name = "level3ToolStripMenuItem";
            level3ToolStripMenuItem.Size = new Size(134, 26);
            level3ToolStripMenuItem.Text = "Level3";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(165, 26);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // pauseToolStripMenuItem
            // 
            pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            pauseToolStripMenuItem.Size = new Size(60, 24);
            pauseToolStripMenuItem.Text = "Pause";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { TimeTextLabel, TimeLabel, EggsTextLabel, EggsLabel });
            statusStrip1.Location = new Point(0, 827);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(882, 26);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // TimeTextLabel
            // 
            TimeTextLabel.Name = "TimeTextLabel";
            TimeTextLabel.Size = new Size(45, 20);
            TimeTextLabel.Text = "Time:";
            // 
            // TimeLabel
            // 
            TimeLabel.Name = "TimeLabel";
            TimeLabel.Size = new Size(0, 20);
            // 
            // EggsTextLabel
            // 
            EggsTextLabel.Name = "EggsTextLabel";
            EggsTextLabel.Size = new Size(85, 20);
            EggsTextLabel.Text = "Eggs eaten:";
            // 
            // EggsLabel
            // 
            EggsLabel.Name = "EggsLabel";
            EggsLabel.Size = new Size(0, 20);
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(0, 31);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(882, 793);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 853);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form";
            Text = "Snake";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem newGameToolStripMenuItem;
        private ToolStripMenuItem level1ToolStripMenuItem;
        private ToolStripMenuItem level2ToolStripMenuItem;
        private ToolStripMenuItem level3ToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel TimeTextLabel;
        private ToolStripStatusLabel TimeLabel;
        private ToolStripStatusLabel EggsTextLabel;
        private ToolStripStatusLabel EggsLabel;
        private System.Windows.Forms.Timer timer1;
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
