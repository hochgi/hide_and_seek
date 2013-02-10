using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HideAndSeek
{

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public BasicEffect m_effect;//public??
        Matrix m_CameraSettings;
        Matrix m_CameraState;
        Vector3 m_CameraTargetPosition;
        Vector3 m_CameraLocation;
        Vector3 m_CameraUpDirection;
        RasterizerState m_RasterizerState;

        World world;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            m_CameraTargetPosition = new Vector3(0, 0, -100);
            m_CameraLocation = new Vector3(0, 30, 10);//fix this, not good!  should be user's real location.
            m_CameraUpDirection = new Vector3(0, 1, 0);

            setCameraSettings();
            setCameraState();

            world = new World(this);

            IsFixedTimeStep = false;//i'm not sure this should be the case, but it's the only way the graphics look ok for now

            Console.WriteLine("Game Initialize Debugging:");
            Console.WriteLine("IsFixedTimeStep " + IsFixedTimeStep);
            Console.WriteLine("TargetElapsedTime " + TargetElapsedTime);

            base.Initialize(); 
        }

        private void setCameraSettings()
        {
            float k_nearPlaneDistance = 0.5f;
            float k_farPlaneDistance = 1000.0f;
            float k_ViewAngle = MathHelper.PiOver4;

            m_CameraSettings = Matrix.CreatePerspectiveFieldOfView(
                k_ViewAngle, GraphicsDevice.Viewport.AspectRatio, k_nearPlaneDistance, k_farPlaneDistance);
        }

        private void setCameraState()
        {
            if (world != null && world.humanPlayer != null)
            {
                m_CameraLocation = world.humanPlayer.location + new Vector3(0, 20, 0);//change!!
                m_CameraTargetPosition = m_CameraLocation;
                m_CameraTargetPosition.Z -= 50;
            }
            Console.WriteLine("m_CameraLocation " + m_CameraLocation);
            m_CameraState = Matrix.CreateLookAt(m_CameraLocation, m_CameraTargetPosition, m_CameraUpDirection);
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            m_effect = new BasicEffect(this.GraphicsDevice);
            m_effect.VertexColorEnabled = true;

            m_RasterizerState = new RasterizerState();
            m_RasterizerState.CullMode = CullMode.None;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            Console.WriteLine("Game Debugging: " + gameTime.ElapsedGameTime);
            Console.WriteLine("IsFixedTimeStep " + IsFixedTimeStep);
            Console.WriteLine("TargetElapsedTime " + TargetElapsedTime);
            Console.WriteLine("IsRunningSlowly " + gameTime.IsRunningSlowly);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //m_CameraLocation = humanPlayer.location;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            // TODO: Add your drawing code here
            m_effect.GraphicsDevice.RasterizerState = m_RasterizerState;

            setCameraState();

            m_effect.View = m_CameraState;
            m_effect.Projection = m_CameraSettings;
            m_effect.World = Matrix.Identity;

            foreach (EffectPass pass in m_effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            base.Draw(gameTime);
        }
    }
}
