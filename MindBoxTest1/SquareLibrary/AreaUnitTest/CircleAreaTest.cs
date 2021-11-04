using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SquareLibrary.AreaUnitTest
{
    [TestClass]
    public class CircleAreaTest
    {
        [TestMethod]
        public void Circle_Area_7_Return_153_94()
        {
            var circle = new Circle(7);

            double expected = 153.94;

            var result = circle.Area();

            Assert.AreEqual(expected, result);
        }
    }
}
