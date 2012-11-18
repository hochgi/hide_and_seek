using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HideAndSeek
{
    // holds a representation of playing field so that players can find best route to their destination
    public class FieldMap
    {
        int sizeX;
        int sizeY;

        int[,] map;
        List<Item>[,] itemMap;

        //constructor for FieldMap class
        public FieldMap(int x, int y)
        {
            sizeX = x;
            sizeY = y;
            map = new int[x, y];
            itemMap = new List<Item>[x, y];
            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    map[i, j] = 0;
                    itemMap[i, j] = null;
                }
        }

        // add a space in map which players can't walk on
        public void addBlock(int x, int y)
        {
            map[x, y]++;
        }

        // add item to map
        public void addItem(Item item, int x, int y)
        {
            map[x, y]++;
            if (itemMap[x, y] == null)
                itemMap[x, y] = new List<Item>();
            itemMap[x, y].Add(item);
        }

        //notify map that someone moved from [x,y] to [z,w]
        public void moveSomeone(FieldNode node1, FieldNode node2)
        {
            if (node1 != null && node1.x >= 0 && node1.y >= 0)
                map[node1.x, node1.y]--;
            if (node2 != null && node2.x >= 0 && node2.y >= 0)
                map[node2.x, node2.y]++;
        }

        // find the neighbor where the Seeker has been least recently
        public FieldNode findBestNotSeen(FieldNode node, int[,] seenMap) 
        {
            if (seenMap.GetLength(0) != sizeX || seenMap.GetLength(1) != sizeY)
                return null;
            LinkedList<FieldNode> sons = new LinkedList<FieldNode>();
            int x = node.x;
            int y = node.y;
            if (x >= sizeX || y >= sizeY || x < 0 || y < 0)
                return null;
            //if there is a neighbor which hasn't been seen recently, return it
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
            // go through all neighbors and find the one that has been seen least recently
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
            //return the neighbor which has been seen least recently
            return best;
        }

        //returns available node which is closest to goal (using manhattan distance)
        public FieldNode getClosestNext(FieldNode node, FieldNode goal)
        {
            //if the goal is a neighbor
            if (Math.Abs(node.x - goal.x) == 1 || Math.Abs(node.y - goal.y) == 1)
            {
                //if the only item in the goal space is the hiding spot, return it
                if (map[goal.x, goal.y] == 1)
                    return goal;
                //otherwise, need to find a new hiding spot
                else
                    throw new SpotTakenException();
            }
            int x = node.x;
            int y = node.y;
            if (x >= sizeX || y >= sizeY || x < 0 || y < 0)
                return null;
            //go through all neighbors and find the one which is closest to the hiding spot, using Manhattan distancea
            FieldNode best = null;
            int bestVal = 300;
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

        //returns a string representation of the map
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

        //check if space on map is still available
        internal bool isAvailable(FieldNode fieldNode)
        {
            return (map[fieldNode.x, fieldNode.y] == 0);
        }

        //find best space to go to while racing to tree
        internal FieldNode findRunSpace(FieldNode node)
        {
            if (node == null)
                return null;
            int x = node.x;
            int y = node.y;
            //if possible, go to the nodes which are closer to the tree (meaning a smaller y value)
            if (y - 1 >= 0 && map[x, y - 1] == 0)
                return new FieldNode(x, y - 1);
            if (y - 1 >= 0 && x - 1 >= 0 && map[x - 1, y - 1] == 0)
                return new FieldNode(x - 1, y - 1);
            if (y - 1 >= 0 && x + 1 < sizeX && map[x + 1, y - 1] == 0)
                return new FieldNode(x + 1, y - 1);
            //otherwise, go sideways (equal y value)
            if (x - 1 >= 0 && map[x - 1, y] == 0)
                return new FieldNode(x - 1, y);
            if (x + 1 < sizeX && map[x + 1, y] == 0)
                return new FieldNode(x + 1, y);
            //if none of the above nodes are available, go to nodes which are farther away from the tree (larger y value)
            if (y + 1 < sizeY && x - 1 >= 0 && map[x - 1, y + 1] == 0)
                return new FieldNode(x - 1, y + 1);
            if (y + 1 < sizeY && map[x, y + 1] == 0)
                return new FieldNode(x, y + 1);
            if (y + 1 < sizeY && x + 1 < sizeX && map[x + 1, y + 1] == 0)
                return new FieldNode(x + 1, y + 1);
            return null;
        }

        //lower count of objects in a certain space
        internal void removeBlock(FieldNode fieldNode)
        {
            map[fieldNode.x, fieldNode.y]--;
        }

        //returns whether location is in conflict with one of the items
        internal bool isConflict(Vector3 location, FieldNode fieldNode)
        {
            List<Item> items = itemMap[fieldNode.x, fieldNode.y];
            if (items == null)
                return false;
            foreach (Item item in items)
                if (item.isConflict(location))
                    return true;
            return false;
        }
    }

    //exception which is thrown when another hider has taken this hider's designated hiding spot, to notify hider to choose a new spot
    public class SpotTakenException : Exception
    {
    }
}
