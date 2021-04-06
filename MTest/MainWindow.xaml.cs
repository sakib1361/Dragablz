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

namespace MTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const double MinDistance = 20;
        private static List<MainWindow> mains = new List<MainWindow>();
        private bool _locked;

        public MainWindow()
        {
            InitializeComponent();
            mains.Add(this);

        }

        private Tuple<double[], double[]> GetPoints()
        {
            var hPoints = new List<double>();
            var vPoins = new List<double>();

            foreach (var item in mains)
            {
                if (item == this)
                {
                    var w = SystemParameters.WorkArea;
                    hPoints.Add(w.Left);
                    hPoints.Add(w.Right);
                    vPoins.Add(w.Top);
                    vPoins.Add(w.Bottom);
                }
                else
                {
                    hPoints.Add(item.Left);
                    hPoints.Add(item.Left + item.Width);
                    vPoins.Add(item.Top);
                    vPoins.Add(item.Top + item.Height);
                }
            }
            return new Tuple<double[], double[]>(hPoints.ToArray(), vPoins.ToArray());
        }
        private void UpdatePos()
        {
            if (_locked || this.WindowState == WindowState.Maximized) return;
            _locked = true;

            var all = GetPoints();
            var hPoints = all.Item1;
            var vPoins = all.Item2;
            foreach (var item in hPoints)
            {
                if (Math.Abs(Left - item) < MinDistance)
                {
                    Left = item;
                    break;
                }
                if (Math.Abs(Left + Width - item) < MinDistance)
                {
                    Left = item - Width;
                    break;
                }

            }
            foreach (var item in vPoins)
            {
                if (Math.Abs(Top - item) < MinDistance)
                {
                    Top = Math.Min(SystemParameters.WorkArea.Bottom - 10, item);
                    break;
                }
                if (Math.Abs(Top + Height - item) < MinDistance)
                {
                    Top = item - Height;
                    break;
                }

            }

            _locked = false;
        }

        private void UpdateSize()
        {
            if (_locked) return;
            _locked = true;

            var all = GetPoints();
            var hPoints = all.Item1;
            var vPoins = all.Item2;
            foreach (var item in hPoints)
            {
                //20
                var l1 = Math.Abs(Left - item);
                if (l1 < MinDistance && l1 > 0)
                {
                    Width += Left - item;
                    Left = item;
                    break;
                }
                if (Math.Abs(Left + Width - item) < MinDistance)
                {
                    Width = item - Left;
                    break;
                }

            }
            foreach (var item in vPoins)
            {
                var t1 = Math.Abs(Top - item);
                if (t1 < MinDistance && t1 > 0)
                {
                    Height += Top - item;
                    Top = item;
                    break;
                }
                if (Math.Abs(Top + Height - item) < MinDistance)
                {
                    Height = item - Top;
                    break;
                }

            }

            _locked = false;
        }


        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            mains.Remove(this);
        }

        private void MetroWindow_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (double.IsNaN(Height) || Height <= 0) return;
            UpdateSize();
        }

        private void MetroWindow_LocationChanged(object sender, EventArgs e)
        {
            if (double.IsNaN(Height) || Height <= 0) return;
            UpdatePos();
        }
    }
}
