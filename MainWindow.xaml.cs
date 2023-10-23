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
        string shapetype="Line";
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
            end = e.GetPosition(myCanvas);
            DisplayStatus();
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                switch (shapetype)
                {
                    case "Line":
                        var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                        line.X2 = end.X;
                        line.Y2 = end.Y;
                        break;
                    case "Rectangle":
                        break;
                    case "Ellipse":
                        break;
                }
            }
        }

        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            start = e.GetPosition(myCanvas);
            myCanvas.Cursor = Cursors.Cross;
            switch (shapetype)
            {
                case "Line":
                    DrawLine(Colors.Gray, 1);
                    break;
                case "Rectangle":
                    break;
                case "Ellipse":
                    break;
            }
            DisplayStatus();
        }

        private void DisplayStatus()
        {
            poslabel.Content = $"座標點:({Math.Round(start.X)},{Math.Round(start.Y)}),({Math.Round(end.X)},{Math.Round(end.Y)})";
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (shapetype)
            {
                case "Line":
                    var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                    line.Stroke = strokebrush;
                    line.StrokeThickness = strokethickness;
                    break;
                case "Rectangle":
                    break;
                case "Ellipse":
                    break;
            }
        }

        private void DrawLine(Color c, int v)
        {
            Brush stroke =new SolidColorBrush(c);
            Line myline = new Line
            {
                Stroke = stroke,
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                StrokeThickness = v,
            };
            myCanvas.Children.Add(myline);
        }
    }
}
