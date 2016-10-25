using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonteCarloPiDemo
{
    public partial class CircleBox : UserControl
    {
        public CircleBox()
        {
            InitializeComponent();
        }
        private int inside = 0;
        private int outside = 0;

        bool EvaluatePoint(PointF p)
        {
            bool result;
            result = p.X * p.X + p.Y * p.Y <= 1;
            PiUpdate?.Invoke(Pi);
            return result;
        }

        public double Pi => 4.0 * ((double)inside / (double)(inside + outside));

        List<PointF> _fdots = new List<PointF>();
        public IEnumerable<Point> Dots => new List<PointF>(_fdots).Select(Project);

        public event Action<double> PiUpdate;

        public int DotSize
        {
            get { return _dotSize; }
            set
            {
                _dotSize = value;
                ReDrawBitmap();
            }
        }

        public Brush OutsideDotBrush
        {
            get { return _outsideDotBrush; }
            set
            {
                _outsideDotBrush = value;
                ReDrawBitmap();
            }
        }

        public Brush InsideDotBrush
        {
            get { return _insideDotBrush; }
            set
            {
                _insideDotBrush = value;
                ReDrawBitmap();
            }
        }

        private void PaintDots(Graphics g)
        {
            foreach (PointF fdot in _fdots)
            {
                var dot = Project(fdot);
                bool pointinside = EvaluatePoint(fdot);
                g.FillEllipse(pointinside ? InsideDotBrush : OutsideDotBrush, new Rectangle(dot.X - (DotSize / 2), dot.Y - (DotSize / 2), DotSize, DotSize));
            }
        }

        Point Center => new Point(Width / 2, Height / 2);
        private Rectangle GetArea()
        {
            Point center = Center;
            int size = Math.Min(Width, Height) - 100;
            Point Pos = new Point(center.X - size / 2, center.Y - size / 2);
            Rectangle area = new Rectangle(Pos, new Size(size, size));
            return area;
        }

        public Rectangle Area => GetArea();

        private void PaintCircle(Graphics g)
        {
            g.FillRectangle(Brushes.White, Area);
            Pen p = new Pen(Color.Black, 5);
            p.Width = 5;
            g.DrawLine(p, Area.X + Area.Width / 2, Area.Y, Area.X + Area.Width / 2, Area.Y + Area.Height);
            g.DrawLine(p, Area.X, Area.Y + Area.Height / 2, Area.X + Area.Width, Area.Y + Area.Height / 2);
            g.DrawEllipse(p, Area);
            Font drawFont = new Font("Arial", 16);

            g.DrawString("1", drawFont, Brushes.Blue, Area.X + Area.Width, (Area.Y + Area.Height / 2) - (drawFont.Height / 2));
            g.DrawString("-1", drawFont, Brushes.Blue, Area.X - drawFont.Height, (Area.Y + Area.Height / 2) - (drawFont.Height / 2));
            g.DrawString("-1", drawFont, Brushes.Blue, Area.X + Area.Width / 2 - drawFont.Height / 2, (Area.Y + Area.Height));
            g.DrawString("1", drawFont, Brushes.Blue, Area.X + Area.Width / 2 - drawFont.Height / 2, (Area.Y) - drawFont.Height);

        }

        Random r = new Random();

        public void AddRandomPoint()
        {
            double x = (r.NextDouble() - 0.5) * 2.0;
            double y = (r.NextDouble() - 0.5) * 2.0;
            PointF p = new PointF((float)x, (float)y);

            bool pointinside = EvaluatePoint(p);
            if (pointinside)
                inside++;
            else
                outside++;

            _fdots.Add(p);
            lock (b)
            {
                BeginInvoke((Action)(() =>
                {
                    Graphics g = Graphics.FromImage(b);
                    Point dot = Project(p);
                    g.FillEllipse(pointinside ? InsideDotBrush : OutsideDotBrush, new Rectangle(dot.X - (DotSize / 2), dot.Y - (DotSize / 2), DotSize, DotSize));
                    pictureBox1.Invalidate();

                }));
            }

            //            DelayedInvalidate();
        }



        private Point Project(PointF fp)
        {
            int x = (int)Math.Round(Center.X + (fp.X / 2.0 * Area.Width));
            int y = (int)Math.Round(Center.Y + (fp.Y / 2.0 * Area.Height));
            return new Point(x, y);
        }

        void ReDrawBitmap()
        {
            b?.Dispose();
            b = new Bitmap(Width, Height);
            Graphics flagGraphics = Graphics.FromImage(b);
            PaintCircle(flagGraphics);
            PaintDots(flagGraphics);
            pictureBox1.Image = b;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ReDrawBitmap();
        }

        private Bitmap b;
        private int _dotSize = 8;
        private Brush _outsideDotBrush = Brushes.Red;
        private Brush _insideDotBrush = Brushes.Blue;

    }
}
