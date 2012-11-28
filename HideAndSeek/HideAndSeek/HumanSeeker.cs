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
    //represents seeker played by human
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HumanSeeker : HumanPlayer, Seeker
    {
        SeekerImp seeker;

        //number to count up to
        int countNum;
        //number counted so far
        int count;

        //Constructor for HumanSeeker class
        public HumanSeeker(Game game, World world, Vector3 location, int walkSpeed, int runSpeed, int id, int countNum)
            : base(game, world, location, walkSpeed, runSpeed, id, false)
        {
            this.countNum = countNum;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            seeker = new SeekerImp(world);
            count = 0;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(this + " updating...");
            //if not done counting yet
            if (count < countNum)
            {
                int lastCount = count + 1;//lastCount should be the number obtained from voice input
                if (lastCount == count + 1)
                    count++;
                if (count == countNum)
                    counting = false;
            }
            //if Seeker isn't racing against anyone at the moment
            else if (seeker.opponent == null)
            {
                //if player is pointing
                if (myInput.isPointing())
                {
                    //check if player is pointing at a specific hider and if so start racing against them
                    Hider hider = selectHider();
                    if (hider != null)
                    {
                        //if player was playing in practice mode, once they found one hider the game is over.
                        if (world.gameType == GameType.SeekPractice)
                        {
                            Console.WriteLine("Good job finding that hider!");
                            Game.Exit();
                        }
                        seeker.hiderFound(hider);
                    }
                }
            }
            //if seeker is racing
            else
            {
                //check the status of the seeker vs. the hider
                SeekerStatus status = seeker.Status(location);
                //if the seeker won
                if (status == SeekerStatus.Won || status == SeekerStatus.WonDone)
                    win();
                //competitor was the last hider, exit the game
                if (status == SeekerStatus.Done || status == SeekerStatus.WonDone)
                    Game.Exit();
            }

            base.Update(gameTime);
        }

        //choose which hider seeker was pointing at
        public Hider selectHider()
        {
            //follow arm and find out who player is pointing at.  if nobody, return null.
            //temporary code!
            foreach (Hider hider in world.hiders)
                //if seeker has not yet found hider, and notices them
                if (!seeker.foundYet(hider))
                {
                    bool blocked = false;
                    for (int j = 0; j < world.numOfItems; j++)
                    {
                        //if seeker can't see hider
                        if (world.items[j].IsBlocking(getEyesPosition(), hider.Location))
                        {
                            blocked = true;
                        }
                    }
                    if (!blocked)
                        return hider;
                }
            return null;
            //end of temporary code!!!
            throw new NotImplementedException();
        }

        //returns the location of player's eyes
        public Vector3 getEyesPosition()
        {
            return myInput.getHeadPosition();//not so great...
        }

        //returns a string representation of the human seeker
        public override string ToString()
        {
            return "Seeker " + base.ToString();
        }

        public Vector3 Location
        {
            get { return location; }
        }

        internal void skipCounting()
        {
            counting = false;
        }
    }
}
