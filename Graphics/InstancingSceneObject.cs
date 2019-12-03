using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleMono3D.Graphics.Materials;
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

        public override void Render(GraphicsDevice graphics, Effect effect, EffectPass pass, BoundingFrustum viewFrustum, bool isInstanced)
        {
            if (isInstanced)
            {
                effect.Parameters["World"].SetValue(Transform);
                effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(Transform)));

                var instanceBuffer = new DynamicVertexBuffer(graphics,new Graphics.Geometry.InstancingVertexDeclaration().VertexDeclaration, Instances.Count, BufferUsage.WriteOnly);
                instanceBuffer.SetData(Instances.ToArray(),0,Instances.Count,SetDataOptions.Discard);

                //for (var i = 0; i < Instances.Count; i++)
                //{
                //    if (viewFrustum.Contains(BoundingBox.CreateFromPoints(Geometry.GetBounds().GetCorners().Select(a => Vector3.Transform(a, Instances[i])))) == ContainmentType.Disjoint)
                //        continue;

                //    var currentTransform = WorldTransform * Instances[i];
                //}
                Geometry.RenderInstanced(graphics, effect, pass, Instances.Count, instanceBuffer);
            }
        }
    }
}
