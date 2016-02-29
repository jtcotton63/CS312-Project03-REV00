using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{    
    class HeapArrayImpl : IDijkstraShortestPathQueue
    {
        // The heap
        int[] heap = null;

        // The location dictionary tells where the specified node is located in the heap.
        Dictionary<int, int> location = null;
        
        // Helper members
        int insertionIndex = 0;

        // Other
        private int HEAP_OPEN_SPACE_PLACEHOLDER = -1;
        private int PARENT_INDEX_LIMIT_MARKER = -1;

        public HeapArrayImpl(int numPoints)
        {
            this.heap = Enumerable.Repeat(HEAP_OPEN_SPACE_PLACEHOLDER, numPoints).ToArray();
            this.location = new Dictionary<int, int>();
        }

        public int getQueueCount()
        {
            return this.location.Count;
        }

        public void insert(int node)
        {
            heap[0] = node;
            location[node] = 0;
            insertionIndex++;
        }

        public int deleteMin(float[] dist)
        {
            // The first node is the node with the smallest distance
            // that is currently on the heap; remove it
            int minNode = heap[0];
            location.Remove(minNode);

            // Balance the heap top down
            int nodeIndex = 0;
            int successorIndex = 1;
            while (true)
            {
                int child1Index = getHeapChild1Index(nodeIndex);
                int child2Index = getHeapChild2Index(nodeIndex);
                bool hasChild1 = heap[child1Index] != HEAP_OPEN_SPACE_PLACEHOLDER;
                bool hasChild2 = heap[child2Index] != HEAP_OPEN_SPACE_PLACEHOLDER;

                if(hasChild1 && hasChild2)
                {
                    // Child 1 has a smaller distance, therefore should be placed first
                    if (dist[heap[child1Index]] < dist[heap[child2Index]])
                        successorIndex = child1Index;
                    // Child 2 has a smaller distance
                    else
                        successorIndex = child2Index;
                }
                else if(hasChild1 && !hasChild2)
                {
                    successorIndex = child1Index;
                }
                else if(hasChild2 && !hasChild1)
                {
                    successorIndex = child2Index;
                }
                else
                {
                    heap[nodeIndex] = HEAP_OPEN_SPACE_PLACEHOLDER;
                    break;
                }

                heap[nodeIndex] = heap[successorIndex];
                heap[successorIndex] = HEAP_OPEN_SPACE_PLACEHOLDER;
                location[heap[nodeIndex]] = nodeIndex;
                nodeIndex = successorIndex;
            }

            return minNode;
        }

        public void decreaseKey(int node, float[] dist)
        {
            int newNodeInsertionIndex;

            if (!location.ContainsKey(node))
            {
                newNodeInsertionIndex = insertionIndex;

                // Check to make sure that none of the nodes' parent spots
                // are empty. This could happen if a node is moved up and there
                // isn't a child node yet.
                int parentIndex = getParentIndex(newNodeInsertionIndex);
                while (parentIndex >= 0 && heap[parentIndex] == HEAP_OPEN_SPACE_PLACEHOLDER)
                {
                    newNodeInsertionIndex = parentIndex;
                    parentIndex = getParentIndex(newNodeInsertionIndex);
                }

                // Insert the node index into the heap at the appropriate heap index
                heap[newNodeInsertionIndex] = node;
                location[node] = newNodeInsertionIndex;

                // The new node was inserted into the heap at
                // the insertionIndex position bc none of its
                // parents were empty. Therefore, increment
                // the insertionIndex counter.
                if (insertionIndex == newNodeInsertionIndex)
                    insertionIndex++;
            }
            else
            {
                newNodeInsertionIndex = location[node];
            }
            
            balanceHeapBottomUp(newNodeInsertionIndex, dist);
        }

        private void balanceHeapBottomUp(int nodeIndex, float[] dist)
        {
            int parentIndex = getParentIndex(nodeIndex);
            while (parentIndex >= 0 && dist[heap[parentIndex]] > dist[heap[nodeIndex]])
            {
                int parentNode = heap[parentIndex];
                // Set the current node to be in the parent node's position
                heap[parentIndex] = heap[nodeIndex];
                heap[nodeIndex] = parentNode;

                // Since the two have been switched,
                // parentNode is currently pointing at the current
                // node in the old parent's position, and nodeIndex
                // is pointing at the old parent node in the current
                // node's old position
                location[parentNode] = nodeIndex;
                location[heap[parentIndex]] = parentIndex;

                // Modify pointers
                nodeIndex = parentIndex;
                parentIndex = getParentIndex(parentIndex);
            }
        }

       /*
        * Returns the index of currIndex's parent in the heap.
        * The parent index of any given index in the heap is located
        * at floor((currIndex - 1) / 2)
        */
        private int getParentIndex(int currIndex)
        {
            if (currIndex == 0)
                return PARENT_INDEX_LIMIT_MARKER;

            return (currIndex - 1) / 2;
        }

        // Child 1 is located at 2 * index + 1
        private int getHeapChild1Index(int currIndex)
        {
            int child1Index = (2 * currIndex) + 1;
            if (child1Index >= heap.Length)
                throw new SystemException("Heap parent at index " + currIndex + " attempted to retrieve child at an index outside of heap bounds");

            return child1Index;
        }

        // Child 2 is located at 2 * index + 2
        private int getHeapChild2Index(int currIndex)
        {
            int child2Index = (2 * currIndex) + 2;
            if (child2Index >= heap.Length)
                throw new SystemException("Heap parent at index " + currIndex + " attempted to retrieve child at an index outside of heap bounds");

            return child2Index;
        }
    }
}
