using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    public class Button : ClickableObject
    {

        public Button(Vector2 pos,ContentManager content, string fileName, int i = 0) : base(pos,content,fileName)
        {     
            index = i;
        }

        public override void Draw(SpriteBatch sBatch)
        {
            int gameHeight = Game1.GetHeight();
            int gameWidth = Game1.GetWidth();
            Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Game1.widthRatio), (int)(texture.Height * Game1.heightRatio));
            sBatch.Begin();
            sBatch.Draw(texture, destRect, color);
            sBatch.End();
        }
       

    }
}
