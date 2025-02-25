namespace MenekuljWinForms
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
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            statusStrip1 = new StatusStrip();
            TimeTextLabel = new ToolStripStatusLabel();
            TimeLabel = new ToolStripStatusLabel();
            EnemiesTextLabel = new ToolStripStatusLabel();
            EnemiesLabel = new ToolStripStatusLabel();
            menuStrip1 = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            newGameToolStripMenuItem = new ToolStripMenuItem();
            x11ToolStripMenuItem = new ToolStripMenuItem();
            x15ToolStripMenuItem = new ToolStripMenuItem();
            x21ToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            pauseToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            statusStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
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
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { TimeTextLabel, TimeLabel, EnemiesTextLabel, EnemiesLabel });
            statusStrip1.Location = new Point(0, 929);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(902, 26);
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
            // EnemiesTextLabel
            // 
            EnemiesTextLabel.Name = "EnemiesTextLabel";
            EnemiesTextLabel.Size = new Size(93, 20);
            EnemiesTextLabel.Text = "Enemies left:";
            // 
            // EnemiesLabel
            // 
            EnemiesLabel.Name = "EnemiesLabel";
            EnemiesLabel.Size = new Size(0, 20);
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem, pauseToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(902, 28);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newGameToolStripMenuItem, saveToolStripMenuItem, loadToolStripMenuItem, exitToolStripMenuItem });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(60, 24);
            menuToolStripMenuItem.Text = "Menu";
            // 
            // newGameToolStripMenuItem
            // 
            newGameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { x11ToolStripMenuItem, x15ToolStripMenuItem, x21ToolStripMenuItem });
            newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            newGameToolStripMenuItem.Size = new Size(165, 26);
            newGameToolStripMenuItem.Text = "New Game";
            // 
            // x11ToolStripMenuItem
            // 
            x11ToolStripMenuItem.Name = "x11ToolStripMenuItem";
            x11ToolStripMenuItem.Size = new Size(131, 26);
            x11ToolStripMenuItem.Text = "11x11";
            // 
            // x15ToolStripMenuItem
            // 
            x15ToolStripMenuItem.Name = "x15ToolStripMenuItem";
            x15ToolStripMenuItem.Size = new Size(131, 26);
            x15ToolStripMenuItem.Text = "15x15";
            // 
            // x21ToolStripMenuItem
            // 
            x21ToolStripMenuItem.Name = "x21ToolStripMenuItem";
            x21ToolStripMenuItem.Size = new Size(131, 26);
            x21ToolStripMenuItem.Text = "21x21";
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
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(0, 31);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(902, 895);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(902, 955);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(tableLayoutPanel1);
            MainMenuStrip = menuStrip1;
            Name = "Form";
            Text = "Run";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel TimeTextLabel;
        private ToolStripStatusLabel TimeLabel;
        private ToolStripStatusLabel EnemiesTextLabel;
        private ToolStripStatusLabel EnemiesLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem pauseToolStripMenuItem;
        private ToolStripMenuItem newGameToolStripMenuItem;
        private ToolStripMenuItem x11ToolStripMenuItem;
        private ToolStripMenuItem x15ToolStripMenuItem;
        private ToolStripMenuItem x21ToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
