﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NetworkRouting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void clearAll()
        {
            startNodeIndex = -1;
            stopNodeIndex = -1;
            sourceNodeBox.Clear();
            sourceNodeBox.Refresh();
            targetNodeBox.Clear();
            targetNodeBox.Refresh();
            arrayTimeBox.Clear();
            arrayTimeBox.Refresh();
            heapTimeBox.Clear();
            heapTimeBox.Refresh();
            differenceBox.Clear();
            differenceBox.Refresh();
            pathCostBox.Clear();
            pathCostBox.Refresh();
            arrayCheckBox.Checked = false;
            arrayCheckBox.Refresh();
            return;
        }

        private void clearSome()
        {
            arrayTimeBox.Clear();
            arrayTimeBox.Refresh();
            heapTimeBox.Clear();
            heapTimeBox.Refresh();
            differenceBox.Clear();
            differenceBox.Refresh();
            pathCostBox.Clear();
            pathCostBox.Refresh();
            return;
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            int randomSeed = int.Parse(randomSeedBox.Text);
            int size = int.Parse(sizeBox.Text);

            Random rand = new Random(randomSeed);
            seedUsedLabel.Text = "Random Seed Used: " + randomSeed.ToString();

            clearAll();
            this.adjacencyList = generateAdjacencyList(size, rand);
            List<PointF> points = generatePoints(size, rand);
            resetImageToPoints(points);
            this.points = points;
        }

        private List<HashSet<int>> generateAdjacencyList(int size, Random rand)
        {
            List<HashSet<int>> adjacencyList = new List<HashSet<int>>();

            for (int i = 0; i < size; i++)
            {
                HashSet<int> adjacentPoints = new HashSet<int>();
                while (adjacentPoints.Count < 3)
                {
                    int point = rand.Next(size);
                    if (point != i) adjacentPoints.Add(point);
                }
                adjacencyList.Add(adjacentPoints);
            }

            return adjacencyList;
        }

        private List<PointF> generatePoints(int size, Random rand)
        {
            List<PointF> points = new List<PointF>();
            for (int i = 0; i < size; i++)
            {
                points.Add(new PointF((float) (rand.NextDouble() * pictureBox.Width), (float) (rand.NextDouble() * pictureBox.Height)));
            }
            return points;
        }

        private void resetImageToPoints(List<PointF> points)
        {
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics graphics = Graphics.FromImage(pictureBox.Image);
            Pen pen;

            if (points.Count < 100)
                pen = new Pen(Color.Blue);
            else
                pen = new Pen(Color.LightBlue);
            foreach (PointF point in points)
            {
                graphics.DrawEllipse(pen, point.X, point.Y, 2, 2);
            }

            this.graphics = graphics;
            pictureBox.Invalidate();
        }

        // These variables are instantiated after the "Generate" button is clicked
        private List<PointF> points = new List<PointF>();
        private Graphics graphics;
        private List<HashSet<int>> adjacencyList;

        // Use this to generate paths (from start) to every node; then, just return the path of interest from start node to end node
        private void solveButton_Click(object sender, EventArgs e)
        {
            // This was the old entry point, but now it is just some form interface handling
            bool ready = true;

            if(startNodeIndex == -1)
            {
                sourceNodeBox.Focus();
                sourceNodeBox.BackColor = Color.Red;
                ready = false;
            }
            if(stopNodeIndex == -1)
            {
                if(!sourceNodeBox.Focused)
                    targetNodeBox.Focus();
                targetNodeBox.BackColor = Color.Red;
                ready = false;
            }
            if (points.Count > 0)
            {
                resetImageToPoints(points);
                paintStartStopPoints();
            }
            else
            {
                ready = false;
            }
            if(ready)
            {
                clearSome();

                IDijkstraShortestPathQueue solver = null;
                List<int> arrayPath = null;
                double arraySeconds = -1.0;
                double heapSeconds = -1.0;
                string timerBoxFormat = "F6";

                // Run the array implementation
                if (arrayCheckBox.Checked)
                {
                    solver = new ArrayImpl(points.Count);
                    Stopwatch arrayTimer = new Stopwatch();
                    arrayTimer.Start();
                    arrayPath = PathSolver.findShortestPath(solver, points, adjacencyList, startNodeIndex, stopNodeIndex);
                    arrayTimer.Stop();
                    arraySeconds = arrayTimer.Elapsed.TotalMilliseconds / 1000;
                }

                // Run the heap implementation
                solver = new HeapArrayImpl(points.Count);
                Stopwatch heapTimer = new Stopwatch();
                heapTimer.Start();
                List<int> heapPath = PathSolver.findShortestPath(solver, points, adjacencyList, startNodeIndex, stopNodeIndex);
                heapTimer.Stop();
                heapSeconds = heapTimer.Elapsed.TotalMilliseconds / 1000;

                // If the list is null, then there isn't a path to the specified node
                if(heapPath == null)
                {
                    pathCostBox.Text = "NO PATH";
                }
                else
                {
                    // Verify that both got the same paths
                    if (arrayCheckBox.Checked)
                        verifyDifferentImplementationPaths(arrayPath, heapPath);

                    // Draw the path
                    float pathCost = drawPath(heapPath);
                    pathCostBox.Text = pathCost.ToString(timerBoxFormat);

                    // Set the appropriate times
                    if (arrayCheckBox.Checked)
                        arrayTimeBox.Text = arraySeconds.ToString(timerBoxFormat);
                    heapTimeBox.Text = heapSeconds.ToString(timerBoxFormat);

                    // Speed up comparison
                    if (arrayCheckBox.Checked)
                        differenceBox.Text = (arraySeconds / heapSeconds).ToString(timerBoxFormat);
                }
            }
        }

        private void verifyDifferentImplementationPaths(List<int> path1, List<int> path2)
        {
            Debug.Assert(path1.Count == path2.Count);

            for (int i = 0; i < path1.Count; i++)
                Debug.Assert(path1[i] == path2[i]);

        }

        private float drawPath(List<int> path)
        {
            Pen black = new Pen(Color.Black);
            int curr = 0;
            int next = 1;
            float pathCost = 0F;
            for (int i = 0; i < path.Count - 1; i++)
            {
                PointF u = points[path[curr]];
                PointF v = points[path[next]];
                graphics.DrawLine(black, points[path[curr]], points[path[next]]);
                float segmentCost = PathSolver.calcDistanceBtwnPoints(u, v);
                pathCost += segmentCost;
                graphics.DrawString(String.Format("{0}", (int) segmentCost), 
                    SystemFonts.DefaultFont, Brushes.Black, calcMidpoint(u, v));
                curr++;
                next++;
            }
            
            pictureBox.Invalidate();
            return pathCost;
        }

        private PointF calcMidpoint(PointF u, PointF v)
        {
            float x = (u.X + v.X) / 2;
            float y = (u.Y + v.Y) / 2;
            return new PointF(x, y);
        }

        private Boolean startStopToggle = true;
        private int startNodeIndex = -1;
        private int stopNodeIndex = -1;
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (points.Count > 0)
            {
                Point mouseDownLocation = new Point(e.X, e.Y);
                int index = ClosestPoint(points, mouseDownLocation);
                if (startStopToggle)
                {
                    startNodeIndex = index;
                    sourceNodeBox.ResetBackColor();
                    sourceNodeBox.Text = "" + index;
                }
                else
                {
                    stopNodeIndex = index;
                    targetNodeBox.ResetBackColor();
                    targetNodeBox.Text = "" + index;
                }
                resetImageToPoints(points);
                paintStartStopPoints();
            }
        }

        private void sourceNodeBox_Changed(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                try{ startNodeIndex = int.Parse(sourceNodeBox.Text); }
                catch { startNodeIndex = -1; }
                if (startNodeIndex < 0 | startNodeIndex > points.Count-1)
                    startNodeIndex = -1;
                if(startNodeIndex != -1)
                {
                    sourceNodeBox.ResetBackColor();
                    resetImageToPoints(points);
                    paintStartStopPoints();
                    startStopToggle = !startStopToggle;
                }
            }
        }

        private void targetNodeBox_Changed(object sender, EventArgs e)
        {
            if (points.Count > 0)
            {
                try { stopNodeIndex = int.Parse(targetNodeBox.Text); }
                catch { stopNodeIndex = -1; }
                if (stopNodeIndex < 0 | stopNodeIndex > points.Count-1)
                    stopNodeIndex = -1;
                if(stopNodeIndex != -1)
                {
                    targetNodeBox.ResetBackColor();
                    resetImageToPoints(points);
                    paintStartStopPoints();
                    startStopToggle = !startStopToggle;
                }
            }
        }
        
        private void paintStartStopPoints()
        {
            if (startNodeIndex > -1)
            {
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.DrawEllipse(new Pen(Color.Green, 6), points[startNodeIndex].X, points[startNodeIndex].Y, 1, 1);
                this.graphics = graphics;
                pictureBox.Invalidate();
            }

            if (stopNodeIndex > -1)
            {
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.DrawEllipse(new Pen(Color.Red, 2), points[stopNodeIndex].X - 3, points[stopNodeIndex].Y - 3, 8, 8);
                this.graphics = graphics;
                pictureBox.Invalidate();
            }
        }

        private int ClosestPoint(List<PointF> points, Point mouseDownLocation)
        {
            double minDist = double.MaxValue;
            int minIndex = 0;

            for (int i = 0; i < points.Count; i++)
            {
                double dist = Math.Sqrt(Math.Pow(points[i].X-mouseDownLocation.X,2) + Math.Pow(points[i].Y - mouseDownLocation.Y,2));
                if (dist < minDist)
                {
                    minIndex = i;
                    minDist = dist;
                }
            }

            return minIndex;
        }
    }
}
