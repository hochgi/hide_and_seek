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

        public Vector3[] limbs; //0=head, 1=r.hand, 2=l.hand, 3=r.foot, 4=l.foot.  can add more if we want...
        public Vector3 location;

        protected int runSpeed = 10;
        protected int walkSpeed = 5;

        MyDrawable myDrawable = null;

        Item goal;
        int[] prevSpace;//square player is on now
        int[] nextSpace;//next square player is going towards

        public Player(Game game, World world)
            : base(game)
        {
            this.world = world;
            myDrawable = new MyDrawable(game);
            // TODO: Construct any child components here;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            //initialize limbs!!
            goal = null;
            nextSpace = null; //0=lower x, 1=lower y, 2=upper x, 3=upper y
            prevSpace = null; //should be initialized to initial location
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (nextSpace != null && nextSpace.Length == 4)
            {
                // if player has reached the next square then:
                if (location.X >= nextSpace[0] && location.Z >= nextSpace[1] && location.X <= nextSpace[2] && location.Z <= nextSpace[3])
                {
                    //update the player's location
                    world.updateLocation(prevSpace, nextSpace);
                    // do what needs to be done, if need to keep moving then find next square
                    if (!act())
                    {
                        prevSpace = nextSpace;
                        nextSpace = getNextSpace();
                    }
                }
                // move towards next square
                else
                {
                    if (location.X < nextSpace[0])
                        location.X += walkSpeed;
                    else if (location.X > nextSpace[2])
                        location.X -= walkSpeed;
                    if (location.Z < nextSpace[1])
                        location.Z += walkSpeed;
                    else if (location.Z > nextSpace[3])
                        location.Z -= walkSpeed;
                }
            }

            base.Update(gameTime);
        }

        // get next square player needs to move to
        private int[] getNextSpace()
        {
            return world.getNextSpace(location);
        }

        // do what needs to be done at certain place and check if need to keep moving
        private abstract bool act();

        public void Win()
        {
            //victory dance and whatever
        }
    }
}
