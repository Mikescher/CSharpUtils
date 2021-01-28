using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSHC.WPF.Controls
{
    /// <summary>
    /// Interaction logic for SpinnerControl.xaml
    /// </summary>
    public partial class SpinnerControl : UserControl
    {
        const int angularDiff = 45;

        public SpinnerType SpinnerType
        {
            get { return (SpinnerType)GetValue(SpinnerTypeProperty); }
            set { SetValue(SpinnerTypeProperty, value); }
        }
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }
        public double Radius
        {
            get { return Diameter / 2; }
        }

        public double ItemRadius
        {
            get
            {
                return Diameter / (360 / angularDiff);
            }
        }

        public double ContinuousSizeReduction
        {
            get { return (double)GetValue(ContinuousSizeReductionProperty); }
            set { SetValue(ContinuousSizeReductionProperty, value); }
        }

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(SpinnerControl), new PropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(nameof(Diameter), typeof(double), typeof(SpinnerControl), new PropertyMetadata(new PropertyChangedCallback(CalculationPropertyChanged)));
        public static readonly DependencyProperty ContinuousSizeReductionProperty = DependencyProperty.Register(nameof(ContinuousSizeReduction), typeof(double), typeof(SpinnerControl), new PropertyMetadata(CalculationPropertyChanged));
        public static readonly DependencyProperty SpinnerTypeProperty = DependencyProperty.Register(nameof(SpinnerType), typeof(SpinnerType), typeof(SpinnerControl), new PropertyMetadata(CalculationPropertyChanged));

        private static void CalculationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var spinner = d as SpinnerControl;
            var grp = spinner.group;
            grp.Children.Clear();

            if(spinner.SpinnerType == SpinnerType.Ring)
            {
                double innerRad = spinner.Radius - spinner.ItemRadius;
                Point center = new Point(0, 0);
                grp.Children.Add(new EllipseGeometry( center, spinner.Radius, spinner.Radius));
                grp.Children.Add(new EllipseGeometry(center, innerRad, innerRad));                
                return;
            }
            var points = GetPointsOnCircle( spinner.Diameter/ 2);

            double r = spinner.ItemRadius;
            foreach (var point in points)
            {
                grp.Children.Add(new EllipseGeometry(point, r, r));
                r -= spinner.ContinuousSizeReduction;
            }
        }

        public SpinnerControl()
        {
            InitializeComponent();
            SpinnerRoot.DataContext = this;
        }

        private static List<Point> GetPointsOnCircle(double radius)
        {
            var result = new List<Point>();

            for(int angle = 0; angle < 360; angle += angularDiff)
            {
                double x = radius * System.Math.Sin(angle * System.Math.PI / 180);
                double y = radius * System.Math.Cos(angle * System.Math.PI / 180);
                result.Add(new Point(x, y));
            }

            return result;
        }
    }

    public enum SpinnerType
    {
        Circles,
        Ring
    }
}
