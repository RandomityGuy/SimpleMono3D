using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SimpleMono3D.UI
{
    public class GuiGridPanel : GuiPanel
    {
        public Vector2 Padding = Vector2.Zero;

        public int Rows;
        public int Columns;

        public GuiGridPanel(Vector2 pos, Vector2 size) : base(pos, size)
        {

        }

        public override Vector2 GetPosition(GuiControl g)
        {
            var basepos = TotalDisplacement;
            var index = Children.IndexOf(g);

            var row = (int)Math.Ceiling((index + 1) / (float)Columns);
            var column = (index + 1) % Columns;

            var x = MathHelper.Lerp(Padding.X + basepos.X, basepos.X + Size.X - Padding.X, (float)column / (Columns));
            var y = MathHelper.Lerp(Padding.Y + basepos.Y, basepos.Y + Size.Y - Padding.Y, (float)(row - 1) / (Rows));

            return new Vector2(x, y);
        }


    }
}
