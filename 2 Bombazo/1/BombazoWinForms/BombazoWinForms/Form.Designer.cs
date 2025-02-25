namespace BombazoWinForms
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
            menuStrip = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            lvl1 = new ToolStripMenuItem();
            lvl2 = new ToolStripMenuItem();
            lvl3 = new ToolStripMenuItem();
            pauseToolStripMenuItem = new ToolStripMenuItem();
            timer = new System.Windows.Forms.Timer(components);
            statusStrip1 = new StatusStrip();
            timeLabel = new ToolStripStatusLabel();
            timeNumLabel = new ToolStripStatusLabel();
            cooldownTetxLabel = new ToolStripStatusLabel();
            cooldownLabel = new ToolStripStatusLabel();
            enemiesBlownTextLabel = new ToolStripStatusLabel();
            enemiesBlownLabel = new ToolStripStatusLabel();
            tableLayoutPanel = new TableLayoutPanel();
            menuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem, pauseToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(881, 28);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lvl1, lvl2, lvl3 });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(60, 24);
            menuToolStripMenuItem.Text = "Menu";
            // 
            // lvl1
            // 
            lvl1.Name = "lvl1";
            lvl1.Size = new Size(134, 26);
            lvl1.Text = "Level1";
            // 
            // lvl2
            // 
            lvl2.Name = "lvl2";
            lvl2.Size = new Size(134, 26);
            lvl2.Text = "Level2";
            // 
            // lvl3
            // 
            lvl3.Name = "lvl3";
            lvl3.Size = new Size(134, 26);
            lvl3.Text = "Level3";
            // 
            // pauseToolStripMenuItem
            // 
            pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            pauseToolStripMenuItem.Size = new Size(60, 24);
            pauseToolStripMenuItem.Text = "Pause";
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { timeLabel, timeNumLabel, cooldownTetxLabel, cooldownLabel, enemiesBlownTextLabel, enemiesBlownLabel });
            statusStrip1.Location = new Point(0, 1041);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(881, 26);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // timeLabel
            // 
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new Size(45, 20);
            timeLabel.Text = "Time:";
            // 
            // timeNumLabel
            // 
            timeNumLabel.Name = "timeNumLabel";
            timeNumLabel.Size = new Size(0, 20);
            // 
            // cooldownTetxLabel
            // 
            cooldownTetxLabel.Name = "cooldownTetxLabel";
            cooldownTetxLabel.Size = new Size(122, 20);
            cooldownTetxLabel.Text = "Bomb cooldown:";
            // 
            // cooldownLabel
            // 
            cooldownLabel.Name = "cooldownLabel";
            cooldownLabel.Size = new Size(0, 20);
            // 
            // enemiesBlownTextLabel
            // 
            enemiesBlownTextLabel.Name = "enemiesBlownTextLabel";
            enemiesBlownTextLabel.Size = new Size(133, 20);
            enemiesBlownTextLabel.Text = "Enemies blown up:";
            // 
            // enemiesBlownLabel
            // 
            enemiesBlownLabel.Name = "enemiesBlownLabel";
            enemiesBlownLabel.Size = new Size(0, 20);
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Location = new Point(12, 31);
            tableLayoutPanel.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Size = new Size(857, 1000);
            tableLayoutPanel.TabIndex = 3;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 1067);
            Controls.Add(tableLayoutPanel);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "Form";
            Text = "Bombazo";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.Timer timer;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem lvl1;
        private ToolStripMenuItem lvl2;
        private ToolStripMenuItem lvl3;
        private ToolStripStatusLabel timeLabel;
        private ToolStripStatusLabel timeNumLabel;
        private ToolStripStatusLabel cooldownTetxLabel;
        private ToolStripStatusLabel cooldownLabel;
        private ToolStripStatusLabel enemiesBlownTextLabel;
        private ToolStripStatusLabel enemiesBlownLabel;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel;
    }
}
