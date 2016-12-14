using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace GraphColoring
{
    public enum GameMode {SinglePlayer, MultiPlayer};
    public enum GameOrder { GN, NG }
    public class Game
    {
        public Player player1;
        public Player player2;
        public GameType gameType;
        public GameMode gameMode;
        public GameOrder gameOrder;
        public GardenGraph graph;
        public Color[] colors;
        public List<Color> usedColors;
        public List<ColorBox> colorBoxes;
        public int whoseTurn;
        public ColorableObject lastClicked = null;
        public int lastClickedIndex = -1;
        public bool gardenerStartedMove;
        public TextBox[] PlayersTexts;
        public TextBox[] PlayerPoints;
        public TextBox WhoseTurnText;
        public List<TextBox> panels;

        public Game(GameType gT, GardenGraph g, int c)
        {
            player2 = new Computer(true);
            colorBoxes = new List<ColorBox>();
            gameType = gT;
            graph = g;
            colors = ColorsCreator.GetColors(c);
            usedColors = new List<Color>();
            this.whoseTurn = 0;
            this.gardenerStartedMove = false;
        }

        public Game(GameType gT, GameMode gM, GardenGraph g, int c, ContentManager content, Player p1, Player p2, GameOrder go)
        {            
            player2 = new Computer(true);
            colorBoxes = new List<ColorBox>();
            gameType = gT;
            gameMode = gM;
            graph = g;
            gameOrder = go;
            colors = ColorsCreator.GetColors(c);
            usedColors = new List<Color>();
            this.whoseTurn = 0;
            this.gardenerStartedMove = false;
            int distx = 55;
            int disty = 30;
            int offset = 10;

            for(int i =0;i<colors.Length;i++)
            {
                Vector2 vect = new Vector2(offset + (distx) * (i % 3), offset + (disty) * ((int)i / 3));
                colorBoxes.Add(new ColorBox(colors[i], content, vect));
            }
            player1 = p1;
            player2 = p2;
            panels = new List<TextBox>() { new TextBox(content, "",new Vector2(0,0),new Vector2(0,0),"Panel") };
            string ps1 = p1.isGardener ? "O: " : "S: ";
            string ps2 = p2.isGardener ? "O: " : "S: ";
            WhoseTurnText = new TextBox(content, "Tura: " + (p1.isGardener ? "Ogrodnika" : "Sasiada"), new Vector2(0, 0), Game1.GetRatioDimensions(new Vector2(300, 0)), Color.White, null, 0, "CzcionkaUI");
            if(p2 is Computer)
            {
                PlayersTexts = new TextBox[] { new TextBox(content, ps1 + p1.login, new Vector2(0, 0), Game1.GetRatioDimensions(new Vector2(10, 400)), Color.White, null, 0, "CzcionkaUI") };
                PlayerPoints = new TextBox[] { new TextBox(content, p1.points.ToString(), new Vector2(0, 0), Game1.GetRatioDimensions(new Vector2(30, 440)), Color.White, null, 0, "CzcionkaUI") };
            }
            else
            {
                PlayersTexts = new TextBox[] { new TextBox(content, ps1 + p1.login, new Vector2(0, 0), Game1.GetRatioDimensions(new Vector2(10, 400)), Color.White, null, 0, "CzcionkaUI"),
                                            new TextBox(content, ps2 + p2.login, new Vector2(0, 0), Game1.GetRatioDimensions(new Vector2(10, 600)), Color.White, null, 0, "CzcionkaUI"),};
                PlayerPoints = new TextBox[] { new TextBox(content, p1.points.ToString(), new Vector2(0, 0), Game1.GetRatioDimensions(new Vector2(30, 440)), Color.White, null, 0, "CzcionkaUI"),
                                           new TextBox(content, p2.points.ToString(), new Vector2(0, 0), Game1.GetRatioDimensions(new Vector2(30, 640)), Color.White, null, 0, "CzcionkaUI"),
                
                };
            }

        }

        /// <summary>
        /// Funkcja zmieniajaca tekst tury
        /// </summary>
        /// <param name="sasiad">czy poprzednio byla tura sasiada</param>
        public void ChangeTurn(bool sasiad)
        {
            WhoseTurnText.text = "Tura: " + (sasiad ? "Ogrodnika" : "Sasiada");
        }

        /// <summary>
        /// Funkcja sprawdzajaca zakonczenie gry
        /// </summary>
        /// <param name="didGardenerWon">parametr czy wygral ogrodnik</param>
        /// <returns>czy gra zostala zakonczona</returns>
        public bool CheckIfEnd(out bool didGardenerWon)
        {
            didGardenerWon = false;

            if ( ( (this.gameType == GameType.VerticesColoring) && (this.graph.coloredFlowersNumber == this.graph.flowersNumber) )
                || ( (this.gameType == GameType.EdgesColoring) && (this.graph.coloredFencesNumber == this.graph.fencesNumber) ) )
            {
                didGardenerWon = true;
                return true;
            }

            if (!this.IsColoringPossible(this.gameType, this.colors))
                return true;

            return false;
        }

        /// <summary>
        /// Funkcja sprawdzajaca nacisniecie na ktorys z kwiatkow grafu
        /// </summary>
        /// <param name="mousePos">pozycja myszy</param>
        /// <param name="index">zwracany indeks kwiatka</param>
        /// <returns></returns>
        public bool CheckIfMouseClickedOnFlower(Point mousePos, out int index)
        {
            index = 0;
            for(int i=0;i<graph.flowers.Count;i++)                
            {
                Flower f = graph.flowers[i];                
                if(f.ContainsPoint(mousePos))
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Funkcja sprawdzajaca nacisniecie na ktorys z plotkow grafu
        /// </summary>
        /// <param name="mousePos">pozycja myszy</param>
        /// <param name="index">zwracany indeks plotka</param>
        /// <returns></returns>
        public bool CheckIfMouseClickedOnFence(Point mousePos, out int index)
        {
            index = 0;
            for (int i = 0; i < graph.fences.Count; i++)
            {
                Fence f = graph.fences[i];
                if (f.ContainsPoint(mousePos))
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Funkcja sprawdzajaca nacisniecie na ktorys z kolorow
        /// </summary>
        /// <param name="mousePos">pozycja myszy</param>
        /// <param name="index">zwracany indeks koloru</param>
        /// <returns></returns>
        public bool CheckIfMouseClickedOnColor(Point mousePos, out int index)
        {
            index = 0;
            for (int i = 0; i < colorBoxes.Count; i++)
            {
                ColorBox cb = colorBoxes[i];
                if (cb.ContainsPoint(mousePos))
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Funkcja rysujaca palete kolorow
        /// </summary>
        /// <param name="sBatch"></param>
        public void DrawColorPalete(SpriteBatch sBatch)
        {
            foreach (TextBox t in panels)
                t.Draw(sBatch);
            foreach (ColorBox cb in colorBoxes)
                cb.Draw(sBatch);
        }

        /// <summary>
        /// funckja rysujaca loginy i punkty graczy
        /// </summary>
        /// <param name="sBatch"></param>
        public void DrawPlayers(SpriteBatch sBatch)
        {
            WhoseTurnText.Draw(sBatch);
            foreach (TextBox t in PlayersTexts)
                t.Draw(sBatch);
            foreach (TextBox t in PlayerPoints)
                t.Draw(sBatch);
        }

        /// <summary>
        /// Funkcja dodajaca punkty
        /// </summary>
        public void AddPoints()
        {
            int index = whoseTurn;
            Player p = index == 0 ? player1 : player2;
            p.points += 50;
            PlayerPoints[index].text = p.points.ToString();
        }

        /// <summary>
        /// Funkcja sprawdzajaca czy mozliwe jest dalsze poprawne kolorowanie
        /// </summary>
        /// <param name="coloringType">typ kolorowania</param>
        /// <param name="colors">tablica kolorow</param>
        /// <returns></returns>
        public bool IsColoringPossible(GameType coloringType, Color[] colors)
        {
            bool correct = false;
            List<Fence> outFences;
            if (coloringType == GameType.EdgesColoring)
            {
                foreach (Fence f in graph.fences)
                    if (f.color != Color.White)
                    {
                        outFences = graph.GetOutFences(f.f1);
                        foreach (Fence ff in outFences)
                        {
                            correct = false;
                            foreach (Color c in colors)
                                if (CheckIfValidMove(ff, c))
                                    correct = true;
                            if (!correct)
                                return false;
                        }

                        outFences = graph.GetOutFences(f.f2);
                        foreach (Fence ff in outFences)
                        {
                            correct = false;
                            foreach (Color c in colors)
                                if (CheckIfValidMove(ff, c))
                                    correct = true;
                            if (!correct)
                                return false;
                        }
                    }
            }

            else if (coloringType == GameType.VerticesColoring)
            {
                foreach (Flower f in graph.flowers)
                    if (f.color != Color.White)
                    {
                        outFences = graph.GetOutFences(f);
                        foreach (Fence ff in outFences)
                        {
                            correct = false;
                            foreach (Color c in colors)
                                if (CheckIfValidMove(ff.f1, c))
                                    correct = true;
                            if (!correct)
                                return false;
                            correct = false;
                            foreach (Color c in colors)
                                if (CheckIfValidMove(ff.f2, c))
                                    correct = true;
                            if (!correct)
                                return false;
                        }
                    }
            }
            return true;
        }

        /// <summary>
        /// Funkcja sprawdzajaca prawidlowosc ruchu
        /// </summary>
        /// <param name="cb">wybrany obiekt</param>
        /// <param name="c">kolor</param>
        /// <returns></returns>
        public bool CheckIfValidMove(ColorableObject cb, Color c)
        {
            List<Fence> outFences;
            if (cb is Flower)
            {
                Flower flower = cb as Flower;
                outFences = graph.GetOutFences(flower);
                foreach (Fence f in outFences)
                {
                    if (f.f1 != flower)
                    {
                        if (f.f1.color == c)
                            return false;
                    }
                    else
                    {
                        if (f.f2.color == c)
                            return false;
                    }
                }
            }
            else
            {
                Fence fence = cb as Fence;
                outFences = graph.GetOutFences(fence.f1);
                foreach (Fence f in outFences)
                {
                    if (f != fence)
                    {
                        if (f.color == c)
                            return false;
                    }
                }
                outFences = graph.GetOutFences(fence.f2);
                foreach (Fence f in outFences)
                {
                    if (f != fence)
                    {
                        if (f.color == c)
                            return false;
                    }
                }
            }
            return true;
        }
        

    }
}
