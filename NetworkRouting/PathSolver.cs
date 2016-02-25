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

        public static List<int> findShortestPath(IDijkstraShortestPathQueue queue, List<PointF> points, List<HashSet<int>> adjacencyList, int startNodeIndex, int stopNodeIndex)
        {
            if (points.Count != adjacencyList.Count)
                throw new SystemException("List of points and adjacency list aren't the same size");

            // Initialize dist and prev arrays
            float[] dist = Enumerable.Repeat(float.MaxValue, points.Count).ToArray();
            int[] prev = Enumerable.Repeat(NODE_LOW, points.Count).ToArray();

            // Initialize the queue with the start node value
            dist[startNodeIndex] = 0;
            queue.insert(startNodeIndex, 0);

            while(queue.getQueueCount() > 0)
            {
                int minIndex = queue.deleteMin();
                PointF u = points[minIndex];
                HashSet<int> neighbors = adjacencyList[minIndex];
                foreach(int neighborIndex in neighbors)
                {
                    PointF v = points[neighborIndex];
                    if (minIndex == neighborIndex || u.Equals(v))
                        throw new SystemException("Point U has a circular reference to itself");

                    float alternateDistance = dist[minIndex] + calDistanceBtwnPoints(u, v);
                    if(alternateDistance < dist[neighborIndex])
                    {
                        dist[neighborIndex] = alternateDistance;
                        prev[neighborIndex] = minIndex;
                        queue.decreaseKey(neighborIndex, alternateDistance);
                    }
                }
            }

            return getPathToNode(prev, stopNodeIndex);
        }

        public static float calDistanceBtwnPoints(PointF one, PointF two)
        {
            return (float) Math.Sqrt((two.Y - one.Y) * (two.Y - one.Y) + (two.X - one.X) * (two.X - one.X));
        }

        public static List<int> getPathToNode(int[] prev, int stopNodeIndex)
        {
            List<int> reverseOrder = new List<int>();

            int index = stopNodeIndex;
            while (prev[index] != NODE_LOW)
            {
                reverseOrder.Add(index);
                index = prev[index];
            }

            // Make sure the first node gets added to the order
            reverseOrder.Add(index);

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
