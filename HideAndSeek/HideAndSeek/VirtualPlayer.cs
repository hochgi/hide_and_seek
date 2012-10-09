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
    public enum Phase { Counting, Looking, Hiding, Running, Done }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class VirtualPlayer : Player
    {
        protected Phase phase;

        protected float[] nextSpace;

        MyDrawable myDrawable;

        public VirtualPlayer(Game game, World world, Vector3 location, int walkSpeed, int runSpeed, int id)
            : base(game, world, location, walkSpeed, runSpeed, id)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            nextSpace = null;
            myDrawable = new MyDrawable(Game, Color.PaleVioletRed, location, new Vector3(11, 11, 11));

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (phase == Phase.Looking)
            {
                if (nextSpace != null && nextSpace.Length == 4)
                {
                    // if player has reached the next square then:
                    if (location.X >= nextSpace[0] && location.Z <= nextSpace[1] && location.X <= nextSpace[2] && location.Z >= nextSpace[3])
                    {
                        Console.WriteLine(this + " reached next square!");
                        //update the player's location
                        world.updateLocation(prevSpace, nextSpace);
                        //Console.WriteLine(world.map);
                        // do what needs to be done, if need to keep moving then find next square
                        if (!act())
                        {
                            Console.WriteLine(this + " action was not successful.  Going to keep looking.");
                            prevSpace = nextSpace;
                            nextSpace = getNextSpace();
                        }
                    }
                    // move towards next square
                    else
                    {
                        if (!world.isAvailable(nextSpace))
                            nextSpace = getNextSpace();
                        //adjust to be more realistic (don't walk in 2 directions at same speed..)
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
                else
                    nextSpace = getNextSpace();
            }
            else if (phase == Phase.Running)
            {
                Console.WriteLine(this + " is running");
                //move towards zero, if reached zero wait by tree
                if (location.Z < 0)
                    location.Z += runSpeed;
                else
                {
                    Console.WriteLine(this + " is done!");
                    phase = Phase.Done;
                }
                //change all of this and update location!!
            }
            else if (phase == Phase.Done)
            {
                //Some done activity for until game is over
            }
            myDrawable.updateLocation(location);
            base.Update(gameTime);
        }

        public abstract bool act();

        public abstract float[] getNextSpace();

        public override void win()
        {
            //victory dance and whatever
            Console.WriteLine(this + " won!");
        }
    }
}