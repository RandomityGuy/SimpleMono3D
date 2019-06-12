using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleMono3D.Graphics
{
    public class GroupObject : SceneObject
    {

        public List<SceneObject> Children { get; private set; }

        public GroupObject()
        {
            Children = new List<SceneObject>();
        }

        public void AddObject(SceneObject so)
        {
            so.Group = this;
            so.scene = this.scene;
            if (this.Group == null) //We dont want to unnecessarily add stuff to the buffers before its even in the scene
            {
                if (so is GroupObject)
                {
                    var children = GetSceneObjects((so as GroupObject).Children);
                    children.ForEach(a => scene.AddToBuffers(a));
                    children.ForEach(a => a.scene = this.scene);
                }
                else
                    scene.AddToBuffers(so);
            }
            Children.Add(so);
        }

        public void RemoveObject(SceneObject so)
        {
            Children.Remove(so);
            so.scene = null;
            so.Group = null;
            if (so is GroupObject)
            {
                var children = GetSceneObjects((so as GroupObject).Children);
                children.ForEach(a => scene.RemoveFromBuffers(a));
                children.ForEach(a => a.scene = null);
            }
            else
                scene.RemoveFromBuffers(so);
        }

        public override void Render(GraphicsDevice graphics, Effect effect,EffectPass pass)
        {
            foreach (var so in Children)
            {
                so.Render(graphics, effect, pass);
            }
        }

        public override void Update(GameTime dt)
        {
            Children.ForEach(a => a.Update(dt));
            base.Update(dt);
        }

        public List<SceneObject> GetSceneObjects(List<SceneObject> list)
        {
            var l = new List<SceneObject>();
            foreach (var so in list)
            {
                if (so is GroupObject)
                    l.AddRange(GetSceneObjects((so as GroupObject).Children));

                if (so.GetType() == typeof(SceneObject)) l.Add(so);
            }
            return l;
        }

    }
}
