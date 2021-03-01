using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMono3D.Graphics.Materials;

namespace SimpleMono3D.UI
{
    public class GuiButton : GuiAlignmentPanel
    {
        protected Action action;
        public Color NormalColor = Color.Gray;
        public Color HoverColor = Color.LightGray;
        public Color PressedColor = Color.DarkGray;
        public Texture2D NormalTexture = ColorMaterials.Gray.Texture;
        public Texture2D HoverTexture = ColorMaterials.LightGray.Texture;
        public Texture2D PressedTexture = ColorMaterials.DarkGray.Texture;


        public GuiButton(Vector2 pos, Vector2 size,Action action) : base(pos,size)
        {
            this.action = action;
        }

        public override void OnMouseLeftButtonPressed(object sender, GameTime dt, MouseState mb,bool inside)
        {
            if (inside)
            action?.Invoke();
        }

        public override void OnMouseEnter(object sender, GameTime dt, MouseState mb, bool inside)
        {
            Texture = HoverTexture;
            Color = HoverColor;
        }

        public override void OnMouseLeave(object sender, GameTime dt, MouseState mb,bool inside)
        {
            Texture = NormalTexture;
            Color = NormalColor;
        }

        public override void OnMouseLeftButton(object sender, GameTime dt, MouseState mb,bool inside)
        {
            if (inside)
            {
                Texture = PressedTexture;
                Color = PressedColor;
            }
        }

        public override void OnMouseLeftButtonReleased(object sender, GameTime dt, MouseState mb,bool inside)
        {
            if (!inside)
            {
                Texture = NormalTexture;
                Color = NormalColor;
            }
            else OnMouseEnter(sender, dt, mb, inside);
        }

    }
}
