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
    public enum SeekerPhase { Counting, Looking, Running, Done };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Seeker : Player
    {

        SeekerPhase phase;

        private int countNum;
        private int count;

        int mapX;
        int mapY;
        private int[,] seenMap;

        Hider opponent;

        private int hidersFound;

        public Seeker(Game game, World world, int countNum)
            : base(game, world, PlayerPhase.Other)
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
            opponent = null;
            phase = SeekerPhase.Counting;
            count = 0;
            mapX = world.mapSizeX();
            mapY = world.mapSizeY();
            seenMap = new int[mapX, mapY];
            for (int i = 0; i < mapX; i++)
                for (int j = 0; j < mapY; j++)
                    seenMap[i, j] = 0;
            base.Initialize();
            hidersFound = 0;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            for (int i = 0; i < mapX; i++)
                for (int j = 0; j < mapY; j++)
                    if (seenMap[i, j] > 0)
                        seenMap[i, j]--;
            if (phase == SeekerPhase.Counting)
            {
                //count to whatever number was given
                count++;
                if (count >= countNum / Game.TargetElapsedTime.Seconds)
                {
                    phase = SeekerPhase.Looking;
                    pPhase = PlayerPhase.Looking;
                }
            }
            else if (phase == SeekerPhase.Looking)
            {
                ////choose item and go to it and see if there is a person behind it.  all of this code needs to be rewritten
                //if (nextItem == null)
                //{
                //    if (seenItems < world.numOfItems)
                //        nextItem = world.items[seenItems];
                //    else
                //        phase = SeekerPhase.Done;
                //}
                //else
                //{
                //    if (location.Z > nextItem.location.Z)
                //        location.Z -= walkSpeed;
                //    else
                //    {
                //        if (location.X > nextItem.location.X)
                //            location.X -= walkSpeed;
                //        else if (location.X < nextItem.location.X)
                //            location.X += walkSpeed;
                //        else
                //        {
                //            if (nextItem.taken == true)
                //            {
                //                // say i found you!
                //                opponent = nextItem.hider;
                //                opponent.Found();
                //                phase = SeekerPhase.Running;
                //            }
                //        }
                //    }
                //}
            }
            else if (phase == SeekerPhase.Running)
            {
                if (location.Z < 0)
                {
                    //if opponent has reached tree before seeker, then they won and seeker looks for more people
                    if (opponent.location.Z >= 0)
                    {
                        opponent.Win();
                        if (hidersFound < world.numOfHiders)
                        {
                            phase = SeekerPhase.Looking;
                            pPhase = PlayerPhase.Looking;
                            prevSpace = world.locSquare(location);//delete if unnecessary!
                            nextSpace = null;
                        }
                        else
                        {
                            phase = SeekerPhase.Done;
                            pPhase = PlayerPhase.Other;
                        }
                    }
                    //keep moving.  also needs to be rewritten
                    else
                        location.Z += runSpeed;
                }
                //seeker won but needs to keep looking for more people
                else
                {
                    Win();
                    if (hidersFound < world.numOfHiders)
                    {
                        phase = SeekerPhase.Looking;
                        pPhase = PlayerPhase.Looking;
                        prevSpace = world.locSquare(location);//delete if unnecessary!
                        nextSpace = null;
                    }
                    else
                    {
                        phase = SeekerPhase.Done;
                        pPhase = PlayerPhase.Other;
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override bool act()
        {
            foreach (Hider hider in world.hiders)
                if (hider.phase != HiderPhase.Done && CanSee(hider))
                {
                    opponent = hider;
                    hidersFound++;
                    hider.Found();
                    phase = SeekerPhase.Running;
                    pPhase = PlayerPhase.Running;
                    return true;
                }
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

        protected override float[] getNextSpace()
        {
            LinkedList<FieldNode> sons = world.getSons(location);
            foreach (FieldNode node in sons)
                if (seenMap[node.x, node.y] == 0)
                    return world.nodeToLoc(node);
            FieldNode best = null;
            int bestVal = 100;
            foreach (FieldNode node in sons)
                if (seenMap[node.x, node.y] < bestVal)
                {
                    best = node;
                    bestVal = seenMap[node.x, node.y];
                }
            return world.nodeToLoc(best);
        }
    }
}
