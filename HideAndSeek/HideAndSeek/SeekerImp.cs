﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    //possible results when Seeker checks his status vs. the hider he was racing against.
    public enum SeekerStatus { Won, Lost, WonDone, Done, None }

    //implementation of functionality for Seeker
    class SeekerImp
    {
        //current hider which seeker is racing against
        public Hider opponent;
        //hiders seeker has found so far
        LinkedList<Hider> hidersFound;

        //constructor for SeekerImp class
        public SeekerImp()
        {
            opponent = null;
            hidersFound = new LinkedList<Hider>();
        }

        //register hider as found
        public void hiderFound(Hider hider)
        {
            opponent = hider;
            hidersFound.AddLast(hider);
            hider.Found();
        }

        //finish competing against a hider
        public bool finishWithHider()
        {
            opponent = null;
            //if all hiders have been found
            if (hidersFound.Count == World.getWorld().numOfHiders)
            {
                return true;
            }
            return false;
        }

        //checks if given hider has been found yet
        internal bool foundYet(Hider hider)
        {
            return hidersFound.Contains(hider);
        }

        //returns status of seeker vs. hider he was racing against
        internal SeekerStatus Status(Vector3 location)
        {
            //if seeker has reached zero before hider
            if (location.Z >= 0)
            {
                //if hider this was the last hider
                if (finishWithHider())
                {
                    Console.WriteLine(this + " I won and I'm done!");
                    return SeekerStatus.WonDone;
                }
                //if this was not the last hider
                else
                {
                    Console.WriteLine(this + " I won, going to find the next hider!");
                    return SeekerStatus.Won;
                }
            }
            //if hider reached zero before seeker
            else if (opponent.Location.Z >= 0)
            {
                //hider won
                opponent.win();
                //if this was the last hider
                if (finishWithHider())
                {
                    Console.WriteLine(this + " I didn't win but I'm done!");
                    return SeekerStatus.Done;
                }
                //if this was not the last hider
                else
                {
                    Console.WriteLine(this + " I didn't win, going to find the next hider!");
                    return SeekerStatus.Lost;
                }
            }
            //if neither hider nor seeker has reached zero yet, return that nothing has happened
            return SeekerStatus.None;
        }

        //returns whether or not seeker can see hider
        public static float CanSee(Hider hider, Vector3 pos)
        {
            List<Vector3> locs = hider.getPartsPositions();
            float res = 0.5f;
            float frac = 0.5f / locs.Count;
            //for each body part in hider
            foreach (Vector3 loc in locs)
                //for each item in world
                for (int j = 0; j < World.getWorld().numOfItems; j++)
                {
                    //if seeker can't see hider
                    if (!World.getWorld().items[j].IsBlocking(pos, loc))
                    {
                        res += frac;
                    }
                }
            return res;
        }
    }
}
