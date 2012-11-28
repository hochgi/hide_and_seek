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
    //different phases a player can be in
    public enum Phase { Counting, Looking, Hiding, Running, RunningEnd, Done }

    //represents a player played by the computer
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class VirtualPlayer : Player
    {
        //the player's phase
        protected Phase phase;

        //square on the map which player is moving towards
        protected float[] nextSpace;

        //drawing code for player
        protected MyDrawable myDrawable;

        //constructor for VirtualPlayer class
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
            //nextSpace is null because player has not yet selected a square
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
            //if player is looking (seeker may be looking for a hider; hider may be looking for a hiding spot)
            if (phase == Phase.Looking)
            {
                if (nextSpace != null && nextSpace.Length == 4)
                {
                    // if player has reached the next square then:
                    if (location.X >= nextSpace[0] && location.Z <= nextSpace[1] && location.X < nextSpace[2] && location.Z > nextSpace[3])
                    {
                        Console.WriteLine(this + " reached next square!  Walking.");
                        //update the player's location
                        world.updateLocation(prevSpace, nextSpace);
                        prevSpace = nextSpace;
                        // do what needs to be done, if need to keep moving then find next square
                        if (!act())
                        {
                            Console.WriteLine(this + " action was not successful.  Going to keep looking.");
                            nextSpace = getNextSpace();
                            if (nextSpace != null)
                                Console.WriteLine(this + " chose next space: " + nextSpace[0] + " " + nextSpace[1] + " " + nextSpace[2] + " " + nextSpace[3]);
                        }
                        else
                            nextSpace = null;
                    }
                    // move towards next square
                    else
                    {
                        //if nextSpace has become unavailable, choose a new one
                        if (!world.isAvailable(nextSpace))
                            nextSpace = getNextSpace();
                        if (nextSpace != null)
                            move(walkSpeed);
                    }
                }
                //if nextSpace is null, find a new one
                else
                    nextSpace = getNextSpace();
            }
            //if player is running
            else if (phase == Phase.Running)
            {
                if (nextSpace != null && nextSpace.Length == 4)
                {
                    // if player has reached the next square then:
                    if (location.X >= nextSpace[0] && location.Z <= nextSpace[1] && location.X < nextSpace[2] && location.Z > nextSpace[3])
                    {
                        Console.WriteLine(this + " reached next square!  Running.");
                        //update the player's location
                        world.updateLocation(prevSpace, nextSpace);
                        prevSpace = nextSpace;
                        //if we have reached the space at the end
                        if (nextSpace != null && nextSpace[1] == 0)
                        {
                            nextSpace = null;
                            phase = Phase.RunningEnd;
                        }
                        else
                            // find space which will get player back to starting point fastest
                            nextSpace = world.getNextRunSpace(location);
                    }
                    // move towards next square
                    else
                    {
                        if (!world.isAvailable(nextSpace))
                            nextSpace = getNextSpace();
                        move(runSpeed);
                    }
                }
                //if nextSpace hasn't been initialized, get a space
                else
                    nextSpace = world.getNextRunSpace(location);
            }
            else if (phase == Phase.RunningEnd)
            {
                location.Z += runSpeed;
            }
            //if player is waiting for game to be done
            else if (phase == Phase.Done)
            {
            }
            myDrawable.updateLocation(location);
            base.Update(gameTime);
        }

        private void move(int speed)
        {
            float addition = (float)(speed / Math.Sqrt(2));
            if (location.X < nextSpace[0])
            {
                if (location.Z < nextSpace[1])
                {
                    location.X += addition;
                    location.Z += addition;
                }
                else if (location.Z >= nextSpace[3])
                {
                    location.X += addition;
                    location.Z -= addition;
                }
                else
                    location.X += speed;
            }
            else if (location.X >= nextSpace[2])
            {
                if (location.Z < nextSpace[1])
                {
                    location.X -= addition;
                    location.Z += addition;
                }
                else if (location.Z >= nextSpace[3])
                {
                    location.X -= addition;
                    location.Z -= addition;
                }
                else
                    location.X -= speed;
            }
            else if (location.Z < nextSpace[1])
                location.Z += speed;
            else if (location.Z >= nextSpace[3])
                location.Z -= speed;
        }

        //action to perform when reach new square
        public abstract bool act();

        //method of choosing next space to advance to
        public abstract float[] getNextSpace();

        //reaction to winning
        public override void win()
        {
            Console.WriteLine(this + " won!");
        }

        //string representation of player
        public override string ToString()
        {
            return "Virtual " + base.ToString();
        }
    }
}