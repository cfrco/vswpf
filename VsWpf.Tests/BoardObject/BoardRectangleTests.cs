using VsWpf.BoardObject;

namespace VsWpf.Tests.BoardObject
{
    public class BoardRectangleTests
    {
        [Fact]
        public void BoardRectangle_Offset()
        {
            BoardRectangle rect = new BoardRectangle()
            {
                LeftTop = new Point(5, 5),
                Width = 30,
                Height = 20,
            };

            rect.Offset(new Point(3, 4));

            Assert.Equal(new Point(8, 9), rect.LeftTop);
        }

        [Fact]
        public void BoardRectangle_CalculateRectangle()
        {
            Point point0 = new Point(100, 50);
            Point point1 = new Point(30, 40);
            
            Point leftTop;
            double width, height;
            BoardRectangle.CalculateRectangle(point0, point1, out leftTop, out width, out height);

            Assert.Equal(new Point(30, 40), leftTop);
            Assert.Equal(70, width);
            Assert.Equal(10, height);
        }

        [Fact]
        public void BoardRectangle_MouseTest()
        {
            BoardRectangle rect = new BoardRectangle()
            {
                LeftTop = new Point(0, 0),
                Width = 50,
                Height = 30,
            };

            bool test1 = rect.MouseTest(new Point(0, 0), 2);
            bool test2 = !rect.MouseTest(new Point(0, 50), 2);
            bool test3 = rect.MouseTest(new Point(50, 0), 2);
            bool test4 = !rect.MouseTest(new Point(0, 53), 2);
            bool test5 = !rect.MouseTest(new Point(25, 20), 2);
            bool test6 = rect.MouseTest(new Point(-2, 25), 2);
            bool test7 = !rect.MouseTest(new Point(-3, 25), 2);

            Assert.True(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.True(test4);
            Assert.True(test5);
            Assert.True(test6);
            Assert.True(test7);

            // Fill
            rect = new BoardRectangle()
            {
                LeftTop = new Point(0, 0),
                Width = 50,
                Height = 30,
                Thickness = 0,
            };

            test1 = rect.MouseTest(new Point(0, 0), 2);
            test2 = !rect.MouseTest(new Point(0, 50), 2);
            test3 = rect.MouseTest(new Point(50, 0), 2);
            test4 = !rect.MouseTest(new Point(0, 53), 2);
            test5 = rect.MouseTest(new Point(25, 20), 2);
            test6 = rect.MouseTest(new Point(-2, 25), 2);
            test7 = !rect.MouseTest(new Point(-3, 25), 2);

            Assert.True(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.True(test4);
            Assert.True(test5);
            Assert.True(test6);
            Assert.True(test7);
        }
    
        [Fact]
        public void BoardRectangle_Clone()
        {
            BoardRectangle rect = new BoardRectangle()
            {
                LeftTop = new Point(1, 2),
                Width = 3,
                Height = 4,
            };

            BoardRectangle? newRectangle = rect.Clone() as BoardRectangle;

            Assert.NotNull(newRectangle);
            Assert.Equal(rect.LeftTop, newRectangle.LeftTop);
            Assert.Equal(rect.Width, newRectangle.Width, 1E-6);
            Assert.Equal(rect.Height, newRectangle.Height, 1E-6);
            Assert.Equal(rect.Thickness, newRectangle.Thickness, 1E-6);
            Assert.Equal(rect.Color, newRectangle.Color);
        }
    }
}