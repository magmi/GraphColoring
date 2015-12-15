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

        public GardenGraph Copy()
        {
            List<Flower> copyFlowers = new List<Flower>();
            List<Fence> copyFences = new List<Fence>();

            foreach(Flower flower in flowers)
                copyFlowers.Add(flower.Copy());

            foreach (Flower flower in flowers)
            {
                foreach (Fence fence in flower.outFences)
                {
                    Fence copyFence = new Fence(copyFlowers[fence.f1.index],
                        copyFlowers[fence.f2.index], fence.content);

                    if (!copyFences.Exists(x=>x.f1.Equals(copyFence.f1) && x.f2.Equals(copyFence.f2)))
                        copyFences.Add(copyFence);
                }
            }

            foreach(Fence fence in copyFences)
            {
                fence.f1.outFences.Add(fence);
                fence.f2.outFences.Add(fence);
            }

            return new GardenGraph(copyFlowers, copyFences);
        }

    }
}
