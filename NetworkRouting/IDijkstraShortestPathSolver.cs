using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    interface IDijkstraShortestPathQueue
    {
        /*
         * Returns the total number of elements in the queue
         */
        int getQueueCount();

        /*
         * For inserting the first node into the queue
         */
        void insert(int index);

        /*
         * Finds the index of the node with the smallest distance weight.
         * Marks the value of the index of the smallest distance weight as removed.
         * Returns the node with the smallest distance weight.
         */
        int deleteMin(float[] dist);

        /*
         * Updates the value of the queue element at the specified index to newWeight.
         */
        void decreaseKey(int index, float[] dist);

    }
}