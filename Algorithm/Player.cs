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

            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };

            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            addNowPos();
            
            while (PosY != _board.DestX || PosY != _board.DestY)
            {
                if (_board.Tiles[PosY + rightY[_dir], PosX + rightX[_dir]] == Board.TileType.Empty)
                {
                    _dir = (_dir - 1 + 4) % 4;
                    PosY += frontY[_dir];
                    PosX += frontX[_dir];
                    addNowPos();
                }
                else if (_board.Tiles[PosY + frontY[_dir], PosX + frontX[_dir]] == Board.TileType.Empty)
                {
                    PosY += frontY[_dir];
                    PosX += frontX[_dir];
                    addNowPos();
                }
                else
                {
                    _dir = (_dir + 1 + 4) % 4;
                }
            }
        }
        
        private void addNowPos()
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
