namespace SyScreenshoter
{
    public partial class MainForm : Form
    {
        private ShadowForm shadowForms;

        public MainForm()
        {
            InitializeComponent();

            var bounds = Screen.PrimaryScreen.Bounds;

            foreach (var screen in Screen.AllScreens)
            {
                var b = new Rectangle();
                b.X = Math.Min(bounds.X, screen.Bounds.X);
                b.Y = Math.Min(bounds.Y, screen.Bounds.Y);
                b.Width = Math.Max(bounds.Right, screen.Bounds.Right) - b.X;
                b.Height = Math.Max(bounds.Bottom, screen.Bounds.Bottom) - b.Y;
                bounds = b;    
            }

            shadowForms = new ShadowForm();
            shadowForms.OnCaptured = bmp =>
            {
                ShowInTaskbar = true;
                WindowState = FormWindowState.Maximized;
                Opacity = 100;
                shadowForms.Visible = false;
                pictureBox.Image = bmp;
            };
            shadowForms.Show();
            shadowForms.SetDesktopBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            shadowForms.Init();
        }
   }
}