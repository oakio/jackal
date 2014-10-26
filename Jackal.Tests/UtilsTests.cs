using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jackal.Tests
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void TestPermutations()
        {
            Assert.IsTrue(Utils.Factorial(0) == 1);
            Assert.IsTrue(Utils.Factorial(1) == 1);
            Assert.IsTrue(Utils.Factorial(4) == 4 * 3 * 2 * 1);

            Console.WriteLine("Permutations:");
            HashSet<string> hashSet=new HashSet<string>();
            for (int i = 0; i < Utils.Factorial(4); i++)
            {
                var rec = Utils.GetPermutation(i, new[] {"1", "2", "3", "4"});
                var val = string.Join(",", rec.ToArray());
                Assert.IsTrue(hashSet.Contains(val) == false);
                Console.WriteLine("{0}: {1}", i, val);
            }
            var set1 = Utils.GetPermutation(0, new[] {"1", "2", "3", "4"});
            var set2 = Utils.GetPermutation(Utils.Factorial(4), new[] {"1", "2", "3", "4"});
            Assert.IsTrue(set1.SequenceEqual(set2));
        }
    }
}