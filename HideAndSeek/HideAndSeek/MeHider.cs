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
    //phase enum!!!

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class MeHider : Hider
    {
        Me myInput;

        public Vector3 location;
        private int walkRate;

        public MeHider(Game game, World world, int id)
            : base(game, world, id)
        {
            // TODO: Construct any child components here
            this.world = world;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();

            myInput = new KinectMe(Game);

            location = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (world.gameType == GameType.HidePractice) //or other situations! 
            {
                if (myInput.isWalking())
                    location.Z += walkRate;
                //else if (myInput.isWalkingRight())
                //    location.X += walkRate;
                //else if (myInput.isWalkingLeft())
                //    location.X -= walkRate;
            }
            base.Update(gameTime);
        }

        public override string ToString()
        {
            return "Me " + base.ToString();
        }
    }
}
