using Jackal;
using Jackal.Players;
using JackalWebHost.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JackalWebHost.Controllers
{
    public class GameController : Controller
    {
        public class GameState {

            public Board board;
            public Game game;
        }



        /// <summary>
        /// Главное окно
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Запуск игры
        /// </summary>
        public JsonResult Start(string players)
        {
            GameState gameState = new GameState();

            var playersList = JsonHelper.DeserialiazeWithType<string[]>(players);


            IPlayer[] gamePlayers = new IPlayer[4];
            int index = 0;

            foreach (var pl in playersList)
            {
                switch (pl)
                {
                    case "robot":
                        gamePlayers[index++] = new SmartPlayer();
                        break;
                    case "human":
                        gamePlayers[index++] = new WebHumanPlayer();
                        break;
                    default:
                        gamePlayers[index++] = new SmartPlayer2();
                        break;
                }
            }

            while (index < 4)
            {
                gamePlayers[index++] = new SmartPlayer();
            }

            int mapId = 887412 + 1;
            gameState.board = new Board(gamePlayers, mapId);
            gameState.game = new Game(gamePlayers, gameState.board);

            Session["test"] = gameState;

            var service = new DrawService();
            var map = service.Map(gameState.board);
            var teams = service.GetStat(gameState.game);

            return Json(new { gamename = "test", map = map, mapId = mapId, teams = teams });
        }

        /// <summary>
        /// Ход игры
        /// </summary>
        public JsonResult Turn(int? turnNum)
        {
            GameState gameState = Session["test"] as GameState;
            if (gameState == null)
                return Json(new { error = true });

            string prevBoardStr = JsonHelper.SerialiazeWithType(gameState.board);

            if (gameState.game.CurrentPlayer is WebHumanPlayer)
            {
                if (turnNum.HasValue)
                {
                    gameState.game.CurrentPlayer.SetHumanMove(turnNum.Value);
                    gameState.game.Turn();
                }
                else
                {
                    // если пользователь не сделал выбор, то перезапрашиваем ход
                }
            }
            else gameState.game.Turn();

            var prevBoard = JsonHelper.DeserialiazeWithType<Board>(prevBoardStr);

            var service = new DrawService();
            var changes = service.Draw(gameState.board, prevBoard);

            // availableMoves
            int currentTeamId = gameState.game.CurrentTeamId;
            var availableMoves = gameState.game.GetAvailableMoves();
            var avMoves = service.DrawAvailableMoves(availableMoves);

            bool isHumanPlayer = gameState.game.CurrentPlayer is WebHumanPlayer;

            var teams = service.GetStat(gameState.game);
            return Json(new { turn = gameState.game.TurnNo, 
                changes = changes, teams = teams, moves = avMoves, 
                isHuman = isHumanPlayer, curTeam = currentTeamId, isGameOver = gameState.game.IsGameOver });
        }

        /// <summary>
        /// Сброс игры
        /// </summary>
        public JsonResult Reset()
        {
            Session["data"] = null;

            return Json(new { result = "ok" });
        }

    }
}
