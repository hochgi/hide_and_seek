using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    // holds a representation of playing field so that players can find best route to hiding place
    class FieldMap
    {
        int sizeX;
        int sizeY;

        int[,] map;

        public FieldMap(int x, int y)
        {
            sizeX = x;
            sizeY = y;
            map = new int[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                    map[i,j] = 0;
        }

        // add a space in map which players can't walk on
        public void addBlock(int x, int y)
        {
            map[x,y] = 1;
        }

        // find all spaces where it is possible to go to from current node
        public LinkedList<FieldNode> findSons(FieldNode node) 
        {
            LinkedList<FieldNode> sons = new LinkedList<FieldNode>();
            int x = node.x;
            int y = node.y;
            int depth = node.depth + 1;
            if (x >= sizeX || y >= sizeY || x < 0 || y < 0)
                return null;
            if (y + 1 < sizeY && map[x, y + 1] != 1)
                sons.AddLast(new FieldNode(x, y + 1, depth, node));
            if (x + 1 < sizeX && y + 1 < sizeY && map[x + 1,y + 1] != 1)
                sons.AddLast(new FieldNode(x + 1, y + 1, depth, node));
            if (x + 1 < sizeX && map[x + 1,y] != 1)
                sons.AddLast(new FieldNode(x + 1, y, depth, node));
            if (x + 1 < sizeX && y - 1 >= 0 && map[x + 1,y - 1] != 1)
                sons.AddLast(new FieldNode(x + 1, y - 1, depth, node));
            if (y - 1 >= 0 && map[x,y - 1] != 1)
                sons.AddLast(new FieldNode(x, y - 1, depth, node));
            if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1,y - 1] != 1)
                sons.AddLast(new FieldNode(x - 1, y - 1, depth, node));
            if (x - 1 >= 0 && map[x - 1,y] != 1)
                sons.AddLast(new FieldNode(x - 1, y, depth, node));
            if (x - 1 >= 0 && y + 1 < sizeY && map[x - 1,y + 1] != 1)
                sons.AddLast(new FieldNode(x - 1, y + 1, depth, node));
            return sons;
        }
    }
}
