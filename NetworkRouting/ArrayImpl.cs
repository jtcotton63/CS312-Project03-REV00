using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class ArrayImpl : IDijkstraShortestPathSolver
    {
        // The queue
        int[] queue = null;
        int[] dist;
        int[] prev;

        // Helper members
        List<PointF> nodes = null;
        List<HashSet<int>> adjList = null;
        int size = 0;

        // Other
        private int QUEUE_DEQUEUED_PLACEHOLDER = -1;
        private int QUEUE_NOT_INITIALIZED_PLACEHOLDER = Int32.MaxValue;

        public ArrayImpl(List<PointF> nodes, List<HashSet<int>> adjacencyList)
        {
            this.nodes = nodes;
            this.adjList = adjacencyList;
        }

        void IDijkstraShortestPathSolver.makeQueue()
        {
            this.queue = Enumerable.Repeat(QUEUE_NOT_INITIALIZED_PLACEHOLDER, nodes.Count).ToArray();
            this.dist = Enumerable.Repeat(Int32.MaxValue, nodes.Count).ToArray();
            this.prev = Enumerable.Repeat(PathSolver.NODE_LOW, nodes.Count).ToArray();
        }

        int IDijkstraShortestPathSolver.getQueueCount()
        {
            return this.size;
        }

        void IDijkstraShortestPathSolver.insert(int index, int weight)
        {
            queue[index] = weight;
            this.size++;
        }

        int IDijkstraShortestPathSolver.deleteMin()
        {
            int minIndex = 0;
            int minValue = queue[minIndex];
            for(int i = 0; i < queue.Length; i++)
            {
                if(queue[i] < minValue)
                {
                    minValue = queue[i];
                    minIndex = i;
                }
            }

            queue[minIndex] = QUEUE_DEQUEUED_PLACEHOLDER;
            this.size--;
            return minIndex;
        }

        void IDijkstraShortestPathSolver.decreaseKey(int index, int newWeight)
        {
            // If the key is being decreased for the first time,
            // it is the same as inserting a new element into the queue.
            // Therefore we increase the size.
            if (queue[index] == QUEUE_NOT_INITIALIZED_PLACEHOLDER)
                this.size++;

            queue[index] = newWeight;

        }

        PointF IDijkstraShortestPathSolver.getPoint(int index)
        {
            return nodes.ElementAt(index);
        }

        HashSet<int> IDijkstraShortestPathSolver.getNeighbors(int index)
        {
            return adjList.ElementAt(index);
        }

        int IDijkstraShortestPathSolver.getDist(int index)
        {
            return dist[index];
        }

        void IDijkstraShortestPathSolver.setDist(int index, int dist)
        {
            this.dist[index] = dist;
        }

        int IDijkstraShortestPathSolver.getPrev(int index)
        {
            return prev[index];
        }

        void IDijkstraShortestPathSolver.setPrev(int index, int prevIndex)
        {
            this.prev[index] = prevIndex;
        }

        int[] IDijkstraShortestPathSolver.getPrev()
        {
            return prev;
        }
    }
}
