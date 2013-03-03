using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.FaceTracking;

namespace HideAndSeek
{
    /// <summary>
    /// This class holds and interprets inputs from human player via Kinect
    /// </summary>
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
        private int countPointing;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        internal KinectInput(Game game)
            : base(game)
        {
            head = new Vector3();
            pointing = false;
            countPointing = 0;
            //initializing the walk history
            walkHistory = new Queue<FeetState>(12);
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


        
        /// <summary>
        /// Computes the walking state of the player according to series of feet states
        /// what about speed??
        /// </summary>
        /// <returns>the walking state of the player, i.e. forwards, backwards, not-walking</returns>
        internal override WalkingState getWalkingState()
        {
            int length = walkHistory.Count();
            FeetState first = walkHistory.ElementAt(length - 1);
            FeetState second = walkHistory.ElementAt(length - 2);
            FeetState third = walkHistory.ElementAt(length - 3);
            FeetState forth = walkHistory.ElementAt(length - 4);
            //checking if the series of last four feet state represents walking
            if (//(D,D),(U,D),(D,D),(D,U)
                first.isBothDown()
                && second.isLeftUp()
                && third.isBothDown()
                && forth.isRightUp()
                ||
                //(D,D),(D,U),(D,D),(U,D)
                first.isBothDown()
                && second.isRightUp()
                && third.isBothDown()
                && forth.isLeftUp()
                ||
                //(U,D),(D,D),(D,U),(D,D)
                first.isLeftUp()
                && second.isBothDown()
                && third.isRightUp()
                && forth.isBothDown()
                ||
                //(D,U),(D,D),(U,D),(D,D)
                first.isRightUp()
                && second.isBothDown()
                && third.isLeftUp()
                && forth.isBothDown())
            {
                //getting face direction in order to know direction of walking
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

        //create a vector3 from skeleton-point
        internal Vector3 getVectorFromPoint(SkeletonPoint p)
        {
            Vector3 v = new Vector3();
            v.X = p.X;
            v.Y = p.Y;
            v.Z = p.Z;
            return v;
        }

        
        /// <summary>
        /// returns the position of the head of the player
        /// </summary>
        /// <returns>possition of the head of the player</returns>
        internal override Microsoft.Xna.Framework.Vector3 getHeadPosition()
        {
            return head;
        }

        /// <summary>
        /// returns all the positions of the body-parts dtected by the Kinect
        /// </summary>
        /// <returns>list of positions of player's body-parts</returns>
        internal override List<Vector3> getPositions()
        {
            return positions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if player is pointing and false otherwise</returns>
        internal override bool isPointing()
        {
            Console.WriteLine("POINTING=" + pointing);
            return pointing;
        }

        /// <summary>
        /// This class represents the feet-state of the player on a specific moment.
        /// It contains each foot position i.e. Up or Down, and a timestamp.
        /// </summary>
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

        /// <summary>
        /// Event handler for events from the Kinect.
        /// Calculates player's position, pointing state, face direction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
            {
                if (SFrame == null)
                {
                    // The image processing took too long. More than 2 frames behind.
                }
                else
                {
                    //get skeletons detected by kinect
                    skeletons = new Skeleton[SFrame.SkeletonArrayLength];
                    SFrame.CopySkeletonDataTo(skeletons);
                    receivedData = true;
                }
            }

            if (receivedData && skeletons != null)
            {
                //get the first skeleton detected by the kinect
                Skeleton currentSkeleton = (from s in skeletons
                                            where s.TrackingState ==
                                            SkeletonTrackingState.Tracked
                                            select s).FirstOrDefault();
                if (currentSkeleton != null)
                {
                    //get head position from the skeleton
                    SkeletonPoint skeletonHead = currentSkeleton.Joints[JointType.FootLeft].Position;
                    //transform the head position to be vector
                    head = getVectorFromPoint(skeletonHead);
                    //get feet positions and push to walking queue in order to detect walking or not
                    SkeletonPoint skeletonLeftFoot = currentSkeleton.Joints[JointType.FootLeft].Position;
                    SkeletonPoint skeletonRightFoot = currentSkeleton.Joints[JointType.FootRight].Position;
                    pushToWalkingQueue(skeletonLeftFoot.Y, skeletonRightFoot.Y);
                    //get right hand position in order to identify pointing (currently pointing is raising your right hand)
                    SkeletonPoint skeletonRightHand = currentSkeleton.Joints[JointType.HandRight].Position;
                    rightHand = getVectorFromPoint(skeletonRightHand);
                    //count frames where right hand is raised in order to avoide flase detections
                    if (rightHand.Y > head.Y)
                    {
                        countPointing++;
                        if (countPointing > 3)
                            pointing = true;
                        else
                            pointing = false;
                    }
                    else
                    {
                        countPointing = 0;
                        pointing = false;
                    }
                    //get all skeleton positions
                    positions = new List<Vector3>();
                    foreach (Joint j in currentSkeleton.Joints)
                    {
                        positions.Add(getVectorFromPoint(j.Position));
                    }
                    //track face
                    FaceTrackFrame faceFrame = faceTracker.Track(sensor.ColorStream.Format, colorPixelData,
                                  sensor.DepthStream.Format, depthPixelData,
                                  currentSkeleton);
                    //if face tracked successfully, player is facing the camera
                    //and otherwise player is back to the camera
                    if (faceFrame.TrackSuccessful)
                    {
                        faceDirection = FaceDirection.Forwards;
                        prevFaceTracked = true;
                        Console.WriteLine("face forwards");
                    }
                    else //if (currentSkeleton != null)
                    {
                        //avoiding false detections of backwards
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

        /// <summary>
        /// calculate the feet-state and push to walking queue
        /// </summary>
        /// <param name="leftFootY">y-coordinate of left foot</param>
        /// <param name="rightFootY">y-coordinate of right foot</param>
        internal void pushToWalkingQueue(float leftFootY, float rightFootY)
        {
            //calc datetime of now
            DateTime timeStamp = DateTime.Now;

            FeetState.footPosition left, right;
            //if the difference between the coordinates in greater than threshold,
            //then one feet is considered up and the other is considered down.
            //otherwise, both considered down
            if (leftFootY - rightFootY > 0.1)
            {
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
            //push the current feet-state to queue only if it is different from the last one pushed,
            //or if the difference between the timestamps is greater than a certain threshold
            if (left != last.left || right != last.right || timeStamp - last.timeStamp >= TIME_DIFF)
            {
                Console.WriteLine("l:" + leftFootY + "r:" + rightFootY + ",(" + left + "," + right + ")");
                FeetState fs = new FeetState(left, right, timeStamp);
                walkHistory.Enqueue(fs);
            }
        }

    }
}
