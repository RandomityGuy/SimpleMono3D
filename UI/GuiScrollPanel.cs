using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.UI
{
    public class GuiScrollPanel : GuiPanel
    {
        GuiScrollBar scrollbarX;
        GuiScrollBar scrollbarY;

        GuiPanel BasePanel;

        Direction direction;

        public GuiScrollPanel(Vector2 pos, Vector2 size,GuiPanel basePanel,Direction scrolldir) : base(pos, size)
        {
            BasePanel = basePanel;
            direction = scrolldir;
            base.AddChild(BasePanel);
            if (scrolldir == Direction.Vertical)
            {
                scrollbarY = new GuiScrollBar(new Vector2(size.X - 10, 0), new Vector2(10, 40)) { Direction = Direction.Vertical };
                scrollbarY.Layer = BasePanel.Layer + 1;
                base.AddChild(scrollbarY);
            }
            if (scrolldir == Direction.Horizontal)
            {
                scrollbarX = new GuiScrollBar(new Vector2(0, size.Y - 10), new Vector2(40, 10)) { Direction = Direction.Horizontal };
                scrollbarX.Layer = BasePanel.Layer + 1;
                base.AddChild(scrollbarX);
            }
            if (scrolldir == Direction.Both)
            {
                scrollbarX = new GuiScrollBar(new Vector2(0, size.Y - 10), new Vector2(40, 10)) { Direction = Direction.Horizontal };
                scrollbarX.Layer = BasePanel.Layer + 1;

                scrollbarY = new GuiScrollBar(new Vector2(size.X - 10, 0), new Vector2(10, 40)) { Direction = Direction.Vertical };
                scrollbarY.Layer = BasePanel.Layer + 1;

                base.AddChild(scrollbarX);
                base.AddChild(scrollbarY);
            }
        }

        public override void AddChild(GuiControl c)
        {
            BasePanel.AddChild(c);
        }

        public override Vector2 GetPosition(GuiControl g)
        {
            if (g.GetType() == typeof(GuiScrollBar))
                return TotalDisplacement + g.Position;

            var basepanel = BasePanel;
            var basepos = TotalDisplacement;

            if (direction == Direction.Vertical)
            {
                var basespace = basepanel.Size.Y;
                if (basespace < Size.Y)
                    return basepos;
                else
                {
                    return basepos - new Vector2(0, MathHelper.Lerp(0, basepanel.Size.Y - Size.Y, scrollbarY.ScrollPercentage));
                }
            }

            if (direction == Direction.Horizontal)
            {
                var basespace = basepanel.Size.X;
                if (basespace < Size.X)
                    return basepos;
                else
                {
                    return basepos - new Vector2(MathHelper.Lerp(0, basepanel.Size.X - Size.X, scrollbarX.ScrollPercentage),0);
                }
            }

            if (direction == Direction.Both)
            {
                float xpos, ypos;

                var basespacex = basepanel.Size.X;
                if (basespacex < Size.X)
                    xpos = basepos.X;
                else
                {
                    xpos = basepos.X - MathHelper.Lerp(0, basepanel.Size.X - Size.X, scrollbarX.ScrollPercentage);
                }

                var basespacey = basepanel.Size.Y;
                if (basespacey < Size.Y)
                    ypos = basepos.Y;
                else
                {
                    ypos = basepos.Y - MathHelper.Lerp(0, basepanel.Size.Y - Size.Y, scrollbarY.ScrollPercentage);
                }

                return new Vector2(xpos, ypos);
            }

            if (direction == Direction.Vertical)
            {
                return TotalDisplacement + g.Position - new Vector2(0,scrollbarY.ScrollPosition);
            }
            if (direction == Direction.Horizontal)
            {
                return TotalDisplacement + g.Position - new Vector2(scrollbarX.ScrollPosition,0);
            }
            return TotalDisplacement + g.Position;
        }

        public override void RemoveChild(GuiControl c)
        {
            BasePanel.RemoveChild(c);
        }
    }
}
