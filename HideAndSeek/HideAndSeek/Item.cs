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
    /// all subclasses of item should define a calculateble primitive,
    /// or a collection of such primitives that "cage" the visible obstacle.
    /// </summary>
    public abstract class Item : Microsoft.Xna.Framework.GameComponent
    {
        int id;

        World world;

        public Vector3 location;
        public Vector3 size; //X = W, Y = H, Z = D
        //add item type for drawing reasons!!

        public bool taken;
        public Hider hider;

        protected MyDrawable myDrawable = null;

        public Item(Game game, Vector3 loc, Vector3 size, int type, World world, int id)
            : base(game)
        {
            // TODO: Construct any child components here
            Game.Components.Add(this);
            this.world = world;
            this.location = loc;
            this.size = size;
            this.id = id;
            myDrawable = new MyDrawable(game, Color.ForestGreen, location, size);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            taken = false;
            hider = null;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        //drawing code
        public DrawableGameComponent GetDrawable()
        {
            return null;
        }

        //checks whether item is blocking seeker from seeing hider
        public bool IsBlocking(Seeker seeker, Hider hider)
        {
            bool rv = false;

            foreach(PrimitiveShape p in getCageShapes())
            {
                foreach (Vector3 v in hider.getPartsPositions())
                {
                    rv = rv || p.isBlockingLineOfSight(seeker.getEyesPosition(), v);
                }
            }
            return rv;
        }

        abstract protected List<PrimitiveShape> getCageShapes();

        public override string ToString()
        {
            return "Item " + id + " at " + location;
        }
    }

    public class Rock : Item
    {
        public Rock(Game Game, Vector3 vector3, Vector3 vector3_2, int p, World world, int id)
            : base (Game, vector3, vector3_2, p, world, id)
        {
            // TODO: Complete member initialization
        }

        protected override List<PrimitiveShape> getCageShapes()
        {
            return null;
        }
    }
}
