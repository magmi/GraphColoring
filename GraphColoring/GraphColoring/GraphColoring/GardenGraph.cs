using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace GraphColoring
{
    enum GameType { VerticesColoring, EdgesColoring };

    class GardenGraph
    {
        public List<Fence> fences;
        public List<Flower> flowers;
        public int flowersNumber;
        public int fencesNumber;
        public int coloredFlowersNumber;
        public int coloredFencesNumber;

        public GardenGraph(List<Flower> flo, List<Fence> fen)
        {
            fences = fen;
            flowers = flo;
            flowersNumber = flowers.Count;
            fencesNumber = fences.Count;

        }        
        
        public void MakeMove(ColorableObject obj, Color c)
        {
            obj.color = c;
            if (obj is Flower)
                coloredFlowersNumber++;
            else
                coloredFencesNumber++;
        }

        public void DrawAllElements(SpriteBatch sBatch)
        {
            foreach (Fence f in fences)
                f.Draw(sBatch);
            foreach(Flower f in flowers)            
                f.Draw(sBatch);           

        }


    }
}
