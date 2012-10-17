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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class Player : Microsoft.Xna.Framework.GameComponent
    {
        protected World world;

        public Vector3 location;
        protected float[] prevSpace;

        protected int walkSpeed;
        protected int runSpeed;

        protected int id;

        public Player(Game game, World world, Vector3 location, int walkSpeed, int runSpeed, int id)
            : base(game)
        {
            this.world = world;
            this.location = location;
            this.walkSpeed = walkSpeed;
            this.runSpeed = runSpeed;
            this.id = id;
            Game.Components.Add(this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            prevSpace = world.locSquare(location);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        //Player's reaction to winning the game
        public abstract void win();

        public override string ToString()
        {
            return id + " at " + location;
        }
    }
}
