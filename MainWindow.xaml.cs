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

namespace painter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string shapetype;
        Color strokecolor = Colors.Red;
        Brush strokebrush = new SolidColorBrush(Colors.Red);
        int strokethickness = 1;
        Point start, end;
        public MainWindow()
        {
            InitializeComponent();
            strokecolorpicker.SelectedColor = strokecolor;
            
        }
        
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetbutton = sender as RadioButton;
            shapetype = targetbutton.Tag.ToString();
        }

        private void stroke_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            strokethickness = (int)stroke_slider.Value;
        }

        private void myCanvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(myCanvas);
            myCanvas.Cursor = Cursors.Cross;
            switch (shapetype)
            {
                case "Line":
                    break;
                case "Rectangle":
                    break;
                case "Ellipse":
                    break;
            }
        }
    }
}
