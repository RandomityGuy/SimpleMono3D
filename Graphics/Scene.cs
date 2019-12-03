using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace SimpleMono3D.Graphics
{
    public class Scene
    {

        public GraphicsDevice graphics;
        public Camera camera;
        public Skybox skybox;

        public Effect effect;
        public Effect instancingEffect;
        GameWindow window;

        bool ready;

        public List<SceneObject> Objects = new List<SceneObject>();

        public T Find<T>()
        {
            foreach (var o in Objects)
                if (o.GetType() == typeof(T)) return (T)Convert.ChangeType(o,typeof(T));

            return default(T);
        }

        public List<SceneObject> GetSceneObjects(List<SceneObject> list)
        {
            var l = new List<SceneObject>();
            foreach (var so in list)
            {
                if (so is GroupObject)
                    l.AddRange(GetSceneObjects((so as GroupObject).Children));
                else
                    l.Add(so);
            }
            return l;
        }

        public Scene(GraphicsDevice device, GameWindow gameWindow, Effect flatShader,Effect skyboxShader,TextureCube skyboxTexture,Model skyboxModel,Effect instancingEffect)
        {
            graphics = device;
            camera = new Camera(gameWindow,Vector3.Up,Vector3.One);
            effect = flatShader;
            window = gameWindow;
            this.instancingEffect = instancingEffect;
            skybox = new Skybox(skyboxTexture, skyboxShader, skyboxModel);
            device.RasterizerState = new RasterizerState();
            ready = false;
            //effect.EnableDefaultLighting();
           // effect.PreferPerPixelLighting = false;
            //effect.World = Matrix.Identity;
        }
        
        public void AddToScene(SceneObject so)
        {
            so.scene = this;
            Objects.Add(so);
            if (ready) //If the game is not ready, add the object directly, it will be added to buffers via SetBuffers()
            {
                if (so is GroupObject)
                {
                    SetSceneForGroupObjects(so as GroupObject);
                    var children = GetSceneObjects((so as GroupObject).Children);
                }
            }
        }

        public void RemoveFromScene(SceneObject so)
        {
            Objects.Remove(so);
            if (so is GroupObject)
            {
                var children = GetSceneObjects((so as GroupObject).Children);
            }
        }

        public void ClearScene()
        {
            Objects.ForEach(a => { a.scene = null; a.Geometry.Dispose(); });
            Objects.Clear();
        }

        public void SetScene(IList<SceneObject> objects)
        {
            ClearScene();
            Objects = new List<SceneObject>();
            foreach (var obj in objects)
                Objects.Add(obj);

        }

        void SetSceneForGroupObjects(GroupObject g)
        {
            foreach (var obj in g.Children)
            {
                obj.scene = this;
                if (obj is GroupObject) SetSceneForGroupObjects(obj as GroupObject);
            }
        }

        public void Render()
        {
            //effect.View = camera.View;
            //effect.Projection = camera.Projection;
            graphics.Clear(Color.Black);

            skybox.Render(camera.View, camera.Projection, camera.Position);

            var viewProjection = camera.View * camera.Projection;
            var viewfrustum = new BoundingFrustum(viewProjection);

            effect.Parameters["View"].SetValue(camera.View);
            instancingEffect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            instancingEffect.Parameters["Projection"].SetValue(camera.Projection);
            // effect.Parameters["AmbientColor"].SetValue(new Vector4(0.1f, 0.1f, 0.1f, 1));
            //effect.Parameters["AmbientIntensity"].SetValue(1f);
            effect.Parameters["DiffuseLightDirection1"].SetValue(new Vector3(-1, -1f, -1f));
            instancingEffect.Parameters["DiffuseLightDirection1"].SetValue(new Vector3(-1, -1f, -1f));
            // effect.Parameters["DiffuseLightDirection2"].SetValue(new Vector3(-0.4f, -0.4f, -0.4f));
            //effect.Parameters["DiffuseColor"].SetValue(new Vector4(1f, 1f, 1f, 1));
            //effect.Parameters["DiffuseIntensity"].SetValue(1f);

            graphics.RasterizerState = new RasterizerState() { CullMode = CullMode.CullClockwiseFace };
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                Objects.ForEach(a => a.Render(graphics, effect,pass,viewfrustum,false));
            }
            foreach (var pass in instancingEffect.CurrentTechnique.Passes)
            {
                Objects.ForEach(a => a.Render(graphics, instancingEffect, pass, viewfrustum,true));
            }
            graphics.RasterizerState = new RasterizerState() { CullMode = CullMode.CullCounterClockwiseFace };
        }

        public void Update(GameTime dt)
        {
            camera.Update(dt);
            Objects.ForEach(a => a.Update(dt));

        }
    }
}
