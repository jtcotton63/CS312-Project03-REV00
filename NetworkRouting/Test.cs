using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class Test
    {
        public static void runTests()
        {
            testGetDistance();
            testBasic();
        }

        private static void testGetDistance()
        {
            PointF one = new PointF(5.0F, 0.0F);
            PointF two= new PointF(0.0F, 4.0F);
            float result = PathSolver.calDistanceBtwnPoints(one, two);
            Debug.Assert((int) result == 6);
        }

        private static void testBasic()
        {
            // Build the list of points
            List<PointF> points = new List<PointF>();
            points.Add(new PointF(10.0F, 60.0F));
            points.Add(new PointF(80.0F, 130.0F));
            points.Add(new PointF(120.0F, 50.0F));
            points.Add(new PointF(180.0F, 10.0F));
            points.Add(new PointF(190.0F, 130.0F));
            points.Add(new PointF(220.0F, 70.0F));
            points.Add(new PointF(160.0F, 90.0F));

            // Build the adjacency list
            List<HashSet<int>> adjacencyList = new List<HashSet<int>>();
            for (int i = 0; i < points.Count; i++)
                adjacencyList.Add(new HashSet<int>());

            adjacencyList[0].Add(1);
            adjacencyList[0].Add(5);
            adjacencyList[1].Add(6);
            adjacencyList[1].Add(4);
            adjacencyList[2].Add(1);
            adjacencyList[2].Add(0);
            adjacencyList[3].Add(6);
            adjacencyList[4].Add(5);
            adjacencyList[4].Add(3);
            adjacencyList[5].Add(3);
            adjacencyList[5].Add(2);
            adjacencyList[6].Add(2);
            adjacencyList[6].Add(0);

            // Array test
            int startNodeIndex = 3;
            int stopNodeIndex = 5;

            IDijkstraShortestPathQueue arrayQ = new ArrayImpl(points.Count);
            List<int> path = PathSolver.findShortestPath(arrayQ, points, adjacencyList, startNodeIndex, stopNodeIndex);
            string result = PathSolver.pathToString(path);
            string expected = "3, 6, 2, 1, 4, 5";
            Debug.Assert(expected.Equals(result));
        }
    }
}
