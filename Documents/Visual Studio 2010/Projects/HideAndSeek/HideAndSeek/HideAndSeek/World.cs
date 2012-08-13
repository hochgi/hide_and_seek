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
        int numOfHiders = 5;
        public int numOfItems = 10;

        public Item[] items;
        Hider[] hiders;
        Seeker seeker;
        MeHider meHider;
        MeSeeker meSeeker;

        Vector3[] borders;

        public World(Game game)
            : base(game)
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

            if (gameType == GameType.Hide || gameType == GameType.Seek)
            {

                items = new Item[numOfItems];
                //order items by z
                for (int i = 0; i < numOfItems; i++)
                    items[i] = new Item(Game, new Vector3(0, 0, -10 * i), new Vector3(1, 1, 1), 0, this);

                hiders = new Hider[numOfHiders];
                for (int i = 0; i < numOfHiders; i++)
                    hiders[i] = new Hider(Game, this);
                gamePhase = GamePhase.Counting;
            }

            else if (gameType == GameType.HidePractice)
            {
                hiders = null;
                items = new Item[1];
                items[0] = new Item(Game, new Vector3(0, 0, -10), new Vector3(1, 1, 1), 0, this);
            }

            else // gameType == SeekPractice
            {
                hiders = new Hider[1];
                hiders[0] = new Hider(Game, this);
                items = new Item[2];
                items[0] = new Item(Game, new Vector3(5, 0, -10), new Vector3(1, 1, 1), 0, this);
                items[1] = new Item(Game, new Vector3(-5, 0, -10), new Vector3(1, 1, 1), 0, this);
            }

            if (gameType == GameType.Hide)
                seeker = new Seeker(Game, this, countNum);
            else
                seeker = null;

            if (gameType == GameType.Hide || gameType == GameType.HidePractice)
            {
                meHider = new MeHider(Game, this);
                meSeeker = null;
            }
            else
            {
                meSeeker = new MeSeeker(Game, this, countNum);
                meHider = null;
            }

            borders = new Vector3[4];
            borders[0] = new Vector3(20, 0, 0);
            borders[1] = new Vector3(-20, 0, 0);
            borders[2] = new Vector3(20, 0, -2000);
            borders[3] = new Vector3(-20, 0, -2000);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if (gameType == GameType.HidePractice)
            {
                //if (gamePhase == GamePhase.Counting)
                //    if (me.location.Z <= items[0].location.Z && me.location.X >= items[0].location.X - items[0].size.X 
                //        && me.location.X <= items[0].location.X + items[0].size.X)
                //        gamePhase = GamePhase.Looking;
                //else if (gamePhase == GamePhase.Looking)
                //        if (me.location.Z <= items[0].location.Z)
                //        {
                //            //gamePhase = GamePhase.Done;
                //            Console.WriteLine("YAYYYYY!!!");
                //        }
            }
            if (gameType == GameType.Hide)
            {
                
            }
            base.Update(gameTime);
        }

        public DrawableGameComponent GetDrawable()
        {
            return null;
        }
    }
}
