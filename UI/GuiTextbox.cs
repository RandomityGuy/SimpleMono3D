using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleMono3D.Input;

namespace SimpleMono3D.UI
{
    public class GuiTextbox : GuiAlignmentPanel
    {
        GuiText text;

        TextField textField;

        bool focused = false;

        public GuiTextbox(Vector2 pos,Vector2 size,string placeholder = "") : base(pos,size)
        {
            text = new GuiText(placeholder, Color.White) { LocationDescription = new AlignmentDescription(Alignment.TopLeft), Position = new Vector2(5,5)};

            textField = new TextField(size,text.Font);
            textField.Text = text.Text;

            InputManager.Bindings.Add(new AnyKeyInputBinding(OnKeyPressed, InputBindingType.Pressed));

            AddChild(text);
        }

        public void OnKeyPressed(object sender, GameTime dt, KeyboardState ks)
        {
            if (focused)
            {
                if (ks.GetPressedKeys().Length != 0)
                {
                    var key = ks.GetPressedKeys()[0];

                    textField.ParseKeyCode(key);

                    text.Text = textField.GetRenderText();
                  
                }
            }
        }

        public override void OnMouseLeftButtonPressed(object sender, GameTime dt, MouseState mb, bool inside)
        {
            if (inside)
                focused = true;
            else
                focused = false;
        }
    }
}
