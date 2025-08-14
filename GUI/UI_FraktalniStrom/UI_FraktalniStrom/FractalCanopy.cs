using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UI_FraktalniStrom
{
    public class FractalCanopy
    {
        private readonly Canvas _canvas;
        private double _angleDeg;
        private int _iterations;
        private double _coef;
        private Brush _branchBrush;
        private Brush _trunkBrush;
        private double _thickness;
        private string _color;

        public string _name { get; set; }

        public FractalCanopy(Canvas canvas, double angle, int iterations, double coef,
                             Brush branchBrush, Brush trunkBrush, double thickness = 6, string name = "", string color = "white")
        {
            _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            _angleDeg = angle;
            _iterations = iterations;
            _coef = coef;
            _branchBrush = branchBrush ?? Brushes.White;
            _trunkBrush = trunkBrush ?? Brushes.White;
            _thickness = thickness;
            _name = name ?? string.Empty;
            _color = color;
        }

        public async Task Render()
        {
            // kdyby rozměry ještě nebyly, zkus RenderSize
            double w = _canvas.ActualWidth > 0 ? _canvas.ActualWidth : _canvas.RenderSize.Width;
            double h = _canvas.ActualHeight > 0 ? _canvas.ActualHeight : _canvas.RenderSize.Height;
            if (w <= 0 || h <= 0) return;

            _canvas.Children.Clear();

            // Kmen: start dole uprostřed
            Point start = new Point(w / 2.0, h - 12);
            double trunkLen = Math.Min(w, h) * 0.25;
            Point end = EndPoint(start, trunkLen, -90);

            AddLine(start, end, _trunkBrush, _thickness);

            // Větvení
            DrawBranch(end, trunkLen * _coef, -90, _iterations, Math.Max(1.0, _thickness * 0.8));

            // (volitelné) Titulek do rohu
            if (!string.IsNullOrWhiteSpace(_name))
            {
                var tb = new System.Windows.Controls.TextBlock
                {
                    Text = _name,
                    Foreground = _branchBrush,
                    Margin = new Thickness(8),
                    FontSize = 14
                };
                _canvas.Children.Add(tb);
            }
        }

        private void DrawBranch(Point start, double length, double angleDeg, int depth, double thickness)
        {
            if (depth <= 0 || length < 1) return;

            // Levá větev
            double aL = angleDeg - _angleDeg;
            Point endL = EndPoint(start, length, aL);
            AddLine(start, endL, _branchBrush, thickness);
            DrawBranch(endL, length * _coef, aL, depth - 1, Math.Max(1.0, thickness * 0.75));

            // Pravá větev
            double aR = angleDeg + _angleDeg;
            Point endR = EndPoint(start, length, aR);
            AddLine(start, endR, _branchBrush, thickness);
            DrawBranch(endR, length * _coef, aR, depth - 1, Math.Max(1.0, thickness * 0.75));
        }

        private static Point EndPoint(Point start, double length, double angleDeg)
        {
            double rad = angleDeg * Math.PI / 180.0;
            return new Point(
                start.X + Math.Cos(rad) * length,
                start.Y + Math.Sin(rad) * length
            );
        }

        private void AddLine(Point a, Point b, Brush brush, double thickness)
        {
            var line = new Line
            {
                X1 = a.X,
                Y1 = a.Y,
                X2 = b.X,
                Y2 = b.Y,
                Stroke = brush,
                StrokeThickness = thickness,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };
            _canvas.Children.Add(line);
        }
    }
}
