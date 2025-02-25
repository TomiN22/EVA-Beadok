namespace MineSweeper
{
    partial class Minesweeper
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
            this.NewGame = new System.Windows.Forms.ToolStripMenuItem();
            this.SixSix = new System.Windows.Forms.ToolStripMenuItem();
            this.TenTen = new System.Windows.Forms.ToolStripMenuItem();
            this.SixTeenSixTeen = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveGame = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadGame = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // Menu
            // 
            this.Menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewGame,
            this.SaveGame,
            this.LoadGame});
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(60, 24);
            this.Menu.Text = "Menu";
            // 
            // NewGame
            // 
            this.NewGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SixSix,
            this.TenTen,
            this.SixTeenSixTeen});
            this.NewGame.Name = "NewGame";
            this.NewGame.Size = new System.Drawing.Size(224, 26);
            this.NewGame.Text = "New game";
            // 
            // SixSix
            // 
            this.SixSix.Name = "SixSix";
            this.SixSix.Size = new System.Drawing.Size(224, 26);
            this.SixSix.Text = "6x6";
            // 
            // TenTen
            // 
            this.TenTen.Name = "TenTen";
            this.TenTen.Size = new System.Drawing.Size(224, 26);
            this.TenTen.Text = "10x10";
            // 
            // SixTeenSixTeen
            // 
            this.SixTeenSixTeen.Name = "SixTeenSixTeen";
            this.SixTeenSixTeen.Size = new System.Drawing.Size(224, 26);
            this.SixTeenSixTeen.Text = "16x16";
            // 
            // SaveGame
            // 
            this.SaveGame.Name = "SaveGame";
            this.SaveGame.Size = new System.Drawing.Size(224, 26);
            this.SaveGame.Text = "Save game";
            // 
            // LoadGame
            // 
            this.LoadGame.Name = "LoadGame";
            this.LoadGame.Size = new System.Drawing.Size(224, 26);
            this.LoadGame.Text = "Load game";
            // 
            // Minesweeper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Minesweeper";
            this.Text = "Minesweeper";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem Menu;
        private ToolStripMenuItem NewGame;
        private ToolStripMenuItem SixSix;
        private ToolStripMenuItem TenTen;
        private ToolStripMenuItem SixTeenSixTeen;
        private ToolStripMenuItem SaveGame;
        private ToolStripMenuItem LoadGame;
    }
}