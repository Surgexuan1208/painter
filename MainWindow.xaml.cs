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
        Brush strokebrush = new SolidColorBrush(Colors.Red);
        Brush fillbrush = new SolidColorBrush(Colors.Yellow);
        int strokethickness = 1;
        Point start, end;
        int debuger=0;
        public MainWindow()
        {
            InitializeComponent();
            strokecolorpicker.SelectedColor = Colors.Red;
            fillcolorpicker.SelectedColor = Colors.Yellow;
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
            if (debuger == 1 && e.LeftButton != MouseButtonState.Pressed)
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
                debuger = 0;
            }
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
            debuger = 1;
            start = e.GetPosition(myCanvas);
            myCanvas.Cursor = Cursors.Cross;
            switch (shapetype)
            {
                case "Line":
                    DrawLine(Colors.Gray, 1);
                    break;
                case "Rectangle":
                    var rec = new Rectangle
                    {
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1,
                        Fill = Brushes.LightGray,
                        Width = 30,
                        Height = 50
                    };
                    myCanvas.Children.Add(rec);
                    rec.SetValue(LeftProperty, start.X);
                    rec.SetValue(TopProperty, start.Y);
                    break;
                case "Ellipse":
                    break;
            }
            DisplayStatus();
        }

        private void DisplayStatus()
        {
            int linecount = myCanvas.Children.OfType<Line>().Count();
            int rectanglecount = myCanvas.Children.OfType<Rectangle>().Count();
            int ellipsecount = myCanvas.Children.OfType<Ellipse>().Count();
            poslabel.Content = $"座標點:({Math.Round(start.X)},{Math.Round(start.Y)}),({Math.Round(end.X)},{Math.Round(end.Y)})";
            shapelabel.Content = $"Line:{linecount} Rectangle:{rectanglecount} Ellipse:{ellipsecount}";
        }

        private void myCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            switch (shapetype)
            {
                case "Line":
                    var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                    line.Stroke = strokebrush;
                    line.StrokeThickness = strokethickness;
                    debuger = 0;
                    break;
                case "Rectangle":
                    break;
                case "Ellipse":
                    break;
            }
        }
        private void strokecolorpicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokebrush = new SolidColorBrush((Color)strokecolorpicker.SelectedColor);
        }
        private void fillcolorpicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillbrush = new SolidColorBrush((Color)fillcolorpicker.SelectedColor);
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
