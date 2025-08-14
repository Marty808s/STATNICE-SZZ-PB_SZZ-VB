using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UI_FraktalniStrom
{
    public class FractalCanopy
    {
        private readonly Canvas _canvas;

        // privátní pole
        private double _angleDeg;
        private int _iterations;
        private double _coef;
        private Brush _branchBrush;
        private Brush _trunkBrush;
        private double _thickness;

        // veřejné vlastnosti (z UI)
        public double Angle { get => _angleDeg; set => _angleDeg = value; }
        public int Iterations { get => _iterations; set => _iterations = value; }
        public double Coef { get => _coef; set => _coef = value; }
        public Brush BranchBrush { get => _branchBrush; set => _branchBrush = value ?? Brushes.White; }
        public Brush TrunkBrush { get => _trunkBrush; set => _trunkBrush = value ?? Brushes.White; }
        public double Thickness { get => _thickness; set => _thickness = value; }
        public string Name { get; set; } = string.Empty;

        public FractalCanopy(Canvas canvas, double angle, int iterations, double coef,
                             Brush branchBrush, Brush trunkBrush, double thickness = 6, string name = "")
        {
            _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            _angleDeg = angle;
            _iterations = iterations;
            _coef = coef;
            _branchBrush = branchBrush ?? Brushes.White;
            _trunkBrush = trunkBrush ?? Brushes.White;
            _thickness = thickness;
            Name = name ?? string.Empty;
        }

        public async Task RenderAsync(
            CancellationToken ct,
            ManualResetEventSlim pauseGate,
            int batchSize = 50,     // menší batch = plynulejší
            int delayMs = 30      // cca 60 FPS; zvětši pro pomalejší animaci
        )
        {
            // uvolni UI hned na začátku
            await Dispatcher.Yield(DispatcherPriority.Background);
            ct.ThrowIfCancellationRequested();

            double w = _canvas.ActualWidth > 0 ? _canvas.ActualWidth : _canvas.RenderSize.Width;
            double h = _canvas.ActualHeight > 0 ? _canvas.ActualHeight : _canvas.RenderSize.Height;
            if (w <= 0 || h <= 0) return;

            _canvas.Children.Clear();

            // kmen
            Point start = new Point(w / 2.0, h - 12);
            double trunkLen = Math.Min(w, h) * 0.25;
            Point end = EndPoint(start, trunkLen, -90);
            AddLine(start, end, TrunkBrush, Thickness);

            // iterativní DFS: (start, len, angle, depth, thickness)
            var stack = new Stack<(Point s, double len, double ang, int d, double t)>();
            stack.Push((end, trunkLen * Coef, -90, Iterations, Math.Max(1.0, Thickness * 0.8)));

            int steps = 0;
            while (stack.Count > 0)
            {
                ct.ThrowIfCancellationRequested();
                pauseGate?.Wait(ct); // pauza

                var (s, len, ang, d, t) = stack.Pop();
                if (d <= 0 || len < 1) continue;

                // čtení aktuálních parametrů = reaguje na změny sliderů i během běhu
                double aL = ang - Angle;
                var endL = EndPoint(s, len, aL);
                AddLine(s, endL, BranchBrush, t);
                stack.Push((endL, len * Coef, aL, d - 1, Math.Max(1.0, t * 0.75)));

                double aR = ang + Angle;
                var endR = EndPoint(s, len, aR);
                AddLine(s, endR, BranchBrush, t);
                stack.Push((endR, len * Coef, aR, d - 1, Math.Max(1.0, t * 0.75)));

                // POMALÉ, PLYNULÉ vykreslování:
                if (++steps % batchSize == 0)
                {
                    // 1) předej CPU dispatcheru, aby UI stihlo vykreslit přidané čáry
                    await Dispatcher.Yield(DispatcherPriority.Background);
                    // 2) volitelná brzda, aby to bylo opravdu vidět
                    if (delayMs > 0)
                        await Task.Delay(delayMs, ct);
                }
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                _canvas.Children.Add(new TextBlock
                {
                    Text = Name,
                    Foreground = BranchBrush,
                    Margin = new Thickness(8),
                    FontSize = 14
                });
            }
        }

        private void DrawBranch(Point start, double length, double angleDeg, int depth, double thickness, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (depth <= 0 || length < 1) return;

            // levá
            double aL = angleDeg - _angleDeg;
            var endL = EndPoint(start, length, aL);
            AddLine(start, endL, _branchBrush, thickness);
            DrawBranch(endL, length * _coef, aL, depth - 1, Math.Max(1.0, thickness * 0.75), ct);

            // pravá
            double aR = angleDeg + _angleDeg;
            var endR = EndPoint(start, length, aR);
            AddLine(start, endR, _branchBrush, thickness);
            DrawBranch(endR, length * _coef, aR, depth - 1, Math.Max(1.0, thickness * 0.75), ct);
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
            _canvas.Children.Add(new Line
            {
                X1 = a.X,
                Y1 = a.Y,
                X2 = b.X,
                Y2 = b.Y,
                Stroke = brush,
                StrokeThickness = thickness,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            });
        }
    }
}
