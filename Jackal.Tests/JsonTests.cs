using System;
using System.Collections.Generic;
using JackalNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Jackal.Tests
{
    [TestClass]
    public class JsonTests
    {
        [TestMethod]
        public void JsonTestAnswer()
        {
            DecisionAnswer answer=new DecisionAnswer();
            answer.RequestId = Guid.NewGuid();
            answer.Decision = 1;

            var message = JsonHelper.SerialiazeWithType(answer, Formatting.Indented);
            var t = JsonHelper.DeserialiazeWithType<NetworkMessage>(message);
            Assert.IsNotNull(t);
            Assert.IsTrue(t is DecisionAnswer);
            Assert.IsTrue(answer.RequestId == (t as DecisionAnswer).RequestId);
            Assert.IsTrue(answer.Decision == (t as DecisionAnswer).Decision);
        }

        [TestMethod]
        public void JsonTestGame()
        {
            const int mapId = 987412 + 1;

            var board = new Board(mapId);

            List<IPlayer> players = new List<IPlayer>();
            players.Add(new TestJsonPlayer());
            while (players.Count < 4)
                players.Add(new SmartPlayer());

            var game = new Game(players.ToArray(), board);

            while (game.IsGameOver == false && game.TurnNo<=4)
            {
                //Console.ReadKey();
                game.Turn();
            }

            //report("Game end.");
        }

        public class TestJsonPlayer : IPlayer
        {
            public void OnNewGame()
            {
            }

            public int OnMove(GameState gameState)
            {
                DecisionRequest request = new DecisionRequest();
                request.RequestId = Guid.NewGuid();
                request.State = gameState;

                var message = JsonHelper.SerialiazeWithType(request, Formatting.Indented);

                var t = JsonHelper.DeserialiazeWithType<NetworkMessage>(message);
                Assert.IsNotNull(t);
                Assert.IsTrue(t is DecisionRequest);

                Assert.IsTrue(request.State.AvailableMoves[0]==(t as DecisionRequest).State.AvailableMoves[0]);

                return 0;
            }
        }
    }
}
