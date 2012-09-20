using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    class KinectMe : Me
    {
        Queue<FeetState> walkHistory;

        internal KinectMe() : base()
        {
            walkHistory = new Queue<FeetState>(20);
            for (int i = 0; i < 20; i++) 
            {
                walkHistory.Enqueue(new FeetState());
            }
        }

        //what about facing direction?? speed??
        internal override bool isWalking()
        {
            throw new NotImplementedException();
            // TODO: check 4 last FeetState instances for a walk cycle.
        }

        //note that this function returns the position of the head including Z-coordinate
        //this can be used to conbinate the virtual position with the actual position 
        internal override Microsoft.Xna.Framework.Vector3 getHeadPosition()
        {
            throw new NotImplementedException();
            // TODO: get the head position from skeleton-detecion
        }

        
        /* this functions will be omitted! do not use them!!
         * 
        internal bool isWalkingRight()
        {
            throw new NotImplementedException();
        }
        
        internal bool isWalkingLeft()
        {
            throw new NotImplementedException();
        }
        */

        /*
         * class represents the feet-state 
         */
        private class FeetState // stink!! ;)
        {
            public enum footPosition { Up, Down };
            footPosition left, right;
            DateTime timeStamp;

            public FeetState() 
            {
                left = footPosition.Down;
                right = footPosition.Down;
                timeStamp = DateTime.Now;
            }
        }
    }
}
