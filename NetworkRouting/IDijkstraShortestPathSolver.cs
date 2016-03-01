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
         * For inserting the root node into the queue. This node
         * is initialized with a distance weight of 0 (if applicable depending
         * on queue implementation details).
         */
        void insert(int index);

        /*
         * Removes the index of the node with the smallest distance weight 
         * from the queue and returns it.
         */
        int deleteMin(float[] dist);

        /*
         * Reevaluates the distance weight value of the node at the given index.
         * As appropriate, moves the node farther up in the queue so that only nodes
         * that have a lesser distance weight will be removed from the queue before it.
         */
        void decreaseKey(int index, float[] dist);

    }
}