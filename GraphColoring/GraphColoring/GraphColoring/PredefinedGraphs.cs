using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace GraphColoring
{
    class PredefinedGraphs
    {
        public static List<GardenGraph> graphs;
        
        public static GardenGraph CreateEmptyGraph(int n, ContentManager content)
        {
            Vector2 center = new Vector2(500, 300);
            int R = 250;
            List<Flower> flowers = CreateflowerList(n, center, R, content);
            List<Fence> fences = new List<Fence>();
            return new GardenGraph(flowers, fences);
        }

        public static GardenGraph GraphZero(ContentManager content)
        {
            Vector2 center = new Vector2(500, 300);
            int R = 250;
            int n = 5;

            float angle = (float)(2 * Math.PI / n);


            List<Flower> flowers = CreateflowerList(n, center, R, content);
            int[,] array = new int[n, n];
            array[0, 1] = 1;
            array[1, 2] = 1;
            array[2, 3] = 1;
            array[3, 4] = 1;
            array[4, 0] = 1;
      
            List<Fence> fences = CreateFenceList(flowers, array, content);
            UpdateFlowerList(fences);
            return new GardenGraph(flowers, fences);
        }
        public static GardenGraph GraphOne(ContentManager content)
        {
            Vector2 center = new Vector2(500, 300);
            int R = 250;
            int n = 5;

            float angle = (float)(2 * Math.PI / n);


            List<Flower> flowers = CreateflowerList(n, center, R, content);
            int[,] array = new int[n, n];
            array[0, 2] = 1;
            array[0, 1] = 1;
            array[1, 2] = 1;
            array[2, 3] = 1;
            array[3, 4] = 1;
            array[4, 2] = 1;

            List<Fence> fences = CreateFenceList(flowers, array, content);
            UpdateFlowerList(fences);
            return new GardenGraph(flowers, fences);
        }

        public static GardenGraph GraphTwo(ContentManager content)
        {
            Vector2 center = new Vector2(500, 300);
            int R = 250;
            int n = 8;

            float angle = (float)(2 * Math.PI / n);


            List<Flower> flowers = CreateflowerList(n, center, R, content);
            int[,] array = new int[n, n];
            array[0, 1] = 1;
            array[1, 2] = 1;
            array[2, 3] = 1;
            array[3, 4] = 1;
            array[4, 5] = 1;
            array[5, 6] = 1;
            array[6, 7] = 1;
            array[7, 0] = 1;

            array[0, 4] = 1;
            array[1, 5] = 1;
            array[2, 6] = 1;
            array[3, 7] = 1;
            List<Fence> fences = CreateFenceList(flowers, array, content);
            UpdateFlowerList(fences);
            return new GardenGraph(flowers, fences);
        }

        public static GardenGraph GraphThree(ContentManager content)
        {
            Vector2 center = new Vector2(500, 300);
            int R = 250;
            int n = 6;

            float angle = (float)(2 * Math.PI / n);


            List<Flower> flowers = CreateflowerList(n, center,R,content);
            int[,] array = new int[n,n];
            array[0, 1] = 1;
            array[1, 2] = 1;
            array[2, 3] = 1;
            array[3, 4] = 1;
            array[4, 5] = 1;
            array[5, 0] = 1;

            array[5, 2] = 1;
            array[0, 3] = 1;
            array[1, 4] = 1;
            List<Fence> fences = CreateFenceList(flowers, array, content);

            UpdateFlowerList(fences);
            return new GardenGraph(flowers, fences);
        }


        public static GardenGraph GraphFour(ContentManager content)
        {
            Vector2 center = new Vector2(300, 150);
            int R = 150;
            int n = 4;

            float angle = (float)(2 * Math.PI / n);

            List<Flower> flowers = new List<Flower>{
                                    new Flower(GetCoordinates(center, R, 0), content),
                                    new Flower(GetCoordinates(center, R, angle), content),
                                     new Flower(GetCoordinates(center, R, angle*2), content),
                                      new Flower(GetCoordinates(center, R, angle*3), content),
                                    };

            List<Fence> fences = new List<Fence> {
                new Fence(flowers[0],flowers[1], content),
                new Fence(flowers[1],flowers[2], content),
                new Fence(flowers[2],flowers[3], content),
                new Fence(flowers[3],flowers[0], content),
                };

            flowers[0].outFences = new List<Fence> { fences[0], fences[3] };
            flowers[1].outFences = new List<Fence> { fences[0], fences[1] };
            flowers[2].outFences = new List<Fence> { fences[1], fences[2] };
            flowers[3].outFences = new List<Fence> { fences[2], fences[3] };




            return new GardenGraph(flowers, fences);
        }
        public static List<Fence> CreateFenceList(List<Flower> flow, int[,] array, ContentManager content)
        {
            List<Fence> fen = new List<Fence>();
            int n = array.GetLength(0);
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (array[i, j] > 0)
                        fen.Add(new Fence(flow[i], flow[j], content));
                }
            return fen;
        }
        public static List<Flower> CreateflowerList(int n, Vector2 center, int R, ContentManager content)
        {
            List<Flower> flowers = new List<Flower>();
             float angle =(float)(2 * Math.PI / n);
            for (int i = 0; i < n;i++ )
            {
                Flower f = new Flower(GetCoordinates(center, R, i * angle), content);
                f.outFences = new List<Fence>();
                flowers.Add(f);
            }
            return flowers;
        }

        public static void UpdateFlowerList(List<Fence> fen)
        {
            foreach(Fence f in fen)
            {
                f.f1.outFences.Add(f);
                f.f2.outFences.Add(f);
            }
        }

        public static Vector2 GetCoordinates(Vector2 Center, int r, float angle)
        {
              int  X =(int)( Center.X + (r * Math.Sin(angle)));
              int  Y =(int)( Center.Y + (r * Math.Cos(angle)));
              return new Vector2(X, Y);
        }
    }
}