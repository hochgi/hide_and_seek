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

        // the idea of the implementation can be found here:
        // http://paulbourke.net/geometry/pointline/
        public override bool isBlockingLineOfSight(Vector3 a, Vector3 b) 
        {
            float u = ((pos.X - a.X) * (b.X - a.X) + (pos.Y - a.Y) * (b.Y - a.Y) + (pos.Z - a.Z) * (b.Z - a.Z)) / Vector3.DistanceSquared(a, b);
            Vector3 plumbPoint = new Vector3(a.X + u * (b.X - a.X), a.Y + u * (b.Y - a.Y), a.Z + u * (b.Z - a.Z));
            return Vector3.Distance(pos, plumbPoint) < rad;
        }
    }
}
