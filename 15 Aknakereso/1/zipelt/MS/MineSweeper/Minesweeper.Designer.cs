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
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this._openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(700, 24);
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
            this.Menu.Size = new System.Drawing.Size(50, 20);
            this.Menu.Text = "Menu";
            // 
            // NewGame
            // 
            this.NewGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SixSix,
            this.TenTen,
            this.SixTeenSixTeen});
            this.NewGame.Name = "NewGame";
            this.NewGame.Size = new System.Drawing.Size(180, 22);
            this.NewGame.Text = "New game";
            // 
            // SixSix
            // 
            this.SixSix.Name = "SixSix";
            this.SixSix.Size = new System.Drawing.Size(180, 22);
            this.SixSix.Text = "6x6";
            // 
            // TenTen
            // 
            this.TenTen.Name = "TenTen";
            this.TenTen.Size = new System.Drawing.Size(180, 22);
            this.TenTen.Text = "10x10";
            // 
            // SixTeenSixTeen
            // 
            this.SixTeenSixTeen.Name = "SixTeenSixTeen";
            this.SixTeenSixTeen.Size = new System.Drawing.Size(180, 22);
            this.SixTeenSixTeen.Text = "16x16";
            // 
            // SaveGame
            // 
            this.SaveGame.Name = "SaveGame";
            this.SaveGame.Size = new System.Drawing.Size(180, 22);
            this.SaveGame.Text = "Save game";
            // 
            // LoadGame
            // 
            this.LoadGame.Name = "LoadGame";
            this.LoadGame.Size = new System.Drawing.Size(180, 22);
            this.LoadGame.Text = "Load game";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "0";
            this.saveFileDialog1.Filter = "Mine table (*.stl)|*.stl";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // _openFileDialog1
            // 
            this._openFileDialog1.FileName = "openFileDialog1";
            this._openFileDialog1.Filter = "Mine table (*.stl)|*.stl";
            this._openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Minesweeper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(700, 338);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
        private SaveFileDialog saveFileDialog1;
        private OpenFileDialog _openFileDialog1;
    }
}