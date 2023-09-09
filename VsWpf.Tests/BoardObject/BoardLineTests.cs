using VsWpf.BoardObject;

namespace VsWpf.Tests.BoardObject
{
    public class BoardLineTests
    {
        [Fact]
        public void BoardLine_Offset()
        {
            BoardLine line = new BoardLine()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(10, 0),
            };

            line.Offset(new Point(5, 5));

            Assert.Equal(new Point(5, 5), line.Point0);
            Assert.Equal(new Point(15, 5), line.Point1);
        }

        [Fact]
        public void BoardLine_MouseTest()
        {
            BoardLine line = new BoardLine()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(10, 0),
            };

            bool test1 = line.MouseTest(new Point(0, 0), 2);
            bool test2 = line.MouseTest(new Point(5, 2), 2);
            bool test3 = line.MouseTest(new Point(6, -1), 2);
            bool test4 = !line.MouseTest(new Point(8, 3), 2);
            bool test5 = line.MouseTest(new Point(12, 0), 2);
            bool test6 = !line.MouseTest(new Point(13, 0), 2);

            Assert.True(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.True(test4);
            Assert.True(test5);
            Assert.True(test6);

            line = new BoardLine()
            {
                Point0 = new Point(0, 0),
                Point1 = new Point(10, 0),
                Thickness = 0,
            };

            test1 = line.MouseTest(new Point(0, 0), 2);
            test2 = line.MouseTest(new Point(5, 2), 2);
            test3 = line.MouseTest(new Point(6, -1), 2);
            test4 = !line.MouseTest(new Point(8, 3), 2);
            test5 = line.MouseTest(new Point(12, 0), 2);
            test6 = !line.MouseTest(new Point(13, 0), 2);

            Assert.True(test1);
            Assert.True(test2);
            Assert.True(test3);
            Assert.True(test4);
            Assert.True(test5);
            Assert.True(test6);
        }

        [Fact]
        public void BoardLine_Clone()
        {
            BoardLine line = new BoardLine()
            {
                Point0 = new Point(1, 2),
                Point1 = new Point(3, 4),
            };

            BoardLine? newLine = line.Clone() as BoardLine;

            Assert.NotNull(newLine);
            Assert.Equal(line.Point0, newLine.Point0);
            Assert.Equal(line.Point1, newLine.Point1);
            Assert.Equal(line.Thickness, newLine.Thickness, 1E-6);
            Assert.Equal(line.Color, newLine.Color);
        }
    }
}