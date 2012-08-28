using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    // inheriting classes should be simple calculateable shapes
    // such as sphere, vertical/horizontal cylinder, cube, etc'...
    abstract class PrimitiveShape
    {
        abstract public bool isBlockingLineOfSight(Vector3 a, Vector3 b);
        abstract public Vector3 getPosition();
    }
}
