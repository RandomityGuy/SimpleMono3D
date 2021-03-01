using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.UI
{
    public class GuiAlignmentPanel : GuiPanel
    {

        public GuiAlignmentPanel(Vector2 pos,Vector2 size) : base(pos,size)
        {

        }

        public override Vector2 GetPosition(GuiControl g)
        {
            var pos = TotalDisplacement;

            var alignment = (Alignment)g.LocationDescription.GetDescription();

            switch (alignment)
            {
                case Alignment.BottomLeft:
                    return (pos + new Vector2(0, Size.Y - g.Size.Y)) + new Vector2(g.Position.X,-g.Position.Y);

                case Alignment.BottomMiddle:
                    return (pos + new Vector2((Size.X / 2) - (g.Size.X/2), Size.Y - g.Size.Y)) + new Vector2(g.Position.X, -g.Position.Y);

                case Alignment.BottomRight:
                    return (pos + new Vector2(Size.X - g.Size.X, Size.Y - g.Size.Y)) + new Vector2(-g.Position.X, -g.Position.Y);

                case Alignment.Left:
                    return (pos + new Vector2(0, (Size.Y / 2) - (g.Size.Y/2))) + g.Position;

                case Alignment.Middle:
                    return (pos + new Vector2((Size.X / 2) - (g.Size.X /2), (Size.Y/2) - (g.Size.Y/2))) + g.Position;

                case Alignment.Right:
                    return (pos + new Vector2(Size.X - g.Size.X, Size.Y / 2 - (g.Size.Y / 2))) + new Vector2(-g.Position.X, g.Position.Y);

                case Alignment.TopLeft:
                    return pos + new Vector2(g.Position.X, g.Position.Y);

                case Alignment.TopMiddle:
                    return (pos + new Vector2((Size.X / 2) - (g.Size.X/2), 0)) + new Vector2(g.Position.X, g.Position.Y);

                case Alignment.TopRight:
                    return (pos + new Vector2(Size.X - g.Size.X,0)) + new Vector2(-g.Position.X,g.Position.Y);

                default:
                    return pos;
            }
        }
    }
}
