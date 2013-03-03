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
    //class to represent any player in the game
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class Player : Microsoft.Xna.Framework.GameComponent
    {
        //player's location
        public Vector3 location;
        //borders of square in map which player is currently in.  0 = bottom X, 1 = bottom Z, 2 = top X, 3 = top Z.
        protected float[] prevSpace;

        //speed at which player walks
        protected int walkSpeed;
        //speed at which player runs
        protected int runSpeed;

        //player's ID number
        protected int id;

        //constructor for Player class
        public Player(Game game, Vector3 location, int walkSpeed, int runSpeed, int id)
            : base(game)
        {
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
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (prevSpace == null)
            {
                //initialize prevSpace to be the square in which player's location is located
                prevSpace = World.getWorld().locSquare(location);
            }
            base.Update(gameTime);
        }

        //Player's reaction to winning the game
        public abstract void win();

        //string representation of player
        public override string ToString()
        {
            return id + " at " + location;
        }
    }
}
