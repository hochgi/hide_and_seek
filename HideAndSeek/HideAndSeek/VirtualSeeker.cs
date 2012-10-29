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
    public class VirtualSeeker : VirtualPlayer, Seeker
    {
        private int countNum;
        private int count;

        int mapX;
        int mapY;
        private int[,] seenMap;

        int lowerSightBound = 50;
        int upperSightBound = 450;

        SeekerImp seeker;

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
            for (int i = 0; i < mapX; i++)
                for (int j = 0; j < mapY; j++)
                    if (seenMap[i, j] > 0)
                    {
                        //Console.WriteLine("SM " + i + " " + j + "=" + seenMap[i, j]);
                        seenMap[i, j]--;
                        //Console.WriteLine("SM " + i + " " + j + "=" + seenMap[i, j]);
                    }
            if (phase == Phase.Counting)
            {
                Console.WriteLine(this + " counting " + count + " out of " + countNum);
                //count to whatever number was given
                count++;
                if ((Game.TargetElapsedTime.Seconds > 0 && count >= countNum / Game.TargetElapsedTime.Seconds)
                    || count >= countNum /** 60*/)//figure this out
                {
                    Console.WriteLine(this + " Ready or not, here I come!");
                    phase = Phase.Looking;
                }
            }
            else if (phase == Phase.Running)
            {
                SeekerStatus status = seeker.Status(location);
                if (status == SeekerStatus.Won || status == SeekerStatus.WonDone)
                    win();
                if (status == SeekerStatus.Done || status == SeekerStatus.WonDone)
                    Game.Exit();
                if (status != SeekerStatus.None)
                {
                    phase = Phase.Looking;
                    nextSpace = null;
                }
            }
            base.Update(gameTime);
        }

        public Hider selectHider()
        {
            foreach (Hider hider in world.hiders)
                if (!seeker.foundYet(hider) && CanFind(hider))
                    return hider;
            return null;
        }

        //returns whether or not seeker notices hider
        private bool CanFind(Hider hider)
        {
            float visibleBodyParts;
            if (CanSee(hider))//change visibleBodyParts when change CanSee return value to accommodate different body parts
                visibleBodyParts = 1.0f;
            else
                visibleBodyParts = 0f;
            float distPercentage = GetDistPercentage(GetDist(hider));
            float totalChance = visibleBodyParts * distPercentage;
            Random rand = new Random();
            double randFloat = rand.NextDouble();
            if (randFloat < totalChance)
                return true;
            else
                return false;
        }

        private float GetDistPercentage(float p)
        {
            if (p > upperSightBound)
                return 0;
            if (p < lowerSightBound)
                return 1;
            float sightRange = upperSightBound - lowerSightBound;
            return p / sightRange;
        }

        private float GetDist(Hider hider)
        {
            float xDist = location.X - ((Player)hider).location.X;
            float zDist = location.Z - ((Player)hider).location.Z;
            return (float)Math.Sqrt(xDist * xDist + zDist * zDist);
        }

        //returns whether or not seeker can see hider
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

        public override bool act()
        {
            Console.WriteLine(this + " acting in space " + nextSpace[0] + " " + nextSpace[1] + " " + nextSpace[2] + " " + nextSpace[3]);
            FieldNode node = world.spaceToNode(nextSpace);
            seenMap[node.x, node.y] = 100;//change number?
            Hider hider = selectHider();
            if (hider != null)
            {
                Console.WriteLine(this + " I found " + hider + "!");
                seeker.hiderFound(hider);
                phase = Phase.Running;
                return true;
            }
            return false;
        }

        public override float[] getNextSpace()
        {
            return world.findBestNotSeen(prevSpace, seenMap);
        }

        public Vector3 getEyesPosition()
        {
            return new Vector3(location.X, location.Y + 9, location.Z);//needs to be changed!
        }

        public override string ToString()
        {
            return "Seeker " + base.ToString();
        }
    }
}
