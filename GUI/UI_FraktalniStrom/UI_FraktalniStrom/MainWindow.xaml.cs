using System.Windows;
using System.Windows.Media;

namespace UI_FraktalniStrom
{
    // https://web.archive.org/web/20070128211616/http://library.thinkquest.org/26242/full/types/ch3.html
    public partial class MainWindow : Window
    {
        private FractalCanopy _drawer;

        public MainWindow()
        {
            InitializeComponent();

            // Kresli až po načtení, aby Canvas znal své rozměry
            Loaded += (_, __) =>
            {
                _drawer = new FractalCanopy(
                    canvas: MyCanvas,
                    angle: 30,          // úhel rozvětvení ve stupních
                    iterations: 10,     // hloubka rekurze
                    coef: 0.70,         // zkrácení větví
                    branchBrush: Brushes.White,
                    trunkBrush: Brushes.SaddleBrown,
                    thickness: 8
                );
                _drawer.Render();
            };
        }
    }
}
