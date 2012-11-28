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
    //represents the hider played by the human player
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class HumanHider : HumanPlayer, Hider
    {
        //constructor for HumanHider class
        public HumanHider(Game game, Vector3 location, int walkSpeed, int runSpeed, int id)
            : base(game, location, walkSpeed, runSpeed, id, false)
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

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Console.WriteLine(this + " updating...");

            base.Update(gameTime);
        }
        
        //returns the locations of player's body parts
        public List<Vector3> getPartsPositions()
        {
            List<Vector3> res = new List<Vector3>();
            res.Add(new Vector3(Location.X, Location.Y, Location.Z));
            return res;
        }

        //when found, do nothing.
        public void Found()
        {
        }

        //returns a string representation of the human hider
        public override string ToString()
        {
            return "Hider " + base.ToString();
        }


        public new Vector3 Location
        {
            get { return location; }
        }
    }
}
