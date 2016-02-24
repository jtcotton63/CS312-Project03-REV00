using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    class HeapArrayImpl : IDijkstraShortestPathQueue
    {
        int IDijkstraShortestPathQueue.getQueueCount()
        {
            throw new NotImplementedException();
        }

        void IDijkstraShortestPathQueue.insert(int index, float weight)
        {
            throw new NotImplementedException();
        }

        int IDijkstraShortestPathQueue.deleteMin()
        {
            throw new NotImplementedException();
        }

        void IDijkstraShortestPathQueue.decreaseKey(int index, float newWeight)
        {
            throw new NotImplementedException();
        }

    }
}
