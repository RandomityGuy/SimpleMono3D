using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMono3D.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SimpleMono3D.UI
{
    public class GuiScrollBar : GuiButton
    {
        public Direction Direction = Direction.Vertical;

        public float ScrollPosition;

        public float ScrollPercentage
        {
            get
            {
                if (Direction == Direction.Vertical)
                    return (ScrollPosition) / (Parent.Size.Y - Size.Y);
                else
                    return (ScrollPosition) / (Parent.Size.X - Size.X);
            }
        }

        bool scrolling = false;

        MouseState scrollbegin;

        public GuiScrollBar(Vector2 pos, Vector2 size) : base(pos,size,null)
        {

        }

        public override void OnMouseMove(object sender, GameTime dt, MouseState mb,bool inside)
        {
            if (scrolling)
            {
                var delta = mb.Position - scrollbegin.Position;
                switch (Direction)
                {
                    case Direction.Vertical:
                        var futureY = Position.Y + delta.Y;
                        if (futureY > Parent.Size.Y - Size.Y)
                        {
                            Position.Y = Parent.Size.Y - Size.Y;
                        }
                        else if (futureY < 0)
                        {
                            Position.Y = 0;
                        }
                        else
                        {
                            Position.Y = futureY;
                        }
                        ScrollPosition = Position.Y;
                        break;

                    case Direction.Horizontal:
                        var futureX = Position.X + delta.X;
                        if (futureX > Parent.Size.X - Size.X)
                        {
                            Position.X = Parent.Size.X - Size.X;
                        }
                        else if (futureX < 0)
                        {
                            Position.X = 0;
                        }
                        else
                        {
                            Position.X = futureX;
                        }
                        ScrollPosition = Position.X;
                        break;

                        //case Direction.Vertical:
                        //    if (TotalDisplacement.Y + delta.Y + Size.Y > Parent.TotalDisplacement.Y + Parent.Size.Y)
                        //        delta.Y = (int)(TotalDisplacement.Y + delta.Y + Size.Y - Parent.TotalDisplacement.Y + Parent.Size.Y);
                        //    Position.Y += delta.Y;
                        //    ScrollPosition += delta.Y;
                        //    break;

                        //case Direction.Horizontal:
                        //    if (TotalDisplacement.X + delta.X + Size.X > Parent.TotalDisplacement.X + Parent.Size.X)
                        //        delta.X = (int)(TotalDisplacement.X + delta.X + Size.X - Parent.TotalDisplacement.X + Parent.Size.X);
                        //    Position.X += delta.X;
                        //    ScrollPosition += delta.X;
                        //    break;
                }
                scrollbegin = mb;
            }
        }

        public override void OnMouseLeftButtonPressed(object sender, GameTime dt, MouseState mb,bool inside)
        {
            if (inside)
            {
                base.OnMouseLeftButtonPressed(sender, dt, mb, inside);
                scrollbegin = mb;
                scrolling = true;
            }
        }

        public override void OnMouseLeftButtonReleased(object sender, GameTime dt, MouseState mb,bool inside)
        {
            base.OnMouseLeftButtonReleased(sender, dt, mb,inside);
            scrolling = false;
        }


    }
}
