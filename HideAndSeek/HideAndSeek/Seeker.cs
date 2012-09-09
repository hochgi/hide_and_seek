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
    enum SeekerPhase { Counting, Looking, Running, Done };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Seeker : Player
    {

        SeekerPhase phase;

        private Item nextItem;
        private int seenItems;

        private int countNum;
        private int count;

        Hider opponent;

        public Seeker(Game game, World world, int countNum)
            : base(game, world)
        {
            // TODO: Construct any child components here
            this.countNum = countNum;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            phase = SeekerPhase.Counting;
            nextItem = null;
            seenItems = 0;
            count = 0;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (phase == SeekerPhase.Counting)
            {
                //count to whatever number was given
                count++;
                if (count >= countNum / Game.TargetElapsedTime.Seconds)
                    phase = SeekerPhase.Looking;
            }
            else if (phase == SeekerPhase.Looking)
            {
                //choose item and go to it and see if there is a person behind it.  all of this code needs to be rewritten
                if (nextItem == null)
                {
                    if (seenItems < world.numOfItems)
                        nextItem = world.items[seenItems];
                    else
                        phase = SeekerPhase.Done;
                }
                else
                {
                    if (location.Z > nextItem.location.Z)
                        location.Z -= walkSpeed;
                    else
                    {
                        if (location.X > nextItem.location.X)
                            location.X -= walkSpeed;
                        else if (location.X < nextItem.location.X)
                            location.X += walkSpeed;
                        else
                        {
                            if (nextItem.taken == true)
                            {
                                // say i found you!
                                opponent = nextItem.hider;
                                opponent.Found();
                                phase = SeekerPhase.Running;
                            }
                        }
                    }
                }
            }
            else if (phase == SeekerPhase.Running)
            {
                if (location.Z < 0)
                {
                    //if opponent has reached tree before seeker, then they won and seeker looks for more people
                    if (opponent.location.Z >= 0)
                    {
                        opponent.Win();
                        phase = SeekerPhase.Looking;
                    }
                    //keep moving.  also needs to be rewritten
                    else
                        location.Z += runSpeed;
                }
                //seeker won but needs to keep looking for more people
                else
                {
                    Win();
                    phase = SeekerPhase.Looking;
                }
            }
            base.Update(gameTime);
        }

        protected override bool act()
        {
            return false;
        }

        //checks whether seeker can see given hider (will be relevant also for practice levels)
        //should be rewritten to call IsBlocking function in Item
        private bool CanSee(Hider hider)
        {
            //create line of sight from seeker's eyes to limbs[i] in hider. i.e., can seeker see limb #i?
            for (int j = 0; j < world.numOfItems; j++)
            {
                if (world.items[j].IsBlocking(this, hider))//if seeker can't see hider
                {
                    return false;
                }
            }
            return true;
        }

        public Vector3 getEyesPosition() 
        {
            throw new NotImplementedException();
        }

        protected override int[] getNextSpace()
        {
            return null;
        }
    }
}
