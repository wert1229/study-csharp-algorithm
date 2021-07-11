using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Pos
    {
        public int Y;
        public int X;
        public Pos(int y, int x) 
        { 
            Y = y; 
            X = x; 
        }
    }

    class Player
    {
        const int MOVE_TICK = 10;
        private int _sumTick = 0;
        public int PosX { get; private set; }
        public int PosY { get; private set; }
        private Board _board;
        private Random _random = new Random();
        
        enum Dir
        {
            UP = 0,
            LEFT = 1,
            DOWN = 2,
            RIGHT = 3
        }

        int _dir = (int)Dir.UP;
        List<Pos> _points = new List<Pos>();
        int _lastIndex = 0;

        public void Init(int posX, int posY, Board board)
        {
            PosX = posX;
            PosY = posY;
            _board = board;

            AStar();
        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                {
                    return 0;
                }
                return F < other.F ? 1 : -1;
            }
        }

        void AStar()
        {
            int[] deltaY = { -1, 0, 1, 0 };
            int[] deltaX = { 0, -1, 0, 1 };

            bool[,] closed = new bool[_board.Size, _board.Size];
            int[,] open = new int[_board.Size, _board.Size];
            for (int i = 0; i < _board.Size; i++)
            {
                for (int j = 0; j < _board.Size; j++)
                {
                    open[i, j] = Int32.MaxValue;
                }
            }

            Pos[,] parent = new Pos[_board.Size, _board.Size];

            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            open[PosY, PosX] = Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX);
            pq.Push(new PQNode() { F = Math.Abs(_board.DestY - PosY) + Math.Abs(_board.DestX - PosX), G = 0, Y = PosY, X = PosX });
            parent[PosY, PosX] = new Pos(PosY, PosX);

            while (pq.Count > 0)
            {
                PQNode node = pq.Pop();

                if (closed[node.Y, node.X])
                {
                    continue;
                }

                closed[node.Y, node.X] = true;
                if (node.Y == _board.DestY && node.X == _board.DestX)
                {
                    break;
                }

                for (int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];

                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                    {
                        continue;
                    }
                    if (_board.Tiles[nextY, nextX] == Board.TileType.Wall)
                    {
                        continue;
                    }
                    if (closed[nextY, nextX])
                    {
                        continue;
                    }

                    int g = node.G + 1;
                    int h = Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX);

                    if (open[nextY, nextX] < g + h)
                    {
                        continue;
                    }

                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);
                }
            }

            CalcPathFromParent(parent);
        }

        private void BFS()
        {
            Queue<Pos> queue = new Queue<Pos>();
            bool[,] visited = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];
            int[,] distance = new int[_board.Size, _board.Size];

            for (int i = 0; i < _board.Size; i++)
            {
                for (int j = 0; j < _board.Size; j++)
                {
                    distance[i, j] = -1;
                }
            }

            int[] deltaY = { -1, 0, 1, 0 };
            int[] deltaX = { 0, 1, 0, -1 };


            queue.Enqueue(new Pos(1, 1));
            visited[1, 1] = true;
            parent[1, 1] = new Pos(1, 1);
            distance[1, 1] = 0;

            while (queue.Count != 0)
            {
                Pos pos = queue.Dequeue();

                for (int i = 0; i < 4; i++)
                {
                    int targetY = pos.Y + deltaY[i];
                    int targetX = pos.X + deltaX[i];

                    if (visited[targetY, targetX] == true)
                    {
                        continue;
                    }

                    if (targetY < 0 || targetY >= _board.Size || targetX < 0 || targetX >= _board.Size)
                    {
                        continue;
                    }

                    if (_board.Tiles[targetY, targetX] == Board.TileType.Wall)
                    {
                        continue;
                    }

                    queue.Enqueue(new Pos(targetY, targetX));
                    visited[targetY, targetX] = true;

                    if (distance[targetY, targetX] == -1 || distance[targetY, targetX] > distance[pos.Y, pos.X] + 1)
                    {
                        parent[targetY, targetX] = pos;
                        distance[targetY, targetX] = distance[pos.Y, pos.X] + 1;
                    }
                }
            }

            CalcPathFromParent(parent);
        }

        void CalcPathFromParent(Pos[,] parent)
        {
            Pos childPos = new Pos(_board.DestY, _board.DestX);
            _points.Add(childPos);

            while (childPos.Y != 1 || childPos.X != 1)
            {
                childPos = parent[childPos.Y, childPos.X];
                _points.Add(childPos);
            }

            _points.Reverse();
        }

        private void RightHand()
        {
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };

            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            AddNowPos();

            while (PosY != _board.DestX || PosY != _board.DestY)
            {
                if (_board.Tiles[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    _dir = (_dir - 1 + 4) % 4;
                    PosY += frontY[_dir];
                    PosX += frontX[_dir];
                    AddNowPos();
                }
                else if (_board.Tiles[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty)
                {
                    PosY += frontY[_dir];
                    PosX += frontX[_dir];
                    AddNowPos();
                }
                else
                {
                    _dir = (_dir + 1 + 4) % 4;
                }
            }
        }
        
        private void AddNowPos()
        {
            _points.Add(new Pos(PosY, PosX));
        }

        public void Update(int deltaTick)
        {
            _sumTick += deltaTick;
            if (_sumTick < MOVE_TICK)
            {
                return;
            }
            _sumTick = 0;

            if (_lastIndex >= _points.Count)
            {
                return;
            }

            Pos pos = _points[_lastIndex++];

            PosY = pos.Y;
            PosX = pos.X;
        }
    }
}
