using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ASAP_Scheduler
{
    public class ASAPAlgorithm
    {
        public List<List<int>> Steps { get; private set; }
        public List<int> ProcessorCounts { get; private set; }

        private int[,] adjacencyMatrix;
        private int operationsCount;

        public static Dictionary<int, List<int>> graphDictionary = new Dictionary<int, List<int>>();

        public void LoadData(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            // Чтение первых двух строк для словаря
            for (int i = 0; i < 2; i++)
            {
                var parts = lines[i].Split(':'); // Разделяем по символу ":"
                int key = int.Parse(parts[0].Trim());
                var values = parts[1].Trim().Split(' '); // Разделяем на числа

                List<int> neighbors = new List<int>();
                foreach (var value in values)
                {
                    neighbors.Add(int.Parse(value));
                }
                graphDictionary[key] = neighbors; // Добавляем в словарь
            }

            // Чтение матрицы смежности и количества операций (следующие строки после словаря)
            operationsCount = int.Parse(lines[2].Trim()); // Получаем количество операций (размер матрицы)
            adjacencyMatrix = new int[operationsCount, operationsCount];

            // Начинаем с 3-й строки, т.к. первые 3 строки уже обработаны
            for (int i = 0; i < operationsCount; i++)
            {
                var row = lines[i + 3].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(int.Parse)
                                      .ToArray();
                for (int j = 0; j < operationsCount; j++)
                {
                    adjacencyMatrix[i, j] = row[j];
                }
            }
        }



        public void ExecuteASAP()
        {
            Steps = new List<List<int>>();
            ProcessorCounts = new List<int>();

            HashSet<int> scheduled = new HashSet<int>();
            int step = 0;

            // Алгоритм ASAP
            while (scheduled.Count < operationsCount)
            {
                List<int> currentStep = new List<int>();

                for (int i = 0; i < operationsCount; i++)
                {
                    if (!scheduled.Contains(i) && CanBeScheduled(i, scheduled))
                    {
                        currentStep.Add(i);
                    }
                }

                if (currentStep.Count == 0) break;

                Steps.Add(currentStep);
                ProcessorCounts.Add(currentStep.Count);
                scheduled.UnionWith(currentStep);
                step++;
            }
        }

        private bool CanBeScheduled(int operation, HashSet<int> scheduled)
        {
            for (int i = 0; i < operationsCount; i++)
            {
                if (adjacencyMatrix[i, operation] == 1 && !scheduled.Contains(i))
                    return false;
            }
            return true;
        }

        public int[,] GetAdjacencyMatrix() => adjacencyMatrix;
    }
}
