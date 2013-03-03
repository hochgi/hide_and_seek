using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek 
{
    enum WalkingState { NotWalking, Forwards, Backwards };
    public enum FaceDirection { Forwards, Backwards };

    /// <summary>
    /// This is an abstract class that holds and interprets input from the human player
    /// </summary>
    public abstract class Input : Microsoft.Xna.Framework.GameComponent
    {
        internal Input(Game game) : base(game) 
        {
            //Game.Components.Add(this);
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
            base.Update(gameTime);
        }

        internal abstract WalkingState getWalkingState();

        internal abstract Vector3 getHeadPosition();

        internal abstract List<Vector3> getPositions();

        internal abstract bool isPointing();

    }
}
