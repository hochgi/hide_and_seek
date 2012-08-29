using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    // inheriting classes should be simple calculateable shapes
    // such as sphere, vertical/horizontal cylinder, cube, etc'...
    public abstract class PrimitiveShape
    {
        abstract public bool isBlockingLineOfSight(Vector3 a, Vector3 b);
        abstract public Vector3 getPosition();
    }

    public class Sphere : PrimitiveShape
    {
        float rad;
        Vector3 pos;

        public Sphere(float radius, Vector3 position) 
        {
            this.pos = position;
            this.rad = radius;
        }

        public override Vector3 getPosition() { return pos; }

        public override bool isBlockingLineOfSight(Vector3 a, Vector3 b) 
        {
            throw new NotImplementedException();
            //Vector3 plumbPoint = the closest point on the line from "a"-"b" to the center of the sphere (pos)
            //float length = new Vector3(plumbPoint.X - pos.X, plumbPoint.Y - pos.Y, plumbPoint.Z - pos.Z).Length();
            //return length < rad;
        }
    }
}
