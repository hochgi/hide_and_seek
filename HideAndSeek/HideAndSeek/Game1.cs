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

        //CHANGED - 2012.11.28 - Gilad (trying out a simple drawing of a tree)
        Texture2D m_tree;
        Rectangle m_spriteArea;
        Vector3 m_treeLocation;

        GameType currentGameType = GameType.MainMenu;
        FlickeringButton btnPlay1, btnPlay2, btnPlay3, btnPlay4, btnPlay5;

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
            //World.getWorld(this);
            m_CameraTargetPosition = new Vector3(0, 0, -100);
            m_CameraUpDirection = new Vector3(0, 1, 0);

            IsMouseVisible = true;
            IsFixedTimeStep = false;//i'm not sure this should be the case, but it's the only way the graphics look ok for now

            //CHANGED - 2012.11.28 - Gilad (trying out a simple drawing of a tree)
            m_spriteArea = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            m_treeLocation = new Vector3(0, 0, -200);//currently not used

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
            if (World.getWorld() != null && World.getWorld().humanPlayer != null)
            {
                m_CameraLocation = World.getWorld().humanPlayer.location + new Vector3(0, 20, 0);//change when we know the position of the human's eyes
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
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();
            //CHANGED - 2012.11.28 - Gilad (trying out a simple drawing of a tree)
            m_tree = this.Content.Load<Texture2D>("tree");
            btnPlay1 = new FlickeringButton(Content.Load<Texture2D>("btnSeek"), graphics.GraphicsDevice, 1);
            btnPlay2 = new FlickeringButton(Content.Load<Texture2D>("btnHide"), graphics.GraphicsDevice, 2);
            btnPlay3 = new FlickeringButton(Content.Load<Texture2D>("btnSeekPractice"), graphics.GraphicsDevice, 3);
            btnPlay4 = new FlickeringButton(Content.Load<Texture2D>("btnHidePractice"), graphics.GraphicsDevice, 4);
            btnPlay5 = new FlickeringButton(Content.Load<Texture2D>("btnExit"), graphics.GraphicsDevice, 5);
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

            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // TODO: Add your update logic here
            MouseState mouse = Mouse.GetState();
            World w = World.getWorld();

            switch (currentGameType)
            {
                case GameType.MainMenu:
                    if (btnPlay1.isClicked) currentGameType = GameType.Seek;
                    else if (btnPlay2.isClicked) currentGameType = GameType.Hide;
                    else if (btnPlay3.isClicked) currentGameType = GameType.SeekPractice;
                    else if (btnPlay4.isClicked) currentGameType = GameType.HidePractice;
                    else if (btnPlay5.isClicked) currentGameType = GameType.Exit;
                    btnPlay1.Update(mouse);
                    btnPlay2.Update(mouse);
                    btnPlay3.Update(mouse);
                    btnPlay4.Update(mouse);
                    btnPlay5.Update(mouse);
                    break;
                case GameType.Seek:
                    if (w == null) 
                    {
                        setCameraSettings();
                        setCameraState();
                        IsMouseVisible = false;
                        w = World.getWorld(this, GameType.Seek);
                    }
                    break;
                case GameType.Hide:
                    if (w == null) 
                    {
                        setCameraSettings();
                        setCameraState();
                        IsMouseVisible = false;
                        w = World.getWorld(this, GameType.Hide);
                    }
                    break;
                case GameType.SeekPractice:
                    if (w == null)
                    {
                        setCameraSettings();
                        setCameraState();
                        IsMouseVisible = false;
                        w = World.getWorld(this, GameType.SeekPractice);
                    }
                    break;
                case GameType.HidePractice:
                    if (w == null)
                    {
                        setCameraSettings();
                        setCameraState();
                        IsMouseVisible = false;
                        w = World.getWorld(this, GameType.HidePractice);
                    }
                    break;
                case GameType.Exit:
                    Exit();
                    break;
            }

            //CHANGED - 2012.11.28 - Gilad (trying out a simple drawing of a tree)
            //TODO: make a billboard of the tree, that is perpendicular to the camera

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (currentGameType == GameType.MainMenu)
            {
                GraphicsDevice.Clear(Color.White);

                spriteBatch.Begin();
                {
                    btnPlay1.Draw(spriteBatch);
                    btnPlay2.Draw(spriteBatch);
                    btnPlay3.Draw(spriteBatch);
                    btnPlay4.Draw(spriteBatch);
                    btnPlay5.Draw(spriteBatch);
                }
                spriteBatch.End();
            }
            else
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

                //CHANGED - 2012.11.28 - Gilad (trying out a simple drawing of a tree)
                //spriteBatch.Begin();
                //spriteBatch.Draw(m_tree, m_spriteArea, new Color(255, 255, 255, 255));
                //spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
