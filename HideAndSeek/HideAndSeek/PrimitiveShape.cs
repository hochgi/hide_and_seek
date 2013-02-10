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
        Vector3 pos;

        public PrimitiveShape(Vector3 position)
        {
            pos = position;
        }

        public Vector3 getPosition() { return pos; }

        abstract public bool isBlockingLineOfSight(Vector3 a, Vector3 b);
    }

    public class Sphere : PrimitiveShape
    {
        float radius;
        

        public Sphere(float radius, Vector3 position) : base(position)
        {
            this.radius = radius;
        }

        // the idea of the implementation can be found here:
        // http://paulbourke.net/geometry/pointline/
        public override bool isBlockingLineOfSight(Vector3 a, Vector3 b) 
        {
            Vector3 pos = getPosition();

            Vector3 cross = Vector3.Cross(new Vector3(pos.X - a.X, pos.Y - a.Y, pos.Z - a.Z), new Vector3(pos.X - b.X, pos.Y - b.Y, pos.Z - b.Z));
            float d = cross.Length() / Vector3.Distance(a, b);

            //float u = -(Vector3.Dot(new Vector3(a.X - pos.X, a.Y - pos.Y, a.Z - pos.Z), new Vector3(b.X - a.X, b.Y - a.Y, b.Z - a.Z)) / Vector3.DistanceSquared(a, b));
            //Vector3 plumbPoint = new Vector3(a.X + u * (b.X - a.X), a.Y + u * (b.Y - a.Y), a.Z + u * (b.Z - a.Z));
            return d < radius;//Vector3.Distance(pos, plumbPoint) < radius;
        }
    }

    public class VerticalCylinder : PrimitiveShape
    {
        float radius;
        float height;

        public VerticalCylinder(float radius, float height, Vector3 position) : base(position)
        {
            this.radius = radius;
            this.height = height;
        }

        // the idea of the implementation can be found here:
        //http://paulbourke.net/geometry/lineline3d/
        public override bool isBlockingLineOfSight(Vector3 p1, Vector3 p2)
        {
            /* 1. find plumbpoint to the line f(y) = (pos.x,pos.z)
             * 2. if point is higher than pos.y + height, or lower than pos.y, return false
             * 3. if distance from plumbpoint to (pos.x,plumbpoint.y,pos.z) < radius, return true, else false.
             */

            Vector3 p3 = getPosition();
            Vector3 p4 = new Vector3(p3.X, p3.Y + height, p3.Z);

            float x1 = p1.X, y1 = p1.Y, z1 = p1.Z,
                  x2 = p2.X, y2 = p2.Y, z2 = p2.Z,
                  x3 = p3.X, y3 = p3.Y, z3 = p3.Z,
                  x4 = p4.X, y4 = p4.Y, z4 = p4.Z;

            float d1321 = (x1 - x3) * (x2 - x1) + (y1 - y3) * (y2 - y1) + (z1 - z3) * (z2 - z1);
            float d2121 = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1);
            float d4321 = (x4 - x3) * (x2 - x1) + (y4 - y3) * (y2 - y1) + (z4 - z3) * (z2 - z1);
            float d1343 = (x1 - x3) * (x4 - x3) + (y1 - y3) * (y4 - y3) + (z1 - z3) * (z4 - z3);
            float d4343 = (x4 - x3) * (x4 - x3) + (y4 - y3) * (y4 - y3) + (z4 - z3) * (z4 - z3);

            float mua = (d1343 * d4321 - d1321 * d4343) / (d2121 * d4343 - d4321 * d4321);

            Vector3 pa = p1 + mua * (p2 - p1);

            if (pa.Y < getPosition().Y || pa.Y > (getPosition().Y + height))
            {
                return false;
            }
            else if (((pa.X - p3.X) * (pa.X - p3.X) + (pa.Z - p3.Z) * (pa.Z - p3.Z)) < (radius * radius))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class AxisAlignedCube : PrimitiveShape
    {
        Vector3 topCorner;

        private static Vector3 min(Vector3 p1, Vector3 p2)
        {
            float x = p1.X, y = p1.Y, z = p1.Z;

            if (p1.X > p2.X)
            {
                x = p2.X;
            }

            if (p1.Y > p2.Y)
            {
                y = p2.Y;
            }

            if (p1.Z > p2.Z)
            {
                z = p2.Z;
            }

            return new Vector3(x, y, z);
        }

        private static Vector3 max(Vector3 p1, Vector3 p2)
        {
            float x = p1.X, y = p1.Y, z = p1.Z;

            if (p1.X < p2.X)
            {
                x = p2.X;
            }

            if (p1.Y < p2.Y)
            {
                y = p2.Y;
            }

            if (p1.Z < p2.Z)
            {
                z = p2.Z;
            }

            return new Vector3(x, y, z);
        }

        public AxisAlignedCube(Vector3 p1, Vector3 p2) : base(min(p1, p2))
        {
            this.topCorner = max(p1, p2);
        }

        public override bool isBlockingLineOfSight(Vector3 p1, Vector3 p2)
        {
            throw new NotImplementedException();
        }
    }
}
