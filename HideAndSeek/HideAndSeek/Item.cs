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
    public class Item : Microsoft.Xna.Framework.GameComponent
    {
        World world;

        public Vector3 location;
        public Vector3 size; //X = W, Y = H, Z = D
        //add item type for drawing reasons!!

        public bool taken;
        public Hider hider;

        public Item(Game game, Vector3 loc, Vector3 size, int type, World world)
            : base(game)
        {
            // TODO: Construct any child components here

            this.world = world;
            this.location = loc;
            this.size = size;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            taken = false;
            hider = null;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        //drawing code
        public DrawableGameComponent GetDrawable()
        {
            return null;
        }

        //checks whether item is blocking seeker from seeing hider
        public bool IsBlocking(Seeker seeker, Hider hider)
        {
            return false;
        }
    }
}
