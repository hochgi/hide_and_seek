using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HideAndSeek
{
    class KeyboardInput : Input
    {
        Vector3 headPos;
        
        internal KeyboardInput(Game game) : base(game) 
        {
            headPos = new Vector3(0, 0, 0);
            Game.Components.Add(this);
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Console.WriteLine("Up");
                headPos.Z -= 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                Console.WriteLine("Down");
                headPos.Z += 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                Console.WriteLine("Right");
                headPos.X += 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                Console.WriteLine("Left");
                headPos.X -= 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Space))
            {
                Console.WriteLine("Space");
            }
            base.Update(gameTime);
        }

        internal override WalkingState getWalkingState()
        {
            Console.WriteLine("Getting Walking State:");
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                Console.WriteLine("Walking forwards");
                return WalkingState.Forwards;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                Console.WriteLine("Walking backwards");
                return WalkingState.Backwards;
            }
            else
            {
                Console.WriteLine("Not walking");
                return WalkingState.NotWalking;
            }
        }

        internal override Microsoft.Xna.Framework.Vector3 getHeadPosition()
        {
            return headPos;
        }

        internal override List<Vector3> getPositions()
        {
            List<Vector3> list = new List<Vector3>();
            list.Add(headPos);
            return list;
        }

        internal override bool isPointing()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space))
                return true;
            else
                return false;
        }

    }
}
