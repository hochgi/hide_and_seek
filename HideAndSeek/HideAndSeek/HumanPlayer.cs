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
    //represents the human player
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class HumanPlayer : Player
    {
        //input class
        protected Input myInput;

        //last recorded location of player's head
        protected Vector3 prevHead;
        //direction player is facing
        //protected FaceDirection faceDir;//why?

        //whether or not player is counting (if playing Seeker)
        protected bool counting;


        //constructor for HumanPlayer class
        public HumanPlayer(Game game, Vector3 location, int walkSpeed, int runSpeed, int id, bool counting)
            : base(game, location, walkSpeed, runSpeed, id)
        {
            this.counting = counting;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            myInput = new KinectInput(Game);
            prevHead = myInput.getHeadPosition();
            //faceDir = myInput.getFaceDirection();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //if player is counting, don't update location.  otherwise, update.
            if (!counting)
            {
                //update location based on where player moved within game range
                Vector3 tempHead = prevHead;
                prevHead = myInput.getHeadPosition();
                location = location - tempHead + prevHead;
                //if player is walking into an item, undo last step
                if (World.getWorld().isConflict(location))
                    location = location - prevHead + tempHead;
                //check if player is walking and update location accordingly
                WalkingState state = myInput.getWalkingState();
                if (state == WalkingState.Forwards)
                {
                    Console.WriteLine(this + " Walking forwards");
                    location.Z -= walkSpeed;
                    //if player is walking into an item, undo last step
                    if (World.getWorld().isConflict(location))
                        location.Z += walkSpeed;
                }
                else if (state == WalkingState.Backwards)
                {
                    Console.WriteLine(this + " Walking backwards");
                    location.Z += walkSpeed;
                    //if player is walking into an item, undo last step
                    if (World.getWorld().isConflict(location))
                        location.Z -= walkSpeed;
                }
                //update face direction
                //faceDir = myInput.getFaceDirection();
                //if player has passed into another square on map, update it
                if (((prevSpace != null) && (location.X < prevSpace[0] || location.Z > prevSpace[1] || location.X >= prevSpace[2]
                    || location.Z <= prevSpace[3])) || (prevSpace == null && !World.getWorld().isOutOfBounds(location)))
                {
                    float[] temp = prevSpace;
                    prevSpace = World.getWorld().locSquare(location);
                    World.getWorld().updateLocation(temp, prevSpace);
                }
            }
            base.Update(gameTime);
        }

        //tell player he won
        public override void win()
        {
            Console.WriteLine("YOU WON!!!");
        }

        //string representation of human player
        public override string ToString()
        {
            return "Human " + base.ToString();
        }
    }
}
