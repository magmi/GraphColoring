using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;

namespace GraphColoring
{
    class Computer : Player
    {
        public bool easyMode;

        public double elapsed;
        public bool startedMove;
        public bool flowerPicked;
        public bool flowerColored;
        private int chosenFlowerIndex;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWndle, String text, String caption, int buttons);

        public Computer(bool _easyMode)
        {
            this.easyMode = _easyMode;
            this.startedMove = false;
            this.flowerPicked = false;
            this.flowerColored = false;
            this.chosenFlowerIndex = 0;
            this.elapsed = 0;
        }

        public void CalculateMove(Game game)
        {
            Random r = new Random();
            int n = game.graph.flowers.Count;
            int m = game.colors.Length;

            if (!startedMove)
            {
                MessageBox(new IntPtr(), "Computer's turn", "Next turn", 0);
                startedMove = true;
            }

            if(easyMode==true)
            {
                int colorIndex = 0;

                if (elapsed > 1 && !flowerPicked)
                {
                    do
                    {
                        chosenFlowerIndex = r.Next(0, n);
                    }
                    while (game.graph.flowers[chosenFlowerIndex].color != Color.White);

                    game.graph.flowers[chosenFlowerIndex].color = Color.LightBlue;
                    game.lastClicked = game.graph.flowers[chosenFlowerIndex];

                    flowerPicked = true;
                }

                if (elapsed > 2 && !flowerColored)
                {
                    do
                    {
                        colorIndex = r.Next(0, m);
                    }
                    while (!game.CheckIfValidMove(game.graph.flowers[chosenFlowerIndex], game.colors[colorIndex]));

                    game.graph.MakeMove(game.lastClicked, game.colors[colorIndex]);
                    game.lastClicked = null;
                    flowerColored = true;
                }

                if (flowerColored)
                {
                    game.whoseTurn = 0;
                    elapsed = 0;
                    startedMove = false;
                    flowerPicked = false;
                    flowerColored = false;
                    game.gardenerStartedMove = false;
                }
            }
        }
    }
}
