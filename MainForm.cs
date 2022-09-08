namespace SyScreenshoter
{
    public partial class MainForm : Form
    {
        private List<Primitive> primitives = new();
        private List<Primitive[]> redoPrimitiveBlocks = new();

        private Primitive? actPrim;

        public MainForm()
        {
            InitializeComponent();

            textBox.Top = -1000;

            var shadowForm = new ShadowForm();
            
            shadowForm.OnCaptured = bmp =>
            {
                shadowForm.Visible = false;
                ShowInTaskbar = true;
                WindowState = FormWindowState.Maximized;
                Opacity = 100;
                pictureBox.BackgroundImage = bmp;
            };
            shadowForm.Show();

            setActiveButton(btPen);
        }

        private void setActiveButton(ToolStripButton bt)
        {
            btPen.Checked = bt == btPen;
            btText.Checked = bt == btText;
        }

        private void btPen_Click(object sender, EventArgs e)
        {
            addActPrimitive();
            setActiveButton(btPen);
            pictureBox.Cursor = Cursors.Arrow;
        }

        private void btText_Click(object sender, EventArgs e)
        {
            addActPrimitive();
            setActiveButton(btText);
            pictureBox.Cursor = Cursors.IBeam;
        }

        private void btCopy_Click(object sender, EventArgs e)
        {
            addActPrimitive();

        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
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
                            Text = textBox.Text + "|"
                        });
                        break;
                }
            }

            using var pen = new Pen(new SolidBrush(Color.Red));
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.Width = 5;

            using var font = new Font(SystemFonts.DefaultFont.FontFamily, 14, FontStyle.Regular);

            var cen = new Point(ClientSize.Width / 2, ClientSize.Height / 2);
            
            foreach (var p in primitives)
            {
                switch (p.Kind)
                {
                    case PrimitiveKind.Line:
                        e.Graphics.DrawLine(pen, cen.X + p.Pt0.X, cen.Y + p.Pt0.Y, cen.X + p.Pt1.X, cen.Y + p.Pt1.Y);
                        break;
                    case PrimitiveKind.Text:
                        e.Graphics.DrawString(p.Text, font, Brushes.Red, cen.X + p.Pt0.X, cen.Y + p.Pt0.Y);
                        break;
                }
            }

            if (actPrim != null) primitives.RemoveAt(primitives.Count - 1);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox.Capture = true;

            var loc = e.Location;
            loc.X -= ClientSize.Width / 2;
            loc.Y -= ClientSize.Height / 2;

            addActPrimitive();
            
            if (btPen.Checked)
            {
                if (primitives.LastOrDefault() != Primitive.UNDO_DELIMITER) primitives.Add(Primitive.UNDO_DELIMITER);
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
                };
                actPrim.Pt0.X -= 3;
                actPrim.Pt0.Y -= 13;
                textBox.Text = "";
                textBox.Visible = true;
                textBox.Focus();
            }

            Refresh();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (actPrim == null) return;

            var loc = e.Location;
            loc.X -= ClientSize.Width / 2;
            loc.Y -= ClientSize.Height / 2;
            
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

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (actPrim?.Kind == PrimitiveKind.Text)
            {
                actPrim.Text = textBox.Text;
                Refresh();
            }
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
                        primitives.Add(Primitive.UNDO_DELIMITER);
                        primitives.Add(actPrim);
                    }
                    actPrim = null;
                    textBox.Visible = false;
                    Focus();
                    break;
            }

            while (primitives.LastOrDefault() == Primitive.UNDO_DELIMITER)
            {
                primitives.RemoveAt(primitives.Count - 1);
            }

            actPrim = null;

            Refresh();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                if (!primitives.Any()) return;
                
                var i = primitives.FindLastIndex(x => x == Primitive.UNDO_DELIMITER);
                redoPrimitiveBlocks.Add(primitives.GetRange(i, primitives.Count - i).ToArray());
                primitives = primitives.GetRange(0, i);

                Refresh();
            }

            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Y)
            {
                if (!redoPrimitiveBlocks.Any()) return;

                var redoBlock = redoPrimitiveBlocks.Last();
                redoPrimitiveBlocks.RemoveAt(redoPrimitiveBlocks.Count - 1);
                primitives.AddRange(redoBlock);

                Refresh();
            }
        }
    }
}