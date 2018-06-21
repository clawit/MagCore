using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MagCore.Monitor.Modules;

namespace UnitTest
{
    [TestClass]
    public class ApiReqTest
    {
        [TestMethod]
        public void GameListLoaderTest()
        {
            GameListLoader.Update();
        }
    }
}
