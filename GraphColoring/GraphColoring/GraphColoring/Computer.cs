using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace GraphColoring
{
    class Computer : Player
    {
        public bool easyMode;

        public Computer(bool easyM)
        {
            easyMode = easyM;
        }

        public void CalculateMove(Game g)
        {
            Random r = new Random();
            int n = g.graph.flowers.Count - 1;
            int m = g.colors.Length - 1;
            //kolorowanie kwiatkow
            if(easyMode==true)
            {
                if (g.graph.coloredFlowersNumber >= g.graph.flowers.Count)
                    return;
                int index = 0,colorIndex=0;
                do
                {
                    index = r.Next(0, n);
                }
                while (g.graph.flowers[index].color != Color.White);

                do
                {
                    colorIndex = r.Next(0, m+1);
                }
                while (!g.graph.IsValidMove(g.graph.flowers[index],g.colors[colorIndex]));

                if (g.graph.IsValidMove(g.graph.flowers[index], g.colors[colorIndex]))
                    g.graph.MakeMove(g.graph.flowers[index], g.colors[colorIndex]);
            }
        }
    }
}
