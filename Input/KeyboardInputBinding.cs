using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Input
{

    public class KeyboardInputBinding : IInputBinding
    {
        internal Keys key;
        internal Modifiers modifiers;
        internal Action<object,GameTime, KeyboardState> func;
        internal InputBindingType type;

        public KeyboardInputBinding(Keys key,Action<object,GameTime,KeyboardState> func,InputBindingType type,Modifiers modifiers = 0)
        {
            this.key = key;
            this.func = func;
            this.type = type;
            this.modifiers = modifiers;
        }

        public void Run(GameTime dt)
        {
            func(this,dt,Keyboard.GetState());
        }

        public InputBindingType GetBindingType()
        {
            return type;
        }

        public bool SatisfiesCondition(KeyboardState prevkb, MouseState prevMb, KeyboardState kb, MouseState mb)
        {
            var pressed = kb.GetPressedKeys();

            var isctrl = ((int)modifiers & (int)Modifiers.Ctrl) == (int)Modifiers.Ctrl;
            var isshift = ((int)modifiers & (int)Modifiers.Shift) == (int)Modifiers.Shift;
            var isalt = ((int)modifiers & (int)Modifiers.Alt) == (int)Modifiers.Alt;

            if (isctrl)
                if (!pressed.Contains(Keys.LeftControl) && !pressed.Contains(Keys.RightControl))
                    return false;

            if (isshift)
                if (!pressed.Contains(Keys.LeftShift) && !pressed.Contains(Keys.RightShift))
                    return false;

            if (isalt)
                if (!pressed.Contains(Keys.LeftAlt) && !pressed.Contains(Keys.RightAlt))
                    return false;

            switch (type)
            {
                case InputBindingType.Pressed:
                    return kb.IsKeyDown(key) && kb.IsKeyUp(key); 

                case InputBindingType.Released:
                    return !kb.IsKeyDown(key) && prevkb.IsKeyDown(key);

                case InputBindingType.Held:
                    return kb.IsKeyDown(key) && prevkb.IsKeyDown(key);

                case InputBindingType.None:
                    return true;

                default:
                    return false;
            }
        }
    }
}
