using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMono3D.Graphics;
using SimpleMono3D.UI;
using Microsoft.Xna.Framework;

namespace SimpleMono3D.UI
{
    public enum Direction
    {
        Horizontal,
        Vertical,
        Both
    }

    public class GuiStackPanel : GuiPanel
    {
        public Vector2 Padding = Vector2.Zero;

        public Direction Direction = Direction.Vertical;

        public GuiStackPanel(Vector2 pos, Vector2 size) : base(pos, size)
        {

        }

        public override Vector2 GetPosition(GuiControl g)
        {
            var basepos = TotalDisplacement;

            var i = 0;
            var curctrl = Children[i];
            while (g != curctrl && i < Children.Count)
            {
                if (Direction == Direction.Vertical)
                    basepos.Y += Children[i].Size.Y;
                else
                    basepos.X += Children[i].Size.X;
                i++;
                curctrl = Children[i];
            }
            if (Direction == Direction.Vertical)
                return basepos + new Vector2(Padding.X,i * Padding.Y);
            else
                return basepos + new Vector2(i * Padding.X, Padding.Y);
        }
    }
}
