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
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < mapX; i++)
                for (int j = 0; j < mapY; j++)
                    if (seenMap[i, j] > 0)
                        seenMap[i, j]--;
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
                    phase = Phase.Looking;
            }
            base.Update(gameTime);
        }

        public Hider selectHider()
        {
            foreach (Hider hider in world.hiders)
                if (!seeker.foundYet(hider) && CanSee(hider))
                    return hider;
            return null;
        }

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

        public Vector3 getEyesPosition()
        {
            return new Vector3(location.X, location.Y + 9, location.Z);//needs to be changed!
        }
    }
}
