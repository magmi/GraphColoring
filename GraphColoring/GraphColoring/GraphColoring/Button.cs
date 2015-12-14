using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    class Button : ClickableObject
    {

        public Button(Vector2 pos,ContentManager content, string fileName, int i = 0) : base(pos,content,fileName)
        {     
            index = i;
        }

        public override void Draw(SpriteBatch sBatch)
        {
            sBatch.Begin();
            sBatch.Draw(texture, position, color);
            sBatch.End();
        }
       

    }
}
