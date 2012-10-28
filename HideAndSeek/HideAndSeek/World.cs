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

    public enum GameType { HidePractice, SeekPractice, Hide, Seek };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class World : Microsoft.Xna.Framework.GameComponent
    {
        public GameType gameType = GameType.Hide;
        int countNum = 100;
        public int numOfHiders = 3;
        public int numOfItems = 4;

        public Item[] items;
        public Hider[] hiders;
        Seeker seeker;//not sure if field is necessary since update gets called anyway, but didn't know where to put it.  may be enough to just create it?
        public HumanPlayer humanPlayer;

        Vector3[] borders;

        int squareSize = 10;
        private FieldMap map; //should be private!  public only for debugging

        public World(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            Game.Components.Add(this);
        }


        //this function needs to be fixed up very badly!!
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            borders = new Vector3[4];
            borders[0] = new Vector3(20, 0, 0);
            borders[1] = new Vector3(-20, 0, 0);
            borders[2] = new Vector3(20, 0, -2000);
            borders[3] = new Vector3(-20, 0, -2000);

            map = new FieldMap((int)Math.Abs(borders[0].X - borders[3].X) / squareSize,
                (int)(Math.Abs(borders[0].Z - borders[3].Z) / squareSize));

            if (gameType == GameType.Hide || gameType == GameType.Seek)
            {
                Random rand = new Random(10);
                items = new Item[numOfItems];
                for (int i = 0; i < numOfItems; i++)
                {
                    //TODO: XML file with all settings
                    items[i] = new Rock(Game, new Vector3(20 - rand.Next(41), 0, -100 * i - 100), new Vector3(10, 10, 10), 0, this, i);
                    //tell map that this place is off-limits
                    map.addBlock((int)Math.Abs(items[i].position.X - borders[1].X) / squareSize, (int)-items[i].position.Z / squareSize);
                    //depending on item size may need to block 2 or more squares?
                }
            }

            hiders = new Hider[numOfHiders];
            if (gameType == GameType.Hide)
            {
                humanPlayer = new HumanHider(Game, this, new Vector3(0, 0, 0), 5, 10, 0);
                hiders[0] = (Hider)humanPlayer;
                for (int i = 1; i < numOfHiders; i++)
                    hiders[i] = new VirtualHider(Game, this, new Vector3(10 * i, 0, 0) + borders[1], 5, 10, i + 1);
                seeker = new VirtualSeeker(Game, this, new Vector3(5, 0, 0), 5, 10, 1, countNum);
            }
            else if (gameType == GameType.Seek)
            {
                humanPlayer = new HumanSeeker(Game, this, new Vector3(0, 0, 0), 5, 10, 0, countNum);
                seeker = (Seeker)humanPlayer;
                for (int i = 0; i < numOfHiders; i++)
                    hiders[i] = new VirtualHider(Game, this, new Vector3(5 * i, 0, 0), 5, 10, i + 1);
            }
            for (int i=0;i<numOfHiders;i++)
                map.addBlock((int)Math.Abs(((Player)hiders[i]).location.X - borders[1].X) / squareSize, (int)-((Player)hiders[i]).location.Z / squareSize);
            map.addBlock((int)Math.Abs(((Player)seeker).location.X - borders[1].X) / squareSize, (int)-((Player)seeker).location.Z / squareSize);
            //need to initialize for practice modes!

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            //Console.WriteLine(map);
            base.Update(gameTime);
        }

        public DrawableGameComponent GetDrawable()
        {
            return null;
        }

        // tell field map that a player has moved from one square to another.
        internal void updateLocation(float[] prevSpace, float[] nextSpace)
        {
            map.moveSomeone((int)Math.Abs(prevSpace[0] - borders[1].X) / squareSize, (int)-prevSpace[1] / squareSize,
                (int)Math.Abs(nextSpace[0] - borders[1].X) / squareSize, (int)-nextSpace[1] / squareSize);
        }

        // get next space for player, using DFS (probably needs to be changed!)
        internal float[] getBestSpace(Vector3 location, Vector3 goal)
        {
            FieldNode node = map.getClosestNext(locToNode(location), locToNode(goal));
            Console.WriteLine("Closest next space: " + node);
            return nodeToLoc(node);
        }

        // convert 3D location to node on field map
        private FieldNode locToNode(Vector3 location)
        {
            if (location.X == borders[0].X)
            {
                if (location.Z == borders[2].Z)
                    return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize - 1, (int)-location.Z / squareSize - 1);
                else
                    return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize - 1, (int)-location.Z / squareSize);
            }
            else if (location.Z == borders[2].Z)
                return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize, (int)-location.Z / squareSize - 1);
            return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize, (int)-location.Z / squareSize);
        }

        // convert node to 3D location
        public float[] nodeToLoc(FieldNode node)
        {
            if (node == null)
                return null;
            float[] res = new float[4];
            res[0] = node.x * squareSize + borders[1].X;
            res[1] = -node.y * squareSize;
            res[2] = (node.x + 1) * squareSize + borders[1].X;
            res[3] = -(node.y + 1) * squareSize;
            return res;
        }

        public float[] locSquare(Vector3 location)
        {
            return nodeToLoc(locToNode(location));
        }

        internal int mapSizeX()
        {
            return (int)(Math.Abs(borders[0].X - borders[1].X)) / squareSize;
        }

        internal int mapSizeY()
        {
            return (int)(Math.Abs(borders[2].Z)) / squareSize;
        }

        public float[] findBestNotSeen(float[] prevSpace, int[,] seenMap) 
        {
            FieldNode node = map.findBestNotSeen(spaceToNode(prevSpace), seenMap);
            Console.WriteLine("BestNotSeen: " + node);
            return nodeToLoc(node);
        }

        internal bool isAvailable(float[] nextSpace)
        {
            return map.isAvailable(spaceToNode(nextSpace));
        }

        public FieldNode spaceToNode(float[] nextSpace)
        {
            return new FieldNode((int)Math.Abs(nextSpace[0] - borders[1].X) / squareSize, (int)-nextSpace[1] / squareSize);
        }

        public override string ToString()
        {
            return map.ToString();
        }

        internal float[] getNextRunSpace(Vector3 location)
        {
            FieldNode node = map.findRunSpace(locToNode(location));
            Console.WriteLine("RunSpace: " + node);
            return nodeToLoc(node);
        }
    }
}
