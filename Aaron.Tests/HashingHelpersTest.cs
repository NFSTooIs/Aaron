using Aaron.Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Tests
{
    [TestClass]
    public class HashingHelpersTest
    {
        [TestMethod]
        public void TestBinHash()
        {
            Assert.AreEqual(0xD6C2ECFAu, HashingHelpers.BinHash("CAR1004"));
            Assert.AreEqual(0u, HashingHelpers.BinHash("!"));
            Assert.AreNotEqual(0u, HashingHelpers.BinHash("!!"));
        }

        [TestMethod]
        public void TestVltHash()
        {
            Assert.AreEqual(0x964A1185u, HashingHelpers.JenkinsHash("car1004"));
            Assert.AreNotEqual(0xA416F8D0u, HashingHelpers.JenkinsHash("car1004"));
            Assert.AreEqual(0x82FC1624u, HashingHelpers.JenkinsHash(""));
        }
    }
}
