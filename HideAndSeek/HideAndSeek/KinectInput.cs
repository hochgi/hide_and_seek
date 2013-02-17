using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.FaceTracking;

namespace HideAndSeek
{
    class KinectInput : Input
    {

        float FLOOR = -0.6F;
        TimeSpan TIME_DIFF = new TimeSpan(5800000);
        Game1 game;
        Queue<FeetState> walkHistory;
        Vector3 head;
        Vector3 rightHand;
        List<Vector3> positions;
        KinectSensor sensor;
        FaceTracker faceTracker;
        FaceDirection faceDirection;
        bool prevFaceTracked = false;
        byte[] colorPixelData;
        short[] depthPixelData;
        Skeleton[] skeletonData;
        private bool pointing;


        internal KinectInput(Game game)
            : base(game)
        {
            head = new Vector3();
            pointing = false;
            walkHistory = new Queue<FeetState>(20);
            for (int i = 0; i < 20; i++)
            {
                walkHistory.Enqueue(new FeetState());
            }
            positions = new List<Vector3>();
            //initializing kinect sensors
            sensor = KinectSensor.KinectSensors[0];
            sensor.ColorStream.Enable();
            sensor.SkeletonStream.Enable();
            sensor.DepthStream.Enable();
            //sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(runtime_SkeletonFrameReady);
            // Listen to the AllFramesReady event to receive KinectSensor's data.
            sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinectSensor_AllFramesReady);
            sensor.Start();
            faceTracker = new FaceTracker(sensor);
            // Initialize data arrays
            colorPixelData = new byte[sensor.ColorStream.FramePixelDataLength];
            depthPixelData = new short[sensor.DepthStream.FramePixelDataLength];
            skeletonData = new Skeleton[6];
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
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
            int length = walkHistory.Count();
            FeetState first = walkHistory.ElementAt(length - 1);
            FeetState second = walkHistory.ElementAt(length - 2);
            FeetState third = walkHistory.ElementAt(length - 3);
            FeetState forth = walkHistory.ElementAt(length - 4);
            /*
            foreach (FeetState fs in walkHistory)
            {
                Console.Write(fs.ToString() + ",");
            }
            Console.Write("\n");
            */
            //Console.WriteLine("1:" + first + ",2:" + second + ",3:" + third + ",4:" + forth);

            if (first.isBothDown()
                && second.isLeftUp()
                && third.isBothDown()
                && forth.isRightUp()
                ||
                first.isBothDown()
                && second.isRightUp()
                && third.isBothDown()
                && forth.isLeftUp()
                ||
                first.isLeftUp()
                && second.isBothDown()
                && third.isRightUp()
                && forth.isBothDown()
                ||
                first.isRightUp()
                && second.isBothDown()
                && third.isLeftUp()
                && forth.isBothDown())
            {
                if (faceDirection == FaceDirection.Forwards)
                {
                    Console.WriteLine("Walking forwards");
                    return WalkingState.Forwards;
                }
                else
                {
                    Console.WriteLine("Walking backwards");
                    return WalkingState.Backwards;
                }
            }
            else
                return WalkingState.NotWalking;
        }

        internal Vector3 getVectorFromPoint(SkeletonPoint p)
        {
            Vector3 v = new Vector3();
            v.X = p.X;
            v.Y = p.Y;
            v.Z = p.Z;
            return v;
        }

        //note that this function returns the position of the head including Z-coordinate
        //this can be used to conbinate the virtual position with the actual position 
        internal override Microsoft.Xna.Framework.Vector3 getHeadPosition()
        {
            return head;
            // TODO: get the head position from skeleton-detecion
        }

        internal override List<Vector3> getPositions()
        {
            return positions;
        }

        internal override bool isPointing()
        {
            Console.WriteLine("POINTING=" + pointing);
            return pointing;
        }

        /*
         * class represents the feet-state 
         */
        private class FeetState
        {
            public enum footPosition { Up, Down };
            public footPosition left, right;
            public DateTime timeStamp;

            public FeetState()
            {
                this.left = footPosition.Down;
                this.right = footPosition.Down;
                this.timeStamp = DateTime.Now;
            }

            public FeetState(footPosition leftFP, footPosition rightFP, DateTime timeStamp)
            {
                this.left = leftFP;
                this.right = rightFP;
                this.timeStamp = timeStamp;
            }

            public bool isBothDown()
            {
                return this.left == footPosition.Down && this.right == footPosition.Down;
            }
            public bool isLeftUp()
            {
                return this.left == footPosition.Up && this.right == footPosition.Down;
            }
            public bool isRightUp()
            {
                return this.left == footPosition.Down && this.right == footPosition.Up;
            }
            public override string ToString()
            {
                return "(" + left + "," + right + ")";
            }
        }

        //internal void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        //{
        //}


        internal void kinectSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            bool receivedData = false;
            Skeleton[] skeletons = null;
            using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
            {
                if (colorImageFrame == null)
                    return;
                colorImageFrame.CopyPixelDataTo(colorPixelData);
            }

