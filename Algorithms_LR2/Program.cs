using System;
using System.Collections.Generic;

namespace Algorithms_LR2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Graph g = new Graph();
            g.GraphOutput();
            Console.WriteLine();
            g.GreedyColoring();
        }
    }

    class Graph
    {
        private const int vertices = 6;
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
            Random rnd = new Random();
            for (int i = 0; i < vertices; i++)
            {
                int times = vertices;
                int[] visited = new int[vertices];
                visited[i] = 1;

                int DEG = rnd.Next(1, 31);
                while (degree[i] < DEG && times > 0)
                {
                    int val = rnd.Next(vertices);
                    int deg = rnd.Next(30);
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
                    if (isAvailableColor(i, AllColors[j], verticesColors))
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

            Console.WriteLine($"Хроматическое число - {recordChromeNumber}");
            for (int i = 0; i < vertices; i++)
            {
                Console.WriteLine($"{i + 1} - {verticesColors[i]}");
            }
            
        }

        private bool isAvailableColor(int vertex, string color, string[] verticesColors)
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