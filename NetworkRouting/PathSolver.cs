using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetworkRouting
{
    static class PathSolver
    {
        public static int NODE_LOW = -1;

        public static void findShortestPath(IDijkstraShortestPathSolver solver)
        {
            solver.makeQueue(); // HELP
            while(solver.getQueueCount() > 0)
            {
                int minIndex = solver.deleteMin();
                PointF u = solver.getPoint(minIndex);
                HashSet<int> neighbors = solver.getNeighbors(minIndex);
                foreach(int neighborIndex in neighbors)
                {
                    PointF v = solver.getPoint(neighborIndex);
                    int alternateDistance = solver.getDist(minIndex) + calDistanceBtwnPoints(u, v);
                    if(alternateDistance < solver.getDist(neighborIndex))
                    {
                        solver.setDist(neighborIndex, alternateDistance);
                        solver.setPrev(neighborIndex, minIndex);
                        solver.decreaseKey(neighborIndex); // HELP
                    }
                }
            }

            // HELP what is returned from this function? 
            // How does the interface know the path from the start to the stop?
        }

        private static int calDistanceBtwnPoints(PointF one, PointF two)
        {
            throw new NotImplementedException();
        }

    }
}
