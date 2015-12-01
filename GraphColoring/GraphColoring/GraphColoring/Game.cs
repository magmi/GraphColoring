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

    class Game
    {
        public Player player1;
        public Player player2;
        public GameType gameType;
        public GameMode gameMode;
        public GardenGraph graph;
        public Color[] colors;
        public List<ColorBox> colorBoxes;
        public int whoseTurn;
        public Flower lastClicked = null;
        public bool gardenerStartedMove;
        public TextBox[] PlayersTexts;
        public List<TextBox> panels;

        public Game(GameType gT, GameMode gM, GardenGraph g, int c, ContentManager content, Player p1, Player p2)
        {            
            player2 = new Computer(true);
            colorBoxes = new List<ColorBox>();
            gameType = gT;
            gameMode = gM;
            graph = g;
            colors = ColorsCreator.GetColors(c);
            this.whoseTurn = 0;
            this.gardenerStartedMove = false;
            int dist = 60;
            int offset = 20;

            for(int i =0;i<colors.Length;i++)
            {
                Vector2 vect = new Vector2(offset + (dist) * (i % 2), offset + (dist) * ((int)i / 2));
                colorBoxes.Add(new ColorBox(colors[i], content, vect));
            }
            player1 = p1;
            player2 = p2;
            panels = new List<TextBox>() { new TextBox(content, "",new Vector2(0,0),new Vector2(0,0),"Panel") };
            if(p2 is Computer)
            {
                PlayersTexts = new TextBox[] { new TextBox(content, p1.login, new Vector2(0, 0), new Vector2(30, 400), Color.White) };
            }
            else
            {
                PlayersTexts = new TextBox[] { new TextBox(content, p1.login, new Vector2(0, 0), new Vector2(30, 400), Color.White),
                                            new TextBox(content, p2.login, new Vector2(0, 0), new Vector2(30, 600), Color.White),
                };
            }

        }

        public bool CheckIfEnd(out bool didGardenerWon)
        {
            didGardenerWon = false;

            if ( ( (this.gameType == GameType.VerticesColoring) && (this.graph.coloredFlowersNumber == this.graph.flowersNumber) )
                || ( (this.gameType == GameType.EdgesColoring) && (this.graph.coloredFencesNumber == this.graph.fencesNumber) ) )
            {
                didGardenerWon = true;
                return true;
            }

            if (!this.graph.IsColoringPossible(this.gameType, this.colors))
                return true;

            return false;
        }

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

        public void DrawColorPalete(SpriteBatch sBatch)
        {
            foreach (TextBox t in panels)
                t.Draw(sBatch);
            foreach (ColorBox cb in colorBoxes)
                cb.Draw(sBatch);
        }

        public void DrawPlayers(SpriteBatch sBatch)
        {
            foreach (TextBox t in PlayersTexts)
                t.Draw(sBatch);
        }

        public bool CheckIfValidMove(Flower flower, Color c)
        {
            foreach(Fence f in flower.outFences)
            {
                if(f.f1!=flower)
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
            return true;
        }

        public void StartGame()
        {

        }

    }
}
