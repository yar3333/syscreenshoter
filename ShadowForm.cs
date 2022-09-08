namespace SyScreenshoter
{
    public partial class ShadowForm : Form
    {
        private int x0;
        private int y0;
        private int x1;
        private int y1;

        private Bitmap bmp;

        private bool inAction;
        
        public Action<Bitmap> OnCaptured;

        public ShadowForm()
        {
            InitializeComponent();
        }

        private void ShadowForm_Load(object sender, EventArgs e)
        {
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

            SetDesktopBounds(bounds.X, bounds.Y, bounds.Width, bounds.Height);

            bmp = new Bitmap(Bounds.Width, Bounds.Height);
            using var g = Graphics.FromImage(bmp);
            g.CopyFromScreen(Bounds.X, Bounds.Y, 0, 0, bmp.Size);
            BackgroundImage = bmp;
        }

        private void ShadowForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            inAction = true;
            Capture = true;

            x0 = x1 = e.X;
            y0 = y1 = e.Y;
        }

        private void ShadowForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!inAction) return;

            x1 = e.X;
            y1 = e.Y;

            Refresh();
        }

        private void ShadowForm_MouseUp(object sender, MouseEventArgs e)
        {
            inAction = false;
            Capture = false;

            if (x0 != x1 && y0 != y1)
            {
                var region = new Rectangle();
                region.X = Math.Min(x0, x1);
                region.Y = Math.Min(y0, y1);
                region.Width = Math.Max(x0, x1) - region.X;
                region.Height = Math.Max(y0, y1) - region.Y;

                var resBmp = new Bitmap(region.Width, region.Height);
                using var g = Graphics.FromImage(resBmp);
                g.DrawImageUnscaled(bmp, -region.X, -region.Y);

                OnCaptured(resBmp);
            }
            Visible = false;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (BackgroundImage != null) base.OnPaintBackground(e);
        }

        private void ShadowForm_Paint(object sender, PaintEventArgs e)
        {
            var color = Color.FromArgb(63, Color.Black);
            using var brush = new SolidBrush(color);

            if (inAction)
            {
                var xx = new[] { 0, Math.Min(x0, x1), Math.Max(x0, x1), ClientSize.Width };
                var yy = new[] { 0,  Math.Min(y0, y1), Math.Max(y0, y1), ClientSize.Height };

                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1) continue;

                        var rect = new Rectangle(xx[j], yy[i], xx[j + 1] - xx[j], yy[i + 1] - yy[i]);
                        e.Graphics.FillRectangle(brush, rect);
                    }
                }
            }
            else
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }
    }
}
