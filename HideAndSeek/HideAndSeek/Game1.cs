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
        //variables for drawing objects
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public BasicEffect m_effect;
        Matrix m_CameraSettings;
        Matrix m_CameraState;
        Vector3 m_CameraTargetPosition;
        Vector3 m_CameraLocation;
        Vector3 m_CameraUpDirection;
        RasterizerState m_RasterizerState;

        //the world, which holds all of the variables and components for the game
        World world;

        //constructor for Game1 class
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
            m_CameraTargetPosition = new Vector3(0, 0, -100);
            m_CameraUpDirection = new Vector3(0, 1, 0);

            setCameraSettings();
            setCameraState();

            world = new World(this);

            IsFixedTimeStep = false;//i'm not sure this should be the case, but it's the only way the graphics look ok for now

            base.Initialize(); 
        }

        //basic definitions for the camera
        private void setCameraSettings()
        {
            float k_nearPlaneDistance = 0.5f;
            float k_farPlaneDistance = 1000.0f;
            float k_ViewAngle = MathHelper.PiOver4;

            m_CameraSettings = Matrix.CreatePerspectiveFieldOfView(
                k_ViewAngle, GraphicsDevice.Viewport.AspectRatio, k_nearPlaneDistance, k_farPlaneDistance);
        }

        //update camera's position with location of human player
        private void setCameraState()
        {
            if (world != null && world.humanPlayer != null)
            {
                m_CameraLocation = world.humanPlayer.location + new Vector3(0, 20, 0);//change when we know the position of the human's eyes
            }
            else
                m_CameraLocation = new Vector3(0, 0, 0);
            m_CameraTargetPosition = m_CameraLocation;
            m_CameraTargetPosition.Z -= 50;
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

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
