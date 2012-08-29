using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    class FieldNode
    {
        public int x;
        public int y;
        public int depth;
        public FieldNode father;

        public FieldNode(int x, int y, int depth, FieldNode father)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
            this.father = father;
        }

        public override bool Equals(FieldNode other)
        {
            return (x == other.x && y == other.y);
        }

    }
}
