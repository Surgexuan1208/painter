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
                        var rectangle = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                        rectangle.Stroke = strokebrush;
                        rectangle.StrokeThickness = strokethickness;
                        rectangle.Fill = fillbrush;
                        break;
                    case "Ellipse":
                        var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        ellipse.Stroke = strokebrush;
                        ellipse.StrokeThickness = strokethickness;
                        ellipse.Fill = fillbrush;
                        break;
                }
                debuger = 0;
            }
            end = e.GetPosition(myCanvas);
            DisplayStatus();
            if (e.LeftButton == MouseButtonState.Pressed)
            {                        
                Point origin = new Point
                {
                    X = Math.Min(start.X, end.X),
                    Y = Math.Min(start.Y, end.Y)
                };
                switch (shapetype)
                {
                    case "Line":
                        var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                        line.X2 = Math.Round(end.X);
                        line.Y2 = Math.Round(end.Y);
                        break;
                    case "Rectangle":
                        var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                        rect.SetValue(Canvas.LeftProperty, origin.X);
                        rect.SetValue(Canvas.TopProperty, origin.Y);
                        rect.Width = Math.Abs(start.X - end.X);
                        rect.Height = Math.Abs(start.Y - end.Y);
                        break;
                    case "Ellipse":
                        var elli = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        elli.SetValue(Canvas.LeftProperty, origin.X);
                        elli.SetValue(Canvas.TopProperty, origin.Y);
                        elli.Width = Math.Abs(start.X - end.X);
                        elli.Height = Math.Abs(start.Y - end.Y);
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
                    Line myline = new Line
                    {
                        Stroke = Brushes.Gray,
                        X1 = start.X,
                        Y1 = start.Y,
                        X2 = end.X,
                        Y2 = end.Y,
                        StrokeThickness = 1,
                    };
                    myCanvas.Children.Add(myline);
                    break;
                case "Rectangle":
                    var rec = new Rectangle
                    {
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1,
                        Fill = Brushes.LightGray
                    };
                    myCanvas.Children.Add(rec);
                    rec.SetValue(LeftProperty, start.X);
                    rec.SetValue(TopProperty, start.Y);
                    break;
                case "Ellipse":
                    var ell = new Ellipse
                    {
                        Stroke = Brushes.Gray,
                        StrokeThickness = 1,
                        Fill = Brushes.LightGray
                    };
                    myCanvas.Children.Add(ell);
                    ell.SetValue(LeftProperty, start.X);
                    ell.SetValue(TopProperty, start.Y);
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
            debuger = 0;
            switch (shapetype)
            {
                case "Line":
                    var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                    line.Stroke = strokebrush;
                    line.StrokeThickness = strokethickness;
                    break;
                case "Rectangle":
                    var rectangle = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                    rectangle.Stroke = strokebrush;
                    rectangle.StrokeThickness = strokethickness;
                    rectangle.Fill = fillbrush;
                    break;
                case "Ellipse":
                    var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                    ellipse.Stroke = strokebrush;
                    ellipse.StrokeThickness = strokethickness;
                    ellipse.Fill = fillbrush;
                    break;
            }
            myCanvas.Cursor = Cursors.Arrow;
        }
        private void strokecolorpicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            strokebrush = new SolidColorBrush((Color)strokecolorpicker.SelectedColor);
        }
        private void clearbtn_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            DisplayStatus();
        }
        private void fillcolorpicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            fillbrush = new SolidColorBrush((Color)fillcolorpicker.SelectedColor);
        }
    }
}