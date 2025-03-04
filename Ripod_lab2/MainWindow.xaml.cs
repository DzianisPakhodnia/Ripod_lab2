using ASAP_Scheduler;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ripod_lab2
{
    public partial class MainWindow : Window
    {
        private ASAPAlgorithm asap;
        private GraphDrawer graphDrawer;
        private GanttChartDrawer ganttDrawer;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Загрузка данных из файла
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            asap = new ASAPAlgorithm();
            asap.LoadData("C:\\Users\\yodas\\source\\repos\\Ripod_lab2\\Ripod_lab2\\TextFile1.txt");
            MessageBox.Show("Данные загружены!");

            // Отображаем матрицу смежности с улучшенным стилем
            DrawAdjacencyMatrix();
        }

        // Запуск алгоритма ASAP
        private void RunASAP_Click(object sender, RoutedEventArgs e)
        {
            if (asap == null)
            {
                MessageBox.Show("Сначала загрузите данные.");
                return;
            }

            asap.ExecuteASAP();

            // Отображаем граф зависимостей
            graphDrawer = new GraphDrawer(GraphCanvas, asap);
            graphDrawer.DrawGraph();

            // Отображаем диаграмму Ганта
            ganttDrawer = new GanttChartDrawer(GanttCanvas, asap);
            ganttDrawer.DrawChart();

            // Показываем шаги и процессоры
            DisplaySteps();
            DisplayProcessors();
        }

        // Отображение матрицы смежности
        private void DrawAdjacencyMatrix()
        {
            AdjacencyMatrixGrid.ItemsSource = ConvertToDataTable(asap.GetAdjacencyMatrix()).DefaultView;
        }

        // Преобразование матрицы смежности в DataTable
        private DataTable ConvertToDataTable(int[,] matrix)
        {
            DataTable table = new DataTable();
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            // Добавление заголовков столбцов
            for (int i = 0; i < cols; i++)
            {
                table.Columns.Add($"Op {i + 1}", typeof(int));
            }

            // Добавление строк в таблицу
            for (int i = 0; i < rows; i++)
            {
                object[] row = new object[cols];
                for (int j = 0; j < cols; j++)
                {
                    row[j] = matrix[i, j];
                }
                table.Rows.Add(row);
            }

            return table;
        }

        // Отображение шагов
        private void DisplaySteps()
        {
            StepsList.Items.Clear();
            for (int i = 0; i < asap.Steps.Count; i++)
            {
                string stepDetails = $"Шаг {i + 1}: {string.Join(", ", asap.Steps[i])}";
                StepsList.Items.Add(stepDetails);
            }
        }

        // Отображение процессоров
        private void DisplayProcessors()
        {
            ProcessorsList.Items.Clear();
            for (int i = 0; i < asap.ProcessorCounts.Count; i++)
            {
                string processorDetails = $"Тип {i + 1}: {asap.ProcessorCounts[i]} процессоров";
                ProcessorsList.Items.Add(processorDetails);
            }
        }
    }
}
