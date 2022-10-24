using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility.Common
{
    public static class StringHandler
    {
        /// <summary>
        /// SQL查询非法关键字
        /// </summary>
        public static List<string> SQLErrorKeyList => new List<string> { "UPDATE", "DELETE", "INSERT", "MERGE", "LOGIN_PWD", "TRUNCATE", "DROP", "CREATE", "ALTER" };

        /// <summary>
        /// SQL查询敏感关键字
        /// </summary>
        public static List<string> SQLIgnorePropertyList => new List<string> { "LOGIN_PWD" };

        public class SpecialCharacterView
        {
            public SpecialCharacterView(string code, string name)
            {
                Code = code;
                Name = name;
            }

            public string Code { get; set; }

            public string Name { get; set; }
        }

        /// <summary>
        /// 数字带圈的特殊字符（目前只有50个）
        /// </summary>
        public static ThreadSafeDictionary<int, SpecialCharacterView> SpecialCharacterMapping => new ThreadSafeDictionary<int, SpecialCharacterView>
        {
            { 1, new SpecialCharacterView("①", "[Circle]1[Circle/]") },
            { 2, new SpecialCharacterView("②", "[Circle]2[Circle/]") },
            { 3, new SpecialCharacterView("③", "[Circle]3[Circle/]") },
            { 4, new SpecialCharacterView("④", "[Circle]4[Circle/]") },
            { 5, new SpecialCharacterView("⑤", "[Circle]5[Circle/]") },
            { 6, new SpecialCharacterView("⑥", "[Circle]6[Circle/]") },
            { 7, new SpecialCharacterView("⑦", "[Circle]7[Circle/]") },
            { 8, new SpecialCharacterView("⑧", "[Circle]8[Circle/]") },
            { 9, new SpecialCharacterView("⑨", "[Circle]9[Circle/]") },
            { 10, new SpecialCharacterView("⑩", "[Circle]10[Circle/]") },
            { 11, new SpecialCharacterView("⑪", "[Circle]11[Circle/]") },
            { 12, new SpecialCharacterView("⑫", "[Circle]12[Circle/]") },
            { 13, new SpecialCharacterView("⑬", "[Circle]13[Circle/]") },
            { 14, new SpecialCharacterView("⑭", "[Circle]14[Circle/]") },
            { 15, new SpecialCharacterView("⑮", "[Circle]15[Circle/]") },
            { 16, new SpecialCharacterView("⑯", "[Circle]16[Circle/]") },
            { 17, new SpecialCharacterView("⑰", "[Circle]17[Circle/]") },
            { 18, new SpecialCharacterView("⑱", "[Circle]18[Circle/]") },
            { 19, new SpecialCharacterView("⑲", "[Circle]19[Circle/]") },
            { 20, new SpecialCharacterView("⑳", "[Circle]20[Circle/]") },
            { 21, new SpecialCharacterView("㉑", "[Circle]21[Circle/]") },
            { 22, new SpecialCharacterView("㉒", "[Circle]22[Circle/]") },
            { 23, new SpecialCharacterView("㉓", "[Circle]23[Circle/]") },
            { 24, new SpecialCharacterView("㉔", "[Circle]24[Circle/]") },
            { 25, new SpecialCharacterView("㉕", "[Circle]25[Circle/]") },
            { 26, new SpecialCharacterView("㉖", "[Circle]26[Circle/]") },
            { 27, new SpecialCharacterView("㉗", "[Circle]27[Circle/]") },
            { 28, new SpecialCharacterView("㉘", "[Circle]28[Circle/]") },
            { 29, new SpecialCharacterView("㉙", "[Circle]29[Circle/]") },
            { 30, new SpecialCharacterView("㉚", "[Circle]30[Circle/]") },
            { 31, new SpecialCharacterView("㉛", "[Circle]31[Circle/]") },
            { 32, new SpecialCharacterView("㉜", "[Circle]32[Circle/]") },
            { 33, new SpecialCharacterView("㉝", "[Circle]33[Circle/]") },
            { 34, new SpecialCharacterView("㉞", "[Circle]34[Circle/]") },
            { 35, new SpecialCharacterView("㉟", "[Circle]35[Circle/]") },
            { 36, new SpecialCharacterView("㊱", "[Circle]36[Circle/]") },
            { 37, new SpecialCharacterView("㊲", "[Circle]37[Circle/]") },
            { 30, new SpecialCharacterView("㊳", "[Circle]38[Circle/]") },
            { 30, new SpecialCharacterView("㊴", "[Circle]39[Circle/]") },
            { 40, new SpecialCharacterView("㊵", "[Circle]40[Circle/]") },
            { 40, new SpecialCharacterView("㊶", "[Circle]41[Circle/]") },
            { 40, new SpecialCharacterView("㊷", "[Circle]42[Circle/]") },
            { 40, new SpecialCharacterView("㊸", "[Circle]43[Circle/]") },
            { 40, new SpecialCharacterView("㊹", "[Circle]44[Circle/]") },
            { 40, new SpecialCharacterView("㊺", "[Circle]45[Circle/]") },
            { 40, new SpecialCharacterView("㊻", "[Circle]46[Circle/]") },
            { 40, new SpecialCharacterView("㊼", "[Circle]47[Circle/]") },
            { 40, new SpecialCharacterView("㊽", "[Circle]48[Circle/]") },
            { 40, new SpecialCharacterView("㊾", "[Circle]49[Circle/]") },
            { 50, new SpecialCharacterView("㊿", "[Circle]50[Circle/]") },
        };

        /// <summary>
        /// 连接字符串
        /// </summary>
        /// <param name="preTag">左分隔符</param>
        /// <param name="proTag">右分隔符</param>
        /// <param name="objects">需要连接的数据</param>
        /// <returns></returns>
        public static string ConcatString(string preTag, string proTag, params object[] objects)
        {
            var strContent = new StringBuilder();
            foreach (var obj in objects)
            {
                if (obj == null)
                {
                    continue;
                }

                var str = obj.ToString();
                if (obj is DateTime)
                {
                    str = ((DateTime)obj).ToString(DateTimeHandler.DateTimeNumberStyle);
                }

                strContent.AppendFormat(preTag + "{0}" + proTag, str);
            }

            return strContent.ToString();
        }

        /// <summary>
        /// 单位比较
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static bool UnitCompare(string unit, string compare)
        {
            if (string.IsNullOrEmpty(unit) || string.IsNullOrEmpty(compare))
            {
                return false;
            }

            var unites = unit.Split('/');
            if (unites.Length <= 0)
            {
                return false;
            }

            if (unites.Length == 1 && string.Equals(unites[0], compare, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            if (unites.Length == 2 && string.Equals(unites[1], compare, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return decimal.TryParse(str.Trim(), out _);
        }

        /// <summary>
        /// 生成特定位数的唯一字符串
        /// </summary>
        /// <param name="num">特定位数,最少大于8位</param>
        /// <returns></returns>
        public static string GenerateUniqueText(int num)
        {
            if (num <= 8)
            {
                num = 8;
            }

            num -= 4;
            var time = DateTimeHandler.CurrentTime.Millisecond.ToString().PadLeft(4, '0');
            string randomResult = string.Empty;
            string readyStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] rtn = new char[num];
            Guid gid = Guid.NewGuid();
            var ba = gid.ToByteArray();
            for (var i = 0; i < num; i++)
            {
                rtn[i] = readyStr[((ba[i] + ba[num + i]) % 35)];
            }

            foreach (char r in rtn)
            {
                randomResult += r;
            }

            return (randomResult.Replace(" ", "") + time).PadLeft(num, '0');
        }

        /// <summary>
        /// 生成Guid
        /// </summary>
        /// <returns></returns>
        public static string AutoId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static bool ContainsWithoutSpace(this List<string> list, string str)
        {
            if (list == null || string.IsNullOrWhiteSpace(str)) return false;
            foreach (var item in list)
            {
                if (item.Trim().Equals(str.Trim()))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 把表名转成类名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TransferString(this string str)
        {
            var returnValue = string.Empty;
            var list = str.NotNullToLower().RemoveEmptySplit("_");
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Substring(0, 1).ToUpper() + list[i].Substring(1);
                if (list[i].ToUpper() == "MED")
                {
                    continue;
                }
                returnValue += list[i];
            }
            return returnValue;
        }

        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        public static string GetHostUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .ToString();
        }

        public static string GetFileUri(this HttpRequest request, string filePath)
        {
            return Path.Combine(new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .ToString(), filePath).Replace(@"\", @"/");
        }
    }
}
