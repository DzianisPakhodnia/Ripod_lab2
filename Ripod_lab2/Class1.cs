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

        public void LoadData(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            operationsCount = int.Parse(lines[0].Trim()); // Убираем возможные лишние пробелы
            adjacencyMatrix = new int[operationsCount, operationsCount];

            for (int i = 0; i < operationsCount; i++) // Начинаем с 0, т.к. i соответствует строке в матрице
            {
                var row = lines[i + 1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
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
