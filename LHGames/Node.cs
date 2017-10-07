using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Web.Api
{
    public class Node
    {
        public Point Position { get; set; }
        Node Parent { get; set; }
        Point StartPoint { get; set; }
        Point EndPoint { get; set; }
        int gCost
        {
            get
            {
                return 0;
            }
        }
        int hCost
        {
            get
            {
                return 0;
            }
        }
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
        public Node(Point position, Point startPoint, Point endPoint, Node parent)
        {
            Position = position;
            StartPoint = startPoint;
            EndPoint = endPoint;
            Parent = parent;
        }
    }
}
