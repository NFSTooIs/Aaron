using System;
using System.Collections.Generic;
using System.Text;
using Aaron.Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aaron.Tests
{
    [TestClass]
    public static class GlobalBootstrap
    {
        [AssemblyInitialize]
        public static void Init(TestContext ctx)
        {
            ctx.WriteLine("Loading hash file");
            HashMapper.LoadStringsFromFile(@"data\hashes.txt");
            ctx.WriteLine("Loaded hash file");
        }
    }
}
