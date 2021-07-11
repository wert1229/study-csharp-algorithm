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
            new List<int>() { 1, 3 },
            new List<int>() { 0, 2, 3 },
            new List<int>() { 1 },
            new List<int>() { 0, 1, 4 },
            new List<int>() { 3, 5 },
            new List<int>() { 4 }
        };


        bool[] visited = new bool[6];

        public void Dijikstra(int start)
        {
            bool[] visited = new bool[6];
            int[] distance = Enumerable.Repeat(Int32.MaxValue, 6).ToArray();
            int[] parent = new int[6];

            distance[start] = 0;
            parent[start] = start;

            while (true)
            {
                int closest = Int32.MaxValue;
                int now = -1;

                for (int i = 0; i < 6; i++)
                {
                    if (visited[i])
                    {
                        continue;
                    }
                    if (distance[i] == Int32.MaxValue || distance[i] >= closest)
                    {
                        continue;
                    }

                    closest = distance[i];
                    now = i;
                }

                if (now == -1)
                {
                    break;
                }

                visited[now] = true;

                for (int next = 0; next < 6; next++)
                {
                    if (_adj[now, next] == -1)
                    {
                        continue;
                    }
                    if (visited[next] == true)
                    {
                        continue;
                    }

                    int nextDist = distance[now] + _adj[now, next];

                    if (nextDist < distance[next])
                    {
                        distance[next] = nextDist;
                        parent[next] = now;
                    }
                }

            }
        }

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

        public void BFS(int now)
        {
            Queue<int> queue = new Queue<int>();

            queue.Enqueue(now);
            visited[now] = true;

            while (queue.Count != 0)
            {
                int value = queue.Dequeue();
                Console.WriteLine(value);

                foreach (int i in _adj2[value])
                {
                    if (visited[i] == true)
                    {
                        continue;
                    }
                    queue.Enqueue(i);
                    visited[i] = true;
                }
            }
        }
    }
}
