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
        private World world;

        SeekerPhase phase;

        private Item nextItem;
        private int seenItems;

        private int countNum;
        private int count;

        Hider opponent;

        public Seeker(Game game, World world, int countNum)
            : base(game)
        {
            // TODO: Construct any child components here
            this.world = world;
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
                count++;
                if (count >= countNum / Game.TargetElapsedTime.Seconds)
                    phase = SeekerPhase.Looking;
            }
            else if (phase == SeekerPhase.Looking)
            {
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
                    if (opponent.location.Z >= 0)
                    {
                        opponent.Win();
                        phase = SeekerPhase.Looking;
                    }
                    else
                        location.Z += runSpeed;
                }
                else
                {
                    Win();
                    phase = SeekerPhase.Looking;
                }
            }
            base.Update(gameTime);
        }

        private bool CanSee(Hider hider)
        {
            for (int i = 0; i < 5; i++)
            {
                bool visible = true;
                //create line
                for (int j = 0; j < world.numOfItems; j++)
                {
                    if (true)//if line passes through item[j]
                    {
                        visible = false;
                        break;
                    }
                }
                if (visible)
                    return true;
            }
            return false;
        }
    }
}
