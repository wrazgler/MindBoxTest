using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SquareLibrary.AreaUnitTest
{
    [TestClass]
    public class TriangleAreaTest
    {
        [TestMethod]
        public void Triangle_Area_2_5_8_Return_30()
        {
            var triangle = new Triangle(5, 12, 13);

            double expected = 30;

            var result = triangle.Area();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Triangle_IsTriangle_3_6_9_Return_True()
        {
            var triangle = new Triangle(3, 6, 8);

            var result = triangle.IsTriangle();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Triangle_IsRightTriangle_6_8_10_Return_True()
        {
            var triangle = new Triangle(6, 8, 10);

            var result = triangle.IsRightTriangle();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Triangle_IsRightTriangle_5_7_11_Return_False()
        {
            var triangle = new Triangle(5, 7, 11);

            var result = triangle.IsRightTriangle();

            Assert.IsFalse(result);
        }
    }
}
