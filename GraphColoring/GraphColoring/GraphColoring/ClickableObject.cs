using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    public class ClickableObject
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

        /// <summary>
        /// Funkcja rysujaca obiekt
        /// </summary>
        /// <param name="sBatch"></param>
        public virtual void Draw(SpriteBatch sBatch)
        {
            Rectangle destRect = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * Game1.widthRatio), (int)(texture.Height * Game1.heightRatio));
            sBatch.Begin();
            sBatch.Draw(texture, destRect, color);
            sBatch.End();
        }

        /// <summary>
        /// Funckja sprawdzajaca zwawieranie punktu w obiekcie
        /// </summary>
        /// <param name="_point"></param>
        /// <returns></returns>
        public bool ContainsPoint(Point _point)
        {
            Rectangle r = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            if (r.Contains(_point))
                return true;
            return false;
        }
    }
}
