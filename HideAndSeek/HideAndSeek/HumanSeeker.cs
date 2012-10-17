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
    public class HumanSeeker : HumanPlayer, Seeker
    {
        SeekerImp seeker;

        int countNum;
        int count;

        public HumanSeeker(Game game, World world, Vector3 location, int walkSpeed, int runSpeed, int id, int countNum)
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
            if (count < countNum)
            {
                int lastCount = 0;//get this value from input!
                if (lastCount == count + 1)
                    count++;
            }
            else if (seeker.opponent == null)
            {
                if (myInput.isPointing())
                {
                    Hider hider = selectHider();
                    if (hider != null)
                        seeker.hiderFound(hider);
                }
            }
            else
            {
                SeekerStatus status = seeker.Status(location);
                if (status == SeekerStatus.Won || status == SeekerStatus.WonDone)
                    win();
                if (status == SeekerStatus.Done || status == SeekerStatus.WonDone)
                    Game.Exit();
            }

            base.Update(gameTime);
        }

        public Hider selectHider()
        {
            //follow arm and find out who player is pointing at.  if nobody, return null.
            throw new NotImplementedException();
        }

        public Vector3 getEyesPosition()
        {
            return myInput.getHeadPosition();//not so great...
        }

        public override string ToString()
        {
            return "Seeker " + base.ToString();
        }
    }
}
