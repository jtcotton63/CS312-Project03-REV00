using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    interface IDijkstraShortestPathSolver
    {
        void makeQueue();

        /*
         * Returns the total number of elements in the queue
         */
        int getQueueCount();

        void insert(int index, int weight);

        /*
         * Finds the index of the node with the smallest distance weight.
         * Marks the value of the index of the smallest distance weight as removed.
         * Returns the index of the smallest distance weight.
         */
        int deleteMin();

        /*
         * Updates the value of the queue element at the specified index to newWeight.
         */
        void decreaseKey(int index, int newWeight);

        // Accesser methods

        PointF getPoint(int index);

        HashSet<int> getNeighbors(int index);

        int getDist(int index);

        void setDist(int index, int dist);

        int getPrev(int index);

        void setPrev(int index, int prevIndex);

        int[] getPrev();
    }
}