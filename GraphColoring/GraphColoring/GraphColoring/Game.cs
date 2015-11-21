using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace GraphColoring
{
    class Game
    {
        public GameType gameType;
        public GardenGraph graph;
        public Color[] colors;
        public List<ColorBox> colorBoxes;

        public Game(GameType gT, GardenGraph g, int c, ContentManager content)
        {
            colorBoxes = new List<ColorBox>();
            gameType = gT;
            graph = g;
            colors = ColorsCreator.GetColors(c);
            int dist = 60;

            for(int i =0;i<colors.Length;i++)
            {
                Vector2 vect = new Vector2((dist)* (i % 2),(dist)*((int)i / 2));
                colorBoxes.Add(new ColorBox(colors[i], content, vect));
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
            foreach (ColorBox cb in colorBoxes)
                cb.Draw(sBatch);
        }

        public void StartGame()
        {

        }

    }
}
