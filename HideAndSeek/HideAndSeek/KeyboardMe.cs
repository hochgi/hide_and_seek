using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HideAndSeek
{
    class KeyboardMe : Me
    {
        Vector3 headPos;

        internal KeyboardMe(Game game) : base(game) 
        {
            headPos = new Vector3(0, 0, 0);
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
                headPos.Z -= 1;
            else if (keyboardState.IsKeyDown(Keys.Down))
                headPos.Z += 1;
            else if (keyboardState.IsKeyDown(Keys.Right))
                headPos.X += 1;
            else if (keyboardState.IsKeyDown(Keys.Left))
                headPos.X -= 1;
            base.Update(gameTime);
        }

        internal override WalkingState getWalkingState()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up))
                return WalkingState.Forwards;
            else if (keyboardState.IsKeyDown(Keys.Down))
                return WalkingState.Backwards;
            else
                return WalkingState.NotWalking;
        }

        internal override Microsoft.Xna.Framework.Vector3 getHeadPosition()
        {
            return headPos;
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
