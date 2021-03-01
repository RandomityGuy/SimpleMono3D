using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Input
{
    public static class KeyCodeToText
    {
        public static string ParseKeyCode(Keys key)
        {
            var kb = Keyboard.GetState();

            var isShiftDown = (kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift));

            if (key >= Keys.A && key <= Keys.Z)
            {
                return (isShiftDown ^ kb.CapsLock) ? key.ToString() : key.ToString().ToLower();
            }

            switch (key)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    return (isShiftDown && key == Keys.D0) ? ")" : "0";

                // number 9
                case Keys.D9:
                case Keys.NumPad9:
                    return (isShiftDown && key == Keys.D9) ? "(" : "9";
                    

                // number 8
                case Keys.D8:
                case Keys.NumPad8:
                    return (isShiftDown && key == Keys.D8) ? "*" : "8";
                    

                // number 7
                case Keys.D7:
                case Keys.NumPad7:
                    return (isShiftDown && key == Keys.D7) ? "&" : "7";
                    

                // number 6
                case Keys.D6:
                case Keys.NumPad6:
                    return (isShiftDown && key == Keys.D6) ? "^" : "6";
                    

                // number 5
                case Keys.D5:
                case Keys.NumPad5:
                    return (isShiftDown && key == Keys.D5) ? "%" : "5";
                    

                // number 4
                case Keys.D4:
                case Keys.NumPad4:
                    return (isShiftDown && key == Keys.D4) ? "$" : "4";
                    

                // number 3
                case Keys.D3:
                case Keys.NumPad3:
                    return (isShiftDown && key == Keys.D3) ? "#" : "3";
                    

                // number 2
                case Keys.D2:
                case Keys.NumPad2:
                    return (isShiftDown && key == Keys.D2) ? "@" : "2";
                    

                // number 1
                case Keys.D1:
                case Keys.NumPad1:
                    return (isShiftDown && key == Keys.D1) ? "!" : "1";
                    

                // question mark
                case Keys.OemQuestion:
                    return isShiftDown ? "?" : "/";
                    

                // quotes
                case Keys.OemQuotes:
                    return isShiftDown ? "\"" : "\"";
                    

                // semicolon
                case Keys.OemSemicolon:
                    return isShiftDown ? ":" : ";";
                    

                // tilde
                case Keys.OemTilde:
                    return isShiftDown ? "~" : "`";
                    

                // open brackets
                case Keys.OemOpenBrackets:
                    return isShiftDown ? "{" : "[";
                    

                // close brackets
                case Keys.OemCloseBrackets:
                    return isShiftDown ? "}" : "]";
                    

                // add
                case Keys.OemPlus:
                case Keys.Add:
                    return (isShiftDown || key == Keys.Add) ? "+" : "=";
                    

                // substract
                case Keys.OemMinus:
                case Keys.Subtract:
                    return isShiftDown ? "_" : "-";
                    

                // decimal dot
                case Keys.OemPeriod:
                case Keys.Decimal:
                    return isShiftDown ? ">" : ".";
                    

                // divide
                case Keys.Divide:
                    return isShiftDown ? "?" : "/";
                    

                // multiply
                case Keys.Multiply:
                    return"*";
                    

                // backslash
                case Keys.OemBackslash:
                    return isShiftDown ? "|" : "\\";
                    

                // comma
                case Keys.OemComma:
                    return isShiftDown ? "<" : ",";
                    

                // tab
                case Keys.Tab:
                    return " ";

                case Keys.Enter:
                    return "\n";

                case Keys.Space:
                    return " ";

                default:
                    return "";

            }

            return "";
        }
    }
}
