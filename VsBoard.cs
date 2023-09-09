using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using vswpf.BoardObject;
using vswpf.Drawer;
using vswpf.RenderEngine;

namespace vswpf
{
    partial class VsBoard : Canvas
    {
        Point mouseMarker = new Point(-1, -1);

        IDrawer? drawer = null;
        HandTool handTool;
        List<IBoardObject> boardObjects = new List<IBoardObject>();

        BoardHistory history = new BoardHistory(100);
        private IBoardObject? mouseDownObject;
        private bool objectMoved = false;

        public event Action<VsBoard, IBoardObject?>? ObjectSelected;

        public VsBoard()
        {
            Background = Brushes.Gray;
            Cursor = Cursors.None;

            handTool = new HandTool(boardObjects);

            MouseDown += onMouseDown;
            MouseUp += onMouseUp;
            MouseMove += onMouseMove;
            MouseLeave += onMouseLeave;
        }

        public void SetDrawer(IDrawer? drawer)
        {
            this.drawer = drawer;
            handTool.Reset();
            ObjectSelected?.Invoke(this, null);
        }

        public void SetDrawerAttributes(double thickness, Color color)
        {
            if (drawer == null)
            {
                return;
            }
            drawer.SetAttributes(thickness, color);
        }

        public void SetHandMode(HandMode mode)
        {
            handTool.Mode = mode;
            handTool.Reset();
        }

        public void ClearObjects()
        {
            boardObjects.Clear();

            InvalidateVisual();
        }

        public IBoardObject[] GetObjects()
        {
            return boardObjects.ToArray();
        }

        public void SetObjects(IBoardObject[] objects)
        {
            boardObjects.Clear();
            foreach (IBoardObject bo in objects)
            {
                boardObjects.Add(bo);
            }
        }

        public void AddObject(IBoardObject? boardObject)
        {
            if (boardObject == null)
            {
                return;
            }

            history.Push(new BoardHistoryItem()
            {
                Index = boardObjects.Count,
                Action = BoardAction.Add,
                New = clone(boardObject),
            });
            boardObjects.Add(boardObject);
        }

        public void Undo()
        {
            BoardHistoryItem? item = history.Pop();
            if (item == null)
            {
                return;
            }

            if (item.Action == BoardAction.Add)
            {
                boardObjects.RemoveAt(item.Index);
            }
            else if (item.Action == BoardAction.Remove)
            {
                if (item.Original != null)
                {
                    boardObjects.Add(clone(item.Original));
                }
            }
            else if (item.Action == BoardAction.Modify)
            {
                if (item.Original != null)
                {
                    boardObjects.RemoveAt(item.Index);
                    boardObjects.Insert(item.Index, clone(item.Original));
                }
            }
        }

        public void Redo()
        {
            BoardHistoryItem? item = history.Forward();
            if (item == null)
            {
                return;
            }

            if (item.Action == BoardAction.Add)
            {
                if (item.New != null)
                {
                    boardObjects.Insert(item.Index, clone(item.New));
                }
            }
            else if (item.Action == BoardAction.Remove)
            {
                boardObjects.RemoveAt(item.Index);
            }
            else if (item.Action == BoardAction.Modify)
            {
                if (item.New != null)
                {
                    boardObjects.RemoveAt(item.Index);
                    boardObjects.Insert(item.Index, clone(item.New));
                }
            }
        }

        private IBoardObject clone(IBoardObject boardObject)
        {
            // When Clone BoardObject, reset it's selected status.
            IBoardObject newBoardObject = boardObject.Clone();
            newBoardObject.Selected = false;
            return newBoardObject;
        }

        private void onMouseDown(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(sender as UIElement);

            if (drawer != null)
            {
                drawer.Start(position);
            }
            else
            {
                handTool.Start(position);
                IBoardObject? boardObject = handTool.GetHoveredObject();
                if (boardObject != null)
                {
                    mouseDownObject = boardObject.Clone();
                }
            }

            InvalidateVisual();
        }

        private void onMouseUp(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(sender as UIElement);

            if (drawer != null)
            {
                if (drawer.Click(position))
                {
                    AddObject(drawer.GetBoardObject());
                }
            }
            else
            {
                handTool.Click(position);
                if (handTool.Mode == HandMode.Erase)
                {
                    IBoardObject? boardObject = handTool.GetHoveredObject();
                    if (boardObject != null)
                    {
                        int index = boardObjects.IndexOf(boardObject);
                        history.Push(new BoardHistoryItem()
                        {
                            Index = index,
                            Action = BoardAction.Remove,
                            Original = clone(boardObject),
                        });
                        boardObjects.RemoveAt(index);
                        handTool.Click(position);
                    }
                }
                else
                {
                    IBoardObject? boardObject = handTool.GetHoveredObject();
                    ObjectSelected?.Invoke(this, boardObject);

                    if (objectMoved && mouseDownObject != null && boardObject != null)
                    {
                        history.Push(new BoardHistoryItem()
                        {
                            Index = boardObjects.IndexOf(boardObject),
                            Action = BoardAction.Modify,
                            Original = clone(mouseDownObject),
                            New = clone(boardObject),
                        });
                        mouseDownObject = null;
                        objectMoved = false;
                    }
                }
            }

            InvalidateVisual();
        }

        private void onMouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(sender as UIElement);
            mouseMarker.X = position.X;
            mouseMarker.Y = position.Y;

            if (drawer != null)
            {
                drawer.Move(position);
            }
            else if (drawer == null)
            {
                if (handTool.Move(position))
                {
                    objectMoved = true;
                }
            }
            
            InvalidateVisual();
        }

        private void onMouseLeave(object sender, MouseEventArgs e)
        {
            mouseMarker = new Point(-1, -1);

            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.PushClip(new RectangleGeometry(new Rect(0, 0, ActualWidth, ActualHeight)));

            IRenderEngine engine = new NativeDrawingRenderEngine(dc);
            foreach (IBoardObject obj in boardObjects)
            {
                if (obj == null)
                {
                    continue;
                }
                obj.Render(engine);
            }

            if (drawer != null && drawer.Started)
            {
                drawer.Render(engine);
            }
            renderMouse(engine);
        }
        private void renderMouse(IRenderEngine engine)
        {
            if (mouseMarker.X < 0 || mouseMarker.Y < 0)
            {
                return;
            }

            if (handTool.Hovered)
            {
                if (handTool.Mode == HandMode.Erase)
                {
                    Crosshairs.Render(engine, mouseMarker, 14, Brushes.Red);
                }
                else
                {
                    Crosshairs.RenderMove(engine, mouseMarker, 14, Brushes.Black);
                }
            }
            else
            {
                Crosshairs.Render(engine, mouseMarker, 14, Brushes.Black);
            }
        }
    }
}
