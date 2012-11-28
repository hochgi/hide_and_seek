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
    //type of game being played
    public enum GameType { HidePractice, SeekPractice, Hide, Seek };

    //represents the game state, which holds all of the information
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class World : Microsoft.Xna.Framework.GameComponent
    {
        //type of game being played
        public GameType gameType = GameType.Hide;
        //number seeker needs to count to
        int countNum = 100;
        //number of hiders
        public int numOfHiders = 3;
        //number of items in playing space
        public int numOfItems = 4;

        public Item[] items;
        public Hider[] hiders;
        Seeker seeker;
        public HumanPlayer humanPlayer;

        //boundaries of playing space
        Vector3[] borders;

        //size of a square on the map
        int squareSize = 10;
        //map representing the space
        private FieldMap map;

        private static World singleWorld = null;

        public static World getWorld()
        {
            return singleWorld;
        }

        public static World getWorld(Game game)
        {
            if (singleWorld == null)
                singleWorld = new World(game);
            return singleWorld;
        }

        //constructor for World class
        private World(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            Game.Components.Add(this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //initialize game space borders
            borders = new Vector3[4];
            borders[0] = new Vector3(20, 0, 0);
            borders[1] = new Vector3(-20, 0, 0);
            borders[2] = new Vector3(20, 0, -2000);
            borders[3] = new Vector3(-20, 0, -2000);

            //create map
            int sizeX = (int)Math.Abs(borders[0].X - borders[3].X) / squareSize;
            int sizeY = (int)(Math.Abs(borders[0].Z - borders[3].Z) / squareSize);
            map = new FieldMap(sizeX, sizeY);

            //if game is a real game and not a practice
            if (gameType == GameType.Hide || gameType == GameType.Seek)
            {
                Random rand = new Random(10);
                items = new Item[numOfItems];
                for (int i = 0; i < numOfItems; i++)
                {
                    //choose space on map which has no item in it yet
                    FieldNode node = null;
                    int x;
                    int y;
                    do
                    {
                        x = rand.Next(sizeX);
                        y = rand.Next(sizeY);
                        node = new FieldNode(x, y);
                    } while (!map.isAvailable(node));
                    //find optimal location in space for item
                    Vector3 loc = Rock.findBestLoc(nodeToLoc(node), ItemType.Rock);
                    items[i] = new Rock(Game, loc, new Vector3(0.25f, 0.25f, 0.25f), 0, i);
                    map.addItem(items[i], x, y);
                }
            }

            hiders = new Hider[numOfHiders];
            //if human player is a hider
            if (gameType == GameType.Hide)
            {
                //create human hider
                humanPlayer = new HumanHider(Game, new Vector3(0, 0, 0), 5, 10, 0);
                hiders[0] = (Hider)humanPlayer;
                //create rest of hiders to be virtual players
                for (int i = 1; i < numOfHiders; i++)
                    hiders[i] = new VirtualHider(Game, new Vector3(10 * i, 0, 0) + borders[1], 5, 10, i + 1);
                //create virtual seeker
                seeker = new VirtualSeeker(Game, new Vector3(5, 0, 0), 5, 10, 1, countNum);
            }
            //if human player is a seeker
            else if (gameType == GameType.Seek)
            {
                //create human seeker
                humanPlayer = new HumanSeeker(Game, new Vector3(0, 0, 0), 5, 10, 0, countNum);
                seeker = (Seeker)humanPlayer;
                //create all hiders to be virtual players
                for (int i = 0; i < numOfHiders; i++)
                    hiders[i] = new VirtualHider(Game, new Vector3(5 * i, 0, 0), 5, 10, i + 1);
            }
            if (gameType == GameType.HidePractice)
            {
                numOfItems = 1;
                numOfHiders = 1;
                items = new Item[1];
                items[0] = new Rock(Game, new Vector3(0, 0, -20), new Vector3(2.5f, 2.5f, 2.5f), 0, 1);
                hiders = new Hider[1];
                humanPlayer = new HumanHider(Game, new Vector3(0, 0, 0), 5, 10, 0);
                hiders[0] = (Hider)humanPlayer;
                seeker = null;
            }
            else if (gameType == GameType.SeekPractice)
            {
                numOfItems = 2;
                numOfHiders = 1;
                items = new Item[2];
                items[0] = new Rock(Game, new Vector3(20, 0, -20), new Vector3(2.5f, 2.5f, 2.5f), 0, 1);
                items[1] = new Rock(Game, new Vector3(-20, 0, -20), new Vector3(2.5f, 2.5f, 2.5f), 0, 2);
                hiders = new Hider[1];
                hiders[0] = new VirtualHider(Game, new Vector3(10, 0, 0), 5, 10, 2);
                seeker = new HumanSeeker(Game, new Vector3(0, 0, 0), 5, 10, 1, 0);
                humanPlayer = (HumanPlayer)seeker;
                Random rand = new Random();
                int i = rand.Next(2);
                ((VirtualHider)hiders[0]).skipSearch(items[i]);
                ((HumanSeeker)seeker).skipCounting();
            }
            else
            {
                //tell map that locations of all players are off-limits
                for (int i = 0; i < numOfHiders; i++)
                    map.addBlock((int)Math.Abs(hiders[i].Location.X - borders[1].X) / squareSize, (int)-(hiders[i].Location.Z / squareSize));
                map.addBlock((int)Math.Abs((seeker.Location.X - borders[1].X)) / squareSize, (int)-seeker.Location.Z / squareSize);
            }
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            //if player was hiding in practice mode, and can't be seen, game is over.
            if (gameType == GameType.HidePractice && SeekerImp.CanSee(hiders[0], Vector3.Zero) == 0)
            {
                Console.WriteLine("Good job hiding!");
                Game.Exit();
            }
            base.Update(gameTime);
        }

        public DrawableGameComponent GetDrawable()
        {
            return null;
        }

        // tell field map that a player has moved from one square to another.
        internal void updateLocation(float[] prevSpace, float[] nextSpace)
        {
            map.moveSomeone(spaceToNode(prevSpace), spaceToNode(nextSpace));
        }

        // get next space for player, which is closest to their goal
        internal float[] getBestSpace(Vector3 location, Vector3 goal)
        {
            FieldNode node = map.getClosestNext(locToNode(location), locToNode(goal));
            Console.WriteLine("Closest next space: " + node);
            return nodeToLoc(node);
        }

        // convert 3D location to node on field map
        private FieldNode locToNode(Vector3 location)
        {
            //if location is outside of game borders
            if (isOutOfBounds(location))
                return new FieldNode(-1, -1);
            //if location is on borders
            if (location.X == borders[0].X)
            {
                if (location.Z == borders[2].Z)
                    return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize - 1, (int)-location.Z / squareSize - 1);
                else
                    return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize - 1, (int)-location.Z / squareSize);
            }
            else if (location.Z == borders[2].Z)
                return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize, (int)-location.Z / squareSize - 1);
            //if location is well inside game
            return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize, (int)-location.Z / squareSize);
        }

        // convert node to 3D location
        public float[] nodeToLoc(FieldNode node)
        {
            if (node == null || node.x < 0 || node.y < 0)
                return null;
            float[] res = new float[4];
            res[0] = node.x * squareSize + borders[1].X;
            res[1] = -node.y * squareSize;
            res[2] = (node.x + 1) * squareSize + borders[1].X;
            res[3] = -(node.y + 1) * squareSize;
            return res;
        }

        //returns borders of square which contains location
        public float[] locSquare(Vector3 location)
        {
            return nodeToLoc(locToNode(location));
        }

        //x-size of map
        internal int mapSizeX()
        {
            return (int)(Math.Abs(borders[0].X - borders[1].X)) / squareSize;
        }

        //y-size of map
        internal int mapSizeY()
        {
            return (int)(Math.Abs(borders[2].Z)) / squareSize;
        }

        //returns neighboring space which has been seen least recently
        public float[] findBestNotSeen(float[] prevSpace, int[,] seenMap) 
        {
            FieldNode node = map.findBestNotSeen(spaceToNode(prevSpace), seenMap);
            Console.WriteLine("BestNotSeen: " + node);
            return nodeToLoc(node);
        }

        //returns whether a space is available
        internal bool isAvailable(float[] nextSpace)
        {
            return map.isAvailable(spaceToNode(nextSpace));
        }

        //converts the borders of a space to the node which represents it
        public FieldNode spaceToNode(float[] nextSpace)
        {
            if (nextSpace == null)
                return new FieldNode(-1, -1);
            return new FieldNode((int)Math.Abs(nextSpace[0] - borders[1].X) / squareSize, (int)-nextSpace[1] / squareSize);
        }

        //string representation of world
        public override string ToString()
        {
            return map.ToString();
        }

        //returns neighboring space which is closest possible to starting point
        internal float[] getNextRunSpace(Vector3 location)
        {
            FieldNode node = map.findRunSpace(locToNode(location));
            Console.WriteLine("RunSpace: " + node);
            return nodeToLoc(node);
        }

        //remove marking that a certain space is off-limits
        internal void removeBlock(float[] prevSpace)
        {
            map.removeBlock(spaceToNode(prevSpace));
        }
        
        //returns whether location is in conflict with one of the items
        internal bool isConflict(Vector3 location)
        {
            if (isOutOfBounds(location))
                return false;
            return map.isConflict(location, locToNode(location));
        }

        public bool isOutOfBounds(Vector3 location)
        {
            return (location.X > borders[0].X || location.X < borders[1].X || location.Z > 0 || location.Z < borders[2].Z);
        }
    }
}
