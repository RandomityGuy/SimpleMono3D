using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Input;
using SimpleMono3D.Input;

namespace SimpleMono3D.UI
{
    public delegate void GuiEvent(object sender, GameTime dt, MouseState mb,bool inside);

    public static class GuiStaticVariables
    {
        public static bool _calledBegin = false;
    }

    public class GuiControl
    {
        internal class GuiLayerComparer : IComparer<GuiControl>
        {
            public int Compare(GuiControl x, GuiControl y)
            {
                if (x.Layer > y.Layer)
                    return 1;
                if (x.Layer == y.Layer)
                    return 0;
                if (x.Layer > y.Layer)
                    return -1;

                return 0;
            }
        }

        public Vector2 Position = Vector2.Zero;

        public Vector2 Size = Vector2.One;

        public List<GuiControl> Children = new List<GuiControl>();

        public GuiControl Parent;

        public int Layer;

        public ILocationDescription LocationDescription;

        public Rectangle Rectangle
        {
            get => new Rectangle(TotalDisplacement.ToPoint(), Size.ToPoint());
        }

        public Rectangle GetRenderRectangle(SpriteBatch sb)
        {
            if (Parent == null)
            {
                return sb.GraphicsDevice.Viewport.Bounds;
            }
            else
            {
                return Rectangle.Intersect(Rectangle, Parent.GetRenderRectangle(sb));
            }
        }
        public virtual Vector2 TotalDisplacement
        {
            get
            {
                if (Parent == null)
                    return Position;
                else
                    return Parent.GetPosition(this);
            }
        }

        List<IInputBinding> Bindings;

        public GuiControl()
        {
            RegisterBindings();
        }

        public virtual Vector2 GetPosition(GuiControl g)
        {
            if (Parent == null)
                return Position + g.Position;
            else
                return Parent.GetPosition(g) + g.Position;
        }

        public IEnumerable<T> FindChildren<T>()
        {
            foreach (var child in Children)
            {
                if (child.GetType() == typeof(T))
                    yield return (T)Convert.ChangeType(child,typeof(T));

                if (child.Children.Count != 0)
                foreach (var ch in child.FindChildren<T>())
                    yield return (T)Convert.ChangeType(child, typeof(T));
            }
        }

        public virtual void AddChild(GuiControl c)
        {
            c.Parent = this;
            if (Children.Count > 0)
            {
                if (Children[0].Layer > c.Layer)
                    Children.Insert(0, c);
                else if (Children.Last().Layer < c.Layer)
                    Children.Add(c);
                else
                {
                    Children.Insert(Children.FindIndex(a => a.Layer == c.Layer), c);
                }
            }
            else
                Children.Add(c);
        }

        public virtual void RemoveChild(GuiControl c)
        {
            c.Parent = null;
            Children.Remove(c);
        }

        public virtual void DebugRender(SpriteBatch sb)
        {
            if (Parent == null)         
                sb.Begin(rasterizerState: new RasterizerState() { ScissorTestEnable = false });

            var BorderTexture = Graphics.Materials.ColorMaterials.Red.Texture;
            var rect = Rectangle;
            var BorderSize = 5;
            var BorderColor = Color.Red;

            sb.Draw(BorderTexture, new Rectangle(rect.Location, new Vector2(Size.X, BorderSize).ToPoint()), BorderColor);
            //Left
            sb.Draw(BorderTexture, new Rectangle(rect.Location, new Vector2(BorderSize, Size.Y).ToPoint()), BorderColor);
            //Right
            sb.Draw(BorderTexture, new Rectangle(rect.Location + new Point(rect.Size.X - BorderSize, 0), new Vector2(BorderSize, Size.Y).ToPoint()), BorderColor);
            //Bottom
            sb.Draw(BorderTexture, new Rectangle(rect.Location + new Point(0, rect.Size.Y - BorderSize), new Vector2(Size.X - BorderSize, BorderSize).ToPoint()), BorderColor);

            DebugRenderChildren(sb);
            if (Parent == null)
                sb.End();

        }

