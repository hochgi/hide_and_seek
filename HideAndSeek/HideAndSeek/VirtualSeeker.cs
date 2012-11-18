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
    //represents the seeker as played by the computer
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class VirtualSeeker : VirtualPlayer, Seeker
    {
        //number to count up to
        private int countNum;
        //number counted to so far
        private int count;

        //dimensions of virtual map
        int mapX;
        int mapY;
        //map of which squares seeker has checked so far
        private int[,] seenMap;

        //distance before which seeker always sees hider
        int lowerSightBound = 50;
        //distance after which seeker will never see hider
        int upperSightBound = 450;

        //implementation of seeker functions
        SeekerImp seeker;

        //constructor for VirtualSeeker class
        public VirtualSeeker(Game game, World world, Vector3 location, int walkSpeed, int runSpeed, int id, int countNum)
            : base(game, world, location, walkSpeed, runSpeed, id)
        {
            this.countNum = countNum;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            phase = Phase.Counting;
            count = 0;
            mapX = world.mapSizeX();
            mapY = world.mapSizeY();
            seenMap = new int[mapX, mapY];
            //initialize all values in map to 0, because seeker hasn't seen anything yet
            for (int i = 0; i < mapX; i++)
                for (int j = 0; j < mapY; j++)
                    seenMap[i, j] = 0;
            seeker = new SeekerImp(world);
            base.Initialize();
            myDrawable.color = Color.DodgerBlue;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(this + " updating...");
            //update values in seenMap as time has passed since seeker visited all those spaces
            for (int i = 0; i < mapX; i++)
                for (int j = 0; j < mapY; j++)
                    if (seenMap[i, j] > 0)       
                        seenMap[i, j]--;
            //if seeker is still counting
            if (phase == Phase.Counting)
            {
                Console.WriteLine(this + " counting " + count + " out of " + countNum);
                //count to whatever number was given
                count++;
                //if done counting
                if ((Game.TargetElapsedTime.Seconds > 0 && count >= countNum / Game.TargetElapsedTime.Seconds)
                    || count >= countNum /** 60*/)
                {
                    Console.WriteLine(this + " Ready or not, here I come!");
                    phase = Phase.Looking;
                }
            }
            //if seeker is running
            else if (phase == Phase.Running || phase == Phase.RunningEnd)
            {
                //check status of seeker in relation to opponent
                SeekerStatus status = seeker.Status(location);
                //if seeker has won
                if (status == SeekerStatus.Won || status == SeekerStatus.WonDone)
                    win();
                //if seeker has found all hiders, end game
                if (status == SeekerStatus.Done || status == SeekerStatus.WonDone)
                    Game.Exit();
                //if race vs. opponent has ended in any way, keep looking
                if (status != SeekerStatus.None)
                {
                    phase = Phase.Looking;
                    nextSpace = null;
                }
            }
            base.Update(gameTime);
        }

        //find a hider
        public Hider selectHider()
        {
            foreach (Hider hider in world.hiders)
                //if seeker has not yet found hider, and notices them
                if (!seeker.foundYet(hider) && CanFind(hider))
                    return hider;
            return null;
        }

        //returns whether or not seeker notices hider
        private bool CanFind(Hider hider)
        {
            float visibleBodyParts;
            if (CanSee(hider))
                visibleBodyParts = 1.0f;
            else
                visibleBodyParts = 0f;
            //calculate relative distance to hider within sight range
            float distPercentage = GetDistPercentage(GetDist(hider));
            //calculate total probability of seeker noticing hider
            float totalChance = visibleBodyParts * distPercentage;
            Random rand = new Random();
            //generate random number, if number is within probability return true.  otherwise, return false
            double randDouble = rand.NextDouble();
            if (randDouble < totalChance)
                return true;
            else
                return false;
        }

        //returns relative distance from seeker to hider within sight range
        private float GetDistPercentage(float p)
        {
            //if p is outside of range
            if (p > upperSightBound)
                return 0;
            //if p is too close to be in range
            if (p < lowerSightBound)
                return 1;
            float sightRange = upperSightBound - lowerSightBound;
            //return relative portion of distance to hider out of total sight range
            return p / sightRange;
        }

        //returns distance between seeker and hider
        private float GetDist(Hider hider)
        {
            float xDist = location.X - ((Player)hider).location.X;
            float zDist = location.Z - ((Player)hider).location.Z;
            return (float)Math.Sqrt(xDist * xDist + zDist * zDist);
        }

        //returns whether or not seeker can see hider
        private bool CanSee(Hider hider)
        {
            //for each item in world
            for (int j = 0; j < world.numOfItems; j++)
            {
                //if seeker can't see hider
                if (world.items[j].IsBlocking(this, hider))
                {
                    return false;
                }
            }
            return true;
        }

        //if arrive at new space, look to see if any hider is visible
        public override bool act()
        {
            Console.WriteLine(this + " acting in space " + nextSpace[0] + " " + nextSpace[1] + " " + nextSpace[2] + " " + nextSpace[3]);
            //mark this space as seen
            FieldNode node = world.spaceToNode(nextSpace);
            seenMap[node.x, node.y] = 100;//change number?
            //find a visible hider
            Hider hider = selectHider();
            //if a hider was found
            if (hider != null)
            {
                //register hider as found and start running
                Console.WriteLine(this + " I found " + hider + "!");
                seeker.hiderFound(hider);
                phase = Phase.Running;
                return true;
            }
            return false;
        }

        //get best space to move to in order to find more hiders
        public override float[] getNextSpace()
        {
            return world.findBestNotSeen(prevSpace, seenMap);
        }

        //return location of eyes
        public Vector3 getEyesPosition()
        {
            return new Vector3(location.X, location.Y + 9, location.Z);//needs to be changed!
        }

        //string representation of virtual seeker
        public override string ToString()
        {
            return "Seeker " + base.ToString();
        }
    }
}
