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
    public class GuiTextButton : GuiButton
    {
        GuiText textgui;
        public Color TextColor
        {
            get => textgui.Color;
            set => textgui.Color = value;
        }

        public GuiTextButton(Vector2 pos, Vector2 size,string text,Action action) : base(pos,size,action)
        {
            textgui = new GuiText(text, Color.Black) { LocationDescription = new AlignmentDescription(Alignment.Middle) };
            AddChild(textgui);
        }

    }
}
