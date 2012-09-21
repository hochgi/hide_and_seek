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

    public enum HiderPhase { Looking, GoingToSpot, Hiding, Running, Done };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Hider : Player
    {
        public HiderPhase phase;

        Item spot;

        public Hider(Game game, World world, int id)
            : base(game, world, PlayerPhase.Looking, id)
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
            phase = HiderPhase.Looking;
            base.Initialize();
            limbs = new Vector3[5];
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
                    //choose random spot which is not taken
                    Random rand = new Random();
                    spot = world.items[rand.Next(world.numOfItems)];
                    while (spot.taken == true)
                        spot = world.items[rand.Next(world.numOfItems)];
                    spot.taken = true;
                    phase = HiderPhase.GoingToSpot;
                    Console.WriteLine(this + " going to hide at " + spot);
                }
                //walking and running code need to be rewritten!!!
                else if (phase == HiderPhase.GoingToSpot)
                {
                    //if (location.Z > spot.location.Z)
                    //    location.Z -= walkSpeed;
                    //else
                    //{
                    //    if (location.X > spot.location.X)
                    //        location.X -= walkSpeed;
                    //    else if (location.X < spot.location.X)
                    //        location.X += walkSpeed;
                    //    else
                    //    {
                    //        //spot may be taken if human player got there first
                    //        if (spot.taken)
                    //            phase = HiderPhase.Looking;
                    //        else
                    //        {
                    //            // go behind hiding spot and bend down
                    //            spot.hider = this;
                    //            phase = HiderPhase.Hiding;
                    //        }
                    //    }
                    //}
                }
                else if (phase == HiderPhase.Running)
                {
                    Console.WriteLine(this + " is running");
                    //move towards zero, if reached zero wait by tree
                    if (location.Z < 0)
                        location.Z += runSpeed;
                    else
                    {
                        Console.WriteLine(this + " is done!");
                        phase = HiderPhase.Done;
                        pPhase = PlayerPhase.Other;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override bool act()
        {
            Console.WriteLine(this + " acting in space " + nextSpace[0] + " " + nextSpace[1] + " " + nextSpace[2] + " " + nextSpace[3]);
            //if hiding spot is in this space
            if (spot.location.X >= nextSpace[0] && spot.location.Z <= nextSpace[1] && spot.location.X <= nextSpace[2]
                && spot.location.Z >= nextSpace[3])
            {
                Console.WriteLine(this + " found hiding spot " + spot);
                //go behind spot and crouch down - need to implement!
                phase = HiderPhase.Hiding;
                pPhase = PlayerPhase.Other;
                return true;
            }
            return false;
        }

        //hider was found, start running back toward tree
        internal void Found()
        {
            Console.WriteLine(this + " was found!  Starting to run back!");
            phase = HiderPhase.Running;
            pPhase = PlayerPhase.Running;
        }

        // return a list of locations for every organ (2 hands, 2 legs, body, head, or whatever...)
        public List<Vector3> getPartsPositions()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Hider " + base.ToString();
        }
    }
}
