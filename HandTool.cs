using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using vswpf.BoardObject;

namespace vswpf
{
    class HandTool
    {
        private List<IBoardObject> boardObjects;

        public HandMode Mode { get; set; }

        // Hover
        [AllowNull]
        private IBoardObject hoveredObject = null;
        public bool Hovered 
        {
            get { return hoveredObject != null; }
        }

        // Move
        private bool movingObject = false;
        private int movingIndex = -1;
        private Point movingStart = new Point(-1, -1);
        [AllowNull]
        private IBoardObject movedObject = null;

        public HandTool(List<IBoardObject> boardObjects)
        {
            this.boardObjects = boardObjects;
        }

        public void Start(Point position)
        {
            checkHover(position);
            if (hoveredObject == null)
            {
                return;
            }

            if (Mode != HandMode.Move)
            {
                return;
            }

            movingStart = position;
            movingObject = true;
            movingIndex = boardObjects.IndexOf(hoveredObject);
            movedObject = hoveredObject.Clone();
            boardObjects.RemoveAt(movingIndex);
            boardObjects.Insert(movingIndex, movedObject);
        }

        public void Move(Point position)
        {
            if (movingObject)
            {
                movedObject = hoveredObject.Clone();
                movedObject.Offset(new Point(position.X - movingStart.X, position.Y - movingStart.Y));
                boardObjects.RemoveAt(movingIndex);
                boardObjects.Insert(movingIndex, movedObject);
                return;
            }

            checkHover(position);
        }
        private void checkHover(Point position)
        {
            bool selected = false;
            foreach (IBoardObject bo in boardObjects)
            {
                double dist = bo.MouseTest(position);
                if (dist < 2)
                {
                    selected = true;
                    hoveredObject = bo;
                    break;
                }
            }
            if (!selected)
            {
                hoveredObject = null;
            }
        }

        public bool Click(Point position)
        {
            if (!movingObject)
            {
                return false;
            }

            Reset();
            checkHover(position);
            return false;
        }

        public IBoardObject GetHoveredObject()
        {
            return hoveredObject;
        }

        public void Reset()
        {
            hoveredObject = null;
            movedObject = null;
            movingObject = false;
        }
    }

    enum HandMode
    {
        Move,
        Erase,
    }
}