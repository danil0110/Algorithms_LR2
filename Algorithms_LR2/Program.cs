using System;

namespace Algorithms_LR2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Graph g = new Graph();
            g.GraphOutput();
        }
    }

    class Graph
    {
        private const int vertices = 20;
        private int[,] graph;
        private int[] degree;

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