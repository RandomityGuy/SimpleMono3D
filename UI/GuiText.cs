using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.UI
{
    public class GuiText : GuiControl
    {
        private string text = "";
        public Color Color = Color.Black;
        public SpriteFont Font = SimpleMono3D.Instance.defaultFont;
        public float Scale = 1;

        public string Text 
        { 
            get => text; 
            set
            {
                text = value;
                UpdateSize();
            }
        }

        public GuiText(string text,Color color) : base()
        {
            Text = text;
            Color = color;
            UpdateSize();
        }

        public GuiText(string text, Color color,float scale) : base()
        {
            Text = text;
            Color = color;
            Scale = scale;
            UpdateSize();
        }

        public GuiText(string text, Color color,SpriteFont font) : base()
        {
            Text = text;
            Color = color;
            Font = font;
            UpdateSize();
        }

        public GuiText(string text, Color color,float scale,SpriteFont font)
        {
            Text = text;
            Color = color;
            Font = font;
            Scale = scale;
            UpdateSize();
        }

        void UpdateSize()
        {
            Size = Font.MeasureString(Text);
        }

        public override void Render(SpriteBatch sb)
        {

            SetupClipping(sb);
            sb.DrawString(Font, Text, TotalDisplacement, Color,0,Vector2.Zero,Scale,SpriteEffects.None,1);
            RenderChildren(sb);

        }
    }
}
