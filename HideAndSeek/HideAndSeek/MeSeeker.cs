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
    public class MeSeeker : Seeker
    {
        Me myInput;
        public Vector3 prevHead;

        public MeSeeker(Game game, World world, int countNum, int id)//is countnum necessary??
            : base(game, world, countNum, id)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            //myInput = new KinectMe(Game);
            myInput = new KeyboardMe(Game);

            prevHead = new Vector3(0, 0, 0);

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            WalkingState state = myInput.getWalkingState();
            if (state == WalkingState.Forwards)
                location.Z -= walkSpeed;
            else if (state == WalkingState.Backwards)
                location.Z += walkSpeed;
            Vector3 tempHead = prevHead;
            prevHead = myInput.getHeadPosition();
            location = location - tempHead + prevHead;

            if (myInput.isPointing())
            {
                findHider();
            }

            if (opponent != null)
            {
                if (location.Z >= 0)
                {
                    Win();
                    finishWithHider();
                }
                else if (opponent.location.Z >= 0)
                {
                    opponent.Win();
                    finishWithHider();
                }
            }
            //base.Update(gameTime);
        }

        public override string ToString()
        {
            return "Me " + base.ToString();
        }
    }
}
