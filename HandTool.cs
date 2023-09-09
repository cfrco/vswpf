using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using vswpf.BoardObject;

namespace vswpf
{
    enum HandMode
    {
        Move,
        Erase,
    }

    class HandTool
    {
        private List<IBoardObject> boardObjects;

        public HandMode Mode { get; set; }

        // Hover
        private IBoardObject? hoveredObject = null;
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

            selectToHovered();

            movingStart = position;
            movingObject = true;
            movingIndex = boardObjects.IndexOf(hoveredObject);
            movedObject = hoveredObject.Clone();
            boardObjects.RemoveAt(movingIndex);
            boardObjects.Insert(movingIndex, movedObject);
        }

        public bool Move(Point position)
        {
            if (movingObject && hoveredObject != null)
            {
                movedObject = hoveredObject.Clone();
                movedObject.Offset(new Point(position.X - movingStart.X, position.Y - movingStart.Y));
                boardObjects.RemoveAt(movingIndex);
                boardObjects.Insert(movingIndex, movedObject);
                return true;
            }

            checkHover(position);
            return false;
        }
        
        private void checkHover(Point position)
        {
            bool selected = false;
            foreach (IBoardObject bo in boardObjects)
            {
                if (bo.MouseTest(position, 2))
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

        private void selectToHovered()
        {
            boardObjects.ForEach(bo => { bo.Selected = false; });
            if (hoveredObject != null)
            {
                hoveredObject.Selected = true;
            }
        }

        public bool Click(Point position)
        {
            Reset();
            checkHover(position);
            selectToHovered();
            return false;
        }

        public IBoardObject? GetHoveredObject()
        {
            return hoveredObject;
        }

        public void Reset()
        {
            hoveredObject = null;
            movedObject = null;
            movingObject = false;
            selectToHovered();
        }
    }
}