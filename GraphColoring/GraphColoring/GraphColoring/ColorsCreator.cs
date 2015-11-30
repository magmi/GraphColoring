using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphColoring
{
    class ColorsCreator
    {
        public static Color[] colors = new Color[] { Color.Red, Color.Blue, Color.Yellow, Color.Green, Color.Purple, Color.Orange, Color.Cyan, Color.Magenta, Color.Pink };

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
