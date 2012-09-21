using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HideAndSeek
{
    // holds a representation of playing field so that players can find best route to hiding place
    public class FieldMap
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
            map[x, y]++;
        }

        //notify map that someone moved from [x,y] to [z,w]
        public void moveSomeone(int x, int y, int z, int w)
        {
            map[x, y]--;
            map[z, w]++;
        }

        // find all spaces where it is possible to go to from current node
        public LinkedList<FieldNode> findSons(FieldNode node) 
        {
            LinkedList<FieldNode> sons = new LinkedList<FieldNode>();
            int x = node.x;
            int y = node.y;
            if (x >= sizeX || y >= sizeY || x < 0 || y < 0)
                return null;
            if (y + 1 < sizeY && map[x, y + 1] <= 0)
                sons.AddLast(new FieldNode(x, y + 1));
            if (x + 1 < sizeX && y + 1 < sizeY && map[x + 1, y + 1] <= 0)
                sons.AddLast(new FieldNode(x + 1, y + 1));
            if (x + 1 < sizeX && map[x + 1, y] <= 0)
                sons.AddLast(new FieldNode(x + 1, y));
            if (x + 1 < sizeX && y - 1 >= 0 && map[x + 1, y - 1] <= 0)
                sons.AddLast(new FieldNode(x + 1, y - 1));
            if (y - 1 >= 0 && map[x, y - 1] <= 0)
                sons.AddLast(new FieldNode(x, y - 1));
            if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1, y - 1] <= 0)
                sons.AddLast(new FieldNode(x - 1, y - 1));
            if (x - 1 >= 0 && map[x - 1, y] <= 0)
                sons.AddLast(new FieldNode(x - 1, y));
            if (x - 1 >= 0 && y + 1 < sizeY && map[x - 1, y + 1] <= 0)
                sons.AddLast(new FieldNode(x - 1, y + 1));
            return sons;
        }

        // find first available space to move to 
        public FieldNode firstSon (FieldNode node)
        {
            int x = node.x;
            int y = node.y;
            if (x >= sizeX || y >= sizeY || x < 0 || y < 0)
                return null;
            if (y + 1 < sizeY && map[x, y + 1] <= 0)
                return new FieldNode(x, y + 1);
            if (x + 1 < sizeX && y + 1 < sizeY && map[x + 1, y + 1] <= 0)
                return new FieldNode(x + 1, y + 1);
            if (x + 1 < sizeX && map[x + 1, y] <= 0)
                return new FieldNode(x + 1, y);
            if (x + 1 < sizeX && y - 1 >= 0 && map[x + 1, y - 1] <= 0)
                return new FieldNode(x + 1, y - 1);
            if (y - 1 >= 0 && map[x, y - 1] <= 0)
                return new FieldNode(x, y - 1);
            if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1, y - 1] <= 0)
                return new FieldNode(x - 1, y - 1);
            if (x - 1 >= 0 && map[x - 1, y] <= 0)
                return new FieldNode(x - 1, y);
            if (x - 1 >= 0 && y + 1 < sizeY && map[x - 1, y + 1] <= 0)
                return new FieldNode(x - 1, y + 1);
            if (y + 1 < sizeY)
                return new FieldNode(x, y + 1);
            if (x + 1 < sizeX)
                return new FieldNode(x + 1, y);
            if (y - 1 >= 0)
                return new FieldNode(x, y - 1);
            if (x - 1 >= 0)
                return new FieldNode(x - 1, y);
            return null;
        }

        public override string ToString()
        {
            String res = "";
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                    res += map[i, j];
                res += "\n";
            }
            return res;
        }
    }
}
