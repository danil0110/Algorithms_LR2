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
            g.GreedyColoring();
            for (int i = 0; i < 10000; i++)
            {
                g.BeeColoringABC(i);
            }
        }
    }

    class Graph
    {
        private const int vertices = 150;
        private int[,] graph; // Матрица смежности
        private int[] degree; // Степени вершин графа
        private int recordChromeNumber; // Рекордное хроматическое число
        private int recordIteration; // Номер рекордной итерации

        private string[] AllColors = // Палитра цветов
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

        // Генерация графа
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

        // Жадная раскраска графа
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

        // Раскраска пчелиным алгоритмом ABC
        public void BeeColoringABC(int iteration)
        {
            Random rng = new Random();
            int[] scouts = new int[3]; // Выбранные вершины для каждого разведчика
            int[] workers = new int[3]; // Количество фуражиров на каждую вершину

            List<int> notVisitedVertices = InitVerticesArray(); // Список всех вершин
            List<string> usedColors = new List<string>(); // Список использованных цветов
            bool[] coloredVertices = new bool[vertices]; // Массив вершин (закрашена || незакрашена)
            string[] verticesColors = new string[vertices]; // Цвет каждой закрашеной вершины
                
            while (!IsAllVerticesColored(coloredVertices))
            {
                bool[] chosen = new bool[vertices]; // Массив для пометки выбранных вершин
                int degreeSum = 0; // Сумма степеней выбранных вершин
                
                // Выбор разведчиками случайных непосещенных вершин 
                for (int i = 0; i < 3; i++)
                {
                    scouts[i] = notVisitedVertices[rng.Next(0, notVisitedVertices.Count)];
                    chosen[scouts[i]] = true;
                    notVisitedVertices.Remove(scouts[i]);
                    degreeSum += degree[scouts[i]];
                }

                // Распределение фуражиров на каждую вершину
                workers[0] = 22 * degree[scouts[0]] / degreeSum;
                workers[1] = 22 * degree[scouts[1]] / degreeSum;
                workers[2] = 22 - workers[0] - workers[1];

                for (int i = 0; i < 3; i++)
                {
                    // Удаление цвета с выбранной вершины
                    string tempColor = verticesColors[scouts[i]];
                    verticesColors[scouts[i]] = "";
                    // Если никакая другая вершина не закрашена в удаленный цвет - удалить
                    // из списка использованных цветов
                    if (!IsAnyVertexColored(tempColor, verticesColors))
                    {
                        usedColors.Remove(tempColor);
                    }
                    
                    for (int j = 0; j < vertices; j++)
                    {
                        if (workers[i] == 1) // Выделение одного фуражира на покраску выбранной вершины
                        {
                            break;
                        }
                        
                        // Если вершина смежная и не выбрана другим разведчиком - красим
                        if (graph[scouts[i], j] == 1 && !chosen[j])
                        {
                            bool coloringCompleted = false;
                            foreach (var el in usedColors)
                            {
                                // Поиск допустимого цвета среди использованных
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

                            // Выбор нового случайного цвета из палитры цветов
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
                    
                    // Раскраска выбранной вершины
                    // Поиск допустимого цвета среди использованных
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

                    // Выбор нового случайного цвета из палитры цветов
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

            // Сравнение текущего результата с наилучшим
            if (usedColors.Count < recordChromeNumber || recordChromeNumber == 0)
            {
                recordChromeNumber = usedColors.Count;
                recordIteration = iteration + 1;
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

        // Инициализация списка всех вершин
        private List<int> InitVerticesArray()
        {
            List<int> array = new List<int>();
            for (int i = 0; i < vertices; i++)
            {
                array.Add(i);
            }
            
            return array;
        }

        // Доступен ли данный цвет для покраски данной вершины
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

        // Проверка все ли вершины закрашены
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

        // Имеет ли данный цвет какая-либо вершина
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
        
        // Вывод матрицы смежности графа
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