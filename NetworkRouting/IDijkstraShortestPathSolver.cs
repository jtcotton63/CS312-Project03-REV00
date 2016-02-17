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

        int getQueueCount();

        void insert();

        int deleteMin();

        void decreaseKey(int index);

        // Accesser methods

        PointF getPoint(int index);

        HashSet<int> getNeighbors(int index);

        int getDist(int index);

        int setDist(int index, int dist);

        int getPrev(int index);

        int setPrev(int index, int prevIndex);

        int getStopNodeIndex();
    }
}