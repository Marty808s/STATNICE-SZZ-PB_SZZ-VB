using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI_FraktalniStrom
{
    public partial class MainWindow : Window
    {
        private FractalCanopy _drawer;
        private CancellationTokenSource _cts;
        private readonly ManualResetEventSlim _pauseGate = new(initialState: true); // true = běží
        private bool _isRunning;
        private bool _isPaused;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += (_, __) =>
            {
                // init hodnot - combobox barev
                string[] colors = { "White", "Red", "Green", "Blue" };
                CB_Barva.ItemsSource = colors;
                CB_Barva.SelectedIndex = 0;

                // Slider - iterace
                // minimální hodnota
                SL_Iterace.Minimum = 1;
                // maximální hodnota
                SL_Iterace.Maximum = 2000;
                // počáteční nastavení (aktuální hodnota)
                SL_Iterace.Value = 20;
                // krok posuvu (volitelné, default je 1)
                SL_Iterace.TickFrequency = 10;

                // Slider - úhel
                // minimální hodnota
                SL_Uhel.Minimum = 1;
                // maximální hodnota
                SL_Uhel.Maximum = 360;
                // počáteční nastavení (aktuální hodnota)
                SL_Iterace.Value = 40;
                // krok posuvu (volitelné, default je 1)
                SL_Iterace.TickFrequency = 10;

                // Slider - koeficient
                // minimální hodnota
                SL_Koeficient.Minimum = 1;
                // maximální hodnota
                SL_Koeficient.Maximum = 20;
                // počáteční nastavení (aktuální hodnota)
                SL_Koeficient.Value = 5;
                // krok posuvu (volitelné, default je 1)
                SL_Koeficient.TickFrequency = 1;

                // přidám všem sliderům handler na ValueChanged - tím změním hodnoty fraktálu a zobrazím hodnoty níže
                foreach (var child in LogicalTreeHelper.GetChildren(SliderGrid)) // pokud jsou v Gridu
                {
                    if (child is Slider slider)
                        slider.ValueChanged += Slider_ValueChanged;
                }

                _drawer = new FractalCanopy(
                    canvas: MyCanvas,
                    angle: 30,          // úhel rozvětvení (stupně)
                    iterations: 10,     // hloubka rekurze
                    coef: 0.70,         // zkrácení větví
                    branchBrush: Brushes.White,
                    trunkBrush: Brushes.White,
                    thickness: 8,
                    name: TB_Nazev?.Text ?? string.Empty
                );

                // překresluj při změně velikosti plátna
                MyCanvas.SizeChanged += (_, __2) => _drawer?.Render();

                _drawer.Render();
            };
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TB_Hodnoty.Text =
                $"Iterace: {SL_Iterace.Value:0} | " +
                $"Úhel: {SL_Uhel.Value:0} | " +
                $"Koeficient: {SL_Koeficient.Value:0.00}";
        }

        private void TB_Nazev_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_drawer == null) return;
            _drawer._name = TB_Nazev.Text;  // použijeme property
            _drawer.Render();
        }

        private void BTN_Start_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning) return;                  // už běží
            _cts = new CancellationTokenSource();
            _pauseGate.Set();                        // povolit průchod
            _isRunning = true;
            _isPaused = false;
        }

    }
}
