using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Graphics
{
    public class InstancingSceneObject : SceneObject
    {
        public List<Matrix> Instances = new List<Matrix>();

        public override void Render(GraphicsDevice graphics, Effect effect, EffectPass pass)
        {
            if (Texture != null)
            {
                effect.Parameters["ModelTexture"].SetValue(Texture);
            }
            for (var i = 0; i < Instances.Count; i++)
            {
                var currentTransform = WorldTransform * Instances[i];

                effect.Parameters["World"].SetValue(currentTransform);
                effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(currentTransform)));
                pass.Apply();
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, vertexStart, indexStart, indexCount / 3);
            }
        }
    }
}
