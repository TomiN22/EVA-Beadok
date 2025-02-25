namespace MaciWinForms
{
    partial class MaciView
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
            MenuItem4x4 = new ToolStripMenuItem();
            MenuItem6x6 = new ToolStripMenuItem();
            MenuItem8x8 = new ToolStripMenuItem();
            pauseMenuItem = new ToolStripMenuItem();
            timeTextLabel = new Label();
            timeLabel = new Label();
            picnicTextLabel = new Label();
            remBasketsLabel = new Label();
            acqBasketsLabel = new Label();
            timer = new System.Windows.Forms.Timer(components);
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { menuToolStripMenuItem, pauseMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1522, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "Menu";
            // 
            // menuToolStripMenuItem
            // 
            menuToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { MenuItem4x4, MenuItem6x6, MenuItem8x8 });
            menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            menuToolStripMenuItem.Size = new Size(60, 24);
            menuToolStripMenuItem.Text = "Menu";
            // 
            // MenuItem4x4
            // 
            MenuItem4x4.Name = "MenuItem4x4";
            MenuItem4x4.Size = new Size(115, 26);
            MenuItem4x4.Text = "4x4";
            // 
            // MenuItem6x6
            // 
            MenuItem6x6.Name = "MenuItem6x6";
            MenuItem6x6.Size = new Size(115, 26);
            MenuItem6x6.Text = "6x6";
            // 
            // MenuItem8x8
            // 
            MenuItem8x8.Name = "MenuItem8x8";
            MenuItem8x8.Size = new Size(115, 26);
            MenuItem8x8.Text = "8x8";
            // 
            // pauseMenuItem
            // 
            pauseMenuItem.Name = "pauseMenuItem";
            pauseMenuItem.Size = new Size(60, 24);
            pauseMenuItem.Text = "Pause";
            // 
            // timeTextLabel
            // 
            timeTextLabel.AutoSize = true;
            timeTextLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            timeTextLabel.Location = new Point(909, 57);
            timeTextLabel.Name = "timeTextLabel";
            timeTextLabel.Size = new Size(69, 31);
            timeTextLabel.TabIndex = 1;
            timeTextLabel.Text = "Time:";
            timeTextLabel.Visible = false;
            // 
            // timeLabel
            // 
            timeLabel.AutoSize = true;
            timeLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            timeLabel.Location = new Point(1098, 57);
            timeLabel.Name = "timeLabel";
            timeLabel.Size = new Size(0, 31);
            timeLabel.TabIndex = 2;
            // 
            // picnicTextLabel
            // 
            picnicTextLabel.AutoSize = true;
            picnicTextLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            picnicTextLabel.Location = new Point(909, 115);
            picnicTextLabel.Name = "picnicTextLabel";
            picnicTextLabel.Size = new Size(162, 31);
            picnicTextLabel.TabIndex = 3;
            picnicTextLabel.Text = "Picnic baskets:";
            picnicTextLabel.Visible = false;
            // 
            // remBasketsLabel
            // 
            remBasketsLabel.AutoSize = true;
            remBasketsLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            remBasketsLabel.Location = new Point(1191, 115);
            remBasketsLabel.Name = "remBasketsLabel";
            remBasketsLabel.Size = new Size(0, 31);
            remBasketsLabel.TabIndex = 4;
            // 
            // acqBasketsLabel
            // 
            acqBasketsLabel.AutoSize = true;
            acqBasketsLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            acqBasketsLabel.Location = new Point(1191, 168);
            acqBasketsLabel.Name = "acqBasketsLabel";
            acqBasketsLabel.Size = new Size(0, 31);
            acqBasketsLabel.TabIndex = 5;
            // 
            // timer
            // 
            timer.Interval = 1000;
            timer.Tick += timer1_Tick;
            // 
            // MaciView
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1522, 1000);
            Controls.Add(acqBasketsLabel);
            Controls.Add(remBasketsLabel);
            Controls.Add(picnicTextLabel);
            Controls.Add(timeLabel);
            Controls.Add(timeTextLabel);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MaciView";
            Text = "Maci Laci";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem menuToolStripMenuItem;
        private ToolStripMenuItem MenuItem4x4;
        private ToolStripMenuItem MenuItem6x6;
        private ToolStripMenuItem MenuItem8x8;
        private Label timeTextLabel;
        private Label timeLabel;
        private Label picnicTextLabel;
        private Label remBasketsLabel;
        private Label acqBasketsLabel;
        private System.Windows.Forms.Timer timer;
        private ToolStripMenuItem pauseMenuItem;
    }
}
