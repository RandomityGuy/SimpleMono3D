using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleMono3D.Graphics.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KdTree;

namespace SimpleMono3D.Graphics.Geometry
{
    public class FaceGroup
    {
        public List<int> Indices;
        public Material Material;
        public IndexBuffer indexBuffer;
    }

    public class MeshGeometry : IGeometry, IDisposable
    {
        public List<Vector3> Positions;
        public List<FaceGroup> FaceGroups;
        public List<Vector2> UV;
        public List<Vector3> Normals;
        public KdTree<float,Vector3> KdTree;

        VertexBuffer vertexBuffer;
        BoundingBox? _staticBounds = null;


        internal bool canRender;

        public void Initialize(GraphicsDevice graphics)
        {
            if (!canRender)
            {
                vertexBuffer = new VertexBuffer(graphics, VertexPositionNormalTexture.VertexDeclaration, Positions.Count, BufferUsage.WriteOnly);
                List<VertexPositionNormalTexture> vb = new List<VertexPositionNormalTexture>();
                for (var i = 0; i < Positions.Count; i++)
                {
                    vb.Add(new VertexPositionNormalTexture(Positions[i], Normals[i], UV[i]));
                }
                vertexBuffer.SetData(vb.ToArray());

                for (var i = 0; i < FaceGroups.Count; i++)
                {
                    FaceGroups[i].indexBuffer = new IndexBuffer(graphics, IndexElementSize.ThirtyTwoBits, FaceGroups[i].Indices.Count, BufferUsage.WriteOnly);
                    FaceGroups[i].indexBuffer.SetData(FaceGroups[i].Indices.ToArray());
                }

                KdTree = new KdTree<float, Vector3>(3, new KdTree.Math.FloatMath());

                foreach (var v in Positions)
                {
                    KdTree.Add(new float[] { v.X, v.Y, v.Z }, v);
                }

                canRender = true;
            }
        }

        public List<Vector3> NearestNeighbourSearch(Vector3 pt,int count)
        {
            if (!canRender) return new List<Vector3>();             
            return KdTree.GetNearestNeighbours(new float[] { pt.X, pt.Y, pt.Z }, count).Select(a => a.Value).ToList();
        }

        public List<Vector3> RadiusSearch(Vector3 pt,int count,float radius)
        {
            if (!canRender) return new List<Vector3>();
            return KdTree.RadialSearch(new float[] { pt.X, pt.Y, pt.Z },radius, count).Select(a => a.Value).ToList();
        }

        public virtual void Render(GraphicsDevice graphics, Effect effect, EffectPass pass)
        {
            if (!canRender) Initialize(graphics);
            graphics.SetVertexBuffer(vertexBuffer);

            foreach (var facegroup in FaceGroups)
            {
                graphics.Indices = facegroup.indexBuffer;
                if (facegroup.Material != null)
                {
                    if (facegroup.Material is ColorMaterial)
                    {
                        effect.Parameters["ModelTexture"].SetValue(facegroup.Material.Texture);
                        //effect.Parameters["IsColorModel"].SetValue(true);

                        //var colorvec = (facegroup.Material as ColorMaterial).color.ToVector4();
                        //colorvec /= 255;

                        //effect.Parameters["ModelColor"].SetValue(colorvec);
                    }
                    else
                    {
                        //effect.Parameters["IsColorModel"].SetValue(false);
                        effect.Parameters["ModelTexture"].SetValue(facegroup.Material.Texture);
                    }
                }
                pass.Apply();
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, facegroup.Indices.Count / 3);
            }
        }

        public virtual void RenderInstanced(GraphicsDevice graphics, Effect effect, EffectPass pass,int instancecount,VertexBuffer instanceVertexBuffer)
        {
            if (!canRender) Initialize(graphics);

            foreach (var facegroup in FaceGroups)
            {
                if (facegroup.Material != null)
                {
                        //effect.Parameters["IsColorModel"].SetValue(false);
                        effect.Parameters["ModelTexture"].SetValue(facegroup.Material.Texture);
                }
                graphics.Indices = facegroup.indexBuffer;

                graphics.SetVertexBuffers(
                    new VertexBufferBinding(vertexBuffer),
                    new VertexBufferBinding(instanceVertexBuffer, 0, 1));

                pass.Apply();

                graphics.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, Positions.Count, 0, facegroup.Indices.Count / 3, instancecount);
            }
        }

        public List<Vector3> GetNormals() => Normals;

        public List<Vector3> GetVertices() => Positions;

        public bool GetCanRender() => canRender;

        public void SetCanRender(bool val)
        {
            canRender = val;
        }

        public BoundingBox GetBounds()
        {
            if (_staticBounds != null) return _staticBounds.Value;
            var pos = GetVertices();

            var min = Vector3.Zero;
            var max = Vector3.Zero;

            foreach (var p in pos)
            {
                if (p.X < min.X) min.X = p.X;
                if (p.Y < min.Y) min.Y = p.Y;
                if (p.Z < min.Z) min.Z = p.Z;

                if (p.X > max.X) max.X = p.X;
                if (p.Y > max.Y) max.Y = p.Y;
                if (p.Z > max.Z) max.Z = p.Z;
            }
            _staticBounds = new BoundingBox(min, max);
            return _staticBounds.Value;
    }

        public void Dispose()
        {
            vertexBuffer.Dispose();
            foreach (var facegroup in FaceGroups)
            {
                facegroup.indexBuffer.Dispose();
            }
            canRender = false;
        }
    }
}
