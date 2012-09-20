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
    public enum GamePhase { Counting, Looking }; //still relevant??

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class World : Microsoft.Xna.Framework.GameComponent
    {
        public GameType gameType = GameType.Hide;
        public GamePhase gamePhase;
        int countNum = 20;
        public int numOfHiders = 5;
        public int numOfItems = 10;

        public Item[] items;
        public Hider[] hiders;
        Seeker seeker;
        MeHider meHider;
        MeSeeker meSeeker;

        Vector3[] borders;

        int squareSize = 10;
        FieldMap map;

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
                items = new Item[numOfItems];
                for (int i = 0; i < numOfItems; i++)
                {
                    items[i] = new Rock(Game, new Vector3(0, 0, -100 * i - 100), new Vector3(10, 10, 10), 0, this, i);
                    //tell map that this place is off-limits
                    //this is not correct because we have negative x coordinates!!!
                    map.addBlock((int)Math.Abs(items[i].location.X - borders[1].X) / squareSize, (int)-items[i].location.Z / squareSize);
                    //depending on item size may need to block 2 or more squares?
                }

                hiders = new Hider[numOfHiders];
                for (int i = 0; i < numOfHiders; i++)
                {
                    hiders[i] = new Hider(Game, this, i+2);
                    //tell map that this place is off-limits
                    //this is not correct because we have negative x coordinates!!!
                    map.addBlock((int)Math.Abs(hiders[i].location.X - borders[1].X) / squareSize, (int)-hiders[i].location.Z / squareSize);
                }
                gamePhase = GamePhase.Counting;
            }

            else if (gameType == GameType.HidePractice)
            {
                hiders = null;
                items = new Item[1];
                items[0] = new Rock(Game, new Vector3(0, 0, -10), new Vector3(1, 1, 1), 0, this, 1);
            }

            else // gameType == SeekPractice
            {
                hiders = new Hider[1];
                hiders[0] = new Hider(Game, this, 2);
                items = new Item[2];
                items[0] = new Rock(Game, new Vector3(5, 0, -10), new Vector3(1, 1, 1), 0, this, 1);
                items[1] = new Rock(Game, new Vector3(-5, 0, -10), new Vector3(1, 1, 1), 0, this, 2);
            }

            if (gameType == GameType.Hide)
                seeker = new Seeker(Game, this, countNum, 1);
            else
                seeker = null;

            if (gameType == GameType.Hide || gameType == GameType.HidePractice)
            {
                meHider = new MeHider(Game, this, 0);
                meSeeker = null;
            }
            else
            {
                meSeeker = new MeSeeker(Game, this, countNum, 0);
                meHider = null;
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
            Console.WriteLine(map);
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
        internal float[] getNextSpace(Vector3 location)
        {
            return nodeToLoc(map.firstSon(locToNode(location)));
        }

        // convert 3D location to node on field map
        private FieldNode locToNode(Vector3 location)
        {
            return new FieldNode((int)Math.Abs(location.X - borders[1].X) / squareSize, (int)-location.Z / squareSize);
        }

        // convert node to 3D location
        public float[] nodeToLoc(FieldNode node)
        {
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

        internal LinkedList<FieldNode> getSons(Vector3 location)
        {
            return map.findSons(locToNode(location));
        }
    }
}
