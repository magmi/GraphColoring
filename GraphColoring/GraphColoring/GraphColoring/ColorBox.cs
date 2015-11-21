using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    class ColorBox
    {
        public Color color;
        public Texture2D texture;
        public Vector2 position;
        public int sideLength = 50;
        public ColorBox(Color c, ContentManager content, Vector2 pos)
        {
            color = c;
            position = pos;
            texture = content.Load<Texture2D>("colorBox");
        }

        public void Draw(SpriteBatch sBatch)
        {

            sBatch.Begin();
            sBatch.Draw(texture, position, color);
            sBatch.End();
        }

        public bool ContainsPoint(Point _point)
        {
            Rectangle r = new Rectangle((int)position.X, (int)position.Y, sideLength, sideLength);
            if (r.Contains(_point))
                return true;
            return false;
        }

    }
}
