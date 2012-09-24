using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Nui;
using Coding4Fun.Kinect.Wpf;
using Microsoft.Xna.Framework;
using System.Windows;
using System.Windows.Forms; 

namespace HideAndSeek
{
    class KinectMe : Me
    {
        Runtime nui;
        Queue<FeetState> walkHistory;

        internal KinectMe(Game game) : base(game)
        {
            //Kinect Runtime
            nui = new Runtime();


            walkHistory = new Queue<FeetState>(20);
            for (int i = 0; i < 20; i++) 
            {
                walkHistory.Enqueue(new FeetState());
            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            //Initialize to do skeletal tracking
            nui.Initialize(RuntimeOptions.UseSkeletalTracking);

            #region TransformSmooth
            //Must set to true and set after call to Initialize
            nui.SkeletonEngine.TransformSmooth = true;

            //Use to transform and reduce jitter
            var parameters = new TransformSmoothParameters
            {
                Smoothing = 0.75f,
                Correction = 0.0f,
                Prediction = 0.0f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            };

            nui.SkeletonEngine.SmoothParameters = parameters;

            #endregion

            //add event to receive skeleton data
            nui.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(nui_SkeletonFrameReady);
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }



        //what about facing direction?? speed??
        internal override WalkingState getWalkingState()
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

        internal override bool isPointing()
        {
            throw new NotImplementedException();
        }

        internal override FaceDirection getFaceDirection()
        {
            throw new NotImplementedException();
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

        void nui_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {

            SkeletonFrame allSkeletons = e.SkeletonFrame;
            int iSkeletonSlot = 0;

            //get the first tracked skeleton
            SkeletonData skeleton = (from s in allSkeletons.Skeletons
                                     where s.TrackingState == SkeletonTrackingState.Tracked
                                     select s).FirstOrDefault();
            if (skeleton != null)
            {
                MessageBox.Show("hi");
            }
            
        }
    }
}
