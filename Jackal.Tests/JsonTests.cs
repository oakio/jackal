using System;
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
            CheckSerialization(new Direction(new TilePosition(new Position(1, 2)), new TilePosition(new Position(3, 4))));
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
            if (json != json2)
            {
                Console.WriteLine(json);
                Console.WriteLine(json2);
            }
            Assert.IsTrue(json == json2);
        }
    }
}
