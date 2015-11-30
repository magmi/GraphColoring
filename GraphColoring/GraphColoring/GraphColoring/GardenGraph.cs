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
        public bool IsColoringPossible(GameType coloringType, Color[] colors) 
        {
            bool correct = false;
            if (coloringType == GameType.EdgesColoring) 
            {                
                foreach (Fence f in fences)
                    if (f.color != null) 
                    {
                        correct = false;
                        foreach (Fence ff in f.f1.outFences)
                        {
                            foreach (Color c in colors)                            
                                if (IsValidMove(ff, c))
                                    correct = true;                            
                            if (!correct)
                                return false;
                        }
                        correct = false;
                        foreach (Fence ff in f.f2.outFences)
                        {
                            foreach (Color c in colors)
                                if (IsValidMove(ff, c))
                                    correct = true;
                            if (!correct)
                                return false;
                        }
                        
                    }
            }


            else if (coloringType == GameType.VerticesColoring)
            {
                foreach (Flower f in flowers)
                    if (f.color != null)
                        foreach (Fence ff in f.outFences)
                        {
                            correct = false;
                            foreach (Color c in colors)
                                if (IsValidMove(ff.f1, c))
                                    correct = true;
                            if (!correct)
                                return false;
                            correct = false;
                            foreach (Color c in colors)
                                if (IsValidMove(ff.f2, c))
                                    correct = true;
                            if (!correct)
                                return false;
                        }
            }
            return true;
        }

        public bool IsValidMove(Fence f, Color c)
        {
            if (f.color != null)
                return true;

            return true;
        }
        public bool IsValidMove(Flower f, Color c)
        {
            for (int i = 0; i < f.outFences.Count;i++ )
            {
                Flower flower = f.outFences[i].f1 == f ? f.outFences[i].f2 : f.outFences[i].f1;
                if (flower.color == c)
                    return false;

            }
            return true;
        }
        public void MakeMove(ColorableObject obj, Color c)
        {
            obj.color = c;
            coloredFlowersNumber++;
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
