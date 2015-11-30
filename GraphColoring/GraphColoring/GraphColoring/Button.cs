using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    class Button
    {
        public Texture2D texture;
        public Vector2 position;
        public string name;
        public Color color = Color.White;
        public Button(Vector2 pos,ContentManager content, string fileName)
        {
            name = fileName;
            position = pos;
            this.texture = content.Load<Texture2D>(fileName);            
            
        }

        public void Draw(SpriteBatch sBatch)
        {

            sBatch.Begin();
            sBatch.Draw(texture, position, color);
            sBatch.End();
        }
        public bool ContainsPoint(Point _point)
        {
            Rectangle r = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (r.Contains(_point))
                return true;
            return false;
        }

    }
}
