//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Kinect;

//namespace HideAndSeek
//{
//    class KinectMe : Me
//    {
//        float FLOOR = -0.75F;
//        TimeSpan TIME_DIFF = new TimeSpan(5000000);
//        Game1 game;
//        Queue<FeetState> walkHistory;
//        Vector3 head;

//        internal KinectMe(Game game)
//            : base(game)
//        {
//            head = new Vector3();
//            walkHistory = new Queue<FeetState>(20);
//            for (int i = 0; i < 20; i++)
//            {
//                walkHistory.Enqueue(new FeetState());
//            }

//            //initializing kinect sensors
//            KinectSensor sensor = KinectSensor.KinectSensors[0];
//            sensor.SkeletonStream.Enable();
//            sensor.SkeletonFrameReady += runtime_SkeletonFrameReady;
//            sensor.Start();
//        }

//        /// <summary>
//        /// Allows the game component to perform any initialization it needs to before starting
//        /// to run.  This is where it can query for any required services and load content.
//        /// </summary>
//        public override void Initialize()
//        {
//            base.Initialize();
//        }

//        /// <summary>
//        /// Allows the game component to update itself.
//        /// </summary>
//        /// <param name="gameTime">Provides a snapshot of timing values.</param>
//        public override void Update(GameTime gameTime)
//        {
//            base.Update(gameTime);
//        }



//        //what about facing direction?? speed??
//        internal override WalkingState getWalkingState()
//        {
//            int length = walkHistory.Count();
//            FeetState first = walkHistory.ElementAt(length - 1);
//            FeetState second = walkHistory.ElementAt(length - 2);
//            FeetState third = walkHistory.ElementAt(length - 3);
//            FeetState forth = walkHistory.ElementAt(length - 4);
//            if (first.isBothDown()
//                && second.isLeftUp()
//                && third.isBothDown()
//                && forth.isRightUp()
//                ||
//                first.isBothDown()
//                && second.isRightUp()
//                && third.isBothDown()
//                && forth.isLeftUp()
//                ||
//                first.isLeftUp()
//                && second.isBothDown()
//                && third.isRightUp()
//                && forth.isBothDown()
//                ||
//                first.isRightUp()
//                && second.isBothDown()
//                && third.isLeftUp()
//                && forth.isBothDown())
//            {
//                //TODO: check face direction
//                return WalkingState.Forwards;
//            }
//            else
//                return WalkingState.NotWalking;
//            //throw new NotImplementedException();
//            // TODO: check 4 last FeetState instances for a walk cycle.
//        }

//        //note that this function returns the position of the head including Z-coordinate
//        //this can be used to conbinate the virtual position with the actual position 
//        internal override Microsoft.Xna.Framework.Vector3 getHeadPosition()
//        {
//            return head;
//            // TODO: get the head position from skeleton-detecion
//        }

//        internal override bool isPointing()
//        {
//            throw new NotImplementedException();
//        }

//        /*
//         * class represents the feet-state 
//         */
//        private class FeetState
//        {
//            public enum footPosition { Up, Down };
//            public footPosition left, right;
//            public DateTime timeStamp;

//            public FeetState()
//            {
//                this.left = footPosition.Down;
//                this.right = footPosition.Down;
//                this.timeStamp = DateTime.Now;
//            }

//            public FeetState(footPosition leftFP, footPosition rightFP, DateTime timeStamp)
//            {
//                this.left = leftFP;
//                this.right = rightFP;
//                this.timeStamp = timeStamp;
//            }

//            public bool isBothDown()
//            {
//                return this.left == footPosition.Down && this.right == footPosition.Down;
//            }
//            public bool isLeftUp()
//            {
//                return this.left == footPosition.Up && this.right == footPosition.Down;
//            }
//            public bool isRightUp()
//            {
//                return this.left == footPosition.Down && this.right == footPosition.Up;
//            }
//            public override string ToString()
//            {
//                return "(" + left + "," + right + ")";
//            }
//        }


//        internal void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
//        {
//            bool receivedData = false;
//            Skeleton[] skeletons = null;
//            using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
//            {
//                if (SFrame == null)
//                {
//                    // The image processing took too long. More than 2 frames behind.
//                }
//                else
//                {
//                    skeletons = new Skeleton[SFrame.SkeletonArrayLength];
//                    SFrame.CopySkeletonDataTo(skeletons);
//                    receivedData = true;
//                }
//            }

//            if (receivedData && skeletons != null)
//            {
//                Skeleton currentSkeleton = (from s in skeletons
//                                            where s.TrackingState ==
//                                            SkeletonTrackingState.Tracked
//                                            select s).FirstOrDefault();
//                if (currentSkeleton != null)
//                {
//                    SkeletonPoint skeletonHead = currentSkeleton.Joints[JointType.FootLeft].Position;
//                    head.X = skeletonHead.X;
//                    head.Y = skeletonHead.Y;
//                    head.Z = skeletonHead.Z;
//                    SkeletonPoint skeletonLeftFoot = currentSkeleton.Joints[JointType.FootLeft].Position;
//                    SkeletonPoint skeletonRightFoot = currentSkeleton.Joints[JointType.FootRight].Position;

//                    pushToWalkingQueue(skeletonLeftFoot.Y, skeletonRightFoot.Y);
//                    //Console.WriteLine("HEAD: " + head);
//                }
//                else
//                {
//                    Console.WriteLine("no skeleton tracked!");
//                }
//            }
//        }

//        internal void pushToWalkingQueue(float leftFootY, float rightFootY)
//        {
//            DateTime timeStamp = DateTime.Now;
//            FeetState.footPosition left, right;
//            if (leftFootY > FLOOR)
//            {
//                Console.WriteLine("HORAY!!!");
//                left = FeetState.footPosition.Up;
//            }
//            else
//                left = FeetState.footPosition.Down;
//            if (rightFootY > FLOOR)
//                right = FeetState.footPosition.Up;
//            else
//                right = FeetState.footPosition.Down;
//            FeetState last = walkHistory.Last();
//            Console.WriteLine("time diff: " + (timeStamp - last.timeStamp).ToString());
//            if (left != last.left || right != last.right || timeStamp - last.timeStamp >= TIME_DIFF)
//            {
//                walkHistory.Enqueue(new FeetState(left, right, timeStamp));
//            }
//        }

//    }
//}