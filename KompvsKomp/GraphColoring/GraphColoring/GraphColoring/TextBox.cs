using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    public class TextBox : ClickableObject
    {       
        public Vector2 textPosition;
        public string text;
        public SpriteFont sp;
        public Color textColor;

        public TextBox(ContentManager content,string t,Vector2 pos, Vector2 tPos, string fileName=null, int i=0,string sf = "SpriteFont1") : base(pos,content,fileName)
        {                                  
            text = t;
            position = pos;
            textPosition = tPos;
            sp = content.Load<SpriteFont>(sf);
            textColor = Color.Black;
            index = i;
        }

        public TextBox(ContentManager content, string t, Vector2 pos, Vector2 tPos, Color col, string fileName = null, int i = 0, string sf = "SpriteFont1")
            : base(pos, content, fileName)
        {            
            text = t;
            position = pos;
            textPosition = tPos;
            sp = content.Load<SpriteFont>(sf);
            textColor =  col;
        }

        public override void Draw(SpriteBatch sBatch)
        {
            sBatch.Begin();
            if(texture!=null)
                sBatch.Draw(texture, position, color);
            sBatch.DrawString(sp, text, textPosition,  textColor);
            sBatch.End();
        }


    }
}
