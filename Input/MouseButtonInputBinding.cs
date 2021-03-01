using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Input
{
    public static class MouseStateExtensions
    {
        public static bool IsButtonDown(this MouseState ms,MouseButton mb)
        {
            switch (mb)
            {
                case MouseButton.Left:
                    return (ms.LeftButton == ButtonState.Pressed);

                case MouseButton.Right:
                    return (ms.RightButton == ButtonState.Pressed);

                case MouseButton.Middle:
                    return (ms.MiddleButton == ButtonState.Pressed);

                default:
                    return false;
            }
        }
    }

    public enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    public class MouseButtonInputBinding : IInputBinding
    {
        internal MouseButton button;
        internal Modifiers modifiers;
        internal Action<object,GameTime,MouseState> func;
        internal InputBindingType type;

        public MouseButtonInputBinding(MouseButton button, Action<object,GameTime,MouseState> func,InputBindingType type,Modifiers modifiers = 0)
        {
            this.button = button;
            this.func = func;
            this.type = type;
            this.modifiers = modifiers;
        }

        public InputBindingType GetBindingType()
        {
            return type;
        }

        public void Run(GameTime dt)
        {
            func(this,dt,Mouse.GetState());
        }

        public bool SatisfiesCondition(KeyboardState prevkb,MouseState prevMb,KeyboardState kb, MouseState mb)
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
                    return mb.IsButtonDown(button) && !prevMb.IsButtonDown(button);

                case InputBindingType.Released:
                    return !mb.IsButtonDown(button) && prevMb.IsButtonDown(button);

                case InputBindingType.Held:
                    return mb.IsButtonDown(button) && prevMb.IsButtonDown(button);

                case InputBindingType.StateChanged:
                    return mb.X != prevMb.X || mb.Y != prevMb.Y;

                case InputBindingType.None:
                    return true;

                default:
                    return false;
            }
        }
    }
}
