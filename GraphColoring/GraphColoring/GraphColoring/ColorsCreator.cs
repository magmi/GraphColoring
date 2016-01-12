using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphColoring
{
    class ColorsCreator
    {
        public static Color[] colors = new Color[] { Color.Yellow, Color.Orange, Color.Red, Color.Magenta, Color.Purple, Color.Blue, Color.Cyan, Color.Lime, Color.Green, };

        public static Color[] GetColors(int colorNr)
        {
            if(colorNr>=colors.Length-1)
            {
                return colors;
            }
            else
            {
                Color[] colorArray = new Color[colorNr];
                for (int i = 0; i < colorArray.Length;i++)
                {
                    colorArray[i] = colors[i];
                }
                return colorArray;
            }
        }
    }
}
