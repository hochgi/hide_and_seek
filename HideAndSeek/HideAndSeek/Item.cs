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
    //different types of items
    public enum ItemType { Rock };

    //represents an item which players can hide behind
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// all subclasses of item should define a calculateble primitive,
    /// or a collection of such primitives that "cage" the visible obstacle.
    /// </summary>
    public abstract class Item : Microsoft.Xna.Framework.GameComponent
    {
        //item's ID number
        int id;

        //item's location
        public Vector3 position;
        //item's size
        public Vector3 size;

        //whether a hider has chosen this item as a hiding place
        public bool taken;

        //DrawableGameComponent of the item
        protected MyDrawable myDrawable = null;

        //constructor for Item class
        public Item(Game game, Vector3 loc, Vector3 size, int type, int id)
            : base(game)
        {
            // TODO: Construct any child components here
            this.position = loc;
            this.size = 10*size;
            this.id = id;
            Game.Components.Add(this);

        }

        //returns the position of the item
        public Vector3 getPosition() 
        {
            return this.position;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            taken = false;
            myDrawable = new MyDrawable(Game, Color.ForestGreen, this.position, this.size);
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
        public bool IsBlocking(Vector3 eyes, Vector3 loc)
        {
            bool rv = false;

            foreach (PrimitiveShape p in getCageShapes())
            {
                rv = rv || p.isBlockingLineOfSight(eyes, loc);
            }
            return rv;
        }

        //returns list of PrimitiveShapes that make up the item
        abstract protected List<PrimitiveShape> getCageShapes();

        //returns string representation of item
        public override string ToString()
        {
            return "Item " + id + " at " + position;
        }

        internal abstract bool isConflict(Vector3 location);

        //return the best location for the item so that it is as close as possible to the front of the square
        static public Vector3 findBestLoc(float[] square, ItemType type)
        {
            return new Vector3(square[0], 0, square[1]);
        }
    }

    //represents a rock as an item
    public class Rock : Item
    {

        List<PrimitiveShape> cage;

        public Rock(Game Game, Vector3 position, Vector3 widthHeightDepth, int type, int id)
            : base (Game, position, widthHeightDepth, type, id)
        {
            // TODO: Complete member initialization
            cage = new List<PrimitiveShape>();
            cage.Add(new Sphere(widthHeightDepth.Length(), position));
        }

        protected override List<PrimitiveShape> getCageShapes()
        {
            return cage;
        }

        //return whether location is conflicting with location
        internal override bool isConflict(Vector3 location)
        {
            return false;
        }
    }




    //represents a rock as an item
    public class Tree : Item, Billboard
    {

        List<PrimitiveShape> cage;
        Texture2D tex;

        public Tree(Game Game, Vector3 position, Vector3 widthHeightDepth, int type, int id)
            : base(Game, position, widthHeightDepth, type, id)
        {
            // TODO: better caging
            cage = new List<PrimitiveShape>();
            cage.Add(new Sphere(widthHeightDepth.Length(), position));

            BillboardSystem bbs = BillboardSystem.getBillboardSystem();
            bbs.addBillboard(this);

            tex = Game.Content.Load<Texture2D>("tree");
        }

        protected override List<PrimitiveShape> getCageShapes()
        {
            return cage;
        }

        //return whether location is conflicting with location
        internal override bool isConflict(Vector3 location)
        {
            //throw new NotImplementedException();
            return false;
        }


        public Vector3[] getQuadBillboard(Vector3 up, Vector3 cameraPosition)
        {
            //throw new NotImplementedException();
            Vector3 z = Vector3.Subtract(cameraPosition, position);
            Vector3 side = Vector3.Cross(up, z);
            z.Normalize();
            side.Normalize();
            Vector3 treeUp = Vector3.Multiply(Vector3.UnitY, size.Y);
            Vector3 treeSide = Vector3.Multiply(side, size.X / 2);

            Vector3[] rv = new Vector3[4];

            rv[0] = new Vector3(0);
            Vector3.Add(ref treeUp, ref treeSide, out rv[0]);
            Vector3.Add(rv[0], position);

            rv[1] = new Vector3(0);
            Vector3.Negate(treeSide);
            Vector3.Add(ref treeUp, ref treeSide, out rv[1]);
            Vector3.Add(rv[1], position);

            rv[2] = new Vector3(2);
            Vector3.Add(ref position, ref treeSide, out rv[0]);

            rv[0] = new Vector3(0);
            Vector3.Negate(treeSide);
            Vector3.Add(ref position, ref treeSide, out rv[0]);

            return rv;
        }


        public Texture2D getTexture()
        {
            return tex;
        }
    }
}
