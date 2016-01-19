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
        public Color chosenColor;
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

        /// <summary>
        /// Funckcja komputera obliczajaca nastepny ruch
        /// </summary>
        /// <param name="game"></param>
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
                    if (this.isGardener)
                        HardModeGardenerForFlowers(game);
                    else
                        HardModeNeighbourForFlowers(game);
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
                    if (this.isGardener)
                        HardModeGardenerForFences(game);
                    else
                        HardModeNeighbourForFences(game);
                }
            }
        }

        /// <summary>
        /// Funckcja ruchu dla poziomu trudnego dla kolorwania kwiatkow dla sasiada
        /// </summary>
        /// <param name="game"></param>
        public void HardModeNeighbourForFlowers(Game game)
        {
            if (elapsed > 1 && !flowerPicked)
            {
                foreach (Flower f in game.graph.flowers)
                {
                    if (f.color != Color.White)
                    {
                        foreach (Fence fn in game.graph.GetOutFences(f))
                        {
                            Flower f1 = fn.f1;
                            if (f1.Equals(f))
                                f1 = fn.f2;

                            foreach (Fence fn2 in game.graph.GetOutFences(f1))
                            {
                                Flower f2 = fn2.f1;
                                if (f2.Equals(f1))
                                    f2 = fn2.f2;

                                if (f2.color == Color.White && !AreFlowersConnected(f2, f, game.graph))
                                {
                                    Color c = PickUnusedColor(game, f2, f);
                                    chosenFlowerIndex = f2.index;
                                    chosenColor = c;
                                    game.graph.flowers[chosenFlowerIndex].color = Color.LightBlue;
                                    game.lastClicked = game.graph.flowers[chosenFlowerIndex];

                                    flowerPicked = true;
                                    return;
                                }

                            }
                        }
                    }
                }

                EasyFlowerPick(game);
                chosenColor = PickUnusedColor(game, game.lastClicked);
            }
            if (elapsed > 2 && !flowerColored)
            {
                game.graph.MakeMove(game.lastClicked, chosenColor, game);
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
        
        /// <summary>
        /// Funckcja ruchu dla poziomu trudnego dla kolorwania plotkow dla sasiada
        /// </summary>
        /// <param name="game"></param>
        public void HardModeNeighbourForFences(Game game)
        {
            if (elapsed > 1 && !flowerPicked)
            {
                foreach (Fence fn in game.graph.fences)
                {
                    if (fn.color != Color.White)
                    {
                        foreach (Fence fn1 in game.graph.GetOutFences(fn.f1))
                        {
                            Flower f1 = fn1.f1;
                            if (f1.Equals(fn.f1))
                                f1 = fn1.f2;

                            foreach (Fence fn2 in game.graph.GetOutFences(f1))
                            {
                                Flower f2 = fn2.f1;
                                if (f2.Equals(f1))
                                    f2 = fn2.f2;

                                if (fn2.color == Color.White && !AreFencesConnected(fn, fn2))
                                {
                                    Color c = PickUnusedColor(game, fn2, fn);
                                    chosenColor = c;
                                    fn2.color = Color.LightBlue;
                                    game.lastClicked = fn2;

                                    flowerPicked = true;
                                    return;
                                }

                            }
                        }

                        foreach (Fence fn1 in game.graph.GetOutFences(fn.f2))
                        {
                            Flower f1 = fn1.f1;
                            if (f1.Equals(fn.f2))
                                f1 = fn1.f2;

                            foreach (Fence fn2 in game.graph.GetOutFences(f1))
                            {
                                Flower f2 = fn2.f1;
                                if (f2.Equals(f1))
                                    f2 = fn2.f2;

                                if (fn2.color == Color.White && !AreFencesConnected(fn, fn2))
                                {
                                    Color c = PickUnusedColor(game, fn2, fn);
                                    chosenColor = c;
                                    fn2.color = Color.LightBlue;
                                    game.lastClicked = fn2;

                                    flowerPicked = true;
                                    return;
                                }

                            }
                        }
                    }
                }

                EasyFlowerPick(game);
                chosenColor = PickUnusedColor(game, game.lastClicked);
            }
            if (elapsed > 2 && !flowerColored)
            {
                game.graph.MakeMove(game.lastClicked, chosenColor, game);
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

        /// <summary>
        /// Funckcja ruchu dla poziomu trudnego dla kolorwania kwiatkow dla ogrodnika
        /// </summary>
        /// <param name="game"></param>
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

                game.graph.MakeMove(game.lastClicked, game.colors[colorIndex], game);
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

        /// <summary>
        /// Funckcja ruchu dla poziomu trudnego dla kolorwania plotkow dla ogrodnika
        /// </summary>
        /// <param name="game"></param>
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

                game.graph.MakeMove(game.lastClicked, game.colors[colorIndex], game);
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

        /// <summary>
        /// Funckcja ruchu dla poziomu latwego dla kolorwania kwiatkow
        /// </summary>
        /// <param name="game"></param>
        public void EasyModeForFlowers(Game game)
        {
            Random r = new Random();
            int m = game.colors.Length;
            int colorIndex = 0;

            if (elapsed > 1 && !flowerPicked)
            {
                EasyFlowerPick(game);
            }

            if (elapsed > 2 && !flowerColored)
            {
                do
                {
                    colorIndex = r.Next(0, m);
                }
                while (!game.CheckIfValidMove(game.graph.flowers[chosenFlowerIndex], game.colors[colorIndex]));

                game.graph.MakeMove(game.lastClicked, game.colors[colorIndex], game);
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

        /// <summary>
        /// Funkcja ruchu dla poziomu latwego dla kolorwania plotkow
        /// </summary>
        /// <param name="game"></param>
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

                game.graph.MakeMove(game.lastClicked, game.colors[colorIndex], game);
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



        /// Ponizej fukncje pomocnicze dla powyzszych funkcji

        /// <summary>
        /// Pomocnicza funkcja szukajaca niepokolorowanego kwiatka o najmiejszym indeksie
        /// </summary>
        /// <param name="flower"></param>
        /// <param name="n"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public int FindSmallestIndex(bool flower, int n, GardenGraph g)
        {
            if (flower)
            {
                for (int i = 0; i < n; i++)
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

        private Color PickUnusedColor(Game game, ColorableObject f)
        {
            Random r = new Random();
            List<Color> unusedColors = new List<Color>();

            foreach (Color c in game.colors)
                if (!game.usedColors.Contains(c))
                    unusedColors.Add(c);

            ListExtension.Shuffle<Color>(unusedColors);

            foreach (Color c in unusedColors)
                if (game.CheckIfValidMove(f, c))
                    return c;

            Color color;
            do
                color = game.colors[r.Next(game.colors.Length)];
            while (!game.CheckIfValidMove(f, color));

            return color;
        }

        private Color PickUnusedColor(Game game, ColorableObject f1, ColorableObject f2)
        {
            Random r = new Random();
            List<Color> unusedColors = new List<Color>();

            foreach (Color c in game.colors)
                if (!game.usedColors.Contains(c))
                    unusedColors.Add(c);

            ListExtension.Shuffle<Color>(unusedColors);

            foreach (Color c in unusedColors)
                if (game.CheckIfValidMove(f1, c) && f1.color != f2.color)
                    return c;

            foreach (Color c in unusedColors)
                if (game.CheckIfValidMove(f1, c))
                    return c;

            Color color;
            do
                color = game.colors[r.Next(game.colors.Length)];
            while (!game.CheckIfValidMove(f1, color));

            return color;
        }

        private bool AreFlowersConnected(Flower f1, Flower f2, GardenGraph graph)
        {
            foreach (Fence fence in graph.fences)
                if ((fence.f1.Equals(f1) && fence.f2.Equals(f2)) || (fence.f1.Equals(f2) && fence.f2.Equals(f1)))
                    return true;

            return false;
        }

        private bool AreFencesConnected(Fence f1, Fence f2)
        {
            if (f1.f1.Equals(f2.f1) || f1.f1.Equals(f2.f2) || f1.f2.Equals(f2.f1) || f1.f2.Equals(f2.f2))
                return true;

            return false;
        }

        public void EasyFlowerPick(Game game)
        {
            Random r = new Random();
            int n = game.graph.flowers.Count;

            do
            {
                chosenFlowerIndex = r.Next(0, n);
            }
            while (game.graph.flowers[chosenFlowerIndex].color != Color.White);

            game.graph.flowers[chosenFlowerIndex].color = Color.LightBlue;
            game.lastClicked = game.graph.flowers[chosenFlowerIndex];

            flowerPicked = true;
        }

        public void EasyFencePick(Game game)
        {
            Random r = new Random();
            int n = game.graph.fences.Count;
            int chosenFenceIndex;

            do
            {
                chosenFenceIndex = r.Next(0, n);
            }
            while (game.graph.fences[chosenFenceIndex].color != Color.White);

            game.graph.fences[chosenFenceIndex].color = Color.LightBlue;
            game.lastClicked = game.graph.fences[chosenFlowerIndex];

            flowerPicked = true;
        }


        
    }
}
