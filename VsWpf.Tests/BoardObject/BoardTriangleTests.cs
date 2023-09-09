using VsWpf.BoardObject;

namespace VsWpf.Tests.BoardObject
{
    public class BoardTriangleTests
    {
        [Fact]
        public void BoardTriangle_Offset()
        {
            BoardTriangle triangle = new BoardTriangle()
            {
                Point0 = new Point(1, 2),
                Point1 = new Point(1, 5),
                Point2 = new Point(8, 5),
            };

            triangle.Offset(new Point(3, 4));

            Assert.Equal(new Point(4, 6), triangle.Point0);
            Assert.Equal(new Point(4, 9), triangle.Point1);
            Assert.Equal(new Point(11, 9), triangle.Point2);
        }

        [Fact]
        public void BoardTriangle_MouseTest()
        {
            BoardTriangle triangle = new BoardTriangle()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(30, 0),
                Point2 = new Point(0, 40),
            };

            bool test1 = triangle.MouseTest(new Point(0, 0), 2);
            bool test2 = triangle.MouseTest(new Point(15, 0), 2);
            bool test3 = triangle.MouseTest(new Point(15, -2), 2);
            bool test4 = !triangle.MouseTest(new Point(15, 3), 2);
            bool test5 = triangle.MouseTest(new Point(0, 20), 2);
            bool test6 = !triangle.MouseTest(new Point(33, 0), 2);
            bool test7 = triangle.MouseTest(new Point(0, 41), 2);
            bool test8 = triangle.MouseTest(new Point(27, 4), 2);
            bool test9 = triangle.MouseTest(new Point(27, 7), 2);
            bool test10 = !triangle.MouseTest(new Point(27, 9), 2);

            Assert.True(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.True(test4);
            Assert.True(test5);
            Assert.True(test6);
            Assert.True(test7);
            Assert.True(test8);
            Assert.True(test9);
            Assert.True(test10);

            triangle = new BoardTriangle()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(30, 0),
                Point2 = new Point(0, 40),
                Thickness = 0,
            };

            test1 = triangle.MouseTest(new Point(0, 0), 2);
            test2 = triangle.MouseTest(new Point(15, 0), 2);
            test3 = triangle.MouseTest(new Point(15, -2), 2);
            test4 = triangle.MouseTest(new Point(15, 3), 2);
            test5 = triangle.MouseTest(new Point(0, 20), 2);
            test6 = !triangle.MouseTest(new Point(33, 0), 2);
            test7 = triangle.MouseTest(new Point(0, 41), 2);
            test8 = triangle.MouseTest(new Point(27, 4), 2);
            test9 = triangle.MouseTest(new Point(27, 7), 2);
            test10 = !triangle.MouseTest(new Point(27, 9), 2);

            Assert.True(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.True(test4);
            Assert.True(test5);
            Assert.True(test6);
            Assert.True(test7);
            Assert.True(test8);
            Assert.True(test9);
            Assert.True(test10);
        }
    
        [Fact]
        public void BoardTriangle_Clone()
        {
            BoardTriangle triangle = new BoardTriangle()
            {
                Point0 = new Point(1, 2),
                Point1 = new Point(1, 3),
                Point2 = new Point(4, 5),
            };

            BoardTriangle? newTriangle = triangle.Clone() as BoardTriangle;

            Assert.NotNull(newTriangle);
            Assert.Equal(triangle.Point0, newTriangle.Point0);
            Assert.Equal(triangle.Point1, newTriangle.Point1);
            Assert.Equal(triangle.Point2, newTriangle.Point2);
            Assert.Equal(triangle.Thickness, newTriangle.Thickness, 1E-6);
            Assert.Equal(triangle.Color, newTriangle.Color);
        }
    }
}