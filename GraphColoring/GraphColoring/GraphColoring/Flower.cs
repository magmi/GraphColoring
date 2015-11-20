﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    class Flower : ColorableObject
    {
        public int index;
        public List<Fence>outFences;
        public Vector2 positionInPixels;
        public int sideLength;
        public Texture2D texture;

        public Flower(Vector2 pos, ContentManager content)
        {
            positionInPixels = pos;
            this.texture = content.Load<Texture2D>("Kwiatek");
        }

        public void Draw(SpriteBatch sBatch)
        {
            sBatch.Begin();
            sBatch.Draw(texture, new Vector2(positionInPixels.X, positionInPixels.Y), Color.White);
            sBatch.End();            
        }

       public bool ContainsPoint(Point _point)
       {
           Rectangle r = new Rectangle((int)positionInPixels.X,(int)positionInPixels.Y,sideLength,sideLength);
           if(r.Contains(_point))
               return true;	
           return false;
       }

    }
}
