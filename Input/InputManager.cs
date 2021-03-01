using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Input
{
    public static class InputManager
    {
        public static List<IInputBinding> Bindings { get; set; } = new List<IInputBinding>();

        public static KeyboardState PreviousKeyboardState;
        public static MouseState PreviousMouseState;

        public static void Update(GameTime span)
        {
            var kb = Keyboard.GetState();
            var mb = Mouse.GetState();

            if (PreviousKeyboardState != null)
            {
                foreach (var b in Bindings)
                {
                    if (b.SatisfiesCondition(PreviousKeyboardState, PreviousMouseState, kb, mb))
                        b.Run(span);
                }
            }

            PreviousKeyboardState = kb;
            PreviousMouseState = mb;
        }
    }
}
