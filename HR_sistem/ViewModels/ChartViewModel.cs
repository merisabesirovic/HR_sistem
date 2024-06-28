using HR_sistem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace HR_sistem.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public ObservableCollection<PerformanceReview> Reviews { get; set; }

        public ChartViewModel(ObservableCollection<PerformanceReview> reviews)
        {
            Reviews = reviews;
        }

        public void DrawChart(Canvas canvas)
        {
            if (Reviews == null || Reviews.Count == 0)
                return;

            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;
            double margin = 40;

            double xStep = (width - 2 * margin) / (Reviews.Count - 1);
            double maxScore = 5; 
            double yStep = (height - 2 * margin) / maxScore;

            
            Line xAxis = new Line
            {
                X1 = margin,
                Y1 = height - margin,
                X2 = width - margin,
                Y2 = height - margin,
                Stroke = Brushes.Black
            };
            Line yAxis = new Line
            {
                X1 = margin,
                Y1 = margin,
                X2 = margin,
                Y2 = height - margin,
                Stroke = Brushes.Black
            };
            canvas.Children.Add(xAxis);
            canvas.Children.Add(yAxis);

            
            for (int i = 0; i < Reviews.Count; i++)
            {
                double x = margin + i * xStep;
                double y = height - margin - (Reviews[i].Score * yStep);

                Ellipse dataPoint = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = Brushes.Red
                };
                Canvas.SetLeft(dataPoint, x - 4);
                Canvas.SetTop(dataPoint, y - 4);
                canvas.Children.Add(dataPoint);

                if (i > 0)
                {
                    double xPrev = margin + (i - 1) * xStep;
                    double yPrev = height - margin - (Reviews[i - 1].Score * yStep);

                    Line line = new Line
                    {
                        X1 = xPrev,
                        Y1 = yPrev,
                        X2 = x,
                        Y2 = y,
                        Stroke = Brushes.Blue,
                        StrokeThickness = 2
                    };
                    canvas.Children.Add(line);
                }
            }
        }
    }
}
