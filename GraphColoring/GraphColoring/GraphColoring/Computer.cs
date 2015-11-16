using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace GraphColoring
{
    class Computer
    {
        public bool easyMode;
        public void CalculateMove(Game g)
        {
            Random r = new Random();
            int n = g.graph.flowers.Count - 1;
            int m = g.colors.Length - 1;
            if(easyMode==true)
            {
                int index = 0,colorIndex=0;
                do
                {
                    index = r.Next(0, n);
                }
                while (g.graph.flowers[index].color != null);

                do
                {
                    colorIndex = r.Next(0, m);
                }
                while (!g.graph.IsValidMove(g.graph.flowers[index],g.colors[colorIndex]));
            }
        }
    }
}
