using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleMono3D.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.UI
{
    public class TextField
    {
        public string Text;

        public int CaretPosition = 0;

        public bool WrapText = true;

        public bool FollowCaret = true;

        public Vector2 Size;

        SpriteFont Font;

        bool dirty = true;

        string renderText = "";

        int renderStart = 0;
        int renderEnd = 0;

        public TextField(Vector2 size,SpriteFont font)
        {
            Size = size;
            Font = font;
        }

        public void ParseKeyCode(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    if (Text != "")
                    {
                        if (CaretPosition > 0)
                        {
                            Text = Text.Remove(CaretPosition - 1, 1);
                            if (CaretPosition > 0)
                                CaretPosition--;
                        }
                    }
                    break;

                case Keys.Delete:
                    if (Text != "")
                    {
                        if (CaretPosition >= 0 && CaretPosition < Text.Length)
                        {
                            Text = Text.Remove(CaretPosition, 1);
                        }
                    }
                    break;

                case Keys.Left:
                    if (CaretPosition > 0)
                        CaretPosition--;
                    break;

                case Keys.Right:
                    if (CaretPosition < Text.Length)
                        CaretPosition++;
                    break;

                default:
                    var keystr = KeyCodeToText.ParseKeyCode(key);
                    if (CaretPosition > Text.Length)
                        CaretPosition = Text.Length;
                    Text = Text.Insert(CaretPosition,keystr);
                    CaretPosition++;
                    break;
            }

            dirty = true;
        }

        public string GetRenderText(bool showCaret = false)
        {
            if (dirty)
            {
                var sb = new StringBuilder();
                var xsize = 0f;
                var ysize = 0f;
                for (int i = 0; i < Text.Length; i++)
                {
                    char ch = Text[i];

                    if (i == CaretPosition)
                        sb.Append("|");

                    if (WrapText)
                    {
                        var chrsize = Font.MeasureString(ch.ToString());
                        xsize += chrsize.X;
                        if (xsize > Size.X)
                        {
                            sb.AppendLine();
                            ysize += chrsize.Y;
                            xsize = 0;
                        }
                        if (ysize > Size.Y)
                        {
                            sb.Clear();
                            ysize = 0;
                        }
                    }
                    sb.Append(ch);

                }
                renderText = sb.ToString();
                dirty = false;
            }
            return renderText;
        }
    }
}
