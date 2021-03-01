using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using SimpleMono3D.UI;
using System.Linq;

namespace SimpleMono3D.Graphics
{
    public struct SearchInfo
    {
        public List<Vector3> Points;
        public SceneObject Object;
    }
    public class Scene
    {

        public GraphicsDevice graphics;
        public SpriteBatch spritebatch;
        public Camera camera;
        public Skybox skybox;

        public Effect effect;
        public Effect instancingEffect;
        GameWindow window;

        bool ready;

        public List<SceneObject> Objects = new List<SceneObject>();

        public GuiControl Canvas;

        public bool RenderDebug = false;

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

        public Scene(GraphicsDevice device, SpriteBatch spritebatch, GameWindow gameWindow, Effect flatShader, Effect skyboxShader, TextureCube skyboxTexture, Model skyboxModel, Effect instancingEffect)
        {
            graphics = device;
            camera = new Camera(gameWindow, Vector3.Up, Vector3.One);
            effect = flatShader;
            window = gameWindow;
            this.instancingEffect = instancingEffect;
            skybox = new Skybox(skyboxTexture, skyboxShader, skyboxModel);
            device.RasterizerState = new RasterizerState();
            ready = false;
            Canvas = new GuiControl()
            {
                Position = Vector2.Zero,
                Size = new Vector2(device.Viewport.Bounds.Width, device.Viewport.Bounds.Height)
            };
            Canvas.AddChild(new GuiCursor());
            this.spritebatch = spritebatch;
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

            if (RenderDebug)
                Canvas.DebugRender(spritebatch);
            else
            {
                Canvas.Render(spritebatch);
            }

        }

        public List<SearchInfo> NearestNeighbourSearch(Vector3 pt,int count)
        {
            var objs = Objects.Where(a => a.Static);
            objs = objs.OrderBy(a => (pt - a.WorldTransform.Translation).LengthSquared()).ToList();

            var ret = new List<SearchInfo>();

            foreach (var obj in objs)
            {
                if (count <= 0)
                    break;

                var sInfo = new SearchInfo();

                var relpos = pt - obj.WorldTransform.Translation;
                sInfo.Points = obj.Geometry.NearestNeighbourSearch(pt, count).Select(a => a + obj.WorldTransform.Translation).ToList();
                sInfo.Object = obj;
                count -= sInfo.Points.Count;
                if (sInfo.Points.Count != 0)
                    ret.Add(sInfo);
            }
            return ret;
        }

        public List<SearchInfo> RadiusSearch(Vector3 pt, int count,float radius)
        {
            var objs = Objects.Where(a => a.Static);
            objs = objs.OrderBy(a => (pt - a.WorldTransform.Translation).LengthSquared()).ToList();

            var ret = new List<SearchInfo>();

            foreach (var obj in objs)
            {
                if (count <= 0)
                    break;

                var sInfo = new SearchInfo();

                var relpos = pt - obj.WorldTransform.Translation;
                sInfo.Points = obj.Geometry.RadiusSearch(pt, count,radius).Select(a => a + obj.WorldTransform.Translation).ToList();
                sInfo.Object = obj;
                count -= sInfo.Points.Count;
                if (sInfo.Points.Count != 0)
                    ret.Add(sInfo);
            }
            return ret;
        }

        public List<SceneObject> BoundingSearch(BoundingSphere sphere,int count)
        {
            var objs = Objects.Where(a => a.Static);
            objs = objs.OrderBy(a => (sphere.Center - a.WorldTransform.Translation).LengthSquared()).ToList();

            var ret = new List<SceneObject>();
            foreach (var obj in objs)
            {
                if (count <= 0)
                    break;

                if (obj.Bounds.Intersects(sphere))
                    ret.Add(obj);

                count--;
            }
            return ret;
        }

        public void Update(GameTime dt)
        {
            camera.Update(dt);
            Objects.ForEach(a => a.Update(dt));

        }
    }
}
