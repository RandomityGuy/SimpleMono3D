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

        public Effect effect;
        GameWindow window;

        bool IsDirty = true;

        public VertexBuffer RawVertexBuffer;
        public IndexBuffer RawIndexBuffer;

        List<VertexPositionNormalTexture> VertexBuffer;
        List<int> IndexBuffer;

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

        public Scene(GraphicsDevice device,GameWindow gameWindow,Effect shader)
        {
            graphics = device;
            camera = new Camera(device, gameWindow);
            effect = shader;
            //effect.EnableDefaultLighting();
           // effect.PreferPerPixelLighting = false;
            //effect.World = Matrix.Identity;
        }
        
        public void AddToScene(SceneObject so)
        {
            so.scene = this;
            Objects.Add(so);
            if (so is GroupObject)
            {
                SetSceneForGroupObjects(so as GroupObject);
                var children = GetSceneObjects((so as GroupObject).Children);
                children.ForEach(a => AddToBuffers(a));
            }
            else
                AddToBuffers(so);
        }

        public void RemoveFromScene(SceneObject so)
        {
            Objects.Remove(so);
            if (so is GroupObject)
            {
                var children = GetSceneObjects((so as GroupObject).Children);
                children.ForEach(a => RemoveFromBuffers(a));
            }
            else
                RemoveFromBuffers(so);
        }

        public void ClearScene()
        {
            Objects.ForEach(a => { a.scene = null; a.canRender = false; });
            Objects.Clear();
            VertexBuffer = new List<VertexPositionNormalTexture>();
            IndexBuffer = new List<int>();
            RegenerateBuffers();
        }

        public void SetScene(IList<SceneObject> objects)
        {
            ClearScene();
            Objects = new List<SceneObject>();
            foreach (var obj in objects)
                Objects.Add(obj);

            SetBuffers();
        }

        void SetSceneForGroupObjects(GroupObject g)
        {
            foreach (var obj in g.Children)
            {
                obj.scene = this;
                if (obj is GroupObject) SetSceneForGroupObjects(obj as GroupObject);
            }
        }

        internal void AddToBuffers(SceneObject so)
        {
            so.SetupVertices(ref VertexBuffer);
            so.SetupIndices(ref IndexBuffer);
            RegenerateBuffers();
        }

        internal void RemoveFromBuffers(SceneObject so)
        {
            if (so.canRender)
            {
                VertexBuffer.RemoveRange(so.vertexStart, so.vertexCount);
                IndexBuffer.RemoveRange(so.indexStart, so.indexCount);
                so.vertexCount = 0;
                so.vertexStart = 0;
                so.indexCount = 0;
                so.indexStart = 0;
                so.canRender = false;
                RegenerateBuffers();
            }
        }

        void RegenerateBuffers()
        {
            RawVertexBuffer = new VertexBuffer(graphics, VertexPositionNormalTexture.VertexDeclaration, VertexBuffer.Count, BufferUsage.WriteOnly);
            RawVertexBuffer.SetData(VertexBuffer.ToArray());
            RawIndexBuffer = new IndexBuffer(graphics, IndexElementSize.ThirtyTwoBits, IndexBuffer.Count, BufferUsage.WriteOnly);
            RawIndexBuffer.SetData(IndexBuffer.ToArray());
        }

        public void SetBuffers()
        {
            var renderObjects = GetSceneObjects(Objects);
            VertexBuffer = new List<VertexPositionNormalTexture>();
            renderObjects.ForEach(a => a.SetupVertices(ref VertexBuffer));
            RawVertexBuffer = new VertexBuffer(graphics, VertexPositionNormalTexture.VertexDeclaration, VertexBuffer.Count, BufferUsage.WriteOnly);
            RawVertexBuffer.SetData(VertexBuffer.ToArray());

            IndexBuffer = new List<int>();
            renderObjects.ForEach(a => a.SetupIndices(ref IndexBuffer));
            RawIndexBuffer = new IndexBuffer(graphics, IndexElementSize.ThirtyTwoBits, IndexBuffer.Count, BufferUsage.WriteOnly);
            RawIndexBuffer.SetData(IndexBuffer.ToArray());
            

            //var statics = GetStaticSceneObjects(Objects);
            //var vertexcount = 0;
            //statics.ForEach(a => vertexcount += a.Positions.Count);
            //RawVertexBuffer = new VertexBuffer(graphics, VertexPositionNormalTexture.VertexDeclaration, vertexcount, BufferUsage.WriteOnly);
            //var vertices = new List<VertexPositionNormalTexture>();
            //statics.ForEach(a => a.SetupVertices(ref vertices));
            //RawVertexBuffer.SetData(vertices.ToArray());
            //var indexcount = 0;
            //statics.ForEach(a => indexcount += a.Indices.Count);
            //RawIndexBuffer = new IndexBuffer(graphics, IndexElementSize.ThirtyTwoBits, indexcount, BufferUsage.WriteOnly);
            //var indices = new List<int>();
            //statics.ForEach(a => a.SetupIndices(ref indices));
            //RawIndexBuffer.SetData(indices.ToArray());
        }

        public void Render()
        {
            //effect.View = camera.View;
            //effect.Projection = camera.Projection;

            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);

            graphics.Indices = RawIndexBuffer;
            graphics.SetVertexBuffer(RawVertexBuffer);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                Objects.ForEach(a => a.Render(graphics, effect,pass));
            }
        }

        public void Update(GameTime dt)
        {
            camera.Update(dt);
            Objects.ForEach(a => a.Update(dt));

        }
    }
}
