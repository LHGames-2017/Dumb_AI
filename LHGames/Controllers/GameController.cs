namespace StarterProject.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("/")]
    public class GameController : Controller
    {
        AIHelper player = new AIHelper();
        List<Point> ressources = new List<Point>();
        List<Point> houses = new List<Point>();
        List<Point> shops = new List<Point>();
        int cptTour = 0;
        Point lastPosition;

        [HttpPost]
        public string Index([FromForm]string map)
        {
            GameInfo gameInfo = JsonConvert.DeserializeObject<GameInfo>(map);
            var carte = AIHelper.DeserializeMap(gameInfo.CustomSerializedMap);
            AIPlayer AI = new AIPlayer();

            if(cptTour == 0)
            {
                for (int i = 0; i < carte.GetLength(0); i++)
                {
                    for (int j = 0; j < carte.GetLength(1); j++)
                    {
                        if (carte[i, j].C == (byte)TileType.R) ressources.Add(new Point(carte[i, j].X, carte[i, j].Y));
                        if (carte[i, j].C == (byte)TileType.H) houses.Add(new Point(carte[i, j].X, carte[i, j].Y));
                        if (carte[i, j].C == (byte)TileType.S) shops.Add(new Point(carte[i, j].X, carte[i, j].Y));
                    }
                }
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
            

            
            string action = DeciderAction(gameInfo);
            lastPosition = gameInfo.Player.Position;
            ++cptTour;
            return action;
        }

        public string DeciderAction(GameInfo gameinfo)
        {

            return AIHelper.CreateMoveAction(gameinfo.Player.Position);
        }
    }
}
