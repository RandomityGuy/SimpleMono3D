using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleMono3D.UI
{
    public class GuiPanel : GuiRectangle
    {

        public GuiPanel(Vector2 pos, Vector2 size) : base(pos, size)
        {

        }

        public override void Render(SpriteBatch sb)
        {
            base.Render(sb);
        }
    }
}
