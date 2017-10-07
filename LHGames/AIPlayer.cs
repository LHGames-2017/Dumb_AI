using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Web.Api
{
    public class AIPlayer
    {
        public void TrouverChemin(Point startingPoint, Point endPoint, Point houseLocation, Tile[,] carte)
        {
            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            Node currentNode = new Node(startingPoint, startingPoint, endPoint, null);
            openNodes.Add(currentNode);

            while (currentNode.Position.X != endPoint.X || currentNode.Position.Y != endPoint.Y)
            {
                //--------------CALCUL DU NODE DU MEILLEUR FCOST------------------

                List<Node> minFCosts = new List<Node>();
                openNodes = openNodes.OrderBy(o => o.fCost).ToList();
                int i = 0;
                int minFCost = openNodes[0].fCost;
                bool listCompleted = false;
                while (!listCompleted && i < openNodes.Count)
                {
                    if (openNodes[i].fCost == minFCost)
                    {
                        minFCosts.Add(openNodes[i]);
                    }
                    else
                    {
                        listCompleted = true;
                    }
                }

                //--------------CALCUL DU NODE DU MEILLEUR HCOST------------------
                currentNode = minFCosts.OrderBy(o => o.hCost).ToList()[0];

                //------------------DÉTECTIONS DES NEIGHBOURS---------------------
                List<Node> neighbours = new List<Node>();
                neighbours.Add(new Node(new Point(currentNode.Position.X - 1, currentNode.Position.Y), startingPoint, endPoint, currentNode));
                neighbours.Add(new Node(new Point(currentNode.Position.X + 1, currentNode.Position.Y), startingPoint, endPoint, currentNode));
                neighbours.Add(new Node(new Point(currentNode.Position.X, currentNode.Position.Y - 1), startingPoint, endPoint, currentNode));
                neighbours.Add(new Node(new Point(currentNode.Position.X, currentNode.Position.Y + 1), startingPoint, endPoint, currentNode));

                //------------------------OPENING OF NODES------------------------
                foreach(Node n in neighbours)
                {
                    if (carte[n.Position.X, n.Position.Y].C != 0 || n.Position.X == houseLocation.X && n.Position.Y == houseLocation.Y)
                    {
                        openNodes.Add(n);
                    }
                }

                //-------------------CLOSIING OF CURRENT NODE---------------------
                openNodes.Remove(currentNode);

            }

        }

    }

}

