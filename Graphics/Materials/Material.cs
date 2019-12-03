using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Graphics.Materials
{
    public abstract class Material
    {
        public string Name { get; internal set; }
        public Texture2D Texture { get; internal set; }
    }
}
