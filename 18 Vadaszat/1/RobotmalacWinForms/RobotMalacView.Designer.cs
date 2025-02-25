namespace RobotmalacWinForms
{
    partial class BekeritesView
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
            Menu = new ToolStripMenuItem();
            NewGameMenuItem = new ToolStripMenuItem();
            ThreeThree = new ToolStripMenuItem();
            FiveFive = new ToolStripMenuItem();
            SevenSeven = new ToolStripMenuItem();
            SaveMenuItem = new ToolStripMenuItem();
            LoadMenuItem = new ToolStripMenuItem();
            infoStripMenuItem = new ToolStripMenuItem();
            commandTextBox = new TextBox();
            commandLabel = new Label();
            buttonOK = new Button();
            playerLabel = new Label();
            p1HealthBar = new ProgressBar();
            p2HealthBar = new ProgressBar();
            p1HealthLabel = new Label();
            p2HealthLabel = new Label();
            buttonPTurn = new Button();
            p1Hlabel = new Label();
            p2Hlabel = new Label();
            saveFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            remainingLabel = new Label();
            remainingCountLabel = new Label();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { Menu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(973, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // Menu
            // 
            Menu.DropDownItems.AddRange(new ToolStripItem[] { NewGameMenuItem, SaveMenuItem, LoadMenuItem, infoStripMenuItem });
            Menu.Name = "Menu";
            Menu.Size = new Size(50, 20);
            Menu.Text = "Menu";
            // 
            // NewGameMenuItem
            // 
            NewGameMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ThreeThree, FiveFive, SevenSeven });
            NewGameMenuItem.Name = "NewGameMenuItem";
            NewGameMenuItem.Size = new Size(134, 22);
            NewGameMenuItem.Text = "New Game";
            // 
            // ThreeThree
            // 
            ThreeThree.Name = "ThreeThree";
            ThreeThree.Size = new Size(92, 22);
            ThreeThree.Text = "3x3";
            // 
            // FiveFive
            // 
            FiveFive.Name = "FiveFive";
            FiveFive.Size = new Size(92, 22);
            FiveFive.Text = "5x5";
            // 
            // SevenSeven
            // 
            SevenSeven.Name = "SevenSeven";
            SevenSeven.Size = new Size(92, 22);
            SevenSeven.Text = "7x7";
            // 
            // SaveMenuItem
            // 
            SaveMenuItem.Name = "SaveMenuItem";
            SaveMenuItem.Size = new Size(134, 22);
            SaveMenuItem.Text = "Save Game";
            // 
            // LoadMenuItem
            // 
            LoadMenuItem.Name = "LoadMenuItem";
            LoadMenuItem.Size = new Size(134, 22);
            LoadMenuItem.Text = "Load Game";
            // 
            // infoStripMenuItem
            // 
            infoStripMenuItem.Name = "infoStripMenuItem";
            infoStripMenuItem.Size = new Size(134, 22);
            infoStripMenuItem.Text = "Info";
            // 
            // commandTextBox
            // 
            commandTextBox.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            commandTextBox.Location = new Point(653, 162);
            commandTextBox.Multiline = true;
            commandTextBox.Name = "commandTextBox";
            commandTextBox.PasswordChar = '*';
            commandTextBox.PlaceholderText = "Max 5 at a time! Use comma to separate.";
            commandTextBox.ScrollBars = ScrollBars.Vertical;
            commandTextBox.Size = new Size(312, 85);
            commandTextBox.TabIndex = 1;
            commandTextBox.Visible = false;
            // 
            // commandLabel
            // 
            commandLabel.AutoSize = true;
            commandLabel.Font = new Font("Nasalization", 12F, FontStyle.Regular, GraphicsUnit.Point);
            commandLabel.Location = new Point(653, 132);
            commandLabel.Name = "commandLabel";
            commandLabel.Size = new Size(274, 14);
            commandLabel.TabIndex = 2;
            commandLabel.Text = "Write the instructions below:";
            commandLabel.Visible = false;
            // 
            // buttonOK
            // 
            buttonOK.Location = new Point(890, 253);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 3;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Visible = false;
            // 
            // playerLabel
            // 
            playerLabel.AutoSize = true;
            playerLabel.BorderStyle = BorderStyle.Fixed3D;
            playerLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            playerLabel.Location = new Point(720, 34);
            playerLabel.Name = "playerLabel";
            playerLabel.Size = new Size(2, 23);
            playerLabel.TabIndex = 4;
            playerLabel.Visible = false;
            // 
            // p1HealthBar
            // 
            p1HealthBar.Location = new Point(653, 358);
            p1HealthBar.Name = "p1HealthBar";
            p1HealthBar.Size = new Size(112, 23);
            p1HealthBar.TabIndex = 5;
            p1HealthBar.Value = 100;
            p1HealthBar.Visible = false;
            // 
            // p2HealthBar
            // 
            p2HealthBar.Location = new Point(859, 358);
            p2HealthBar.Name = "p2HealthBar";
            p2HealthBar.Size = new Size(112, 23);
            p2HealthBar.TabIndex = 6;
            p2HealthBar.Value = 100;
            p2HealthBar.Visible = false;
            // 
            // p1HealthLabel
            // 
            p1HealthLabel.AutoSize = true;
            p1HealthLabel.BorderStyle = BorderStyle.Fixed3D;
            p1HealthLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            p1HealthLabel.Location = new Point(653, 315);
            p1HealthLabel.Name = "p1HealthLabel";
            p1HealthLabel.Size = new Size(114, 23);
            p1HealthLabel.TabIndex = 7;
            p1HealthLabel.Text = "Player1 health:";
            p1HealthLabel.Visible = false;
            // 
            // p2HealthLabel
            // 
            p2HealthLabel.AutoSize = true;
            p2HealthLabel.BorderStyle = BorderStyle.Fixed3D;
            p2HealthLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            p2HealthLabel.Location = new Point(859, 315);
            p2HealthLabel.Name = "p2HealthLabel";
            p2HealthLabel.Size = new Size(114, 23);
            p2HealthLabel.TabIndex = 8;
            p2HealthLabel.Text = "Player2 health:";
            p2HealthLabel.Visible = false;
            // 
            // buttonPTurn
            // 
            buttonPTurn.Location = new Point(773, 263);
            buttonPTurn.Margin = new Padding(3, 2, 3, 2);
            buttonPTurn.Name = "buttonPTurn";
            buttonPTurn.Size = new Size(88, 75);
            buttonPTurn.TabIndex = 9;
            buttonPTurn.UseVisualStyleBackColor = true;
            buttonPTurn.Visible = false;
            // 
            // p1Hlabel
            // 
            p1Hlabel.AutoSize = true;
            p1Hlabel.Location = new Point(887, 384);
            p1Hlabel.Name = "p1Hlabel";
            p1Hlabel.Size = new Size(0, 15);
            p1Hlabel.TabIndex = 10;
            p1Hlabel.Visible = false;
            // 
            // p2Hlabel
            // 
            p2Hlabel.AutoSize = true;
            p2Hlabel.Location = new Point(1094, 384);
            p2Hlabel.Name = "p2Hlabel";
            p2Hlabel.Size = new Size(0, 15);
            p2Hlabel.TabIndex = 11;
            p2Hlabel.Visible = false;
            // 
            // remainingLabel
            // 
            remainingLabel.AutoSize = true;
            remainingLabel.BorderStyle = BorderStyle.Fixed3D;
            remainingLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            remainingLabel.Location = new Point(720, 72);
            remainingLabel.Name = "remainingLabel";
            remainingLabel.Size = new Size(130, 23);
            remainingLabel.TabIndex = 12;
            remainingLabel.Text = "Remaining steps:";
            remainingLabel.Visible = false;
            // 
            // remainingCountLabel
            // 
            remainingCountLabel.AutoSize = true;
            remainingCountLabel.BorderStyle = BorderStyle.Fixed3D;
            remainingCountLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            remainingCountLabel.Location = new Point(856, 72);
            remainingCountLabel.Name = "remainingCountLabel";
            remainingCountLabel.Size = new Size(21, 23);
            remainingCountLabel.TabIndex = 13;
            remainingCountLabel.Text = "0";
            remainingCountLabel.Visible = false;
            // 
            // BekeritesView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(973, 461);
            Controls.Add(remainingCountLabel);
            Controls.Add(remainingLabel);
            Controls.Add(p2Hlabel);
            Controls.Add(p1Hlabel);
            Controls.Add(buttonPTurn);
            Controls.Add(p2HealthLabel);
            Controls.Add(p1HealthLabel);
            Controls.Add(p2HealthBar);
            Controls.Add(p1HealthBar);
            Controls.Add(playerLabel);
            Controls.Add(buttonOK);
            Controls.Add(commandLabel);
            Controls.Add(commandTextBox);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MainMenuStrip = menuStrip1;
            Name = "BekeritesView";
            Text = "Bekerites";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem Menu;
        private ToolStripMenuItem NewGameMenuItem;
        private ToolStripMenuItem SaveMenuItem;
        private ToolStripMenuItem LoadMenuItem;
        private ToolStripMenuItem ThreeThree;
        private ToolStripMenuItem FiveFive;
        private ToolStripMenuItem SevenSeven;
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
        private Label remainingLabel;
        private Label remainingCountLabel;
    }
}