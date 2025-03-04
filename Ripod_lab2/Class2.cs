using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ASAP_Scheduler
{
    public class GraphDrawer
    {
        private Canvas canvas;
        private ASAPAlgorithm asap;

        public GraphDrawer(Canvas graphCanvas, ASAPAlgorithm algorithm)
        {
            canvas = graphCanvas;
            asap = algorithm;
        }

        public void DrawGraph()
        {
            canvas.Children.Clear();
            var matrix = asap.GetAdjacencyMatrix();
            int size = matrix.GetLength(0);

            Dictionary<int, (double X, double Y)> positions = new Dictionary<int, (double, double)>();
            double radius = 150;
            double centerX = canvas.ActualWidth / 2;
            double centerY = canvas.ActualHeight / 2;

            // Расставляем узлы по кругу
            for (int i = 0; i < size; i++)
            {
                double angle = 2 * System.Math.PI * i / size;
                double x = centerX + radius * System.Math.Cos(angle);
                double y = centerY + radius * System.Math.Sin(angle);
                positions[i] = (x, y);
            }

            // Рисуем рёбра с направлениями
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        Line line = new Line
                        {
                            X1 = positions[i].X,
                            Y1 = positions[i].Y,
                            X2 = positions[j].X,
                            Y2 = positions[j].Y,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2
                        };
                        canvas.Children.Add(line);

                        // Рисуем стрелку для обозначения зависимости
                        DrawArrow(positions[i].X, positions[i].Y, positions[j].X, positions[j].Y);
                    }
                }
            }

            // Рисуем узлы (операции)
            foreach (var pos in positions)
            {
                Ellipse ellipse = new Ellipse
                {
                    Width = 30,
                    Height = 30,
                    Fill = Brushes.LightBlue
                };
                Canvas.SetLeft(ellipse, pos.Value.X - 15);
                Canvas.SetTop(ellipse, pos.Value.Y - 15);
                canvas.Children.Add(ellipse);

                TextBlock text = new TextBlock
                {
                    Text = $"Op {pos.Key + 1}",
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(text, pos.Value.X - 10);
                Canvas.SetTop(text, pos.Value.Y - 10);
                canvas.Children.Add(text);
            }
        }

        // Метод для рисования стрелки
        private void DrawArrow(double x1, double y1, double x2, double y2)
        {
            var arrowHead = new Polygon
            {
                Points = new PointCollection
                {
                    new System.Windows.Point(x2, y2),
                    new System.Windows.Point(x2 - 5, y2 - 5),
                    new System.Windows.Point(x2 - 5, y2 + 5)
                },
                Fill = Brushes.Black
            };
            canvas.Children.Add(arrowHead);

            Line arrowLine = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            canvas.Children.Add(arrowLine);
        }
    }
}
