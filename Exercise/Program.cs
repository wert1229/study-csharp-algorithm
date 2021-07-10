using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    class Graph
    {
        int[,] _adj = new int[6, 6]
        {
            { 0, 1, 0, 1, 0, 0 },
            { 1, 0, 1, 1, 0, 0 },
            { 0, 1, 0, 0, 0, 0 },
            { 1, 1, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1 },
            { 0, 0, 0, 0, 1, 0 }
        };

        List<int>[] _adj2 = new List<int>[6]
        {
            new List<int>() { 1, 3},
            new List<int>() { 0, 2, 3},
            new List<int>() { 1 },
            new List<int>() { 0, 1},
            new List<int>() { 5},
            new List<int>() { 4 }
        };


        bool[] visited = new bool[6];

        public void DFS(int now)
        {
            Console.WriteLine(now);
            
            visited[now] = true;

            for (int i = 0; i < _adj.GetLength(1); i++)
            {
                if (_adj[now, i] == 1 && !visited[i])
                {
                    DFS(i);
                }
            }
        }

        public void DFS2(int now)
        {
            Console.WriteLine(now);

            visited[now] = true;

            foreach (int i in _adj2[now])
            {
                if (!visited[i])
                {
                    DFS2(i);
                }
            }
        }

        public void SearchAll()
        {
            for (int i = 0; i < 6; i++)
            {
                if (!visited[i])
                {
                    DFS(i);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();
            graph.SearchAll();
        }
    }
}
