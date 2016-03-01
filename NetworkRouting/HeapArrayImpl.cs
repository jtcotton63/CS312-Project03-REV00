using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{    
    class HeapArrayImpl : IDijkstraShortestPathQueue
    {
        /* 
         * The heap stores the elements that have the least distance weight at heap[0].
         * The heap acts as a binary tree, which each node's parent being a node that has
         * a smaller distance weight than that of the current node.
         */
        int[] heap = null;

        // The location dictionary tells where the specified node is located in the heap.
        // Each pair {x, y} indicates that node x can be found in the heap at heap[y].
        Dictionary<int, int> location = null;
        
        // Helper members
        int insertionIndex = 0;
        private int HEAP_OPEN_SPACE_PLACEHOLDER = -1;

        public HeapArrayImpl(int numPoints)
        {
            this.heap = Enumerable.Repeat(HEAP_OPEN_SPACE_PLACEHOLDER, numPoints).ToArray();
            this.location = new Dictionary<int, int>();
        }

        public int getQueueCount()
        {
            return this.location.Count;
        }

        // Places the root node in the heap at heap[0]
        public void insert(int node)
        {
            heap[0] = node;
            location[node] = 0;
            insertionIndex++;
        }

       /*
        * Removes the node at heap[0] and returns it.
        *
        * Replaces the missing node at heap[0] with the child node that has the next smallest
        * distance weight. Likewise, fills the empoty spot left by the child being moved up with
        * its child that has the smallest distance weight. This is repeated until a node with no children is found.
        */
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
                bool hasChild1 = child1Index >= 0 && heap[child1Index] != HEAP_OPEN_SPACE_PLACEHOLDER;
                bool hasChild2 = child2Index >= 0 && heap[child2Index] != HEAP_OPEN_SPACE_PLACEHOLDER;

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

       /*
        * Performs two functions:
        * 1) If a node doesn't exist in the heap, adds it in the next available position
        *    where it will have a parent node. After doing this, proceeds to part 2).
        * 2) Relocates the node in the heap. The node should only come after nodes that have
        *    smaller distances so that they are removed least-distance-weight first.
        */
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

       /*
        * Determines if the node at the given index has a parent that has a
        * smaller distance weight. If so, the parent is moved to the current node's
        * position, and the current node is put in the parent's position.
        * This process is repeated until the given node is the root node or
        * has a parent that has a smaller distance weight than it does.
        */
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
        * 
        * Returns -1 if the node has no parent.
        */
        private int getParentIndex(int currIndex)
        {
            if (currIndex == 0)
                return -1;

            return (currIndex - 1) / 2;
        }

        // Child 1 is located at 2 * index + 1
        // Returns -1 if the node has no child.
        private int getHeapChild1Index(int currIndex)
        {
            int child1Index = (2 * currIndex) + 1;
            if (child1Index >= heap.Length)
                return -1;

            return child1Index;
        }

        // Child 2 is located at 2 * index + 2
        // Returns -1 if the node has no child.
        private int getHeapChild2Index(int currIndex)
        {
            int child2Index = (2 * currIndex) + 2;
            if (child2Index >= heap.Length)
                return -1;

            return child2Index;
        }
    }
}
