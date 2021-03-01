using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SimpleMono3D.Graphics.Materials;

namespace SimpleMono3D.UI
{
    public class GuiRectangle : GuiControl
    {
        public Texture2D Texture = ColorMaterials.Gray.Texture;

        public Texture2D BorderTexture = ColorMaterials.Gray.Texture;

        public Color Color = Color.Gray;

        public Color BorderColor = Color.Black;

        public int BorderSize = 2;

        public GuiRectangle(Vector2 position,Vector2 size) : base()
        {
            Position = position;
            Size = size;
        }

        public GuiRectangle(Vector2 position, Vector2 size,Texture2D texture) : base()
        {
            Position = position;
            Size = size;
            Texture = texture;
        }

        public override void Render(SpriteBatch sb)
        {

            SetupClipping(sb);
            var rect = new Rectangle(TotalDisplacement.ToPoint(), Size.ToPoint());

            //Draw Rect
            sb.Draw(Texture, rect, Parent?.Rectangle, Color);

            RenderChildren(sb);

            SetupClipping(sb);

            //Draw Border
            //Top
            sb.Draw(BorderTexture, new Rectangle(rect.Location, new Vector2(Size.X, BorderSize).ToPoint()), BorderColor);
            //Left
            sb.Draw(BorderTexture, new Rectangle(rect.Location, new Vector2(BorderSize, Size.Y).ToPoint()), BorderColor);
            //Right
            sb.Draw(BorderTexture, new Rectangle(rect.Location + new Point(rect.Size.X-BorderSize,0), new Vector2(BorderSize, Size.Y).ToPoint()), BorderColor);
            //Bottom
            sb.Draw(BorderTexture, new Rectangle(rect.Location + new Point(0,rect.Size.Y-BorderSize), new Vector2(Size.X - BorderSize, BorderSize).ToPoint()), BorderColor);


        }
    }
}
