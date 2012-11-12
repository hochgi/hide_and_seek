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
    //represents hider played by the computer
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class VirtualHider : VirtualPlayer, Hider
    {
        //hiding spot hider has selected
        Item spot;

        //constructor for VirtualHider class
        public VirtualHider(Game game, World world, Vector3 location, int walkSpeed, int runSpeed, int id)
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
            spot = null;
            phase = Phase.Looking;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(this + " updating...");
            //if hider is looking for a spot but has not chosen one yet
            if (phase == Phase.Looking && spot == null)
            {
                //choose random spot which is not taken
                Random rand = new Random();
                spot = world.items[rand.Next(world.numOfItems)];
                while (spot.taken == true)
                    spot = world.items[rand.Next(world.numOfItems)];
                //mark spot as taken
                spot.taken = true;
                Console.WriteLine(this + " going to hide at " + spot);
            }
            //if hider was running back to zero and has passed it, change phase to done
            else if (phase == Phase.Running && location.Z >= 0)
                phase = Phase.Done;

            base.Update(gameTime);
        }

        //when arriving in new space, check if hiding spot is in this space and if so hide behind it
        public override bool act()
        {
            Console.WriteLine(this + " acting in space " + nextSpace[0] + " " + nextSpace[1] + " " + nextSpace[2] + " " + nextSpace[3]);
            //if hiding spot is in this space
            if (spot.position.X >= nextSpace[0] && spot.position.Z <= nextSpace[1] && spot.position.X < nextSpace[2]
                && spot.position.Z > nextSpace[3])
            {
                Console.WriteLine(this + " found hiding spot " + spot);
                phase = Phase.Hiding;
                return true;
            }
            return false;
        }

        //choose next square to advance hider to
        public override float[] getNextSpace()
        {
            try
            {
                //find next space to advance to 
                return world.getBestSpace(location, spot.position);
            }
            catch (SpotTakenException e)
            {
                //if spot is now taken, need to find new spot
                spot = null;
                return null;
            }
        }

        //if hider was found, start to run back
        public void Found()
        {
            Console.WriteLine(this + " was found!  Starting to run back!");
            phase = Phase.Running;
        }

        // return a list of locations for every organ (2 hands, 2 legs, body, head, or whatever...)
        public List<Vector3> getPartsPositions()
        {
            List<Vector3> rv = new List<Vector3>();
            rv.Add(new Vector3(location.X + 2.5f, location.Y, location.Z));
            rv.Add(new Vector3(location.X - 2.5f, location.Y, location.Z));
            rv.Add(new Vector3(location.X, location.Y + 10, location.Z));

            return rv;
        }

        //string representation of the hider
        public override string ToString()
        {
            return "Hider " + base.ToString();
        }
    }
}
