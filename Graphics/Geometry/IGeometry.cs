using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMono3D.Graphics.Geometry
{ 

    public interface IGeometry : IDisposable
    {

        void Render(GraphicsDevice graphics, Effect effect, EffectPass pass);

        List<Vector3> NearestNeighbourSearch(Vector3 pt, int count);

        List<Vector3> RadiusSearch(Vector3 pt, int count, float radius);

        void RenderInstanced(GraphicsDevice graphics, Effect effect, EffectPass pass, int instancecount, VertexBuffer instanceVertexBuffer);

        List<Vector3> GetVertices();

        List<Vector3> GetNormals();

        BoundingBox GetBounds();

        bool GetCanRender();

        void SetCanRender(bool value);

        void Initialize(GraphicsDevice graphics);
    }
}
