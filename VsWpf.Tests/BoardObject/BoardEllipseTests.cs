using VsWpf.BoardObject;

namespace VsWpf.Tests.BoardObject
{
    public class BoardEllipseTests
    {
        [Fact]
        public void BoardEllipse_Offset()
        {
            BoardEllipse ellipse = new BoardEllipse()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(5, 10),
            };

            ellipse.Offset(new Point(5, 5));

            Assert.Equal(new Point(5, 5), ellipse.Point0);
            Assert.Equal(new Point(10, 15), ellipse.Point1);
        }

        [Fact]
        public void BoardEllipse_CalculateEllipse()
        {
            BoardEllipse ellipse = new BoardEllipse()
            {
                Point0 = new Point(5, 10),
                Point1 = new Point(10, 20),
            };

            double x, y, rx, ry;
            BoardEllipse.CalculateEllipse(ellipse.Point0, ellipse.Point1, out x, out y, out rx, out ry);
            
            Assert.Equal(7.5, x, 1E-6);
            Assert.Equal(15, y, 1E-6);
            Assert.Equal(2.5, rx, 1E-6);
            Assert.Equal(5, ry, 1E-6);
        }

        [Fact]
        public void BoardEllipse_MouseTest()
        {
            BoardEllipse ellipse = new BoardEllipse()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(50, 100),
            };

            bool test1 = !ellipse.MouseTest(new Point(0, 0), 2);
            bool test2 = ellipse.MouseTest(new Point(0, 50), 2);
            bool test3 = !ellipse.MouseTest(new Point(25, 50), 2);
            bool test4 = ellipse.MouseTest(new Point(25, 100), 2);

            Assert.True(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.True(test4);

            // Fill
            ellipse = new BoardEllipse()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(50, 100),
                Thickness = -1,
            };

            test1 = !ellipse.MouseTest(new Point(0, 0), 2);
            test2 = ellipse.MouseTest(new Point(0, 50), 2);
            test3 = ellipse.MouseTest(new Point(25, 50), 2);
            test4 = ellipse.MouseTest(new Point(25, 100), 2);

            Assert.True(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.True(test4);
        }
    
        [Fact]
        public void BoardEllipse_Clone()
        {
            BoardEllipse ellipse = new BoardEllipse()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(5, 10),
            };

            BoardEllipse? newEllipse = ellipse.Clone() as BoardEllipse;

            Assert.NotNull(newEllipse);
            Assert.Equal(ellipse.Point0, newEllipse.Point0);
            Assert.Equal(ellipse.Point0, newEllipse.Point0);
            Assert.Equal(ellipse.Thickness, newEllipse.Thickness, 1E-6);
            Assert.Equal(ellipse.Color, newEllipse.Color);
        }
    }
}