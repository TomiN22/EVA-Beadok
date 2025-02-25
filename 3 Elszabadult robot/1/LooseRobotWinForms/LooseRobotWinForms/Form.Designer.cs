namespace LooseRobotWinForms
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
            timer1 = new System.Windows.Forms.Timer(components);
            menuStrip = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            NewGameMenu = new ToolStripMenuItem();
            Menu7x7 = new ToolStripMenuItem();
            Menu11x11 = new ToolStripMenuItem();
            Menu15x15 = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            pauseToolStripMenuItem = new ToolStripMenuItem();
            TimeTextStrip = new StatusStrip();
            TimeLabelText = new ToolStripStatusLabel();
            TimeLabel = new ToolStripStatusLabel();
            saveFileDialog1 = new SaveFileDialog();
            openFileDialog1 = new OpenFileDialog();
            tableLayoutPanel1 = new TableLayoutPanel();
            menuStrip.SuspendLayout();
            TimeTextStrip.SuspendLayout();
            SuspendLayout();
            // 
            // timer1
            // 
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem, pauseToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(882, 28);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "Menu";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { NewGameMenu, saveToolStripMenuItem, loadToolStripMenuItem, exitToolStripMenuItem });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(60, 24);
            menuToolStripMenuItem.Text = "Menu";
            // 
            // NewGameMenu
            // 
            NewGameMenu.DropDownItems.AddRange(new ToolStripItem[] { Menu7x7, Menu11x11, Menu15x15 });
            NewGameMenu.Name = "NewGameMenu";
            NewGameMenu.Size = new Size(165, 26);
            NewGameMenu.Text = "New Game";
            // 
            // Menu7x7
            // 
            Menu7x7.Name = "Menu7x7";
            Menu7x7.Size = new Size(131, 26);
            Menu7x7.Text = "7x7";
            // 
            // Menu11x11
            // 
            Menu11x11.Name = "Menu11x11";
            Menu11x11.Size = new Size(131, 26);
            Menu11x11.Text = "11x11";
            // 
            // Menu15x15
            // 
            Menu15x15.Name = "Menu15x15";
            Menu15x15.Size = new Size(131, 26);
            Menu15x15.Text = "15x15";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(165, 26);
            saveToolStripMenuItem.Text = "Save";
            // 
            // loadToolStripMenuItem
            // 
            loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            loadToolStripMenuItem.Size = new Size(165, 26);
            loadToolStripMenuItem.Text = "Load";
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
            // TimeTextStrip
            // 
            TimeTextStrip.ImageScalingSize = new Size(20, 20);
            TimeTextStrip.Items.AddRange(new ToolStripItem[] { TimeLabelText, TimeLabel });
            TimeTextStrip.Location = new Point(0, 927);
            TimeTextStrip.Name = "TimeTextStrip";
            TimeTextStrip.Size = new Size(882, 26);
            TimeTextStrip.TabIndex = 1;
            TimeTextStrip.Text = "Time:";
            // 
            // TimeLabelText
            // 
            TimeLabelText.Name = "TimeLabelText";
            TimeLabelText.Size = new Size(45, 20);
            TimeLabelText.Text = "Time:";
            // 
            // TimeLabel
            // 
            TimeLabel.Name = "TimeLabel";
            TimeLabel.Size = new Size(0, 20);
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(12, 31);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(850, 800);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 953);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(TimeTextStrip);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "Form";
            Text = "LooseRobot";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            TimeTextStrip.ResumeLayout(false);
            TimeTextStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private MenuStrip menuStrip;
        private StatusStrip TimeTextStrip;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem NewGameMenu;
        private ToolStripMenuItem Menu7x7;
        private ToolStripMenuItem Menu11x11;
        private ToolStripMenuItem Menu15x15;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripStatusLabel TimeLabelText;
        private ToolStripStatusLabel TimeLabel;
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog openFileDialog1;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
