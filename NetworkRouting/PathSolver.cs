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

        public static int[] findShortestPath(IDijkstraShortestPathQueue queue, List<PointF> points, List<HashSet<int>> adjacencyList, int startNodeIndex)
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

            // HELP what is returned from this function? 
            // How does the interface know the path from the start to the stop?
            // When the queue runs out, then all the paths have been explored (that are connected to this tree anyways).
            // If the stopnode still doesn't have a prev, then the two nodes aren't connected in any way
            // Mark de-queued nodes as -1 in queue
            return prev;
        }

        public static float calDistanceBtwnPoints(PointF one, PointF two)
        {
            return (float) Math.Sqrt((two.Y - one.Y) * (two.Y - one.Y) + (two.X - one.X) * (two.X - one.X));
        }

    }
}
