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

        protected Hider opponent;

        protected LinkedList<Hider> hidersFound;

        public Seeker(Game game, World world, int countNum, int id)
            : base(game, world, PlayerPhase.Other, id)
        {
            // TODO: Construct any child components here
            this.countNum = countNum;
            //myDrawable = new MyDrawable(game, Color.Navy); //this is temporary!!!
            myDrawable.color = Color.Navy;
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
            hidersFound = new LinkedList<Hider>();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(this + " updating");
            if (nextSpace != null)
                Console.WriteLine("next space: " + nextSpace[0] + " " + nextSpace[1] + " " + nextSpace[2] + " " + nextSpace[3]);
            else
                Console.WriteLine("next space is null!");
            // TODO: Add your update code here
            for (int i = 0; i < mapX; i++)
                for (int j = 0; j < mapY; j++)
                    if (seenMap[i, j] > 0)
                        seenMap[i, j]--;
            if (phase == SeekerPhase.Counting)
            {
                Console.WriteLine(this + " counting " + count + " out of " + countNum);
                //count to whatever number was given
                count++;
                if ((Game.TargetElapsedTime.Seconds > 0 && count >= countNum / Game.TargetElapsedTime.Seconds)
                    || count >= countNum /** 60*/)
                {
                    Console.WriteLine(this + " Ready or not, here I come!");
                    phase = SeekerPhase.Looking;
                    pPhase = PlayerPhase.Looking;
                }
            }
            else if (phase == SeekerPhase.Looking)
            {
                Console.WriteLine(this + " is looking");
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
                Console.WriteLine(this + " is running");
                if (location.Z < 0)
                {
                    //if opponent has reached tree before seeker, then they won and seeker looks for more people
                    if (opponent.location.Z >= 0)
                    {
                        opponent.Win();
                        finishWithHider();
                    }
                    //keep moving.  also needs to be rewritten
                    else
                        location.Z += runSpeed;
                }
                //seeker won but needs to keep looking for more people
                else
                {
                    Win();
                    finishWithHider();
                }
            }
            base.Update(gameTime);
        }

        protected override bool act()
        {
            if (findHider() != null)
                return true;
            return false;
        }

        //Picks a hider which can be seen
        protected Hider findHider()
        {
            foreach (Hider hider in world.hiders)
                if (!hidersFound.Contains(hider) && CanSee(hider))
                {
                    Console.WriteLine(this + " I found " + hider + "!");
                    opponent = hider;
                    hidersFound.AddLast(hider);
                    hider.Found();
                    phase = SeekerPhase.Running;
                    pPhase = PlayerPhase.Running;
                    return hider;
                }
            return null;
        }

        //finish competing against a hider
        protected void finishWithHider()
        {
            opponent = null;
            if (hidersFound.Count == world.numOfHiders)
            {
                Console.WriteLine(this + " IS DONE!!!!!!!!!!!!!");
                Game.Exit();//change later!
            }
            else
            {
                Console.WriteLine(this + " finished with " + opponent + ". going to find next hider!");
                phase = SeekerPhase.Looking;
                pPhase = PlayerPhase.Looking;
                prevSpace = world.locSquare(location);//delete if unnecessary!
                nextSpace = null;
            }
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
            return location;//needs to be changed!
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

        public override string ToString()
        {
            return "Seeker " + base.ToString();
        }
    }
}
