namespace StarterProject.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Linq;

    [Route("/")]
    public class GameController : Controller
    {
        AIPlayer AI = new AIPlayer();
        AIHelper player = new AIHelper();
        List<Point> ressources = new List<Point>();
        List<Point> houses = new List<Point>();
        List<Point> shops = new List<Point>();
        int cptTour = 0;
        int storedRessources = 0;
        Point lastPosition;

        int indiceJoueurProche = 6969;


        [HttpPost]
        public string Index([FromForm]string map)
        {
            GameInfo gameInfo = JsonConvert.DeserializeObject<GameInfo>(map);
            var carte = AIHelper.DeserializeMap(gameInfo.CustomSerializedMap);

            indiceJoueurProche = IsPlayerNear(gameInfo);
            if (indiceJoueurProche != 6969)
            {
                indiceJoueurProche = 6969;
                return AIHelper.CreateAttackAction(gameInfo.OtherPlayers[indiceJoueurProche].Value.Position);
            }



            if (cptTour == 0)
            {
                ressources = new List<Point>();
                for (int i = 0; i < carte.GetLength(0); i++)
                {
                    for (int j = 0; j < carte.GetLength(1); j++)
                    {
                        Point point = new Point(carte[i, j].X, carte[i, j].Y);
                        if (carte[i, j].C == (byte)TileType.R && !ressources.Exists(x => x.X == point.X && x.Y == point.Y)) ressources.Add(point);
                        if (carte[i, j].C == (byte)TileType.H && !houses.Exists(x => x.X == point.X && x.Y == point.Y)) houses.Add(point);
                        if (carte[i, j].C == (byte)TileType.S && !shops.Exists(x => x.X == point.X && x.Y == point.Y)) shops.Add(point);
                    }
                }
                lastPosition = gameInfo.Player.Position;
            }
            Point déplacement = gameInfo.Player.Position - lastPosition;
            if(déplacement.X != 0 || déplacement.Y != 0)
            {
                if(déplacement.X == 0)
                {
                    int j = déplacement.Y == 1 ? 0 : carte.GetLength(1) - 1;
                    for (int i = 0; i < carte.GetLength(0); i++)
                    {
                        Point point = new Point(carte[i, j].X, carte[i, j].Y);
                        if (carte[i, j].C == (byte)TileType.R && !ressources.Exists(x=>x.X == point.X && x.Y == point.Y)) ressources.Add(point);
                        if (carte[i, j].C == (byte)TileType.H && !houses.Exists(x => x.X == point.X && x.Y == point.Y)) houses.Add(point);
                        if (carte[i, j].C == (byte)TileType.S && !shops.Exists(x => x.X == point.X && x.Y == point.Y)) shops.Add(point);
                    }
                }
                else
                {
                    int i = déplacement.X == -1 ? 0 : carte.GetLength(1) - 1;
                    for (int j = 0; i < carte.GetLength(1); i++)
                    {
                        Point point = new Point(carte[i, j].X, carte[i, j].Y);
                        if (carte[i, j].C == (byte)TileType.R && !ressources.Exists(x => x.X == point.X && x.Y == point.Y)) ressources.Add(point);
                        if (carte[i, j].C == (byte)TileType.H && !houses.Exists(x => x.X == point.X && x.Y == point.Y)) houses.Add(point);
                        if (carte[i, j].C == (byte)TileType.S && !shops.Exists(x => x.X == point.X && x.Y == point.Y)) shops.Add(point);
                    }
                }
            }
            

            
            string action = DeciderAction(gameInfo, carte);
            lastPosition = gameInfo.Player.Position;
            ++cptTour;
            return action;
        }

        public int IsPlayerNear(GameInfo gameinfo)
        {
            for (int i = 0; i < gameinfo.OtherPlayers.Count(); i++)
            {
                if (
                    Point.Distance(gameinfo.OtherPlayers[i].Value.Position, gameinfo.Player.Position) == 1)
                {
                    return i;
                }

            }
            return 6969;
        }

        public string DeciderAction(GameInfo gameinfo, Tile[,] carte)
        {
            List<Point> chemin;
            
            if (gameinfo.Player.CarriedResources < 0.9f * gameinfo.Player.CarryingCapacity)
            {
                if (storedRessources >= 15000)
                {
                    storedRessources = 0;
                    return AIHelper.CreateUpgradeAction(UpgradeType.CarryingCapacity);
                }
                chemin = AI.TrouverChemin(gameinfo.Player.Position, ressources[0]-new Point(1,0), gameinfo.Player.HouseLocation,carte);
                if (chemin.Count == 0)
                {
                    ressources = ressources.OrderBy(x => Point.Distance(x, gameinfo.Player.Position)).ToList();
                    return AIHelper.CreateCollectAction(ressources[0]);
                }
                else
                {
                    return AIHelper.CreateMoveAction(chemin[0]);
                }
            }
            else
            {
                chemin = AI.TrouverChemin(gameinfo.Player.Position, gameinfo.Player.HouseLocation, gameinfo.Player.HouseLocation,carte);
                if(chemin.Count == 1)
                {
                    storedRessources += gameinfo.Player.CarriedResources;
                }
                if (gameinfo.OtherPlayers.Count == 0 || gameinfo.OtherPlayers.Exists(x => Point.Distance(x.Value.Position, gameinfo.Player.Position) < 4))
                {
                    return AIHelper.CreateMoveAction(chemin[0]);
                }
                else
                {
                    return AIHelper.CreateMoveAction(chemin[0]);
                }
            }
            return AIHelper.CreateMoveAction(gameinfo.Player.Position);
        }
    }
}
