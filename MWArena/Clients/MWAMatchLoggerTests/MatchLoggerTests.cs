using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchLogger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace MatchLogger.Tests
{
    [TestClass()]
    public class MatchLoggerTests
    {
        [TestMethod()]
        public void HandleLevelLoadTest()
        {

            MatchLogger.HandleLevelLoad(new byte[]{13,13});
            Assert.IsTrue(true);
        }
    }
}
