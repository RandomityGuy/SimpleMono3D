using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Graphics.Materials
{
    public static class ColorMaterials
    {
        public static ColorMaterial White => ColorMaterial.FromColor(Color.White);

        public static ColorMaterial Black => ColorMaterial.FromColor(Color.Black);

        public static ColorMaterial Red => ColorMaterial.FromColor(Color.Red);

        public static ColorMaterial Blue => ColorMaterial.FromColor(Color.Blue);

        public static ColorMaterial Green => ColorMaterial.FromColor(Color.Green);

        public static ColorMaterial Orange => ColorMaterial.FromColor(Color.Orange);

        public static ColorMaterial Yellow => ColorMaterial.FromColor(Color.Yellow);

        public static ColorMaterial Purple => ColorMaterial.FromColor(Color.Violet);

        public static ColorMaterial Cyan => ColorMaterial.FromColor(Color.Cyan);

        public static ColorMaterial Magenta => ColorMaterial.FromColor(Color.Magenta);

        public static ColorMaterial Maroon => ColorMaterial.FromColor(Color.Maroon);

        public static ColorMaterial Indigo => ColorMaterial.FromColor(Color.Indigo);

        public static ColorMaterial LightGray => ColorMaterial.FromColor(Color.LightGray);

        public static ColorMaterial Gray => ColorMaterial.FromColor(Color.Gray);

        public static ColorMaterial DarkGray => ColorMaterial.FromColor(Color.DarkGray);

        public static ColorMaterial Pink => ColorMaterial.FromColor(Color.Pink);
    }

    public class ColorMaterial : Material
    {
        public Color color;

        public static ColorMaterial FromColor(Color c)
        {
            var cm = new ColorMaterial();
            var tex = new Texture2D(SimpleMono3D.Instance.graphics.GraphicsDevice, 1, 1);
            var data = new List<Color>();
            data.Add(c);
            tex.SetData(data.ToArray());
            cm.Texture = tex;
            cm.Name = c.PackedValue.ToString();
            cm.color = c;
            return cm;
        }
    }
}
