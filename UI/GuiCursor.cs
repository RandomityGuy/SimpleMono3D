using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleMono3D.Graphics.Materials;

namespace SimpleMono3D.UI
{
    public class GuiCursor : GuiControl
    {
        public new int Layer = -1;
        
        public GuiCursor()
        {
            RegisterBindings();
        }

        public override void OnMouseMove(object sender, GameTime dt, MouseState mb,bool inside)
        {
            Position = mb.Position.ToVector2();
        }

        public override void Render(SpriteBatch sb)
        {

            SetupClipping(sb);
            sb.Draw(ColorMaterials.White.Texture, new Rectangle(Position.ToPoint(),new Point(10,10)), Color.White);

        }
    }
}
