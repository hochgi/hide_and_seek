using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    // may be obselete?  just replace with vector2!
    //represents a node in the field map
    class FieldNode
    {
        public int x;
        public int y;

        public FieldNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        //public override bool Equals(FieldNode other)
        //{
        //    return (x == other.x && y == other.y);
        //}

    }
}
