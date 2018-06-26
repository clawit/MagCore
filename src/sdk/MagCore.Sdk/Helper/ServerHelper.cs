using MagCore.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Sdk.Helper
{
    public static class ServerHelper
    {
        public static void Initialize(string server)
        {
            ApiReq._url = server;
        }
    }
}
