using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SimpleMono3D.Input;

namespace SimpleMono3D.Graphics
{
    public enum CameraMode
    {
        FreeOrbit,
        FixedOrbit
    }

    public class Camera
    {
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }

        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Up;

        public Vector3 LookAtPoint;

        public CameraMode Mode = CameraMode.FixedOrbit;

        public float CameraSensitivity = 0.6f;
        public float CameraPanSensitivity = 0.05f;
        public float CameraZoomSensitivity = 0.7f;
        public float CameraZoomSpeed = 15f;
        public float CameraZoomDeceleration = 250f;

        public float CameraSpeed = 15f;

        float camZoomSpeed;

        public float CameraDistance = 5;
        public float CameraMinDistance = 1;
        public float CameraMaxDistance = 25;

        public float CameraPitch;
        public float CameraYaw;

        int centerX;
        int centerY;


        List<IInputBinding> RegisteredBindings;


        MouseState prevMouseState;

        GameWindow gw;

        public Camera(GameWindow window,Vector3 up,Vector3 LookAt)
        {
            Up = up;
            gw = window;
            LookAtPoint = LookAt;

            prevMouseState = Mouse.GetState();

            centerX = window.ClientBounds.Width/2;
            centerY = window.ClientBounds.Height/2;

            CreateLookAt();
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)window.ClientBounds.Width / (float)window.ClientBounds.Height, 0.5f, 2000);
        }

        void CreateLookAt()
        {
            View = Matrix.CreateLookAt(Position, LookAtPoint, Up);
        }

        public void Update(GameTime delta)
        {
            CreateLookAt();
        }

        #region Default Controls
        public void MoveForward(object sender,GameTime delta,KeyboardState state)
        {
            this.LookAtPoint += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * Direction ;
            this.Position += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * Direction;
        }

        public void MoveLeft(object sender, GameTime delta, KeyboardState state)
        {
            var sideways = -Vector3.Cross(Direction, Up);
            sideways.Normalize();

            this.LookAtPoint += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * sideways;
            this.Position += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * sideways;
        }

        public void MoveRight(object sender, GameTime delta, KeyboardState state)
        {
            var sideways = Vector3.Cross(Direction, Up);
            sideways.Normalize();

            this.LookAtPoint += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * sideways;
            this.Position += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * sideways;
        }

        public void MoveBackward(object sender, GameTime delta, KeyboardState state)
        {
            this.LookAtPoint -= (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * Direction;
            this.Position -= (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * Direction;
        }
        #endregion
        //Editor - movement
        #region Editor Controls
        public void MoveForward2D(object sender, GameTime delta, KeyboardState state)
        {
            var dir = Direction.Project(Up);

            this.LookAtPoint += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * dir;
            this.Position += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * dir;
        }

        public void MoveLeft2D(object sender, GameTime delta, KeyboardState state)
        {
            var dir = Direction.Project(Up);
            var sideways = -Vector3.Cross(dir, Up);
            sideways.Normalize();

            this.LookAtPoint += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * sideways;
            this.Position += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * sideways;
        }

        public void MoveRight2D(object sender, GameTime delta, KeyboardState state)
        {
            var dir = Direction.Project(Up);
            var sideways = Vector3.Cross(dir, Up);
            sideways.Normalize();

            this.LookAtPoint += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * sideways;
            this.Position += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * sideways;
        }

        public void MoveBackward2D(object sender, GameTime delta, KeyboardState state)
        {
            var dir = Direction.Project(Up);

            this.LookAtPoint -= (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * dir;
            this.Position -= (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed *dir;
        }

        public void MoveUp2D(object sender, GameTime delta, KeyboardState state)
        {
            var dir = Up;

            this.LookAtPoint += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * dir;
            this.Position += (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * dir;
        }
        public void MoveDown2D(object sender, GameTime delta, KeyboardState state)
        {
            var dir = Up;

            this.LookAtPoint -= (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * dir;
            this.Position -= (float)delta.ElapsedGameTime.TotalSeconds * CameraSpeed * dir;
        }
        #endregion

        public void Pan(object sender,GameTime delta, MouseState mb)
        {
            var cursorPos = mb.Position.ToVector2();

            var deltapos = new Vector2(centerX - cursorPos.X, centerY - cursorPos.Y);

            var dir = Direction.Project(Up);

            var sideways = Vector3.Cross(dir, Up);
            sideways.Normalize();

            var forwardvec = -CameraPanSensitivity * deltapos.Y * dir;
            var sidewaysvec = CameraPanSensitivity * deltapos.X * sideways;

            var resultant = forwardvec + sidewaysvec;

            Mouse.SetPosition(centerX, centerY);

            this.LookAtPoint += resultant;
            this.Position += resultant;
        }

        public void Orbit(object sender,GameTime delta,MouseState mb)
        {
            var cursorPos = mb.Position.ToVector2();

            var deltapos = new Vector2(centerX - cursorPos.X, centerY - cursorPos.Y);

            Mouse.SetPosition(centerX, centerY);

            var rotX = (MathHelper.TwoPi * CameraSensitivity * deltapos.X * 0.001f); //* (float)delta.ElapsedGameTime.TotalSeconds);
            var rotY = (MathHelper.TwoPi * CameraSensitivity * deltapos.Y * 0.001f); //* (float)delta.ElapsedGameTime.TotalSeconds);          

            CameraYaw -= rotX;
            CameraPitch += rotY;

            if (rotX != 0 || rotY != 0 || camZoomSpeed != 0)
            {
                var relcamdir = LookAtPoint - Position;
                var relcampos = relcamdir;
                relcamdir.Normalize();

                //Totally original code i swear
                switch (Mode)
                {
                    case CameraMode.FreeOrbit:
                        var rotmatrix = Matrix.CreateFromAxisAngle(Up, rotX);
                        var otherdir = Vector3.Cross(relcamdir, Up);
                        otherdir.Normalize();
                        rotmatrix *= Matrix.CreateFromAxisAngle(otherdir, rotY);

                        var newrelpos = Vector3.Transform(relcampos, rotmatrix);

                        newrelpos.Normalize();
                        newrelpos *= CameraDistance;

                        Position = LookAtPoint - newrelpos;

                        Direction = newrelpos;
                        Direction.Normalize();

                        Up = Vector3.Cross(otherdir, newrelpos);
                        Up.Normalize();
                        break;

                    case CameraMode.FixedOrbit:

                        if (CameraPitch > MathHelper.Pi) CameraPitch = 3.1415f;
                        if (CameraPitch < 0) CameraPitch = 0.000001f;

                        var x = CameraDistance * Math.Sin(CameraPitch) * Math.Cos(CameraYaw);
                        var y = CameraDistance * Math.Sin(CameraPitch) * Math.Sin(CameraYaw);
                        var z = CameraDistance * Math.Cos(CameraPitch);

                        var newrelpos2 = new Vector3((float)x, (float)z, (float)y);

                        Position = LookAtPoint + newrelpos2;

                        Direction = -newrelpos2;
                        Direction.Normalize();
                        break;

                }
            }
        }

        public void Zoom(object sender,GameTime delta,MouseState mb)
        {
            if (prevMouseState != null)
            {
                var deltascroll = (prevMouseState.ScrollWheelValue - mb.ScrollWheelValue) / 24;

                if (deltascroll != 0)
                {
                    camZoomSpeed = CameraZoomSpeed * deltascroll * CameraZoomSensitivity;
                }

                if (camZoomSpeed != 0)
                {
                    CameraDistance += camZoomSpeed * (float)delta.ElapsedGameTime.TotalSeconds;

                    CameraDistance = MathHelper.Clamp(CameraDistance, CameraMinDistance, CameraMaxDistance);

                    camZoomSpeed += (-1) * Math.Sign(camZoomSpeed) * CameraZoomDeceleration * (float)delta.ElapsedGameTime.TotalSeconds;
                }

                if (Math.Abs(camZoomSpeed) < CameraZoomDeceleration * (float)delta.ElapsedGameTime.TotalSeconds)
                    camZoomSpeed = 0;
            }
            prevMouseState = mb;
        }

        public void RegisterDefaultBindings()
        {
            RegisteredBindings = new List<IInputBinding>()
            {
                new KeyboardInputBinding(Keys.W, MoveForward,InputBindingType.Held),
                new KeyboardInputBinding(Keys.S, MoveBackward, InputBindingType.Held),
                new KeyboardInputBinding(Keys.A, MoveLeft, InputBindingType.Held),
                new KeyboardInputBinding(Keys.D, MoveRight, InputBindingType.Held),
                new MouseButtonInputBinding(MouseButton.Left,Orbit,InputBindingType.None)
            };
            InputManager.Bindings.AddRange(RegisteredBindings);
        }

        public void RegisterEditorBindings()
        {
            RegisteredBindings = new List<IInputBinding>()
            {
                new KeyboardInputBinding(Keys.W, MoveForward2D,InputBindingType.Held),
                new KeyboardInputBinding(Keys.S, MoveBackward2D, InputBindingType.Held),
                new KeyboardInputBinding(Keys.A, MoveLeft2D, InputBindingType.Held),
                new KeyboardInputBinding(Keys.D, MoveRight2D, InputBindingType.Held),
                new KeyboardInputBinding(Keys.Q, MoveDown2D, InputBindingType.Held),
                new KeyboardInputBinding(Keys.E, MoveUp2D, InputBindingType.Held),
                new MouseButtonInputBinding(MouseButton.Left,Pan,InputBindingType.Held),
                new MouseButtonInputBinding(MouseButton.Left,Orbit,InputBindingType.None),
                new MouseButtonInputBinding(MouseButton.Left,Zoom,InputBindingType.None)
            };
            InputManager.Bindings.AddRange(RegisteredBindings);
        }

        public void RegisterCustomBindings(List<IInputBinding> bindings)
        {
            RegisteredBindings = bindings;
            InputManager.Bindings.AddRange(bindings);
        }

        public void ClearBindings()
        {
            foreach (var binding in RegisteredBindings)
            {
                if (InputManager.Bindings.Contains(binding))
                    InputManager.Bindings.Remove(binding);
            }
        }
    }
}

