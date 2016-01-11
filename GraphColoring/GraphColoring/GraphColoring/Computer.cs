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
        private bool[] activatedObjects;
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

        public int FindSmallestIndex(bool flower, int n, GardenGraph g)
        {
            if(flower)
            {
                for(int i =0;i<n;i++)
                {
                    if (g.flowers[i].color == Color.White)
                        return i;
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (g.fences[i].color == Color.White)
                        return i;
                }
            }
            return -1;
        }


        public void HardModeFindFence(Game game)
        {
            if (activatedObjects == null)
            {
                activatedObjects = new bool[game.graph.fencesNumber];
            }
            //Pierwszy ruch
            if (game.lastClickedIndex == -1)
            {
                chosenFlowerIndex = 0;
                activatedObjects[0] = true;
                game.graph.fences[chosenFlowerIndex].color = Color.LightBlue;
                game.lastClicked = game.graph.fences[chosenFlowerIndex];
                flowerPicked = true;
            }
            //Ktorys ruch
            else
            {
                int index = game.lastClickedIndex;
                activatedObjects[index] = true;
                int i = FindSmallestIndex(false, index, game.graph);
                if (i == -1)
                {
                    i = FindSmallestIndex(false, game.graph.fencesNumber, game.graph);
                    if (i > -1)
                    {
                        activatedObjects[i] = true;
                        chosenFlowerIndex = i;
                        game.graph.fences[chosenFlowerIndex].color = Color.LightBlue;
                        game.lastClicked = game.graph.fences[chosenFlowerIndex];
                        flowerPicked = true;
                    }
                    return;
                }
                else
                {
                    game.lastClickedIndex = i;
                    HardModeGardenerForFences(game);
                }

            }
        }
        

        public void HardModeFindFlower(Game game)
        {
            if (activatedObjects == null)
            {
                activatedObjects = new bool[game.graph.flowersNumber];
            }
            //Pierwszy ruch
            if (game.lastClickedIndex == -1)
            {
                chosenFlowerIndex = 0;
                activatedObjects[0] = true;                
                game.graph.flowers[chosenFlowerIndex].color = Color.LightBlue;
                game.lastClicked = game.graph.flowers[chosenFlowerIndex];
                flowerPicked = true;
            }
            //Ktorys ruch
            else
            {
                int index = game.lastClickedIndex;
                activatedObjects[index] = true;
                int i = FindSmallestIndex(true, index, game.graph);
                if (i == -1)
                {
                    i = FindSmallestIndex(true, game.graph.flowersNumber, game.graph);
                    if (i > -1)
                    {
                        activatedObjects[i] = true;
                        chosenFlowerIndex = i;
                        game.graph.flowers[chosenFlowerIndex].color = Color.LightBlue;
                        game.lastClicked = game.graph.flowers[chosenFlowerIndex];
                        flowerPicked = true;

                            
                        

                    }
                    return;
                }
                else
                {
                    game.lastClickedIndex = i;
                    HardModeGardenerForFlowers(game);
                }

            }
        }

        public void HardModeGardenerForFlowers(Game game)
        {
            Random r = new Random();
            int m = game.colors.Length;
            int colorIndex = 0;
            if (elapsed > 1 && !flowerPicked)
            {
                HardModeFindFlower(game);              
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

        public void HardModeGardenerForFences(Game game)
        {
            Random r = new Random();
            int m = game.colors.Length;
            int colorIndex = 0;
            if (elapsed > 1 && !flowerPicked)
            {
                HardModeFindFence(game);
            }

            if (elapsed > 2 && !flowerColored)
            {
                do
                {
                    colorIndex = r.Next(0, m);
                }
                while (!game.CheckIfValidMove(game.graph.fences[chosenFlowerIndex], game.colors[colorIndex]));

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
        public void EasyModeForFlowers(Game game)
        {
            Random r = new Random();
            int n = game.graph.flowers.Count;
            int m = game.colors.Length;
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

        public void EasyModeForFences(Game game)
        {
            Random r = new Random();
            int n = game.graph.fences.Count;
            int m = game.colors.Length;
            int colorIndex = 0;

            if (elapsed > 1 && !flowerPicked)
            {
                do
                {
                    chosenFlowerIndex = r.Next(0, n);
                }
                while (game.graph.fences[chosenFlowerIndex].color != Color.White);

                game.graph.fences[chosenFlowerIndex].color = Color.LightBlue;
                game.lastClicked = game.graph.fences[chosenFlowerIndex];

                flowerPicked = true;
            }

            if (elapsed > 2 && !flowerColored)
            {
                do
                {
                    colorIndex = r.Next(0, m);
                }
                while (!game.CheckIfValidMove(game.graph.fences[chosenFlowerIndex], game.colors[colorIndex]));

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
        public void CalculateMove(Game game)
        {
            game.ChangeTurn(base.isGardener);
            if (!startedMove)
            {
                startedMove = true;
            }
            if (game.gameType == GameType.VerticesColoring)
            {
                if (easyMode == true)
                {                    
                    EasyModeForFlowers(game);
                }
                else
                {
                    //sasiad
                    HardModeGardenerForFlowers(game);
                }
            }
            else
            {
                if (easyMode == true)
                {
                    EasyModeForFences(game);
                }
                else
                {
                    //sasiad
                    HardModeGardenerForFences(game);
                }
            }
        }
    }
}