        public virtual void Render(SpriteBatch sb)
        {
            SetupClipping(sb);
            RenderChildren(sb);

            if (Parent == null)
            {
                sb.End();
                GuiStaticVariables._calledBegin = false;
                sb.GraphicsDevice.RasterizerState = new RasterizerState() { ScissorTestEnable = false, DepthClipEnable = true };
                sb.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
        }

        internal void SetupClipping(SpriteBatch sb)
        {
            if (GuiStaticVariables._calledBegin)
            {
                sb.End();
                GuiStaticVariables._calledBegin = false;
                sb.GraphicsDevice.RasterizerState = new RasterizerState() { ScissorTestEnable = false, DepthClipEnable = true };
                sb.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }

            sb.GraphicsDevice.ScissorRectangle = GetRenderRectangle(sb);

            sb.Begin(rasterizerState: new RasterizerState() { ScissorTestEnable = true });
            GuiStaticVariables._calledBegin = true;

        }

        public void RenderChildren(SpriteBatch sb)
        {
            foreach (var ch in Children)
                ch.Render(sb);
        }

        public void DebugRenderChildren(SpriteBatch sb)
        {
            foreach (var ch in Children)
                ch.DebugRender(sb);
        }

        public void RegisterBindings()
        {
            Bindings = new List<IInputBinding>()
            {
                new MouseButtonInputBinding(MouseButton.Left,_MouseLeftButtonPressed,InputBindingType.Pressed),
                new MouseButtonInputBinding(MouseButton.Left,_MouseLeftButtonReleased,InputBindingType.Released),
                new MouseButtonInputBinding(MouseButton.Left,_MouseLeftButton,InputBindingType.Held),
                new MouseButtonInputBinding(MouseButton.Right,_MouseRightButtonPressed,InputBindingType.Pressed),
                new MouseButtonInputBinding(MouseButton.Right,_MouseRightButtonReleased,InputBindingType.Released),
                new MouseButtonInputBinding(MouseButton.Right,_MouseRightButton,InputBindingType.Held),
                new MouseButtonInputBinding(MouseButton.Left,_MouseMove,InputBindingType.StateChanged)
            };

            InputManager.Bindings.AddRange(Bindings);
        }

        public void UnregisterBindings()
        {
            Bindings.ForEach(a => InputManager.Bindings.Remove(a));
        }

        public bool IsInsideControl(MouseState mb)
        {
            if (new Rectangle(TotalDisplacement.ToPoint(), Size.ToPoint()).Contains(mb.X, mb.Y))
                return true;
            else
                return false;
        }

        public event GuiEvent MouseMove;

        public event GuiEvent MouseEnter;

        public event GuiEvent MouseLeave;

        public event GuiEvent MouseLeftButton;

        public event GuiEvent MouseLeftButtonPressed;

        public event GuiEvent MouseLeftButtonReleased;

        public event GuiEvent MouseRightButton;
                                   
        public event GuiEvent MouseRightButtonPressed;
                                   
        public event GuiEvent MouseRightButtonReleased;

        void _MouseMove(object sender,GameTime dt,MouseState mb)
        {
            if (IsInsideControl(mb) && !IsInsideControl(InputManager.PreviousMouseState)) //Mouse just entered
            {
                MouseEnter?.Invoke(sender, dt, mb,true);
                OnMouseEnter(sender, dt, mb,true);
            }

            if (!IsInsideControl(mb) && IsInsideControl(InputManager.PreviousMouseState)) //Mouse just left
            {
                MouseLeave?.Invoke(sender, dt, mb,false);
                OnMouseLeave(sender, dt, mb,false);
            }
            var inside = IsInsideControl(mb);
            MouseMove?.Invoke(sender, dt, mb,inside);
            OnMouseMove(sender, dt, mb,inside);

        }

        void _MouseLeftButton(object sender,GameTime dt,MouseState mb)
        {
            var inside = IsInsideControl(mb);
            MouseLeftButton?.Invoke(sender, dt, mb, inside );
            OnMouseLeftButton(sender, dt, mb, inside);
        }

        void _MouseLeftButtonPressed(object sender,GameTime dt,MouseState mb)
        {
            var inside = IsInsideControl(mb);
            MouseLeftButtonPressed?.Invoke(sender, dt, mb, inside);
            OnMouseLeftButtonPressed(sender, dt, mb, inside);
        }

        void _MouseLeftButtonReleased(object sender,GameTime dt,MouseState mb)
        {
            var inside = IsInsideControl(mb);
            MouseLeftButtonReleased?.Invoke(sender, dt, mb, inside);
            OnMouseLeftButtonReleased(sender, dt, mb, inside);
        }

        void _MouseRightButton(object sender,GameTime dt,MouseState mb)
        {
            var inside = IsInsideControl(mb);
            MouseRightButton?.Invoke(sender, dt, mb, inside);
            OnMouseRightButton(sender, dt, mb, inside);
        }

        void _MouseRightButtonPressed(object sender,GameTime dt,MouseState mb)
        {
            var inside = IsInsideControl(mb);
            MouseRightButtonPressed?.Invoke(sender, dt, mb, inside);
            OnMouseRightButtonPressed(sender, dt, mb, inside);
        }

        void _MouseRightButtonReleased(object sender,GameTime dt,MouseState mb)
        {
            var inside = IsInsideControl(mb);
            MouseRightButtonReleased?.Invoke(sender, dt, mb, inside);
            OnMouseRightButtonReleased(sender, dt, mb, inside);
        }

        public virtual void OnMouseEnter(object sender,GameTime dt,MouseState mb,bool inside)
        {

        }

        public virtual void OnMouseLeave(object sender,GameTime dt,MouseState mb,bool inside)
        {

        }

        public virtual void OnMouseMove(object sender,GameTime dt,MouseState mb, bool inside)
        {

        }

        public virtual void OnMouseLeftButton(object sender,GameTime dt,MouseState mb, bool inside)
        {

        }

        public virtual void OnMouseLeftButtonPressed(object sender,GameTime dt,MouseState mb, bool inside)
        {

        }

        public virtual void OnMouseLeftButtonReleased(object sender,GameTime dt,MouseState mb, bool inside)
        {

        }

        public virtual void OnMouseRightButton(object sender,GameTime dt,MouseState mb, bool inside)
        {

        }

        public virtual void OnMouseRightButtonPressed(object sender,GameTime dt,MouseState mb, bool inside)
        {

        }

        public virtual void OnMouseRightButtonReleased(object sender,GameTime dt,MouseState mb, bool inside)
        {

        }

    }
}
