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
        float[] queue = null;

        // Helper members
        int size = 0;

        // Other
        private float QUEUE_DEQUEUED_PLACEHOLDER = -1;
        private float QUEUE_NOT_INITIALIZED_PLACEHOLDER = float.MaxValue;

        public ArrayImpl(int numPoints)
        {
            this.queue = Enumerable.Repeat(QUEUE_NOT_INITIALIZED_PLACEHOLDER, numPoints).ToArray();
        }

        int IDijkstraShortestPathQueue.getQueueCount()
        {
            return this.size;
        }

        void IDijkstraShortestPathQueue.insert(int index, float weight)
        {
            queue[index] = weight;
            this.size++;
        }

        int IDijkstraShortestPathQueue.deleteMin()
        {
            int minIndex = -1;
            float minValue = QUEUE_NOT_INITIALIZED_PLACEHOLDER;
            for(int i = 0; i < queue.Length; i++)
            {
                // The value of queue[i] has to be gte 0
                // bc -1 indicates that a node has alredy been popped
                if(queue[i] >= 0 && queue[i] < minValue)
                {
                    minValue = queue[i];
                    minIndex = i;
                }
            }

            if (minIndex == -1)
                throw new SystemException("The minIndex was not initialized; nothing to pop on the queue");

            queue[minIndex] = QUEUE_DEQUEUED_PLACEHOLDER;
            this.size--;
            return minIndex;
        }

        void IDijkstraShortestPathQueue.decreaseKey(int index, float newWeight)
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
