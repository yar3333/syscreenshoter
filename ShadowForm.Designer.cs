namespace SyScreenshoter
{
    partial class ShadowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ShadowForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShadowForm";
            this.ShowInTaskbar = false;
            this.Text = "SyScreenshoter";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ShadowForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShadowForm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShadowForm_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ShadowForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShadowForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ShadowForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}