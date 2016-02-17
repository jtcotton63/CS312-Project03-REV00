using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class ArrayImpl : IDijkstraShortestPathSolver
    {
        List<PointF> nodes = null;
        List<HashSet<int>> adjList = null;
        int[] dist;
        int[] prev;
        int stopNodeIndex = -1;

        public ArrayImpl(List<PointF> nodes, List<HashSet<int>> adjacencyList, int startNodeIndex, int stopNodeIndex)
        {
            this.nodes = nodes;
            this.adjList = adjacencyList;
            this.dist = Enumerable.Repeat(Int32.MaxValue, adjList.Count).ToArray();
            this.prev = Enumerable.Repeat(PathSolver.NODE_LOW, adjList.Count).ToArray();
            this.stopNodeIndex = stopNodeIndex;
        }
    }
}
