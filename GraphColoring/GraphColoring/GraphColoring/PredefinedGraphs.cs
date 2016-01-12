using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace GraphColoring
{
    class PredefinedGraphs
    {
        public static List<GardenGraph> graphs;
        public static Vector2 center = new Vector2(600, 320);
        /// <summary>
        /// Metoda tworząca graf o n wierzcholkach bez krawedzi
        /// </summary>
        /// <param name="n">liczba krawedzi</param>
        /// <param name="content">menadzer zawartosci</param>
        /// <returns>Zwraca graf o n krawedziach</returns>
        public static GardenGraph CreateEmptyGraph(int n, ContentManager content)
        {
            int height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int R = 50*n;
            if (R > height / 2 - 300)
                R = height / 2 - 300;
            List<Flower> flowers = CreateflowerList(n, center, R, content);
            List<Fence> fences = new List<Fence>();
            return new GardenGraph(flowers, fences);
        }

        /// <summary>
        /// Graf Predefiniowany
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static GardenGraph GraphZero(ContentManager content)
        {            
            int R = 250;
            int n = 5;

            float angle = (float)(2 * Math.PI / n);


            List<Flower> flowers = CreateflowerList(n, center, R, content);
            int[,] array = new int[n, n];
            array[0, 1] = 1;
            array[1, 2] = 1;
            array[2, 3] = 1;
            array[3, 4] = 1;
            array[4, 0] = 1;
      
            List<Fence> fences = CreateFenceList(flowers, array, content);
            return new GardenGraph(flowers, fences);
        }
        /// <summary>
        /// Graf Predefiniowany
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static GardenGraph GraphOne(ContentManager content)
        {
            int R = 250;
            int n = 5;

            float angle = (float)(2 * Math.PI / n);


            List<Flower> flowers = CreateflowerList(n, center, R, content);
            int[,] array = new int[n, n];
            array[0, 2] = 1;
            array[0, 1] = 1;
            array[1, 2] = 1;
            array[2, 3] = 1;
            array[3, 4] = 1;
            array[4, 2] = 1;

            List<Fence> fences = CreateFenceList(flowers, array, content);
            return new GardenGraph(flowers, fences);
        }
        /// <summary>
        /// Graf Predefiniowany
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static GardenGraph GraphTwo(ContentManager content)
        {
            int R = 250;
            int n = 8;

            float angle = (float)(2 * Math.PI / n);


            List<Flower> flowers = CreateflowerList(n, center, R, content);
            int[,] array = new int[n, n];
            array[0, 1] = 1;
            array[1, 2] = 1;
            array[2, 3] = 1;
            array[3, 4] = 1;
            array[4, 5] = 1;
            array[5, 6] = 1;
            array[6, 7] = 1;
            array[7, 0] = 1;

            array[0, 4] = 1;
            array[1, 5] = 1;
            array[2, 6] = 1;
            array[3, 7] = 1;
            List<Fence> fences = CreateFenceList(flowers, array, content);
            return new GardenGraph(flowers, fences);
        }
        /// <summary>
        /// Graf Predefiniowany
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static GardenGraph GraphThree(ContentManager content)
        {
            int R = 250;
            int n = 6;

            float angle = (float)(2 * Math.PI / n);


            List<Flower> flowers = CreateflowerList(n, center,R,content);
            int[,] array = new int[n,n];
            array[0, 1] = 1;
            array[1, 2] = 1;
            array[2, 3] = 1;
            array[3, 4] = 1;
            array[4, 5] = 1;
            array[5, 0] = 1;

            array[5, 2] = 1;
            array[0, 3] = 1;
            array[1, 4] = 1;
            List<Fence> fences = CreateFenceList(flowers, array, content);

            return new GardenGraph(flowers, fences);
        }

        /// <summary>
        /// Graf Predefiniowany
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static GardenGraph GraphFour(ContentManager content)
        {           
            int R = 150;
            int n = 4;
            Texture2D fenceTexture = content.Load<Texture2D>("Plotek");
            Texture2D flowerTexture = content.Load<Texture2D>("Kwiatek");

            float angle = (float)(2 * Math.PI / n);

            List<Flower> flowers = new List<Flower>{
                                    new Flower(GetCoordinates(center, R, 0), "Kwiatek", 0),
                                    new Flower(GetCoordinates(center, R, angle), "Kwiatek", 1),
                                     new Flower(GetCoordinates(center, R, angle*2), "Kwiatek", 2),
                                      new Flower(GetCoordinates(center, R, angle*3), "Kwiatek", 3),
                                    };

            List<Fence> fences = new List<Fence> {
                new Fence(flowers[0],flowers[1], "Plotek"),
                new Fence(flowers[1],flowers[2], "Plotek"),
                new Fence(flowers[2],flowers[3], "Plotek"),
                new Fence(flowers[3],flowers[0], "Plotek"),
                };

            return new GardenGraph(flowers, fences);
        }

        /// <summary>
        /// Funkcja pomocnicza do tworzenia listy krawedzi 
        /// </summary>
        /// <param name="flow">lista wierzcholkow</param>
        /// <param name="array">tablica krawedzi, gdzie array[i,j] !=0 oznacza istnienie krawedzi i,j</param>
        /// <param name="content">menadzer zawartosci</param>
        /// <returns></returns>
        public static List<Fence> CreateFenceList(List<Flower> flow, int[,] array, ContentManager content)
        {
            List<Fence> fen = new List<Fence>();
            int n = array.GetLength(0);
            Texture2D fenceTexture = content.Load<Texture2D>("Plotek");
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    if (array[i, j] > 0)
                        fen.Add(new Fence(flow[i], flow[j], "Plotek"));
                }
            return fen;
        }

        /// <summary>
        /// Funkcja pomocnicza do tworzenia listy wierzcholkow
        /// </summary>
        /// <param name="n">liczba wierzcholkow</param>
        /// <param name="center">vector centrum polozenia grafu</param>
        /// <param name="R">promien kola na ktorym polozony jest graf</param>
        /// <param name="content">menadzer zawartosci</param>
        /// <returns></returns>
        public static List<Flower> CreateflowerList(int n, Vector2 center, int R, ContentManager content)
        {
            List<Flower> flowers = new List<Flower>();
            Texture2D flowerTexture = content.Load<Texture2D>("Kwiatek");
            float angle =(float)(2 * Math.PI / n);
            for (int i = 0; i < n;i++ )
            {
                Flower f = new Flower(GetCoordinates(center, R, i * angle), "Kwiatek", i);
                flowers.Add(f);
            }
            return flowers;
        }

        /// <summary>
        /// Funkcja pomocnicza do okreslenia polozenia nastepnego wierzcholka na kole
        /// </summary>
        /// <param name="Center">pozycja centrum kola</param>
        /// <param name="r">promien kola</param>
        /// <param name="angle">kat polozenia</param>
        /// <returns>zwraca vector polozenia wierzcholka</returns>
        public static Vector2 GetCoordinates(Vector2 Center, int r, float angle)
        {
              int  X =(int)( Center.X + (r * Math.Sin(angle)));
              int  Y =(int)( Center.Y + (r * Math.Cos(angle)));
              return new Vector2(X, Y);
        }
    }
}