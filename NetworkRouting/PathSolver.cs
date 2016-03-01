using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    static class PathSolver
    {
        private static int NODE_LOW = -1;

       /*
        * Finds the shortest path between startNode and stopNode.
        * 
        * If such a path exists, returns a list of the nodes that form the path
        * from startNode to stopNode. If such a path doesn't exist, returns null.
        */
        public static List<int> findShortestPath(IDijkstraShortestPathQueue queue, List<PointF> points, List<HashSet<int>> adjacencyList, int startNodeIndex, int stopNodeIndex)
        {          
            //Each point needs to have exactly one adjacency list 
            if (points.Count != adjacencyList.Count)
                throw new SystemException("List of points and adjacency list aren't the same size");

            // Initialize dist and prev arrays
            float[] dist = Enumerable.Repeat(float.MaxValue, points.Count).ToArray();
            int[] prev = Enumerable.Repeat(NODE_LOW, points.Count).ToArray();

            // Initialize the queue with the start node value
            queue.insert(startNodeIndex);
            dist[startNodeIndex] = 0;

            // This algorithm should run as long as there is a new node to explore
            // on the queue
            while (queue.getQueueCount() > 0)
            {
                // minIndex is the index of the node that currently has the least weight and has not been visited.
                // The minIndex is the index of this node in the points list.
                int minIndex = queue.deleteMin(dist);
                PointF u = points[minIndex];
                HashSet<int> neighbors = adjacencyList[minIndex];

                foreach(int neighborIndex in neighbors)
                {
                    PointF v = points[neighborIndex];

                    // This implementation of the algorithm not designed to handle
                    // circular references to nodes.
                    if (minIndex == neighborIndex || u.Equals(v))
                        throw new SystemException("Point with index" + minIndex + "  has a circular reference to itself");

                    float alternateDistance = dist[minIndex] + calcDistanceBtwnPoints(u, v);
                    // If the alternate distance is shorter than the currently known distance:
                    // Update the niehgboring node's distance in the dist array
                    // Reset the neighbor node's parent to be the current node
                    // Decrease the distance key value of the neighboring node in the queue (possibly 
                    // causing the neighboring node to be removed from the queue sooner).
                    if(alternateDistance < dist[neighborIndex])
                    {
                        dist[neighborIndex] = alternateDistance;
                        prev[neighborIndex] = minIndex;
                        queue.decreaseKey(neighborIndex, dist);
                    }
                }
            }

            // getPathToNode determines if there is a path from
            // stopNodeIndex to startNodeIndex. If there is, it returns that
            // path in OPPOSITE order (starting at startNodeIndex and ending at
            // stopNodeIndex). If there isn't, it returns null.
            return getPathToNode(prev, startNodeIndex, stopNodeIndex);
        }

       /*
        *  Calculates the distance between two points
        */
        public static float calcDistanceBtwnPoints(PointF one, PointF two)
        {
            return (float) Math.Sqrt((two.Y - one.Y) * (two.Y - one.Y) + (two.X - one.X) * (two.X - one.X));
        }

        /*
         * Determines if there is a path from
         * stopNode to startNode. If there is, it returns that
         * path in OPPOSITE order (starting at startNode and ending at
         * stopNode). If there isn't, it returns null.
         */
        public static List<int> getPathToNode(int[] prev, int startNode, int stopNode)
        {
            List<int> reverseOrder = new List<int>();
            HashSet<int> visitedNodes = new HashSet<int>();

            int index = stopNode;
            while (prev[index] != NODE_LOW)
            {
                if(!visitedNodes.Contains(index))
                {
                    visitedNodes.Add(index);
                    reverseOrder.Add(index);
                    index = prev[index];
                }
                // If the node has already been visited, then we've hit a cycle
                // This should never happen bc Dijkstra's algorithm always visits the node
                // with the smallest distance first.
                else
                {
                    throw new SystemException("Cycle found in prev array");
                }
            }

            // Make sure the first node gets added to the order
            reverseOrder.Add(index);

            if (reverseOrder[reverseOrder.Count - 1] != startNode)
                return null;

            reverseOrder.Reverse();
            return reverseOrder;
        }

        public static String pathToString(List<int> order)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < order.Count; i++)
            {
                sb.Append(order[i]);
                sb.Append(", ");
            }

            // Remove the , and the space left by the last iteration
            if(sb.Length > 0)
                sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }

    }
}
