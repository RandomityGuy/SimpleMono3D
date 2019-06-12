using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleMono3D.Graphics
{
    public class SceneObject
    {
        public Matrix Transform = Matrix.Identity;
        public List<Vector3> Positions;
        public List<int> Indices;
        public List<Vector2> UV;
        public List<Vector3> Normals;

        public Texture2D Texture;

        public GroupObject Group;

        public Scene scene;

        public Guid ID = Guid.NewGuid();

        BoundingBox? _staticBounds = null;

        List<Vector3> _worldPositions = null;

        List<Vector3> _worldNormals = null;

        public BoundingBox Bounds
        {
            get
            {
                if (Static)
                {
                    if (_staticBounds != null) return _staticBounds.Value;
                }
                var pos = Positions.Select(a => Vector3.Transform(a, Transform)).ToList();

                var min = Vector3.Zero;
                var max = Vector3.Zero;

                foreach(var p in pos)
                {
                    if (p.X < min.X) min.X = p.X;
                    if (p.Y < min.Y) min.Y = p.Y;
                    if (p.Z < min.Z) min.Z = p.Z;

                    if (p.X > max.X) max.X = p.X;
                    if (p.Y > max.Y) max.Y = p.Y;
                    if (p.Z > max.Z) max.Z = p.Z;
                }
                if (Static) _staticBounds = new BoundingBox(min, max);
                return new BoundingBox(min, max);
            }
        }

        public List<Vector3> WorldPositions
        {
            get
            {
                if (Static && _worldPositions != null) return _worldPositions;
                _worldPositions = Positions.Select(a => Vector3.Transform(a, WorldTransform)).ToList();
                return _worldPositions;
            }
        }

        public List<Vector3> WorldNormals
        {
            get
            {
                if (Static && _worldNormals != null) return _worldNormals;
                _worldNormals = Normals.Select(a => Vector3.Transform(a, WorldTransform)).ToList();
                return _worldNormals;
            }
        }

        public Matrix WorldTransform
        {
            get
            {
                if (Group != null) return Group.WorldTransform * Transform;
                else return Transform;
            }
        }

        public bool Static = false;

        internal int vertexStart;
        internal int vertexCount;
        internal int indexStart;
        internal int indexCount;
        internal bool canRender;

        public SceneObject()
        {
            Positions = new List<Vector3>();
            Indices = new List<int>();
            UV = new List<Vector2>();
            Normals = new List<Vector3>();
        }

        public virtual void SetupVertices(ref List<VertexPositionNormalTexture> vb)
        {
            if (!canRender)
            {
                vertexStart = vb.Count;
                //var pos = Positions.Select(a => Vector3.Transform(a, WorldTransform)).ToList();
                //var norm = Normals.Select(a => Vector3.Transform(a, WorldTransform)).ToList();
                for (var i = 0; i < Positions.Count; i++)
                {
                    vb.Add(new VertexPositionNormalTexture(Positions[i], Normals[i], UV[i]));
                }
                vertexCount = Positions.Count;
            }
        }

        public virtual void SetupIndices(ref List<int> ib)
        {
            if (!canRender)
            {
                indexStart = ib.Count;
                ib.AddRange(Indices);
                indexCount = Indices.Count;
                canRender = true;
            }
        }

        public virtual void Render(GraphicsDevice graphics,Effect effect,EffectPass pass)
        {
            effect.Parameters["World"].SetValue(WorldTransform);
            effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(WorldTransform)));
            if (Texture != null)
            {
                effect.Parameters["ModelTexture"].SetValue(Texture);
            }
            pass.Apply();
            graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, vertexStart, indexStart, indexCount / 3);
        }

        public virtual void Update(GameTime dt)
        {

        }
    }
}
