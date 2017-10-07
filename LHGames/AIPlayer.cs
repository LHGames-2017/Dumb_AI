using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Web.Api
{
    public class AIPlayer
    {
        public List<Point> TrouverChemin(Point startingPoint, Point endPoint, Point houseLocation, Tile[,] carte)
        {
            List<Node> openNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            Node currentNode = new Node(startingPoint, startingPoint, endPoint, null);
            openNodes.Add(currentNode);

            while (openNodes.Count>0 && currentNode.Position.X != endPoint.X || currentNode.Position.Y != endPoint.Y)
            {
                //--------------CALCUL DU NODE DU MEILLEUR FCOST------------------

                List<Node> minFCosts = new List<Node>();
                openNodes = openNodes.OrderBy(o => o.fCost).ToList();
                int cpt = 0;
                int minFCost = openNodes[0].fCost;
                bool listCompleted = false;
                while (!listCompleted && cpt < openNodes.Count)
                {
                    if (openNodes[cpt].fCost == minFCost)
                    {
                        minFCosts.Add(openNodes[cpt]);
                    }
                    else
                    {
                        listCompleted = true;
                    }
                    ++cpt;
                }

                //--------------CALCUL DU NODE DU MEILLEUR HCOST------------------
                currentNode = minFCosts.OrderBy(o => o.hCost).ToList()[0];

                //------------------DÉTECTIONS DES NEIGHBOURS---------------------
                List<Node> neighbours = new List<Node>();
                Point posCarte = new Point(0, 0);
                for(int i = 0; i< carte.GetLength(0); ++i)
                {
                    for(int j = 0; j< carte.GetLength(1); ++j)
                    {
                        if(carte[i,j].X == currentNode.Position.X && carte[i,j].Y == currentNode.Position.Y)
                        {
                            posCarte = new Point(i, j);
                        }
                    }
                }
                AjouterNeighbour(openNodes, -1, 0, startingPoint, endPoint, currentNode, posCarte, carte, houseLocation, closedNodes);
                AjouterNeighbour(openNodes, 1, 0, startingPoint, endPoint, currentNode, posCarte, carte, houseLocation, closedNodes);
                AjouterNeighbour(openNodes, 0, -1, startingPoint, endPoint, currentNode, posCarte, carte, houseLocation, closedNodes);
                AjouterNeighbour(openNodes, 0, 1, startingPoint, endPoint, currentNode, posCarte, carte, houseLocation, closedNodes);


                //-------------------CLOSIING OF CURRENT NODE---------------------
                openNodes.Remove(currentNode);
                closedNodes.Add(currentNode);

            }

            List<Point> partiescheminALEnvers = new List<Point>();
            List<Point> partieschemin = new List<Point>();
            while (!(currentNode.Position.X == startingPoint.X && currentNode.Position.Y == startingPoint.Y))
            {
                partiescheminALEnvers.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }
            for (int i = partiescheminALEnvers.Count - 1; i >= 0; i--)
            {
                partieschemin.Add(partiescheminALEnvers[i]);
            }
            return partieschemin; //ye men
        }
        private void AjouterNeighbour(List<Node> neighbours, int deltaX, int deltaY, Point startp, Point endP, Node parentNode, Point posCarte, Tile[,] carte, Point houseLocation, List<Node> closed)
        {
            try
            {
                if (closed.FindIndex(n => n.Position.X == parentNode.Position.X + deltaX && n.Position.Y == parentNode.Position.Y + deltaY) == -1 && (carte[posCarte.X + deltaX, posCarte.Y + deltaY].C == 0 || houseLocation.X == parentNode.Position.X + deltaX && houseLocation.Y == parentNode.Position.Y + deltaY))
                {
                    neighbours.Add(new Node(new Point(parentNode.Position.X + deltaX, parentNode.Position.Y + deltaY), startp, endP, parentNode));
                }
            }
            catch (Exception) { }
        }
    }

}

