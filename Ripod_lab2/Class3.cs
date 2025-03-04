using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ASAP_Scheduler
{
    public class GanttChartDrawer
    {
        private Canvas canvas;
        private ASAPAlgorithm asap;

        public GanttChartDrawer(Canvas ganttCanvas, ASAPAlgorithm algorithm)
        {
            canvas = ganttCanvas;
            asap = algorithm;
        }

        public void DrawChart()
        {
            canvas.Children.Clear();
            int stepHeight = 50;
            int xOffset = 50;

            // Прорисовываем шаги и операции с цветовой дифференциацией
            for (int i = 0; i < asap.Steps.Count; i++)
            {
                // Заголовок для шага
                TextBlock stepLabel = new TextBlock { Text = $"Шаг {i + 1}" };
                Canvas.SetLeft(stepLabel, 5);
                Canvas.SetTop(stepLabel, i * stepHeight + 10);
                canvas.Children.Add(stepLabel);

                // Прямоугольники для операций с типами
                foreach (var op in asap.Steps[i])
                {
                    // Установка цвета в зависимости от типа операции
                    Brush opColor = (op % 2 == 0) ? Brushes.Coral : Brushes.LightGreen;

                    Rectangle rect = new Rectangle
                    {
                        Width = 60,
                        Height = 30,
                        Fill = opColor
                    };
                    Canvas.SetLeft(rect, xOffset + op * 80);
                    Canvas.SetTop(rect, i * stepHeight + 5);
                    canvas.Children.Add(rect);

                    TextBlock text = new TextBlock
                    {
                        Text = $"Op {op + 1}",
                        Foreground = Brushes.Black
                    };
                    Canvas.SetLeft(text, xOffset + op * 80 + 10);
                    Canvas.SetTop(text, i * stepHeight + 10);
                    canvas.Children.Add(text);
                }
            }
        }
    }
}
