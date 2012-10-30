using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    //represents a node in the field map
    public class FieldNode
    {
        public int x;
        public int y;

        //constructor for FieldNode class
        public FieldNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        //calculate distance between to spaces
        public int ManhattanDist(FieldNode other)
        {
            return Math.Abs(x - other.x) + Math.Abs(y - other.y);
        }

        //returns a string representation of the node
        public override string ToString()
        {
            return "FieldNode: (" + x + "," + y + ")";
        }

    }
}
