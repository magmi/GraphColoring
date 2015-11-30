using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    class TextBox
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 textPosition;
        public string text;
        public SpriteFont sp;

        public TextBox(ContentManager content, string t, Vector2 pos, Vector2 tPos, string fileName=null)
        {           
            if(fileName!=null)
                texture = content.Load<Texture2D>(fileName);
            text = t;
            position = pos;
            textPosition = tPos;
            sp = content.Load<SpriteFont>("SpriteFont1");

        }
        public void Draw(SpriteBatch sBatch)
        {

            sBatch.Begin();
            if(texture!=null)
                sBatch.Draw(texture, position, Color.White);
            sBatch.DrawString(sp, text, textPosition, Color.Black);
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
