using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace GraphColoring
{
    class Fence : ColorableObject
    {
        public Flower f1;
        public Flower f2;
        public Texture2D texture;
        public Vector2 position;
        public float scale;
        public float angle;

        public Fence(Flower _f1,Flower _f2, ContentManager content)
        {
            int originalLength = 260; 
            int length = 0;
            
            f1 = _f1;
            f2 = _f2;
            texture = content.Load<Texture2D>("Plotek");
         //   position = new Vector2(f1.position.X < f2.position.X ? f1.position.X : f2.position.X,
        //                        f1.position.X < f2.position.X ? f1.position.Y : f2.position.Y);
            position = f1.center;
            color = Color.White;
            length = (int)Math.Sqrt((f1.position.X - f2.position.X) * (f1.position.X - f2.position.X) +
                (f1.position.Y - f2.position.Y) * (f1.position.Y - f2.position.Y));
            scale = (float) length / originalLength;
            angle = (float)GetAngleOfLineBetweenTwoPoints(f1.center, f2.center);


        }

        public double GetAngleOfLineBetweenTwoPoints(Vector2 p1, Vector2 p2)
        { 
            float xDiff = p2.X - p1.X;
            float yDiff = p2.Y - p1.Y;
            return Math.Atan2(yDiff, xDiff);// *(180 / Math.PI); 
        } 

        public void Draw(SpriteBatch sBatch)
        {

            sBatch.Begin();
            sBatch.Draw(texture, position, null, color, angle, Vector2.Zero, scale, SpriteEffects.None, 0f);
            sBatch.End();
        }
    }
}
