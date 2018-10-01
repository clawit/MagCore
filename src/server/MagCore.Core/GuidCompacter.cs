using System;
using System.Globalization;

namespace MagCore.Core
{

    public static class GuidCompacter
    {
        private const string BASE_CHAR = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //转换为段字符
        private static string GetLongCode(UInt64 num, int length)
        {
            string str = "";
            while (num > 0)
            {
                int cur = (int)(num % 36);
                str = BASE_CHAR[cur] + str;
                num = num / 36;
            }
            if (str.Length > length)
            {
                str = str.Substring(str.Length - length);
            }
            else
            {
                str = str.PadLeft(length, '0');
            }

            return str;
        }

        //解析段字符
        private static UInt64 GetLongNum(string str)
        {
            UInt64 num = 0;
            for (int i = 0; i < str.Length; i++)
            {
                num += (UInt64)BASE_CHAR.IndexOf(str[i]) * (UInt64)Math.Pow(BASE_CHAR.Length, str.Length - i - 1);
            }

            return num;
        }

        /// <summary>
        /// 压缩GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string Compact(string guid)
        {
            string s = guid.ToUpper();
            string s1 = s.Substring(0, 16);
            string s2 = s.Substring(16);
            UInt64 l1 = UInt64.Parse(s1, NumberStyles.HexNumber);
            UInt64 l2 = UInt64.Parse(s2, NumberStyles.HexNumber);
            string str1 = GetLongCode(l1, 13);
            string str2 = GetLongCode(l2, 13);

            return str1 + str2;
        }

        /// <summary>
        /// 解压GUID
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Uncompact(string str)
        {
            if (str.Length != 26)
            {
                throw new Exception("字符串错误！长度必须是26位！");
            }
            str = str.ToUpper();
            string s1 = str.Substring(0, 13);
            string s2 = str.Substring(13);
            UInt64 l1 = GetLongNum(s1);
            UInt64 l2 = GetLongNum(s2);
            string str1 = l1.ToString("X");
            string str2 = l2.ToString("X");
            string strGuid = str1.PadLeft(16, '0');
            strGuid += str2.PadLeft(16, '0');

            return strGuid;
        }
    }
}
