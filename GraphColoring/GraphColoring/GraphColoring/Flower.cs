using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    [Serializable]
    public class Flower : ColorableObject
    {
        public int index;
        public Vector2 position;
        public Vector2 center;
        public int sideLength;
        public Texture2D texture;
        public string textureAsset;

        public Flower()
        { }

        public Flower(Vector2 pos, string _textureAsset, int index)
        {
            this.index = index;
            color = Color.White;
            position = pos;
            sideLength = 152;
            center = new Vector2(pos.X + sideLength / 2, pos.Y + sideLength / 2);
            textureAsset = _textureAsset;
            texture = Globals.content.Load<Texture2D>(textureAsset);
        }

        public Flower(Vector2 pos, int index)
        {
            this.index = index;
            color = Color.White;
            position = pos;
            sideLength = 152;
            center = new Vector2(pos.X + sideLength / 2, pos.Y + sideLength / 2);           
        }

        public void Draw(SpriteBatch sBatch)
        {
           
            sBatch.Begin();

            sBatch.Draw(texture, position, color);

            sBatch.End();            
        }

       public bool ContainsPoint(Point _point)
       {
           Rectangle r = new Rectangle((int)position.X,(int)position.Y,sideLength,sideLength);
           if(r.Contains(_point))
               return true;	
           return false;
       }

       public Flower Copy()
       {
           return new Flower(position, textureAsset, index);
       }

    }
}
