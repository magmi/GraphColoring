using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace GraphColoring
{
    public enum GameType { VerticesColoring, EdgesColoring };

    [Serializable]
    public class GardenGraph
    {
        public List<Fence> fences;
        public List<Flower> flowers;
        public int flowersNumber;
        public int fencesNumber;
        public int coloredFlowersNumber;
        public int coloredFencesNumber;

        public GardenGraph()
        { }

        public GardenGraph(List<Flower> flo, List<Fence> fen)
        {
            fences = fen;
            flowers = flo;
            flowersNumber = flowers.Count;
            fencesNumber = fences.Count;

        }        
        
        /// <summary>
        /// Funkcja robiaca ruch
        /// </summary>
        /// <param name="obj">wybrany element</param>
        /// <param name="c">kolor</param>
        /// <param name="game">gra</param>
        public void MakeMove(ColorableObject obj, Color c, Game game)
        {
            obj.color = c;
            if (obj is Flower)
                coloredFlowersNumber++;
            else
                coloredFencesNumber++;

            if (!game.usedColors.Contains(c))
                game.usedColors.Add(c);
        }

        /// <summary>
        /// Funkcja rysujaca wszystkie elementy w grafie
        /// </summary>
        /// <param name="sBatch"></param>
        public void DrawAllElements(SpriteBatch sBatch)
        {
            foreach (Fence f in fences)
                f.Draw(sBatch);
            foreach(Flower f in flowers)            
                f.Draw(sBatch);           

        }

        /// <summary>
        /// Pomocnicza funckcja kopiujaca graf
        /// </summary>
        /// <returns>skopiowany graf</returns>
        public GardenGraph Copy()
        {
            List<Flower> copyFlowers = new List<Flower>();
            List<Fence> copyFences = new List<Fence>();

            foreach(Flower flower in flowers)
                copyFlowers.Add(flower.Copy());

            foreach(Fence fence in fences)
            {
                Fence copyFence = new Fence(copyFlowers[fence.f1.index],
                    copyFlowers[fence.f2.index], "Plotek");

                copyFences.Add(copyFence);
            }

            return new GardenGraph(copyFlowers, copyFences);
        }

        /// <summary>
        /// Funckja zwracajaca plotki wychodzace z kwiatka
        /// </summary>
        /// <param name="flower">kwiatek</param>
        /// <returns>lista z plotkami</returns>
        public List<Fence> GetOutFences(Flower flower)
        {
            List<Fence> outFences = new List<Fence>();

            foreach(Fence fence in fences)
            {
                if (fence.f1.Equals(flower) || fence.f2.Equals(flower))
                    outFences.Add(fence);
            }

            return outFences;
        }
    }
}
