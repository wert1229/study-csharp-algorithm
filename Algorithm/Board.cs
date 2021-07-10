using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Board
    {
        private const char CIRCLE = '\u25cf';
        public TileType[,] Tiles { get; private set; }
        public int Size { get; private set; }
        public int DestX { get; private set; }
        public int DestY { get; private set; }

        private Player _player;
        private Random _rand;

        public enum TileType
        {
            Empty,
            Wall
        }

        public void Init(int size, Player player)
        {
            Tiles = new TileType[size, size];
            Size = size;
            _player = player;
            _rand = new Random();
            DestX = Size - 2;
            DestY = Size - 2;

            GenerateBySideWind();
        }
        
        private void GenerateByBinaryTree()
        {
            FillWithWall();

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        continue;
                    }
                        
                    if (x == Size - 2 && y == Size - 2)
                    {
                        continue;
                    }

                    if (x == Size - 2)
                    {
                        Tiles[y + 1, x] = TileType.Empty; 
                        continue;
                    }

                    if (y == Size - 2)
                    {
                        Tiles[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (_rand.Next(0, 2) == 0)
                    {
                        Tiles[y, x + 1] = TileType.Empty;
                    }
                    else
                    {
                        Tiles[y + 1, x] = TileType.Empty;
                    }
                }
            }
        }

        private void GenerateBySideWind()
        {
            FillWithWall();

            for (int y = 0; y < Size; y++)
            {
                int rightCount = 1;

                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        continue;
                    }
                        
                    if (x == Size - 2 && y == Size - 2)
                    {
                        continue;
                    }

                    if (x == Size - 2)
                    {
                        Tiles[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    if (y == Size - 2)
                    {
                        Tiles[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (_rand.Next(0, 2) == 0)
                    {
                        Tiles[y, x + 1] = TileType.Empty;
                        rightCount++;
                    }
                    else
                    {
                        int _randomIndex = _rand.Next(0, rightCount);
                        Tiles[y + 1, x - _randomIndex * 2] = TileType.Empty;
                        rightCount = 1;
                    }
                }
            }
        }

        private void FillWithWall()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                    {
                        Tiles[y, x] = TileType.Wall;
                    }
                    else
                    {
                        Tiles[y, x] = TileType.Empty;
                    }
                }
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x == _player.PosX && y == _player.PosY)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (x == DestX && y == DestY)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                    }
                    else
                    {
                        Console.ForegroundColor = GetTileColor(Tiles[y, x]);
                    }

                    Console.Write(CIRCLE);
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;
        }

        private ConsoleColor GetTileColor(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Green;
            }
        }
    }
}
