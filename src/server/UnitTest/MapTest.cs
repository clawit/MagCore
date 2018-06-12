using MagCore.Model.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace UnitTest
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void TestRectMapLoad()
        {
            var map = new RectMap(@"E:\github\MagCore\src\server\MagCore.Server\Maps\RectSmall.map");
            Assert.IsNotNull(map);

            map.Load();
            Assert.AreEqual(map.Size.H, 10);
            Assert.AreEqual(map.Size.W, 10);

            for (int i = 0; i < map.Rows.Count; i++)
            {
                for (int j = 0; j < map.Rows[i].Count; j++)
                {
                    Debug.Write((int)map.Rows[i].Cells[j].Type);
                }
                Debug.WriteLine("");
            }
        }
    }
}
