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
    public abstract class HumanPlayer : Player
    {
        protected Me myInput;

        protected Vector3 prevHead;
        protected FaceDirection faceDir;//why?

        public HumanPlayer(Game game, World world, Vector3 location, int walkSpeed, int runSpeed, int id)
            : base(game, world, location, walkSpeed, runSpeed, id)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            myInput = new KeyboardMe(Game);//change..
            prevHead = myInput.getHeadPosition();//initialize location to same place?
            faceDir = myInput.getFaceDirection();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //must insert collision detection for human player!!!!!!!
            Vector3 tempHead = prevHead;
            prevHead = myInput.getHeadPosition();
            location = location - tempHead + prevHead;
            //may want to try to get speed from user instead of using walkSpeed.  in that case walk/runSpeed should be members of virtualPlayer 
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
            //Basically the hider is a totally passive position.  All that happens is the location is updated, and we just hope
            //that at some point he'll make it back to the tree...
            //base.Update(gameTime);
            faceDir = myInput.getFaceDirection();
            if (location.X < prevSpace[0] || location.Z > prevSpace[1] || location.X > prevSpace[2] || location.Z < prevSpace[3])
            {
                float[] temp = prevSpace;
                prevSpace = world.locSquare(location);
                world.updateLocation(temp, prevSpace);
            }
            base.Update(gameTime);
        }

        //tell player he won
        public override void win()
        {
            Console.WriteLine("YOU WON!!!");
        }

        public override string ToString()
        {
            return "Human " + base.ToString();
        }
    }
}
