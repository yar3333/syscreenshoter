namespace SyScreenshoter
{
    public partial class MainForm : Form
    {
        private const int PEN_WIDTH = 5;
        private const int FONT_SIZE = 14;
        private const int NEW_LINE_DY = 20;
        
        private static readonly Primitive UNDO_DELIMITER = new() { Kind = PrimitiveKind.UndoDelimiter };

        private readonly ShadowForm shadowForm;

        private List<Primitive> primitives = new();
        private readonly List<Primitive[]> redoPrimitiveBlocks = new();

        private Primitive? actPrim;

        private readonly Pen pen;
        private readonly Font font;

        public MainForm()
        {
            InitializeComponent();

            shadowForm = new ShadowForm();
            
            shadowForm.OnCaptured = bmp =>
            {
                shadowForm.Visible = false;
                ShowInTaskbar = true;
                WindowState = FormWindowState.Maximized;
                Opacity = 100;
                pictureBox.Image = bmp;
            };
            
            shadowForm.Show();

            pen = new Pen(new SolidBrush(Color.Red));
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.Width = PEN_WIDTH;

            font = new Font(SystemFonts.DefaultFont.FontFamily, FONT_SIZE, FontStyle.Regular);

            setActiveButton(btPen);
        }

        private void setActiveButton(ToolStripButton bt)
        {
            addActPrimitive();
            
            btPen.Checked = bt == btPen;
            btText.Checked = bt == btText;
        }

        private void btPen_Click(object sender, EventArgs e)
        {
            setActiveButton(btPen);
            pictureBox.Cursor = Cursors.Arrow;
        }

        private void btText_Click(object sender, EventArgs e)
        {
            setActiveButton(btText);
            pictureBox.Cursor = Cursors.IBeam;
        }

        private void btCopy_Click(object? sender, EventArgs? e)
        {
            addActPrimitive();

            var minX = - pictureBox.Image.Width  / 2;
            var minY = - pictureBox.Image.Height / 2;
            var maxX =   pictureBox.Image.Width  / 2;
            var maxY =   pictureBox.Image.Height / 2;

            foreach (var p in primitives)
            {
                switch (p.Kind)
                {
                    case PrimitiveKind.Line:
                        minX = Math.Min(minX, Math.Min(p.Pt0.X - PEN_WIDTH, p.Pt1.X - PEN_WIDTH));
                        minY = Math.Min(minY, Math.Min(p.Pt0.Y - PEN_WIDTH, p.Pt1.Y - PEN_WIDTH));
                        maxX = Math.Max(maxX, Math.Max(p.Pt0.X + PEN_WIDTH, p.Pt1.X + PEN_WIDTH));
                        maxY = Math.Max(maxY, Math.Max(p.Pt0.Y + PEN_WIDTH, p.Pt1.Y + PEN_WIDTH));
                        break;

                    case PrimitiveKind.Text:
                        var sz = TextRenderer.MeasureText(p.Text, font);
                        minX = Math.Min(minX, p.Pt0.X);
                        minY = Math.Min(minY, p.Pt0.Y);
                        maxX = Math.Max(maxX, p.Pt0.X + sz.Width);
                        maxY = Math.Max(maxY, p.Pt0.Y + sz.Height);
                        break;
                }
            }
            
            using var bmp = new Bitmap(maxX - minX, maxY - minY, pictureBox.Image.PixelFormat);
            using var g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, bmp.Width, bmp.Height);
            var dx = -minX - pictureBox.Image.Width / 2;
            var dy = -minY - pictureBox.Image.Height / 2;
            g.DrawImage(pictureBox.Image, dx, dy);
            drawPrimitives(new Point(-minX, -minY), g);
            
            Clipboard.SetImage(bmp);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            var cen = new Point(pictureBox.ClientSize.Width / 2, pictureBox.ClientSize.Height / 2);
            
            drawPrimitives(cen, e.Graphics);
        }

        private void drawPrimitives(Point cen, Graphics g)
        {
            if (actPrim != null)
            {
                switch (actPrim.Kind)
                {
                    case PrimitiveKind.Line:
                        primitives.Add(actPrim);
                        break;
                    case PrimitiveKind.Text:
                        primitives.Add(new Primitive
                        {
                            Kind = PrimitiveKind.Text,
                            Pt0 = actPrim.Pt0,
                            Text = actPrim.Text + "|"
                        });
                        break;
                }
            }
            
            foreach (var p in primitives)
            {
                switch (p.Kind)
                {
                    case PrimitiveKind.Line:
                        g.DrawLine(pen, cen.X + p.Pt0.X, cen.Y + p.Pt0.Y, cen.X + p.Pt1.X, cen.Y + p.Pt1.Y);
                        break;
                    case PrimitiveKind.Text:
                        g.DrawString(p.Text, font, Brushes.Red, cen.X + p.Pt0.X, cen.Y + p.Pt0.Y);
                        break;
                }
            }

            if (actPrim != null) primitives.RemoveAt(primitives.Count - 1);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox.Capture = true;

            var loc = e.Location;
            loc.X -= pictureBox.ClientSize.Width / 2;
            loc.Y -= pictureBox.ClientSize.Height / 2;

            addActPrimitive();
            
            if (btPen.Checked)
            {
                if (primitives.LastOrDefault() != UNDO_DELIMITER) primitives.Add(UNDO_DELIMITER);
                actPrim = new Primitive
                {
                    Kind = PrimitiveKind.Line,
                    Pt0 = loc,
                    Pt1 = loc,
                };
            }
            else if (btText.Checked)
            {
                actPrim = new Primitive
                {
                    Kind = PrimitiveKind.Text,
                    Pt0 = loc,
                    Text = ""
                };
                actPrim.Pt0.X -= 3;
                actPrim.Pt0.Y -= 13;
            }

            Refresh();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (actPrim == null) return;

            var loc = e.Location;
            loc.X -= pictureBox.ClientSize.Width / 2;
            loc.Y -= pictureBox.ClientSize.Height / 2;
            
            switch (actPrim.Kind)
            {
                case PrimitiveKind.Line:
                    actPrim.Pt1 = loc;
                    addActPrimitive();
                    actPrim = new Primitive
                    {
                        Kind = PrimitiveKind.Line,
                        Pt0 = loc,
                        Pt1 = loc
                    };
                    break;

                case PrimitiveKind.Text:
                    break;
            }
            
            Refresh();
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBox.Capture = false;
            
            if (actPrim == null) return;

            switch (actPrim.Kind)
            {
                case PrimitiveKind.Line:
                    addActPrimitive();
                    break;

                case PrimitiveKind.Text:
                    break;
            }
            
            Refresh();
        }

        private void addActPrimitive()
        {
            if (actPrim == null) return;

            switch (actPrim.Kind)
            {
                case PrimitiveKind.Line:
                    if (actPrim.Pt0 != actPrim.Pt1) primitives.Add(actPrim);
                    break;

                case PrimitiveKind.Text:
                    if (actPrim.Text != "")
                    {
                        primitives.Add(UNDO_DELIMITER);
                        primitives.Add(actPrim);
                    }
                    break;
            }

            while (primitives.LastOrDefault() == UNDO_DELIMITER)
            {
                primitives.RemoveAt(primitives.Count - 1);
            }

            actPrim = null;

            Refresh();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == 0 && e.KeyCode == Keys.Escape)
            {
                Application.Exit();
            }

            if (shadowForm.Visible) return;
            
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                if (!primitives.Any()) return;
                
                addActPrimitive();
                
                var i = primitives.FindLastIndex(x => x == UNDO_DELIMITER);
                redoPrimitiveBlocks.Add(primitives.GetRange(i, primitives.Count - i).ToArray());
                primitives = primitives.GetRange(0, i);

                Refresh();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Y || e.Modifiers == (Keys.Control|Keys.Shift) && e.KeyCode == Keys.Z)
            {
                if (!redoPrimitiveBlocks.Any()) return;

                addActPrimitive();
                
                var redoBlock = redoPrimitiveBlocks.Last();
                redoPrimitiveBlocks.RemoveAt(redoPrimitiveBlocks.Count - 1);
                primitives.AddRange(redoBlock);

                Refresh();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
            {
                btCopy_Click(null, null);
            }
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (shadowForm.Visible) return;

            if (actPrim?.Kind != PrimitiveKind.Text) return;

            if (e.KeyChar == '\b')
            {
                if (actPrim.Text != "")
                {
                    actPrim.Text = actPrim.Text.Substring(0, actPrim.Text.Length - 1);
                    e.Handled = true;
                    Refresh();
                }
            }
            if (e.KeyChar == '\r')
            {
                var pt = actPrim.Pt0;
                addActPrimitive();
                actPrim = new Primitive
                {
                    Kind = PrimitiveKind.Text,
                    Pt0 = new Point(pt.X, pt.Y + NEW_LINE_DY),
                    Text = ""
                };
                e.Handled = true;
                Refresh();
            }
            else if (!char.IsControl(e.KeyChar))
            {
                actPrim.Text += e.KeyChar;
                e.Handled = true;
                Refresh();
            }
        }
    }
}