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
        public FieldNode findBestNotSeen(FieldNode node, int[,] seenMap) 
        {
            if (seenMap.GetLength(0) != sizeX || seenMap.GetLength(1) != sizeY)
                return null;
            LinkedList<FieldNode> sons = new LinkedList<FieldNode>();
            int x = node.x;
            int y = node.y;
            if (x >= sizeX || y >= sizeY || x < 0 || y < 0)
                return null;
            //may make sense to change around order on these spaces
            if (y + 1 < sizeY && map[x, y + 1] <= 0 && seenMap[x, y + 1] == 0)
                return new FieldNode(x, y + 1);
            if (x + 1 < sizeX && y + 1 < sizeY && map[x + 1, y + 1] <= 0 && seenMap[x + 1, y + 1] == 0)
                return new FieldNode(x + 1, y + 1);
            if (x + 1 < sizeX && map[x + 1, y] <= 0 && seenMap[x + 1, y] == 0)
                return new FieldNode(x + 1, y);
            if (x + 1 < sizeX && y - 1 >= 0 && map[x + 1, y - 1] <= 0 && seenMap[x + 1, y - 1] == 0)
                return new FieldNode(x + 1, y - 1);
            if (y - 1 >= 0 && map[x, y - 1] <= 0 && seenMap[x, y - 1] == 0)
                return new FieldNode(x, y - 1);
            if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1, y - 1] <= 0 && seenMap[x - 1, y - 1] == 0)
                return new FieldNode(x - 1, y - 1);
            if (x - 1 >= 0 && map[x - 1, y] <= 0 && seenMap[x - 1, y] == 0)
                return new FieldNode(x - 1, y);
            if (x - 1 >= 0 && y + 1 < sizeY && map[x - 1, y + 1] <= 0 && seenMap[x - 1, y+1] == 0)
                return new FieldNode(x - 1, y + 1);
            FieldNode best = null;
            int bestVal = 101;
            if (y + 1 < sizeY && map[x, y + 1] <= 0)
            {
                FieldNode next = new FieldNode(x, y + 1);
                int val = seenMap[x, y + 1];
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x + 1 < sizeX && y + 1 < sizeY && map[x + 1, y + 1] <= 0)
            {
                FieldNode next = new FieldNode(x + 1, y + 1);
                int val = seenMap[x + 1, y + 1];
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x + 1 < sizeX && map[x + 1, y] <= 0)
            {
                FieldNode next = new FieldNode(x + 1, y);
                int val = seenMap[x + 1, y];
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x + 1 < sizeX && y - 1 >= 0 && map[x + 1, y - 1] <= 0)
            {
                FieldNode next = new FieldNode(x + 1, y - 1);
                int val = seenMap[x + 1, y - 1];
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (y - 1 >= 0 && map[x, y - 1] <= 0)
            {
                FieldNode next = new FieldNode(x, y - 1);
                int val = seenMap[x, y - 1];
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1, y - 1] <= 0)
            {
                FieldNode next = new FieldNode(x - 1, y - 1);
                int val = seenMap[x - 1, y - 1];
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x - 1 >= 0 && map[x - 1, y] <= 0)
            {
                FieldNode next = new FieldNode(x - 1, y);
                int val = seenMap[x - 1, y];
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x - 1 >= 0 && y + 1 < sizeY && map[x - 1, y + 1] <= 0)
            {
                FieldNode next = new FieldNode(x - 1, y + 1);
                int val = seenMap[x - 1, y + 1];
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            return best;
        }

        //returns available node which is closest to goal (using manhattan distance)
        public FieldNode getClosestNext(FieldNode node, FieldNode goal)
        {
            if (Math.Abs(node.x - goal.x) == 1 || Math.Abs(node.y - goal.y) == 1)
            {
                if (map[goal.x, goal.y] == 1)
                    return goal;
                else
                    throw new SpotTakenException();
            }
            int x = node.x;
            int y = node.y;
            if (x >= sizeX || y >= sizeY || x < 0 || y < 0)
                return null;
            FieldNode best = null;
            int bestVal = 300;//??
            if (y + 1 < sizeY && map[x, y + 1] <= 0)
            {
                FieldNode next = new FieldNode(x, y + 1);
                int val = next.ManhattanDist(goal);
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x + 1 < sizeX && y + 1 < sizeY && map[x + 1, y + 1] <= 0)
            {
                FieldNode next = new FieldNode(x + 1, y + 1);
                int val = next.ManhattanDist(goal);
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x + 1 < sizeX && map[x + 1, y] <= 0)
            {
                FieldNode next = new FieldNode(x + 1, y);
                int val = next.ManhattanDist(goal);
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x + 1 < sizeX && y - 1 >= 0 && map[x + 1, y - 1] <= 0)
            {
                FieldNode next = new FieldNode(x + 1, y - 1);
                int val = next.ManhattanDist(goal);
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (y - 1 >= 0 && map[x, y - 1] <= 0)
            {
                FieldNode next = new FieldNode(x, y - 1);
                int val = next.ManhattanDist(goal);
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x - 1 >= 0 && y - 1 >= 0 && map[x - 1, y - 1] <= 0)
            {
                FieldNode next = new FieldNode(x - 1, y - 1);
                int val = next.ManhattanDist(goal);
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x - 1 >= 0 && map[x - 1, y] <= 0)
            {
                FieldNode next = new FieldNode(x - 1, y);
                int val = next.ManhattanDist(goal);
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            if (x - 1 >= 0 && y + 1 < sizeY && map[x - 1, y + 1] <= 0)
            {
                FieldNode next = new FieldNode(x - 1, y + 1);
                int val = next.ManhattanDist(goal);
                if (val < bestVal)
                {
                    bestVal = val;
                    best = next;
                }
            }
            return best;
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

        internal bool isAvailable(FieldNode fieldNode)
        {
            return (map[fieldNode.x, fieldNode.y] == 0);
        }

        internal FieldNode findRunSpace(FieldNode node)
        {
            if (node == null)
                return null;
            int x = node.x;
            int y = node.y;
            if (y - 1 >= 0 && map[x, y - 1] == 0)
                return new FieldNode(x, y - 1);
            if (y - 1 >= 0 && x - 1 >= 0 && map[x - 1, y - 1] == 0)
                return new FieldNode(x - 1, y - 1);
            if (y - 1 >= 0 && x + 1 < sizeX && map[x + 1, y - 1] == 0)
                return new FieldNode(x + 1, y - 1);
            if (x - 1 >= 0 && map[x - 1, y] == 0)
                return new FieldNode(x - 1, y);
            if (x + 1 < sizeX && map[x + 1, y] == 0)
                return new FieldNode(x + 1, y);
            if (y + 1 < sizeY && x - 1 >= 0 && map[x - 1, y + 1] == 0)
                return new FieldNode(x - 1, y + 1);
            if (y + 1 < sizeY && map[x, y + 1] == 0)
                return new FieldNode(x, y + 1);
            if (y + 1 < sizeY && x + 1 < sizeX && map[x + 1, y + 1] == 0)
                return new FieldNode(x + 1, y + 1);
            return null;
        }
    }

    public class SpotTakenException : Exception
    {
    }
}
