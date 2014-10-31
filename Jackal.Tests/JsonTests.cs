using System;
using System.Collections;
using System.Collections.Generic;
using Jackal.Players;
using JackalNetwork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Jackal.Tests
{
    [TestClass]
    public class JsonTests
    {
        [TestMethod]
        public void JsonJackalObjects()
        {
            CheckSerialization(new Position(1, 2),true);
            CheckSerialization(new Direction(new Position(1, 2), new Position(3, 4)),true);
            CheckSerialization(new TilePosition( new Position(1, 2),3), true);
        }


        private static void CheckSerialization<T>(T obj,bool print=false) where T : class
        {
            var json = JsonHelper.SerialiazeWithType<T>(obj, Formatting.Indented);
            if (print)
            {
                Console.WriteLine(obj.GetType().Name);
                Console.WriteLine(json);
            }
            var obj2 = JsonHelper.DeserialiazeWithType<T>(json);
            Assert.IsNotNull(obj2);
            var json2 = JsonHelper.SerialiazeWithType<T>(obj2, Formatting.Indented);
            if (json == json2)
            {
                Console.WriteLine(json);
                Console.WriteLine(json2);
                Assert.IsTrue(json == json2);
            }
        }

        [TestMethod]
        public void JsonTestAnswer()
        {
            DecisionAnswer answer=new DecisionAnswer();
            answer.RequestId = Guid.NewGuid();
            answer.Decision = 1;

            CheckSerialization(answer);
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

            while (game.IsGameOver == false)
            {
                //Console.ReadKey();
                game.Turn();
            }

            Console.WriteLine("Game end, turns count: "+game.TurnNo);
        }

        private class TestJsonPlayer : BlankPlayer
        {
            public override int OnMove(GameState gameState)
            {
                DecisionRequest request = new DecisionRequest(gameState);
                request.RequestId = Guid.NewGuid();

                var gameState2 = request.GetGameState();
                string json = JsonHelper.SerialiazeWithType(gameState, Formatting.Indented);
                string json2 = JsonHelper.SerialiazeWithType(gameState2, Formatting.Indented);

                Assert.IsTrue(json==json2);

                CheckSerialization(request);

                return base.OnMove(gameState);
            }
        }
    }
}
