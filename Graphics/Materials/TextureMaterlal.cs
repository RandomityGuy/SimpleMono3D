using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Graphics.Materials
{

    public class TextureMaterial : Material
    {
        public static TextureMaterial Load(string path)
        {
            var s = File.OpenRead(path);
            var tm = new TextureMaterial();
            tm.Texture = Texture2D.FromStream(SimpleMono3D.Instance.graphics.GraphicsDevice, s);
            tm.Name = path;
            return tm;
        }
    }
}
