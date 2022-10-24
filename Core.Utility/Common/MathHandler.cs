using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Utility.Common
{
    public static class MathHandler
    {
        /// <summary>
        /// 保留小数点后位数，如果精度不够，就保留两位有效数字
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static decimal DecimalNumber(decimal d, int n)
        {
            var str = "";
            for (var i = 0; i < n; i++)
            {
                str += "#";
            }
            var value = Convert.ToDecimal(d.ToString("0." + str));
            if (d != 0 && value == 0)
            {
                //说明保留的位数太小
                value = DecimalPoint(d, n);
            }
            return value;
        }

        /// <summary>
        /// 小数点后有效位数进行四舍五入，比如0.00353,取2则返回0.0035
        /// </summary>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private static decimal DecimalPoint(decimal d, int n)
        {
            if (d == 0)
                return 0;
            var s = Math.Floor(d); //整位数
            var point = d - s; //小数位
            if (point == 0)
            {
                return d;
            }
            if (point > 0)
            {
                point = point + Convert.ToDecimal(0.0000000001);
            }
            else
            {
                point = point - Convert.ToDecimal(0.0000000001);
            }
            int www;
            if (point > 1 || point < -1)
                www = n - (int)Math.Log10(Math.Abs(Convert.ToDouble(point))) - 1;
            else
                www = n + (int)Math.Log10(1.0 / Math.Abs(Convert.ToDouble(point)));
            if (www < 0)
            {
                point = (int)(point / Convert.ToDecimal(Math.Pow(10, 0 - www))) * Convert.ToDecimal(Math.Pow(10, 0 - www));
                www = 0;
            }
            var value = s + Math.Round(point, www);
            var valueStr = value.ToString(CultureInfo.InvariantCulture);
            if (valueStr.Contains("."))
            {
                value = Convert.ToDecimal(valueStr.TrimEnd('0'));
            }
            return value;
        }

        /// <summary>
        /// 换算
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="toUnit">转换后的单位</param>
        /// <param name="fromUnit">转换前的单位</param>
        /// <returns></returns>
        public static decimal BytesToSize(decimal bytes, string toUnit, string fromUnit)
        {
            var returnValue = bytes;
            toUnit = toUnit.NotNullToLower();
            fromUnit = fromUnit.NotNullToLower();
            if (toUnit == "iu")
            {
                toUnit = "u";
            }
            if (fromUnit == "iu")
            {
                fromUnit = "u";
            }
            if (toUnit == "ug")
            {
                toUnit = "μg";
            }
            if (fromUnit == "ug")
            {
                fromUnit = "μg";
            }
            var ss = new List<BytesToSizeView>();
            ss.Add(new BytesToSizeView { Rate = 1000, UnitList = new List<string> { "μg", "mg", "g" } });
            ss.Add(new BytesToSizeView { Rate = 1000, UnitList = new List<string> { "u", "万u" } });
            foreach (var s in ss)
            {
                var toIndex = s.UnitList.FindIndex(entity => entity == toUnit);
                var fromIndex = s.UnitList.FindIndex(entity => entity == fromUnit);
                if (toIndex < 0 || fromIndex < 0)
                {
                    continue;
                }
                if (fromIndex < toIndex)
                {
                    returnValue = bytes / Convert.ToDecimal(Math.Pow(s.Rate, toIndex - fromIndex));
                }
                else if (fromIndex > toIndex)
                {
                    returnValue = bytes * Convert.ToDecimal(Math.Pow(s.Rate, fromIndex - toIndex));
                }
                break;
            }
            return returnValue;
        }

        private static readonly string[] thousands = { "", "M", "MM", "MMM" };
        private static readonly string[] hundreds = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
        private static readonly string[] tens = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        private static readonly string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

        // 转为罗马数字
        public static string IntToRoman(int num)
        {
            if (num < 0) return String.Empty;
            StringBuilder roman = new StringBuilder();
            roman.Append(thousands[num / 1000]);
            roman.Append(hundreds[num % 1000 / 100]);
            roman.Append(tens[num % 100 / 10]);
            roman.Append(ones[num % 10]);
            return roman.ToString();
        }
    }

    public class BytesToSizeView
    {
        public int Rate { get; set; }

        public List<string> UnitList { get; set; }
    }
}
