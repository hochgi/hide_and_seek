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

    enum HiderPhase { Looking, GoingToSpot, Hiding, Running, Done };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Hider : Player
    {
        private World world;
        HiderPhase phase;

        Item spot;

        public Vector3[] limbs; //0=head, 1=r.hand, 2=l.hand, 3=r.foot, 4=l.foot.  can add more if we want...

        public Hider(Game game, World world)
            : base(game)
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
            phase = HiderPhase.Looking;
            base.Initialize();
            limbs = new Vector3[5];
            //initialize limbs!!
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (world.gameType == GameType.Hide || world.gameType == GameType.Seek)
            {
                if (phase == HiderPhase.Looking)
                {
                    Random rand = new Random();
                    spot = world.items[rand.Next(world.numOfItems)];
                    while (spot.taken == true)
                        spot = world.items[rand.Next(world.numOfItems)];
                    spot.taken = true;
                    phase = HiderPhase.GoingToSpot;
                }
                else if (phase == HiderPhase.GoingToSpot)
                {
                    if (location.Z > spot.location.Z)
                        location.Z -= walkSpeed;
                    else
                    {
                        if (location.X > spot.location.X)
                            location.X -= walkSpeed;
                        else if (location.X < spot.location.X)
                            location.X += walkSpeed;
                        else
                        {
                            // go behind hiding spot and bend down
                            spot.hider = this;
                            phase = HiderPhase.Hiding;
                        }
                    }
                }
                else if (phase == HiderPhase.Running)
                {
                    if (location.Z < 0)
                        location.Z += runSpeed;
                    else
                        phase = HiderPhase.Done;
                }
            }

            base.Update(gameTime);
        }

        internal void Found()
        {
            phase = HiderPhase.Running;
        }
    }
}
