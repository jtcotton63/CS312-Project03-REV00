using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class ArrayImpl : IDijkstraShortestPathQueue
    {
        // The queue
        int[] queue = null;

        // Helper members
        int size = 0;

        // Other
        private int QUEUE_DEQUEUED_PLACEHOLDER = -1;
        private int QUEUE_NOT_INITIALIZED_PLACEHOLDER = Int32.MaxValue;

        public ArrayImpl(int numPoints)
        {
            this.queue = Enumerable.Repeat(QUEUE_NOT_INITIALIZED_PLACEHOLDER, numPoints).ToArray();
        }

        int IDijkstraShortestPathQueue.getQueueCount()
        {
            return this.size;
        }

        void IDijkstraShortestPathQueue.insert(int index, int weight)
        {
            queue[index] = weight;
            this.size++;
        }

        int IDijkstraShortestPathQueue.deleteMin()
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

        void IDijkstraShortestPathQueue.decreaseKey(int index, int newWeight)
        {
            // If the key is being decreased for the first time,
            // it is the same as inserting a new element into the queue.
            // Therefore we increase the size.
            if (queue[index] == QUEUE_NOT_INITIALIZED_PLACEHOLDER)
                this.size++;

            queue[index] = newWeight;

        }

    }
}
