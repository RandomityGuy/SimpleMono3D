using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Input
{
    public enum InputBindingType
    {
        Pressed,
        Released,
        Held,
        StateChanged,
        None
    }

    public enum Modifiers
    {
        Ctrl = 1,
        Shift = 2,
        Alt = 4
    }

    public interface IInputBinding
    {
        void Run(GameTime dt);
        InputBindingType GetBindingType();
        bool SatisfiesCondition(KeyboardState prevkb, MouseState prevMb, KeyboardState kb, MouseState mb);
    }
}
