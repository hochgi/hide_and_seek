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

        public Vector3 prevHead;

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

            //myInput = new KinectMe(Game);
            myInput = new KeyboardMe(Game);

            prevHead = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //may want to try to get speed from user instead of using walkSpeed.  Don't forget to make all changes in Seeker too!
            WalkingState state = myInput.getWalkingState();
            if (state == WalkingState.Forwards)
            {
                Console.WriteLine(this + " Walking forwards");
                location.Z -= walkSpeed;
            }
            else if (state == WalkingState.Backwards)
            {
                Console.WriteLine(this + " Walking backwards");
                location.Z += walkSpeed;
            }
            Vector3 tempHead = prevHead;
            prevHead = myInput.getHeadPosition();
            location = location - tempHead + prevHead;
            //Basically the hider is a totally passive position.  All that happens is the location is updated, and we just hope
            //that at some point he'll make it back to the tree...
            //base.Update(gameTime);
        }

        public override string ToString()
        {
            return "Me " + base.ToString();
        }
    }
}
