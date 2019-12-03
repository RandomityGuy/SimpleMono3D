using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleMono3D.Graphics.Geometry;
using SimpleMono3D.Graphics.Materials;

namespace SimpleMono3D.Graphics
{
    public class SceneObject
    {
        public Matrix Transform = Matrix.Identity;

        public IGeometry Geometry;

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
                return BoundingBox.CreateFromPoints(Geometry.GetBounds().GetCorners().Select(a => Vector3.Transform(a, Transform)));
            }
        }

        public List<Vector3> WorldPositions
        {
            get
            {
                if (Static && _worldPositions != null) return _worldPositions;
                _worldPositions = Geometry.GetVertices().Select(a => Vector3.Transform(a, WorldTransform)).ToList();
                return _worldPositions;
            }
        }

        public List<Vector3> WorldNormals
        {
            get
            {
                if (Static && _worldNormals != null) return _worldNormals;
                _worldNormals = Geometry.GetNormals().Select(a => Vector3.Transform(a, WorldTransform)).ToList();
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

        public SceneObject()
        {
        }

        public virtual void Render(GraphicsDevice graphics, Effect effect, EffectPass pass,BoundingFrustum viewFrustum,bool isInstanced)
        {
            if (!isInstanced)
            {
                if (viewFrustum.Contains(Bounds) == ContainmentType.Disjoint)
                    return;
                effect.Parameters["World"].SetValue(WorldTransform);
                effect.Parameters["WorldInverseTranspose"].SetValue(Matrix.Transpose(Matrix.Invert(WorldTransform)));
                Geometry.Render(graphics, effect, pass);
            }
        }

        public virtual void Update(GameTime dt)
        {

        }
    }
}
