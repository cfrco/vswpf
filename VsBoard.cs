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

        IDrawer drawer = null;
        HandTool handTool;
        List<IBoardObject> boardObjects = new List<IBoardObject>();

        public event Action<VsBoard, IBoardObject> ObjectSelected;

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

        public void SetDrawer(IDrawer drawer)
        {
            this.drawer = drawer;
            handTool.Reset();
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
                    IBoardObject boardObject = drawer.GetBoardObject();
                    if (boardObject != null)
                    {
                        boardObjects.Add(boardObject);
                    }
                }
            }
            else
            {
                handTool.Click(position);
                if (handTool.Mode == HandMode.Erase)
                {
                    IBoardObject boardObject = handTool.GetHoveredObject();
                    if (boardObject != null)
                    {
                        boardObjects.Remove(boardObject);
                        handTool.Click(position);
                    }
                }
                else
                {
                    IBoardObject boardObject = handTool.GetHoveredObject();
                    ObjectSelected?.Invoke(this, boardObject);
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
                handTool.Move(position);
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
