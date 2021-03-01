using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleMono3D.Graphics;
using SimpleMono3D.Input;
using System;

namespace SimpleMono3D
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SimpleMono3D : Game
    {
        public static SimpleMono3D Instance;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Scene Scene;
        Effect flatShader;
        Effect instancedFlatShader;
        Effect skyboxShader;
        public Effect GuiRenderEffect;
        Model skyboxModel;
        TextureCube skyboxTexture;
        public SpriteFont defaultFont;
        string skyboxTexturePath;
        public SimpleMono3D(string skybox)
        {
            graphics = new GraphicsDeviceManager(this) { PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8 };
            Content.RootDirectory = "Content";
            skyboxTexturePath = skybox;
        }

        //OVERRIDE THIS 
        [STAThread]
        public static void StartGame(Type gameType,string skybox)
        {
            Instance = (SimpleMono3D)Activator.CreateInstance(gameType, skybox);//new SimpleMono3D(skybox);
            using (var game = Instance)
                Instance.Run();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            this.IsFixedTimeStep = false;
            //this.TargetElapsedTime = System.TimeSpan.FromMilliseconds(16);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            flatShader = Content.Load<Effect>("FlatEffect");
            instancedFlatShader = Content.Load<Effect>("FlatInstancedEffect");
            skyboxShader = Content.Load<Effect>("SkyboxEffect");
            skyboxModel = Content.Load<Model>("SkyboxCube");
            defaultFont = Content.Load<SpriteFont>("DefaultFont");
            GuiRenderEffect = Content.Load<Effect>("GuiRenderEffect");

            skyboxTexture = Content.Load<TextureCube>(skyboxTexturePath);
            Scene = new Scene(graphics.GraphicsDevice, spriteBatch,Window, flatShader, skyboxShader, skyboxTexture, skyboxModel,instancedFlatShader);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputManager.Update(gameTime);
            Scene.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            Scene.Render();
            base.Draw(gameTime);
        }
    }
}
