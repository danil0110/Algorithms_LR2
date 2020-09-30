using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Algorithms_LR2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Graph g = new Graph();
            Console.WriteLine();
            g.GreedyColoring();
            g.BeeColoringABC();
        }
    }

    class Graph
    {
        private const int vertices = 150;
        private int[,] graph;
        private int[] degree;
        private int recordChromeNumber;
        private int recordIteration;

        private string[] AllColors =
        {
            "Красный", "Зеленый", "Голубой", "Желтый", "Фиолетовый",
            "Оранжевый", "Лаймовый", "Синий", "Черный", "Белый",
            "Коричневый", "Небесный", "Абрикосовый", "Аметистовый", "Алый",
            "Бежевый", "Пурпурный", "Болотный", "Бронзовый", "Серебряный",
            "Индиго", "Маджента", "Люминесцентный", "Медовый", "Фуксия",
            "Мятный", "Ниагара", "Нефритовый", "Оливковый", "Охра"
        };

        public Graph()
        {
            graph = new int[vertices, vertices];
            degree = new int[vertices];
            GraphGenerate();
        }

        private void GraphGenerate()
        {
            Random rng = new Random();
            for (int i = 0; i < vertices; i++)
            {
                int times = vertices;
                int[] visited = new int[vertices];
                visited[i] = 1;

                int DEG = rng.Next(1, 31);
                while (degree[i] < DEG && times > 0)
                {
                    int val = rng.Next(vertices);
                    int deg = rng.Next(30);
                    if (i != val && degree[val] < deg)
                    {
                        if (visited[val] == 0)
                        {
                            degree[val]++;
                            degree[i]++;
                            graph[i, val] = 1;
                            graph[val, i] = 1;
                            visited[val] = 1;
                        }
                    }

                    times--;
                }
            }
        }

        public void GreedyColoring()
        {
            List<string> usedColors = new List<string>();
            string[] verticesColors = new string[vertices];
            for (int i = 0; i < vertices; i++)
            {
                for (int j = 0; j < AllColors.Length; j++)
                {
                    if (IsAvailableColor(i, AllColors[j], verticesColors))
                    {
                        verticesColors[i] = AllColors[j];
                        if (!usedColors.Contains(AllColors[j]))
                        {
                            usedColors.Add(AllColors[j]);
                        }

                        break;
                    }
                }
            }

            recordIteration = 1;
            recordChromeNumber = usedColors.Count;

            Console.WriteLine("Раскраска жадным алгоритмом");
            Console.WriteLine($"Хроматическое число - {recordChromeNumber}");
            for (int i = 0; i < vertices; i++)
            {
                Console.WriteLine($"{i + 1} - {verticesColors[i]}");
            }
            Console.WriteLine();
        }

        public void BeeColoringABC()
        {
            Random rng = new Random();
            int[] scouts = new int[3];
            int[] workers = new int[3];

            for (int k = 0; k < 1000; k++)
            {
                List<int> notVisitedVertices = InitVerticesArray();
                List<string> usedColors = new List<string>();
                bool[] coloredVertices = new bool[vertices];
                string[] verticesColors = new string[vertices];
                
                while (!IsAllVerticesColored(coloredVertices))
                {
                    bool[] chosen = new bool[vertices];
                    int degreeSum = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        scouts[i] = notVisitedVertices[rng.Next(0, notVisitedVertices.Count)];
                        chosen[scouts[i]] = true;
                        notVisitedVertices.Remove(scouts[i]);
                        degreeSum += degree[scouts[i]];
                    }

                    workers[0] = 22 * degree[scouts[0]] / degreeSum;
                    workers[1] = 22 * degree[scouts[1]] / degreeSum;
                    workers[2] = 22 - workers[0] - workers[1];

                    for (int i = 0; i < 3; i++)
                    {
                        string tempColor = verticesColors[scouts[i]];
                        verticesColors[scouts[i]] = "";
                        if (!IsAnyVertexColored(tempColor, verticesColors))
                        {
                            usedColors.Remove(tempColor);
                        }
                    
                        for (int j = 0; j < vertices; j++)
                        {
                            if (workers[i] == 1)
                            {
                                break;
                            }
                        
                            if (graph[scouts[i], j] == 1 && !chosen[j])
                            {
                                bool coloringCompleted = false;
                                foreach (var el in usedColors)
                                {
                                    if (IsAvailableColor(j, el, verticesColors))
                                    {
                                        tempColor = verticesColors[j];
                                        verticesColors[j] = el;
                                        if (!IsAnyVertexColored(tempColor, verticesColors))
                                        {
                                            usedColors.Remove(tempColor);
                                        }

                                        coloredVertices[j] = true;
                                        coloringCompleted = true;
                                        workers[i]--;
                                        break;
                                    }
                                }

                                if (!coloringCompleted)
                                {
                                    string color = AllColors[rng.Next(0, 30)];
                                    while (usedColors.Contains(color))
                                    {
                                        color = AllColors[rng.Next(0, 30)];
                                    }

                                    verticesColors[j] = color;
                                    coloredVertices[j] = true;
                                    usedColors.Add(color);
                                    workers[i]--;
                                }
                                
                            }
                        }
                        
                        foreach (var el in usedColors)
                        {
                            if (IsAvailableColor(scouts[i], el, verticesColors))
                            {
                                verticesColors[scouts[i]] = el;
                                coloredVertices[scouts[i]] = true;
                                workers[i]--;
                                break;
                            }
                        }

                        if (verticesColors[scouts[i]] == "")
                        {
                            string newColor = AllColors[rng.Next(0, 30)];
                            while (usedColors.Contains(newColor))
                            {
                                newColor = AllColors[rng.Next(0, 30)];
                            }

                            verticesColors[scouts[i]] = newColor;
                            coloredVertices[scouts[i]] = true;
                            usedColors.Add(newColor);
                            workers[i]--;
                        }
                    
                    }

                }

                if (usedColors.Count < recordChromeNumber || recordChromeNumber == 0)
                {
                    recordChromeNumber = usedColors.Count;
                    recordIteration = k + 1;
                    Console.WriteLine("--- РЕКОРД ---");
                    Console.WriteLine($"Хроматическое число - {recordChromeNumber}");
                    Console.WriteLine($"Номер итерации - {recordIteration}");
                    Console.WriteLine("Рекордная раскраска:");
                    for (int i = 0; i < vertices; i++)
                    {
                        Console.WriteLine($"{i + 1} - {verticesColors[i]}");
                    }
                    Console.WriteLine();
                }
                
            }
            
        }

        private List<int> InitVerticesArray()
        {
            List<int> array = new List<int>();
            for (int i = 0; i < vertices; i++)
            {
                array.Add(i);
            }
            
            return array;
        }

        private bool IsAvailableColor(int vertex, string color, string[] verticesColors)
        {
            for (int i = 0; i < vertices; i++)
            {
                if (graph[vertex, i] == 1)
                {
                    if (color == verticesColors[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsAllVerticesColored(bool[] array)
        {
            foreach (var el in array)
            {
                if (!el)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsAnyVertexColored(string color, string[] verticesColors)
        {
            foreach (var el in verticesColors)
            {
                if (color == el)
                {
                    return true;
                }
            }

            return false;
        }
        
        public void GraphOutput()
        {
            for (int i = 0; i < vertices; i++)
            {
                for (int j = 0; j < vertices; j++)
                {
                    Console.Write($"{graph[i, j], 3}");
                }

                Console.WriteLine();
            }
        }

    }
    
}