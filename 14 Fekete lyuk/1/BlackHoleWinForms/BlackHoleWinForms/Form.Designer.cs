namespace BlackHoleWinForms
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
            menuStrip1 = new MenuStrip();
            menuToolStripMenuItem = new ToolStripMenuItem();
            newGameToolStripMenuItem = new ToolStripMenuItem();
            x5ToolStripMenuItem = new ToolStripMenuItem();
            x7ToolStripMenuItem = new ToolStripMenuItem();
            x9ToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            loadToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            P1ShipsTextLabel = new ToolStripStatusLabel();
            P1ShipsLabel = new ToolStripStatusLabel();
            P2ShipsTextLabel = new ToolStripStatusLabel();
            P2ShipsLabel = new ToolStripStatusLabel();
            PlayerLabel = new ToolStripStatusLabel();
            tableLayoutPanel1 = new TableLayoutPanel();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
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
            newGameToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { x5ToolStripMenuItem, x7ToolStripMenuItem, x9ToolStripMenuItem });
            newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            newGameToolStripMenuItem.Size = new Size(165, 26);
            newGameToolStripMenuItem.Text = "New Game";
            // 
            // x5ToolStripMenuItem
            // 
            x5ToolStripMenuItem.Name = "x5ToolStripMenuItem";
            x5ToolStripMenuItem.Size = new Size(115, 26);
            x5ToolStripMenuItem.Text = "5x5";
            // 
            // x7ToolStripMenuItem
            // 
            x7ToolStripMenuItem.Name = "x7ToolStripMenuItem";
            x7ToolStripMenuItem.Size = new Size(115, 26);
            x7ToolStripMenuItem.Text = "7x7";
            // 
            // x9ToolStripMenuItem
            // 
            x9ToolStripMenuItem.Name = "x9ToolStripMenuItem";
            x9ToolStripMenuItem.Size = new Size(115, 26);
            x9ToolStripMenuItem.Text = "9x9";
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
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { P1ShipsTextLabel, P1ShipsLabel, P2ShipsTextLabel, P2ShipsLabel, PlayerLabel });
            statusStrip1.Location = new Point(0, 627);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 26);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // P1ShipsTextLabel
            // 
            P1ShipsTextLabel.Name = "P1ShipsTextLabel";
            P1ShipsTextLabel.Size = new Size(101, 20);
            P1ShipsTextLabel.Text = "Player 1 ships:";
            // 
            // P1ShipsLabel
            // 
            P1ShipsLabel.Name = "P1ShipsLabel";
            P1ShipsLabel.Padding = new Padding(100, 0, 0, 0);
            P1ShipsLabel.Size = new Size(100, 20);
            // 
            // P2ShipsTextLabel
            // 
            P2ShipsTextLabel.Name = "P2ShipsTextLabel";
            P2ShipsTextLabel.Padding = new Padding(100, 0, 0, 0);
            P2ShipsTextLabel.Size = new Size(201, 20);
            P2ShipsTextLabel.Text = "Player 2 ships:";
            // 
            // P2ShipsLabel
            // 
            P2ShipsLabel.Name = "P2ShipsLabel";
            P2ShipsLabel.Size = new Size(0, 20);
            // 
            // PlayerLabel
            // 
            PlayerLabel.Name = "PlayerLabel";
            PlayerLabel.Padding = new Padding(100, 0, 0, 0);
            PlayerLabel.Size = new Size(100, 20);
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(12, 31);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(776, 593);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 653);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form";
            Text = "Black Hole";
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
        private ToolStripMenuItem x5ToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem x7ToolStripMenuItem;
        private ToolStripMenuItem x9ToolStripMenuItem;
        private ToolStripStatusLabel P1ShipsTextLabel;
        private ToolStripStatusLabel P1ShipsLabel;
        private ToolStripStatusLabel P2ShipsTextLabel;
        private ToolStripStatusLabel P2ShipsLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ToolStripStatusLabel PlayerLabel;
    }
}
