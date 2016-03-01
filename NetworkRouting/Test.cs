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
            testBasic1();
            testBasic2();
        }

        private static void testGetDistance()
        {
            PointF one = new PointF(5.0F, 0.0F);
            PointF two= new PointF(0.0F, 4.0F);
            float result = PathSolver.calcDistanceBtwnPoints(one, two);
            Debug.Assert((int) result == 6);
        }

        private static void testBasic1()
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

        private static void testBasic2()
        {
            // Build the list of points
            List<PointF> points = new List<PointF>();
            points.Add(new PointF(485.0F, 164.0F));
            points.Add(new PointF(501.0F, 184.0F));
            points.Add(new PointF(293.0F, 131.0F));
            points.Add(new PointF(215.0F, 151.0F));
            points.Add(new PointF(263.0F, 185.0F));
            points.Add(new PointF(455.0F, 21.0F));
            points.Add(new PointF(480.0F, 30.0F));
            points.Add(new PointF(477.0F, 213.0F));
            points.Add(new PointF(119.0F, 235.0F));
            points.Add(new PointF(122.0F, 31.0F));
            points.Add(new PointF(400.0F, 211.0F));
            points.Add(new PointF(343.0F, 255.0F));

            // Build the adjacency list
            List<HashSet<int>> adjacencyList = new List<HashSet<int>>();
            for (int i = 0; i < points.Count; i++)
                adjacencyList.Add(new HashSet<int>());

            adjacencyList[0].Add(1);
            adjacencyList[0].Add(7);
            adjacencyList[0].Add(10);
            adjacencyList[1].Add(2);
            adjacencyList[1].Add(6);
            adjacencyList[1].Add(4);
            adjacencyList[2].Add(11);
            adjacencyList[2].Add(4);
            adjacencyList[2].Add(6);
            adjacencyList[3].Add(9);
            adjacencyList[3].Add(8);
            adjacencyList[3].Add(0);
            adjacencyList[4].Add(0);
            adjacencyList[4].Add(10);
            adjacencyList[4].Add(1);
            adjacencyList[5].Add(10);
            adjacencyList[5].Add(9);
            adjacencyList[5].Add(2);
            adjacencyList[6].Add(7);
            adjacencyList[6].Add(5);
            adjacencyList[6].Add(0);
            adjacencyList[7].Add(11);
            adjacencyList[7].Add(5);
            adjacencyList[7].Add(6);
            adjacencyList[8].Add(6);
            adjacencyList[8].Add(4);
            adjacencyList[8].Add(5);
            adjacencyList[9].Add(1);
            adjacencyList[9].Add(2);
            adjacencyList[9].Add(11);
            adjacencyList[10].Add(0);
            adjacencyList[10].Add(8);
            adjacencyList[10].Add(6);
            adjacencyList[11].Add(5);
            adjacencyList[11].Add(10);
            adjacencyList[11].Add(7);

            int startNodeIndex = 3;
            int stopNodeIndex = 7;

            string expected = "3, 0, 7";

            // Array test
            IDijkstraShortestPathQueue arrayQ = new ArrayImpl(points.Count);
            List<int> arrayPath = PathSolver.findShortestPath(arrayQ, points, adjacencyList, startNodeIndex, stopNodeIndex);
            string arrayResult = PathSolver.pathToString(arrayPath);
            Debug.Assert(expected.Equals(arrayResult));

            // Heap test
            IDijkstraShortestPathQueue heapQ = new HeapArrayImpl(points.Count);
            List<int> heapPath = PathSolver.findShortestPath(heapQ, points, adjacencyList, startNodeIndex, stopNodeIndex);
            string heapResult = PathSolver.pathToString(heapPath);
            Debug.Assert(expected.Equals(heapResult));
        }
    }
}
