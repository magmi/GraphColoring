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

        public static GardenGraph GraphOne(ContentManager content)
        {
            Vector2 center = new Vector2(  300,150);
            int R = 150;
            int n = 4;

            float angle =(float)(2 * Math.PI / n);

            int distance = 300;
            List<Flower> flowers = new List<Flower>{
                                    new Flower(GetCoordinates(center, R, 0), content),
                                    new Flower(GetCoordinates(center, R, angle), content),
                                     new Flower(GetCoordinates(center, R, angle*2), content),
                                      new Flower(GetCoordinates(center, R, angle*3), content),
                                    };

            List<Fence> fences = new List<Fence> {
                new Fence(flowers[0],flowers[1]),
                new Fence(flowers[1],flowers[2]),
                new Fence(flowers[2],flowers[3]),
                new Fence(flowers[3],flowers[0]),
                };
            flowers[0].outFences = new List<Fence> { fences[0], fences[3] };
            flowers[1].outFences = new List<Fence> { fences[0], fences[1] };
            flowers[2].outFences = new List<Fence> { fences[1], fences[2] };
            flowers[3].outFences = new List<Fence> { fences[2], fences[3] };


            return new GardenGraph(flowers,fences);
        }

        public static Vector2 GetCoordinates(Vector2 Center, int r, float angle)
        {
              int  X =(int)( Center.X + (r * Math.Sin(angle)));
              int  Y =(int)( Center.Y + (r * Math.Cos(angle)));
              return new Vector2(X, Y);
        }
    }
}