            using (DepthImageFrame depthImageFrame = e.OpenDepthImageFrame())
            {
                if (depthImageFrame == null)
                    return;
                depthImageFrame.CopyPixelDataTo(depthPixelData);
            }

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame == null)
                    return;
                skeletonFrame.CopySkeletonDataTo(skeletonData);
            }

            //var skeleton = skeletonData.FirstOrDefault(s => s.TrackingState == SkeletonTrackingState.Tracked);
            //if (skeleton == null)
            //    return;
            using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
            {
                if (SFrame == null)
                {
                    // The image processing took too long. More than 2 frames behind.
                }
                else
                {
                    skeletons = new Skeleton[SFrame.SkeletonArrayLength];
                    SFrame.CopySkeletonDataTo(skeletons);
                    receivedData = true;
                }
            }

            if (receivedData && skeletons != null)
            {
                Skeleton currentSkeleton = (from s in skeletons
                                            where s.TrackingState ==
                                            SkeletonTrackingState.Tracked
                                            select s).FirstOrDefault();
                if (currentSkeleton != null)
                {
                    SkeletonPoint skeletonHead = currentSkeleton.Joints[JointType.FootLeft].Position;
                    //head.X = skeletonHead.X;
                    //head.Y = skeletonHead.Y;
                    //head.Z = skeletonHead.Z;
                    head = getVectorFromPoint(skeletonHead);
                    SkeletonPoint skeletonLeftFoot = currentSkeleton.Joints[JointType.FootLeft].Position;
                    SkeletonPoint skeletonRightFoot = currentSkeleton.Joints[JointType.FootRight].Position;

                    pushToWalkingQueue(skeletonLeftFoot.Y, skeletonRightFoot.Y);
                    //Console.WriteLine("HEAD: " + head);

                    SkeletonPoint skeletonRightHand = currentSkeleton.Joints[JointType.HandRight].Position;
                    rightHand = getVectorFromPoint(skeletonRightHand);
                    if (rightHand.Y > head.Y)
                    {
                        pointing = true;
                    }
                    else
                    {
                        pointing = false;
                    }

                    positions = new List<Vector3>();
                    foreach (Joint j in currentSkeleton.Joints)
                    {
                        positions.Add(getVectorFromPoint(j.Position));
                    }

                    FaceTrackFrame faceFrame = faceTracker.Track(sensor.ColorStream.Format, colorPixelData,
                                  sensor.DepthStream.Format, depthPixelData,
                                  currentSkeleton);
                    if (faceFrame.TrackSuccessful)
                    {
                        faceDirection = FaceDirection.Forwards;
                        prevFaceTracked = true;
                        Console.WriteLine("face forwards");
                    }
                    else //if (currentSkeleton != null)
                    {
                        if (prevFaceTracked)
                        {
                            faceDirection = FaceDirection.Forwards;
                            prevFaceTracked = false;
                        }
                        else
                        {
                            faceDirection = FaceDirection.Backwards;
                            Console.WriteLine("face backwards");
                        }
                    }
                }
                else
                {
                    pointing = false;
                    //Console.WriteLine("no skeleton tracked! " + DateTime.Now);
                }
            }

        }

        internal void pushToWalkingQueue(float leftFootY, float rightFootY)
        {
            DateTime timeStamp = DateTime.Now;
            FeetState.footPosition left, right;
            if (leftFootY - rightFootY > 0.1)
            {
                //Console.WriteLine("HORAY!!!");
                left = FeetState.footPosition.Up;
                right = FeetState.footPosition.Down;
            }
            else if (rightFootY - leftFootY > 0.1)
            {
                right = FeetState.footPosition.Up;
                left = FeetState.footPosition.Down;
            }
            else
            {
                left = FeetState.footPosition.Down;
                right = FeetState.footPosition.Down;
            }
            FeetState last = walkHistory.Last();
            //Console.WriteLine("time diff: " + (timeStamp - last.timeStamp).ToString());
            if (left != last.left || right != last.right || timeStamp - last.timeStamp >= TIME_DIFF)
            {
                Console.WriteLine("l:" + leftFootY + "r:" + rightFootY + ",(" + left + "," + right + ")");
                FeetState fs = new FeetState(left, right, timeStamp);
                walkHistory.Enqueue(fs);
            }
        }


        internal void pushToWalkingQueue2(float leftFootY, float rightFootY)
        {
            DateTime timeStamp = DateTime.Now;
            FeetState.footPosition left, right;
            if (leftFootY > FLOOR)
            {
                //Console.WriteLine("HORAY!!!");
                left = FeetState.footPosition.Up;
            }
            else
                left = FeetState.footPosition.Down;
            if (rightFootY > FLOOR)
                right = FeetState.footPosition.Up;
            else
                right = FeetState.footPosition.Down;
            FeetState last = walkHistory.Last();
            //Console.WriteLine("time diff: " + (timeStamp - last.timeStamp).ToString());
            if (left != last.left || right != last.right || timeStamp - last.timeStamp >= TIME_DIFF)
            {
                Console.WriteLine("l:" + leftFootY + "r:" + rightFootY);
                FeetState fs = new FeetState(left, right, timeStamp);
                walkHistory.Enqueue(fs);
            }
        }

    }
}
