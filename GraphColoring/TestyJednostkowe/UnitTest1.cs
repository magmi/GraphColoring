using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphColoring;
using Microsoft.Xna.Framework;
namespace TestyJednostkowe
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FenceContainsTest()
        {
            Flower f1 = new Flower(new Vector2(0,0),0);
            Flower f2 = new Flower(new Vector2(100,100),1);
            Fence f = new Fence(f1, f2);
            Point mp = new Point(50,50);
            bool result = f.ContainsPoint(mp);

            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void FlowerContainsTest()
        {

            Flower f = new Flower(new Vector2(0, 0), 0);            
            Point mp = new Point(5, 5);
            bool result = f.ContainsPoint(mp);

            Assert.AreEqual(true, result);
        }
    }
}
