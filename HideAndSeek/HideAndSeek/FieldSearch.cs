//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace HideAndSeek
//{
//    //may be obselete??!?!
//    // implementation of DFS algorithm
//    class FieldSearch
//    {
//        static public LinkedList<Direction> FindPath(FieldMap map, FieldNode start, FieldNode goal)
//        {
//            LinkedList<FieldNode> path = new LinkedList<FieldNode>();
//            Stack<FieldNode> stack = new Stack<FieldNode>();
//            stack.Push(start);
//            while (stack.Count > 0)
//            {
//                FieldNode node = stack.Pop();
//                //AddToPath(path, node);
//                if (node.Equals(goal))
//                    return TranslateToDirections(path);
//                LinkedList<FieldNode> sons = map.findSons(node);
//                //Iterator<Node> iter = sons.descendingIterator();
//                foreach (FieldNode son in sons)
//                {
//                    if (!stack.Contains(son) && !path.Contains(son))
//                        stack.Push(son);
//                }
//            }
//            return null;
//        }

//        private static LinkedList<Direction> TranslateToDirections(LinkedList<FieldNode> path)
//        {
//            LinkedList<Direction> list = new LinkedList<Direction>();
//            //foreach (FieldNode node in path)
//            return list;
//        }

//        //private static void AddToPath(LinkedList<FieldNode> path, FieldNode node)
//        //{
//        //    while (path.Count>0 && !path.Last.Equals(node.father))
//        //        path.RemoveLast();
//        //    if (!path.Contains(node))
//        //        path.AddLast(node);
//        //}
//    }
//}
