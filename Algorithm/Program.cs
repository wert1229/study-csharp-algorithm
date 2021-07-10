using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Player player = new Player();
            board.Init(25, player);
            player.Init(1, 1, board);

            Console.CursorVisible = false;

            const int WAIT_TICK = 1000 / 50;

            int lastTick = 0;

            while (true)
            {
                int currentTick = System.Environment.TickCount;
                int deltaTick = currentTick - lastTick;
                if (deltaTick < WAIT_TICK)
                {
                    continue;
                }
                lastTick = currentTick;

                player.Update(deltaTick);

                Console.SetCursorPosition(0, 0);
                board.Render();
            }
        }
    }
}
