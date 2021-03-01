using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Input
{
    public class AnyKeyInputBinding : IInputBinding
    {
        internal Action<object, GameTime, KeyboardState> func;
        internal InputBindingType type;

        Keys pressedKey;

        public AnyKeyInputBinding(Action<object, GameTime, KeyboardState> func, InputBindingType type)
        {
            this.func = func;
            this.type = type;
        }

        public InputBindingType GetBindingType()
        {
            return type;
        }

        public void Run(GameTime dt)
        {
            func(this, dt, new KeyboardState(new Keys[] { pressedKey },Keyboard.GetState().CapsLock));
        }

        public bool SatisfiesCondition(KeyboardState prevkb, MouseState prevMb, KeyboardState kb, MouseState mb)
        {
            var prevkeys = prevkb.GetPressedKeys();
            var keys = kb.GetPressedKeys();

            switch (type)
            {
                case InputBindingType.Held:
                    var heldkeys = keys.Where(a => prevkeys.Contains(a));
                    if (heldkeys.Count() != 0)
                    {
                        pressedKey = heldkeys.First();
                        return true;
                    }
                    return false;

                case InputBindingType.Pressed:
                    var pressedkeys = keys.Where(a => !prevkeys.Contains(a));
                    if (pressedkeys.Count() != 0)
                    {
                        pressedKey = pressedkeys.First();
                        return true;
                    }
                    return false;

                case InputBindingType.Released:
                    var releasedkeys = prevkeys.Where(a => !keys.Contains(a));
                    if (releasedkeys.Count() != 0)
                    {
                        pressedKey = releasedkeys.First();
                        return true;
                    }
                    return false;

                case InputBindingType.StateChanged:
                    var changedkeys = keys.Where(a => !prevkeys.Contains(a)).Union(prevkeys.Where(a => !keys.Contains(a)));
                    if (changedkeys.Count() != 0)
                    {
                        pressedKey = changedkeys.First();
                        return true;
                    }
                    return false;

                case InputBindingType.None:
                    if (keys.Count() != 0)
                    {
                        pressedKey = keys[0];
                        return true;
                    }
                    return false;
            }

            return false;
        }
    }
}
