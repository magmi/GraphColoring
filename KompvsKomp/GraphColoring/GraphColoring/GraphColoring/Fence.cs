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
    public class Fence : ColorableObject
    {
        public Flower f1;
        public Flower f2;
        public Texture2D texture;
        public string textureAsset;
        public Vector2 position;
        public float angle;
        public int height = 20;

        public Fence()
        { }
        public Fence(Flower _f1, Flower _f2)
        {
            f1 = _f1;
            f2 = _f2;
            int length = (int)Math.Sqrt((f1.position.X - f2.position.X) * (f1.position.X - f2.position.X) +
                (f1.position.Y - f2.position.Y) * (f1.position.Y - f2.position.Y));
            position = f1.center;
            color = Color.White;
            angle = (float)GetAngleOfLineBetweenTwoPoints(f1.center, f2.center);
        }
        public Fence(Flower _f1, Flower _f2, string _textureAsset)
        {            
            f1 = _f1;
            f2 = _f2;
            int length = (int)Math.Sqrt((f1.position.X - f2.position.X) * (f1.position.X - f2.position.X) +
                (f1.position.Y - f2.position.Y) * (f1.position.Y - f2.position.Y));               
            textureAsset = _textureAsset;
            Texture2D originalTexture = Globals.content.Load<Texture2D>(textureAsset);
            Rectangle sourceRectangle = new Rectangle(0, 0, length, originalTexture.Height);
            texture = new Texture2D(originalTexture.GraphicsDevice, sourceRectangle.Width, sourceRectangle.Height);
            Color[] data = new Color[sourceRectangle.Width * sourceRectangle.Height];
            originalTexture.GetData(0, sourceRectangle, data, 0, data.Length);
            texture.SetData(data);         
            position = f1.center;
            color = Color.White;
            angle = (float)GetAngleOfLineBetweenTwoPoints(f1.center, f2.center);
        }

        /// <summary>
        /// Funkcja obliczajaca w radianach kat miedzy dwoma puntkami
        /// </summary>
        /// <param name="p1">punkt 1</param>
        /// <param name="p2">punkt 2</param>
        /// <returns></returns>
        public double GetAngleOfLineBetweenTwoPoints(Vector2 p1, Vector2 p2)
        { 
            float xDiff = p2.X - p1.X;
            float yDiff = p2.Y - p1.Y;
            return Math.Atan2(yDiff, xDiff);// *(180 / Math.PI); 
        }
        /// <summary>
        /// Funckaj obliczajaca odleglosc miedzy dwoma kwiatkami
        /// </summary>
        /// <returns></returns>
        public int GetDistanceBetweenFlowers()
        {
            return (int)Math.Sqrt((f2.position.X - f1.position.X) * (f2.position.X - f1.position.X) 
                + (f2.position.Y - f1.position.Y) * (f2.position.Y - f1.position.Y));
        }

        /// <summary>
        /// Funkcja sprawdzajaca zawieranie punktu przez plotek
        /// </summary>
        /// <param name="_point"></param>
        /// <returns></returns>
        public bool ContainsPoint(Point _point)
        {
            double an = -angle;
            double cos = Math.Cos(an);
            double sin = Math.Sin(an);
            int nx = (int)(cos * (_point.X - position.X) - sin * (_point.Y - position.Y) + position.X);
            int ny = (int)(sin * (_point.X - position.X) + cos * (_point.Y - position.Y) + position.Y);           
            Rectangle r = new Rectangle((int)position.X, (int)position.Y,GetDistanceBetweenFlowers(), height);            
            if (r.Contains(new Point(nx,ny)))
                return true;
            return false;
        }

        /// <summary>
        /// Funkcja rysujaca plotek
        /// </summary>
        /// <param name="sBatch"></param>
        public void Draw(SpriteBatch sBatch)
        {
            sBatch.Begin();           
            sBatch.Draw(texture, position, null, color, angle, Vector2.Zero, 1, SpriteEffects.None, 0f);
            sBatch.End();
        }
    }
}
