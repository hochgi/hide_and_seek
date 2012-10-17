using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    public enum SeekerStatus { Won, Lost, WonDone, Done, None }

    class SeekerImp
    {
        World world;

        public Hider opponent;
        LinkedList<Hider> hidersFound;

        public SeekerImp(World world)
        {
            this.world = world;
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
            if (hidersFound.Count == world.numOfHiders)
            {
                //Console.WriteLine(this + " IS DONE!!!!!!!!!!!!!");
                return true;
            }
            else
                //Console.WriteLine(this + " finished with " + opponent + ". going to find next hider!");
            return false;
        }

        internal bool foundYet(Hider hider)
        {
            return hidersFound.Contains(hider);
        }

        internal SeekerStatus Status(Vector3 location)
        {
            if (location.Z >= 0)
            {
                opponent.Done();
                if (finishWithHider())
                {
                    Console.WriteLine(this + " I won and I'm done!");
                    return SeekerStatus.WonDone;
                }
                else
                {
                    Console.WriteLine(this + " I won, going to find the next hider!");
                    return SeekerStatus.Won;
                }
            }
            else if (((Player)opponent).location.Z >= 0)
            {
                opponent.win();
                opponent.Done();
                if (finishWithHider())
                {
                    Console.WriteLine(this + " I didn't win but I'm done!");
                    return SeekerStatus.Done;
                }
                else
                {
                    Console.WriteLine(this + " I didn't win, going to find the next hider!");
                    return SeekerStatus.Lost;
                }
            }
            return SeekerStatus.None;
        }
    }
}
