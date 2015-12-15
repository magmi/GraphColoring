﻿using System;
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
    class Game
    {
        public Player player1;
        public Player player2;
        public GameType gameType;
        public GameMode gameMode;
        public GameOrder gameOrder;
        public GardenGraph graph;
        public Color[] colors;
        public List<ColorBox> colorBoxes;
        public int whoseTurn;
        public ColorableObject lastClicked = null;
        public int lastClickedIndex = -1;
        public bool gardenerStartedMove;
        public TextBox[] PlayersTexts;
        public TextBox[] PlayerPoints;
        public TextBox WhoseTurnText;
        public List<TextBox> panels;

        public Game(GameType gT, GameMode gM, GardenGraph g, int c, ContentManager content, Player p1, Player p2, GameOrder go)
        {            
            player2 = new Computer(true);
            colorBoxes = new List<ColorBox>();
            gameType = gT;
            gameMode = gM;
            graph = g;
            gameOrder = go;
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
            string ps1 = p1.isGardener ? "O: " : "S: ";
            string ps2 = p2.isGardener ? "O: " : "S: ";
            WhoseTurnText = new TextBox(content, "Tura: " + (p1.isGardener ? "Ogrodnika" : "Sasiada"), new Vector2(0, 0), new Vector2(400, 0), Color.White);
            if(p2 is Computer)
            {
                PlayersTexts = new TextBox[] { new TextBox(content, ps1 + p1.login, new Vector2(0, 0), new Vector2(10, 400), Color.White) };
                PlayerPoints = new TextBox[] { new TextBox(content, p1.points.ToString(), new Vector2(0, 0), new Vector2(30, 440), Color.White) };
            }
            else
            {
                PlayersTexts = new TextBox[] { new TextBox(content, ps1 + p1.login, new Vector2(0, 0), new Vector2(10, 400), Color.White),
                                            new TextBox(content, ps2 + p2.login, new Vector2(0, 0), new Vector2(10, 600), Color.White),};
                PlayerPoints = new TextBox[] { new TextBox(content, p1.points.ToString(), new Vector2(0, 0), new Vector2(30, 440), Color.White),
                                           new TextBox(content, p2.points.ToString(), new Vector2(0, 0), new Vector2(30, 640), Color.White),
                
                };
            }

        }

        public void ChangeTurn(bool sasiad)
        {
            WhoseTurnText.text = "Tura: " + (sasiad ? "Ogrodnika" : "Sasiada");
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

            if (!this.IsColoringPossible(this.gameType, this.colors))
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
            WhoseTurnText.Draw(sBatch);
            foreach (TextBox t in PlayersTexts)
                t.Draw(sBatch);
            foreach (TextBox t in PlayerPoints)
                t.Draw(sBatch);
        }

        public void AddPoints()
        {
            int index = whoseTurn;
            Player p = index == 0 ? player1 : player2;
            p.points += 50;
            PlayerPoints[index].text = p.points.ToString();
        }

        public bool IsColoringPossible(GameType coloringType, Color[] colors)
        {
            bool correct = false;
            if (coloringType == GameType.EdgesColoring)
            {
                foreach (Fence f in graph.fences)
                    if (f.color != null)
                    {
                        correct = false;
                        foreach (Fence ff in f.f1.outFences)
                        {
                            foreach (Color c in colors)
                                if (CheckIfValidMove(ff, c))
                                    correct = true;
                            if (!correct)
                                return false;
                        }
                        correct = false;
                        foreach (Fence ff in f.f2.outFences)
                        {
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
                    if (f.color != null)
                        foreach (Fence ff in f.outFences)
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
            return true;
        }

        public bool CheckIfValidMove(ColorableObject cb, Color c)
        {
            if (cb is Flower)
            {
                Flower flower = cb as Flower;
                foreach (Fence f in flower.outFences)
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
                foreach (Fence f in fence.f1.outFences)
                {
                    if (f != fence)
                    {
                        if (f.color == c)
                            return false;
                    }
                }
                foreach (Fence f in fence.f2.outFences)
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
        
        public void StartGame()
        {

        }

    }
}
