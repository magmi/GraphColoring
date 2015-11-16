using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace GraphColoring
{
    class Flower : ColorableObject
    {
        public int index;
        public List<Fence>outFences;
        public Vector2 positionInPixels;
        public int sideLength;

        public Flower()
        { }
    }
}
