using System;
using MagCore.Server.WxApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class WxTest
    {
        [TestMethod]
        public void TestWxAccessToken()
        {
            string s = WxBase.GetAccessToken();
        }
    }
}
