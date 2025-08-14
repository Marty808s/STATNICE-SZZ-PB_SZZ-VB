using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI_FraktalniStrom
{
    public partial class MainWindow : Window
    {
        private FractalCanopy _drawer;
        private CancellationTokenSource _cts;
        private readonly ManualResetEventSlim _pauseGate = new(true);
        private bool _started; // <-- přidaná závlačka

        public MainWindow()
        {
            InitializeComponent();

            Loaded += async (_, __) =>
            {
                // --- barvy ---
                string[] colors = { "White", "Red", "Green", "Blue" };
                CB_Barva.ItemsSource = colors;
                CB_Barva.SelectedIndex = 0;
                CB_Barva.SelectionChanged += (_, __2) =>
                {
                    if (_drawer == null) return;
                    var name = CB_Barva.SelectedItem as string;
                    if (string.IsNullOrWhiteSpace(name)) return;
                    var brush = (Brush)new BrushConverter().ConvertFromString(name);
                    _drawer.BranchBrush = brush;
                    _drawer.TrunkBrush = brush;
                    RestartRender(); // ← spustí se jen když _started == true
                };

                // --- Slidery ---
                SL_Iterace.Minimum = 1;
                SL_Iterace.Maximum = 100;
                SL_Iterace.Value = 10;
                SL_Iterace.TickFrequency = 10;
                SL_Iterace.IsSnapToTickEnabled = true;

                SL_Uhel.Minimum = 0;
                SL_Uhel.Maximum = 90;
                SL_Uhel.Value = 40;
                SL_Uhel.TickFrequency = 1;
                SL_Uhel.IsSnapToTickEnabled = true;

                SL_Koeficient.Minimum = 0.10;
                SL_Koeficient.Maximum = 0.70;
                SL_Koeficient.Value = 0.20;
                SL_Koeficient.TickFrequency = 0.1;
                SL_Koeficient.IsSnapToTickEnabled = true;

                foreach (var child in LogicalTreeHelper.GetChildren(SliderGrid))
                    if (child is Slider s) s.ValueChanged += Slider_ValueChanged;

                // --- kreslíř (jen vytvořit, nespouštět) ---
                var brush0 = Brushes.White;
                _drawer = new FractalCanopy(
                    canvas: MyCanvas,
                    angle: SL_Uhel.Value,
                    iterations: (int)SL_Iterace.Value,
                    coef: SL_Koeficient.Value,
                    branchBrush: brush0,
                    trunkBrush: brush0,
                    thickness: 8,
                    name: TB_Nazev?.Text ?? string.Empty
                );

                //MyCanvas.SizeChanged += (_, __2) => RestartRender();
                UpdateHud();
            };
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_drawer == null) return;

            _drawer.Iterations = (int)SL_Iterace.Value;
            _drawer.Angle = SL_Uhel.Value;
            _drawer.Coef = SL_Koeficient.Value;

            UpdateHud();
            //RestartRender(); // ← proběhne jen po Start
        }

        private void UpdateHud()
        {
            TB_Hodnoty.Text =
                $"Iterace: {SL_Iterace.Value:0} | " +
                $"Úhel: {SL_Uhel.Value:0} | " +
                $"Koeficient: {SL_Koeficient.Value:0.00}";
        }

        private void TB_Nazev_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_drawer == null) return;
            _drawer.Name = TB_Nazev.Text;
            RestartRender(); // ← proběhne jen po Start
        }

        private void RestartRender()
        {
            if (!_started) return;    
            _ = StartRenderAsync();
        }

        private async Task StartRenderAsync()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                await Task.Yield();
            }

            _cts = new CancellationTokenSource();
            try
            {
                await _drawer.RenderAsync(_cts.Token, _pauseGate);
            }
            catch (OperationCanceledException)
            {
                return;
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
            }
        }

        private async void BTN_Start_Click(object sender, RoutedEventArgs e)
        {
            if (_cts != null) return;          // už běží -> nic nespouštěj podruhé

            _started = true;                   // uživatel spustil rendr
            BTN_Start.IsEnabled = false;       // Start během běhu vypnout
            BTN_Pozastavit.IsEnabled = true;
            BTN_Pokracovat.IsEnabled = false;
            BTN_Prerusit.IsEnabled = true;

            await StartRenderAsync();          // spustím první vykreslení

            BTN_Start.IsEnabled = true;
            BTN_Pozastavit.IsEnabled = false;
            BTN_Pokracovat.IsEnabled = false;
            BTN_Prerusit.IsEnabled = false;
        }


        private void BTN_Pozastavit_Click(object sender, RoutedEventArgs e)
        {
            _pauseGate.Reset();              // PAUSE
            BTN_Pozastavit.IsEnabled = false;
            BTN_Pokracovat.IsEnabled = true;
        }

        private void BTN_Pokracovat_Click(object sender, RoutedEventArgs e)
        {
            _pauseGate.Set();                // RESUME
            BTN_Pozastavit.IsEnabled = true;
            BTN_Pokracovat.IsEnabled = false;
        }

        private void BTN_Prerusit_Click(object sender, RoutedEventArgs e)
        {
            _cts?.Cancel();                  // STOP
            _started = false;
            BTN_Pozastavit.IsEnabled = true;
            BTN_Pokracovat.IsEnabled = false;
            MyCanvas.Children.Clear();
        }
    }
}
