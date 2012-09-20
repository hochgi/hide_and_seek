using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    class KeyboardMe : Me
    {
        internal KeyboardMe(Game game) : base(game) { }


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
            base.Update(gameTime);
        }

        internal override bool isWalking()
        {
            throw new NotImplementedException();
        }

        internal override Microsoft.Xna.Framework.Vector3 getHeadPosition()
        {
            throw new NotImplementedException();
        }
    }
}
