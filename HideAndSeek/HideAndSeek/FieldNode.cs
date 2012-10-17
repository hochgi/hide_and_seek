using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    // may be obselete?  just replace with vector2!
    //represents a node in the field map
    public class FieldNode
    {
        public int x;
        public int y;

        public FieldNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        //calculate distance between to spaces
        //we may decide to use a better calculation.
        public int ManhattanDist(FieldNode other)
        {
            return Math.Abs(x - other.x) + Math.Abs(y - other.y);
        }

        public override string ToString()
        {
            return "FieldNode: (" + x + "," + y + ")";
        }

    }
}
