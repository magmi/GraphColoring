using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphColoring;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TestyJednostkowe
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FenceContainsTest1()
        {
            Flower f1 = new Flower(new Vector2(746, 97), 2);
            Flower f2 = new Flower(new Vector2(453, 97), 3);
            Fence f = new Fence(f1, f2);
            Point mp = new Point(651, 160);
            bool result = f.ContainsPoint(mp);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void FenceContainsTest2()
        {
            Flower f1 = new Flower(new Vector2(746, 97), 2);
            Flower f2 = new Flower(new Vector2(453, 97), 3);
            Fence f = new Fence(f1, f2);
            Point mp = new Point(880, 160);
            bool result = f.ContainsPoint(mp);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void FlowerContainsTest1()
        {
            Flower f = new Flower(new Vector2(0, 0), 0);            
            Point mp = new Point(5, 5);
            bool result = f.ContainsPoint(mp);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void FlowerContainsTest2()
        {
            Flower f = new Flower(new Vector2(0, 0), 0);
            Point mp = new Point(250, 250);
            bool result = f.ContainsPoint(mp);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void FenceAngleTest()
        {
            Flower f1 = new Flower(new Vector2(0, 0), 0);
            Flower f2 = new Flower(new Vector2(600, 600), 1);
            Fence f = new Fence(f1, f2);
            double angle = f.GetAngleOfLineBetweenTwoPoints(f1.position, f2.position);
            double expected = Math.PI / 4; 
            Assert.AreEqual(expected, angle);
        }

        [TestMethod]
        public void EndGameTest1()
        {
            List<Flower> flowers = new List<Flower>()
            {
                new Flower(new Vector2(0, 0), 0),
                new Flower(new Vector2(500, 300), 1),
                new Flower(new Vector2(300, 150), 2),
            };
            List<Fence> fences = new List<Fence>()
            {
                new Fence(flowers[0],flowers[1]),
                new Fence(flowers[1],flowers[2]),
                new Fence(flowers[2],flowers[0]),
            };
            GardenGraph gg = new GardenGraph(flowers, fences);
            GraphColoring.Game game = new GraphColoring.Game(GameType.EdgesColoring, gg, 0);
            bool result;
            game.CheckIfEnd(out result);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void EndGameTest2()
        {
            List<Flower> flowers = new List<Flower>()
            {
                new Flower(new Vector2(0, 0), 0),
                new Flower(new Vector2(500, 300), 1),
                new Flower(new Vector2(300, 150), 2),
            };
            List<Fence> fences = new List<Fence>()
            {
                new Fence(flowers[0],flowers[1]),
                new Fence(flowers[1],flowers[2]),
                new Fence(flowers[2],flowers[0]),
            };
            GardenGraph gg = new GardenGraph(flowers, fences);
            GraphColoring.Game game = new GraphColoring.Game(GameType.EdgesColoring, gg, 0);
            bool result;
            bool res;
            result = game.CheckIfEnd(out res);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void EndGameTest3()
        {
            List<Flower> flowers = new List<Flower>()
            {
                new Flower(new Vector2(0, 0), 0),
                new Flower(new Vector2(500, 300), 1),
                new Flower(new Vector2(300, 150), 2),
            };
            List<Fence> fences = new List<Fence>()
            {
                new Fence(flowers[0],flowers[1]),
                new Fence(flowers[1],flowers[2]),
                new Fence(flowers[2],flowers[0]),
            };
            GardenGraph gg = new GardenGraph(flowers, fences);
            
            GraphColoring.Game game = new GraphColoring.Game(GameType.VerticesColoring, gg, 1);
            gg.MakeMove(flowers[0], game.colors[0], game);
            bool result;
            bool res;
            result = game.CheckIfEnd(out res);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void EndGameTest4()
        {
            List<Flower> flowers = new List<Flower>()
            {
                new Flower(new Vector2(0, 0), 0),
                new Flower(new Vector2(500, 300), 1),
                new Flower(new Vector2(300, 150), 2),
            };
            List<Fence> fences = new List<Fence>()
            {
                new Fence(flowers[0],flowers[1]),
                new Fence(flowers[1],flowers[2]),
                new Fence(flowers[2],flowers[0]),
            };
            GardenGraph gg = new GardenGraph(flowers, fences);

            GraphColoring.Game game = new GraphColoring.Game(GameType.VerticesColoring, gg, 1);
            gg.MakeMove(flowers[0], game.colors[0], game);
            bool result;
            bool res;
            result = game.CheckIfEnd(out res);
            Assert.AreEqual(false, res);
        }

        [TestMethod]
        public void EndGameTest5()
        {
            List<Flower> flowers = new List<Flower>()
            {
                new Flower(new Vector2(0, 0), 0),
                new Flower(new Vector2(500, 300), 1),
                new Flower(new Vector2(300, 150), 2),
            };
            List<Fence> fences = new List<Fence>()
            {
                new Fence(flowers[0],flowers[1]),
                new Fence(flowers[1],flowers[2]),
                new Fence(flowers[2],flowers[0]),
            };
            GardenGraph gg = new GardenGraph(flowers, fences);

            GraphColoring.Game game = new GraphColoring.Game(GameType.VerticesColoring, gg, 3);
            gg.MakeMove(flowers[0], game.colors[0], game);
            gg.MakeMove(flowers[1], game.colors[1], game);
            gg.MakeMove(flowers[2], game.colors[2], game);
            bool result;
            bool res;
            result = game.CheckIfEnd(out res);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void EndGameTest6()
        {
            List<Flower> flowers = new List<Flower>()
            {
                new Flower(new Vector2(0, 0), 0),
                new Flower(new Vector2(500, 300), 1),
                new Flower(new Vector2(300, 150), 2),
            };
            List<Fence> fences = new List<Fence>()
            {
                new Fence(flowers[0],flowers[1]),
                new Fence(flowers[1],flowers[2]),
                new Fence(flowers[2],flowers[0]),
            };
            GardenGraph gg = new GardenGraph(flowers, fences);

            GraphColoring.Game game = new GraphColoring.Game(GameType.VerticesColoring, gg, 3);
            gg.MakeMove(flowers[0], game.colors[0], game);
            gg.MakeMove(flowers[1], game.colors[1], game);
            gg.MakeMove(flowers[2], game.colors[2], game);
            bool result;
            bool res;
            result = game.CheckIfEnd(out res);
            Assert.AreEqual(true, res);
        }

        [TestMethod]
        public void ValidMoveTest1()
        {
            List<Flower> flowers = new List<Flower>()
            {
                new Flower(new Vector2(0, 0), 0),
                new Flower(new Vector2(500, 300), 1),
                new Flower(new Vector2(300, 150), 2),
            };
            List<Fence> fences = new List<Fence>()
            {
                new Fence(flowers[0],flowers[1]),
                new Fence(flowers[1],flowers[2]),
                new Fence(flowers[2],flowers[0]),
            };
            GardenGraph gg = new GardenGraph(flowers, fences);
            GraphColoring.Game game = new GraphColoring.Game(GameType.EdgesColoring, gg, 0);
            bool result = game.CheckIfValidMove(fences[0], Color.Red);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ValidMoveTest2()
        {
            List<Flower> flowers = new List<Flower>()
            {
                new Flower(new Vector2(0, 0), 0),
                new Flower(new Vector2(500, 300), 1),
            };
            List<Fence> fences = new List<Fence>()
            {
                new Fence(flowers[0],flowers[1]),
            };
            GardenGraph gg = new GardenGraph(flowers, fences);
            GraphColoring.Game game = new GraphColoring.Game(GameType.EdgesColoring, gg, 0);
            flowers[0].color = Color.Red;
            bool result = game.CheckIfValidMove(flowers[1], Color.Red);
            Assert.AreEqual(false, result);
        }
    }
}
