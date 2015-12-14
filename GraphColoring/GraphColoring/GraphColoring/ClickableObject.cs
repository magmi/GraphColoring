using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    class ClickableObject
    {
        public Texture2D texture;
        public Vector2 position;
        public Color color = Color.White;
        public string name;
        public int index;
        
        public ClickableObject(Vector2 pos,ContentManager content, string fileName=null)
        {
            name = fileName;
            if(fileName!=null)
                texture = content.Load<Texture2D>(fileName);
            position = pos;            
        }

        public virtual void Draw(SpriteBatch sBatch){ }
        public bool ContainsPoint(Point _point)
        {
            Rectangle r = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (r.Contains(_point))
                return true;
            return false;
        }
    }
}
