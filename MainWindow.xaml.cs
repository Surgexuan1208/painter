using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        string actionType = "Draw";
        public MainWindow()
        {
            InitializeComponent();
            strokecolorpicker.SelectedColor = Color.FromArgb(255,255,0,0);
            fillcolorpicker.SelectedColor = Colors.Yellow;
        }
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            var targetbutton = sender as RadioButton;
            shapetype = targetbutton.Tag.ToString();
            actionType = "Draw";
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
                    case "Polyline":
                        var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                        polyline.Points.Add(end);
                        polyline.Stroke = strokebrush;
                        polyline.StrokeThickness = strokethickness;
                        polyline.Fill = fillbrush;
                        break;
                }
                debuger = 0;
            }
            end = e.GetPosition(myCanvas);
            DisplayStatus();
            switch (actionType)
            {
                case "Draw":
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        Point origin = new Point
                        {
                            X = Math.Min(start.X, end.X),
                            Y = Math.Min(start.Y, end.Y)
                        };
                        double width = Math.Abs(end.X - start.X);
                        double height = Math.Abs(end.Y - start.Y);
                        switch (shapetype)
                        {
                            case "Line":
                                var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                                line.X2 = end.X;
                                line.Y2 = end.Y;
                                break;
                            case "Rectangle":
                                var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                                rect.Width = width;
                                rect.Height = height;
                                rect.SetValue(Canvas.LeftProperty, origin.X);
                                rect.SetValue(Canvas.TopProperty, origin.Y);
                                break;
                            case "Ellipse":
                                var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                                ellipse.Width = width;
                                ellipse.Height = height;
                                ellipse.SetValue(Canvas.LeftProperty, origin.X);
                                ellipse.SetValue(Canvas.TopProperty, origin.Y);
                                break;
                            case "Polyline":
                                var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                                polyline.Points.Add(end);
                                break;
                        }
                    }
                    break;
                case "Erase":
                    var shape = e.OriginalSource as Shape;
                    myCanvas.Children.Remove(shape);
                    if (myCanvas.Children.Count == 0) myCanvas.Cursor = Cursors.Arrow;
                    break;
            }
        }
        private void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            debuger = 1;
            start = e.GetPosition(myCanvas);
            myCanvas.Cursor = Cursors.Cross;
            if (actionType == "Draw")
            {
                switch (actionType)
                {
                    case "Draw":
                        switch (shapetype)
                        {
                            case "Line":
                                Line line = new Line
                                {
                                    Stroke = Brushes.Gray,
                                    StrokeThickness = 1,
                                    X1 = start.X,
                                    Y1 = start.Y,
                                    X2 = end.X,
                                    Y2 = end.Y
                                };
                                myCanvas.Children.Add(line);
                                break;

                            case "Rectangle":
                                var rect = new Rectangle
                                {
                                    Stroke = Brushes.Gray,
                                    StrokeThickness = 1,
                                    Fill = Brushes.LightGray,
                                };
                                myCanvas.Children.Add(rect);
                                rect.SetValue(Canvas.LeftProperty, start.X);
                                rect.SetValue(Canvas.TopProperty, start.Y);
                                break;

                            case "Ellipse":
                                var ellipse = new Ellipse
                                {
                                    Stroke = Brushes.Gray,
                                    StrokeThickness = 1,
                                    Fill = Brushes.LightGray,
                                };
                                myCanvas.Children.Add(ellipse);
                                ellipse.SetValue(Canvas.LeftProperty, start.X);
                                ellipse.SetValue(Canvas.TopProperty, start.Y);
                                break;

                            case "Polyline":
                                var polyline = new Polyline
                                {
                                    Stroke = Brushes.Gray,
                                    StrokeThickness = 1,
                                    Fill = Brushes.LightGray,
                                };
                                myCanvas.Children.Add(polyline);
                                break;
                        }
                        break;
                    case "Erase":
                        break;
                }
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
            if (actionType == "Draw")
            {
                switch (shapetype)
                {
                    case "Line":
                        var line = myCanvas.Children.OfType<Line>().LastOrDefault();
                        line.Stroke = strokebrush;
                        line.StrokeThickness = strokethickness;
                        break;
                    case "Rectangle":
                        var rect = myCanvas.Children.OfType<Rectangle>().LastOrDefault();
                        rect.Stroke = strokebrush;
                        rect.Fill = fillbrush;
                        rect.StrokeThickness = strokethickness;
                        break;
                    case "Ellipse":
                        var ellipse = myCanvas.Children.OfType<Ellipse>().LastOrDefault();
                        ellipse.Stroke = strokebrush;
                        ellipse.Fill = fillbrush;
                        ellipse.StrokeThickness = strokethickness;
                        break;
                    case "Polyline":
                        var polyline = myCanvas.Children.OfType<Polyline>().LastOrDefault();
                        polyline.Stroke = strokebrush;
                        polyline.Fill = fillbrush;
                        polyline.StrokeThickness = strokethickness;
                        break;
                }
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
        private void eraserButton_Click(object sender, RoutedEventArgs e)
        {
            actionType = "Erase";
            myCanvas.Cursor = Cursors.Hand;
            DisplayStatus();
        }
        private void saveCanvas_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "儲存畫布";
            saveFileDialog.Filter = "Png檔案|*.png|所有檔案|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                    (int)myCanvas.ActualWidth,
                    (int)myCanvas.ActualHeight,
                    96d, 96d, PixelFormats.Default);

                renderBitmap.Render(myCanvas);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                string fileName = saveFileDialog.FileName;
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    encoder.Save(fs);
                }

                MessageBox.Show($"Canvas content saved as {fileName}");
            }
        }
        private void clearMenuItem_Click(object sender, RoutedEventArgs e)
        {
            myCanvas.Children.Clear();
            DisplayStatus();
        }
    }
}