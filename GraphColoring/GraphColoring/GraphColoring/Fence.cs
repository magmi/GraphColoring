using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphColoring
{
    class Fence : ColorableObject
    {
        public Flower f1;
        public Flower f2;

        public Fence(Flower _f1,Flower _f2)
        {
            f1 = _f1;
            f2 = _f2;
        }
    }
}
