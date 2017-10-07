using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Web.Api
{
    public class AIPlayer
    {
        public void TrouverChemin(Point startingPoint, Point endPoint)
        {
            List<Point> openPoints = new List<Point>();
            List<Point> closedPoints = new List<Point>();
            Point currentPoint = startingPoint;
            openPoints.Add(startingPoint);

            while (currentPoint!=endPoint)
            {
                //--------------CALCUL DU POINT DU MEILLEUR COST------------------
                List<Point> minFCosts = new List<Point>();
                int minFCost = FindFCost(openPoints[0], startingPoint, endPoint);
                for (int p = 1; p < openPoints.Count; p++)
                {
                    int tempCost = FindFCost(openPoints[p], startingPoint, endPoint);
                    if (tempCost < minFCost)
                    {
                        minFCost = tempCost;
                    }
                }
                for (int i = 0; i < openPoints.Count; i++)
                {
                    if (FindFCost(openPoints[i], startingPoint, endPoint) == minFCost)
                        minFCosts.Add(openPoints[i]);
                }
                if(minFCosts.Count <=2)
                {
                    int minHCost = FindDistance(minFCosts[0], endPoint);
                    for (int i = 1; i < minFCosts.Count; i++)
                    {
                        int tempCost = FindDistance(minFCosts[i], endPoint);
                        if (tempCost< minHCost)
                        {
                            minHCost = tempCost;
                        }
                    }
                    for (int j= 0; j < minFCosts.Count; j++)
                    {
                        if (FindDistance(minFCosts[j], endPoint) == minHCost)
                            currentPoint = minFCosts[j];
                    }

                    openPoints.Remove(currentPoint);

                }

            }


            
        
        }
        int FindDistance(Point a, Point b)
        {
            Point temp = a - b;
            return (Math.Abs(temp.X) + Math.Abs(temp.Y));
        
        }
        int FindFCost(Point p,Point sPoint,Point ePoint)
        {
            int fCost = /*g(n)*/FindDistance(p, sPoint) + /*h(n)*/FindDistance(p, ePoint);
            return fCost;
        }
        
    }
}
