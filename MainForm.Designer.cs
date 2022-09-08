namespace SyScreenshoter
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolbar = new System.Windows.Forms.ToolStrip();
            this.btPen = new System.Windows.Forms.ToolStripButton();
            this.btText = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btCopy = new System.Windows.Forms.ToolStripButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // toolbar
            // 
            this.toolbar.AutoSize = false;
            this.toolbar.ImageScalingSize = new System.Drawing.Size(34, 34);
            this.toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btPen,
            this.btText,
            this.toolStripSeparator1,
            this.btCopy});
            this.toolbar.Location = new System.Drawing.Point(0, 0);
            this.toolbar.Name = "toolbar";
            this.toolbar.Size = new System.Drawing.Size(800, 40);
            this.toolbar.TabIndex = 0;
            this.toolbar.Text = "toolStrip1";
            // 
            // btPen
            // 
            this.btPen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btPen.Image = ((System.Drawing.Image)(resources.GetObject("btPen.Image")));
            this.btPen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btPen.Name = "btPen";
            this.btPen.Size = new System.Drawing.Size(38, 37);
            this.btPen.Text = "Pen";
            this.btPen.Click += new System.EventHandler(this.btPen_Click);
            // 
            // btText
            // 
            this.btText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btText.Image = ((System.Drawing.Image)(resources.GetObject("btText.Image")));
            this.btText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btText.Name = "btText";
            this.btText.Size = new System.Drawing.Size(38, 37);
            this.btText.Text = "Text";
            this.btText.Click += new System.EventHandler(this.btText_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // btCopy
            // 
            this.btCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btCopy.Image = ((System.Drawing.Image)(resources.GetObject("btCopy.Image")));
            this.btCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btCopy.Name = "btCopy";
            this.btCopy.Size = new System.Drawing.Size(38, 37);
            this.btCopy.Text = "Copy to clipboard";
            this.btCopy.Click += new System.EventHandler(this.btCopy_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 40);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(800, 410);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
            this.pictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.toolbar);
            this.Name = "MainForm";
            this.Opacity = 0D;
            this.ShowInTaskbar = false;
            this.Text = "SyScreenshoter";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ToolStrip toolbar;
        private ToolStripButton btText;
        private ToolStripButton btCopy;
        private PictureBox pictureBox;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btPen;
    }
}