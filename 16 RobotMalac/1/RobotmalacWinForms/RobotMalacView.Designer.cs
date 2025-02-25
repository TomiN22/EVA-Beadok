namespace RobotmalacWinForms
{
    partial class RobotMalacView
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Menu = new System.Windows.Forms.ToolStripMenuItem();
            this.NewGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FourFour = new System.Windows.Forms.ToolStripMenuItem();
            this.SixSix = new System.Windows.Forms.ToolStripMenuItem();
            this.EightEight = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commandTextBox = new System.Windows.Forms.TextBox();
            this.commandLabel = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.playerLabel = new System.Windows.Forms.Label();
            this.p1HealthBar = new System.Windows.Forms.ProgressBar();
            this.p2HealthBar = new System.Windows.Forms.ProgressBar();
            this.p1HealthLabel = new System.Windows.Forms.Label();
            this.p2HealthLabel = new System.Windows.Forms.Label();
            this.buttonPTurn = new System.Windows.Forms.Button();
            this.p1Hlabel = new System.Windows.Forms.Label();
            this.p2Hlabel = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Menu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1184, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Menu
            // 
            this.Menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewGameMenuItem,
            this.SaveMenuItem,
            this.LoadMenuItem,
            this.infoStripMenuItem});
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(50, 20);
            this.Menu.Text = "Menu";
            // 
            // NewGameMenuItem
            // 
            this.NewGameMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FourFour,
            this.SixSix,
            this.EightEight});
            this.NewGameMenuItem.Name = "NewGameMenuItem";
            this.NewGameMenuItem.Size = new System.Drawing.Size(134, 22);
            this.NewGameMenuItem.Text = "New Game";
            // 
            // FourFour
            // 
            this.FourFour.Name = "FourFour";
            this.FourFour.Size = new System.Drawing.Size(92, 22);
            this.FourFour.Text = "4x4";
            // 
            // SixSix
            // 
            this.SixSix.Name = "SixSix";
            this.SixSix.Size = new System.Drawing.Size(92, 22);
            this.SixSix.Text = "6x6";
            // 
            // EightEight
            // 
            this.EightEight.Name = "EightEight";
            this.EightEight.Size = new System.Drawing.Size(92, 22);
            this.EightEight.Text = "8x8";
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.Size = new System.Drawing.Size(134, 22);
            this.SaveMenuItem.Text = "Save Game";
            // 
            // LoadMenuItem
            // 
            this.LoadMenuItem.Name = "LoadMenuItem";
            this.LoadMenuItem.Size = new System.Drawing.Size(134, 22);
            this.LoadMenuItem.Text = "Load Game";
            // 
            // infoStripMenuItem
            // 
            this.infoStripMenuItem.Name = "infoStripMenuItem";
            this.infoStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.infoStripMenuItem.Text = "Info";
            // 
            // commandTextBox
            // 
            this.commandTextBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.commandTextBox.Location = new System.Drawing.Point(835, 162);
            this.commandTextBox.Multiline = true;
            this.commandTextBox.Name = "commandTextBox";
            this.commandTextBox.PasswordChar = '*';
            this.commandTextBox.PlaceholderText = "Max 5 at a time! Use comma to separate.";
            this.commandTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.commandTextBox.Size = new System.Drawing.Size(312, 85);
            this.commandTextBox.TabIndex = 1;
            this.commandTextBox.Visible = false;
            // 
            // commandLabel
            // 
            this.commandLabel.AutoSize = true;
            this.commandLabel.Font = new System.Drawing.Font("Nasalization", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.commandLabel.Location = new System.Drawing.Point(835, 127);
            this.commandLabel.Name = "commandLabel";
            this.commandLabel.Size = new System.Drawing.Size(274, 14);
            this.commandLabel.TabIndex = 2;
            this.commandLabel.Text = "Write the instructions below:";
            this.commandLabel.Visible = false;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(835, 253);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Visible = false;
            // 
            // playerLabel
            // 
            this.playerLabel.AutoSize = true;
            this.playerLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.playerLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playerLabel.Location = new System.Drawing.Point(835, 34);
            this.playerLabel.Name = "playerLabel";
            this.playerLabel.Size = new System.Drawing.Size(123, 23);
            this.playerLabel.TabIndex = 4;
            this.playerLabel.Text = "Player 1 on turn";
            this.playerLabel.Visible = false;
            // 
            // p1HealthBar
            // 
            this.p1HealthBar.Location = new System.Drawing.Point(835, 358);
            this.p1HealthBar.Name = "p1HealthBar";
            this.p1HealthBar.Size = new System.Drawing.Size(112, 23);
            this.p1HealthBar.TabIndex = 5;
            this.p1HealthBar.Value = 100;
            this.p1HealthBar.Visible = false;
            // 
            // p2HealthBar
            // 
            this.p2HealthBar.Location = new System.Drawing.Point(1033, 358);
            this.p2HealthBar.Name = "p2HealthBar";
            this.p2HealthBar.Size = new System.Drawing.Size(112, 23);
            this.p2HealthBar.TabIndex = 6;
            this.p2HealthBar.Value = 100;
            this.p2HealthBar.Visible = false;
            // 
            // p1HealthLabel
            // 
            this.p1HealthLabel.AutoSize = true;
            this.p1HealthLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.p1HealthLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.p1HealthLabel.Location = new System.Drawing.Point(835, 315);
            this.p1HealthLabel.Name = "p1HealthLabel";
            this.p1HealthLabel.Size = new System.Drawing.Size(114, 23);
            this.p1HealthLabel.TabIndex = 7;
            this.p1HealthLabel.Text = "Player1 health:";
            this.p1HealthLabel.Visible = false;
            // 
            // p2HealthLabel
            // 
            this.p2HealthLabel.AutoSize = true;
            this.p2HealthLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.p2HealthLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.p2HealthLabel.Location = new System.Drawing.Point(1033, 315);
            this.p2HealthLabel.Name = "p2HealthLabel";
            this.p2HealthLabel.Size = new System.Drawing.Size(114, 23);
            this.p2HealthLabel.TabIndex = 8;
            this.p2HealthLabel.Text = "Player2 health:";
            this.p2HealthLabel.Visible = false;
            // 
            // buttonPTurn
            // 
            this.buttonPTurn.Location = new System.Drawing.Point(1006, 34);
            this.buttonPTurn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPTurn.Name = "buttonPTurn";
            this.buttonPTurn.Size = new System.Drawing.Size(88, 75);
            this.buttonPTurn.TabIndex = 9;
            this.buttonPTurn.UseVisualStyleBackColor = true;
            this.buttonPTurn.Visible = false;
            // 
            // p1Hlabel
            // 
            this.p1Hlabel.AutoSize = true;
            this.p1Hlabel.Location = new System.Drawing.Point(887, 384);
            this.p1Hlabel.Name = "p1Hlabel";
            this.p1Hlabel.Size = new System.Drawing.Size(0, 15);
            this.p1Hlabel.TabIndex = 10;
            this.p1Hlabel.Visible = false;
            // 
            // p2Hlabel
            // 
            this.p2Hlabel.AutoSize = true;
            this.p2Hlabel.Location = new System.Drawing.Point(1094, 384);
            this.p2Hlabel.Name = "p2Hlabel";
            this.p2Hlabel.Size = new System.Drawing.Size(0, 15);
            this.p2Hlabel.TabIndex = 11;
            this.p2Hlabel.Visible = false;
            // 
            // RobotMalacView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1184, 461);
            this.Controls.Add(this.p2Hlabel);
            this.Controls.Add(this.p1Hlabel);
            this.Controls.Add(this.buttonPTurn);
            this.Controls.Add(this.p2HealthLabel);
            this.Controls.Add(this.p1HealthLabel);
            this.Controls.Add(this.p2HealthBar);
            this.Controls.Add(this.p1HealthBar);
            this.Controls.Add(this.playerLabel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.commandLabel);
            this.Controls.Add(this.commandTextBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "RobotMalacView";
            this.Text = "Epic robot-pig fight";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem Menu;
        private ToolStripMenuItem NewGameMenuItem;
        private ToolStripMenuItem SaveMenuItem;
        private ToolStripMenuItem LoadMenuItem;
        private ToolStripMenuItem FourFour;
        private ToolStripMenuItem SixSix;
        private ToolStripMenuItem EightEight;
        private TextBox commandTextBox;
        private Label commandLabel;
        private Button buttonOK;
        private Label playerLabel;
        private ProgressBar p1HealthBar;
        private ProgressBar p2HealthBar;
        private Label p1HealthLabel;
        private Label p2HealthLabel;
        private Button buttonPTurn;
        private Label p1Hlabel;
        private Label p2Hlabel;
        private ToolStripMenuItem infoStripMenuItem;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
    }
}