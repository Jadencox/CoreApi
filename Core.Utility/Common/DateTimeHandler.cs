using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Core.Utility.Common
{
    public class DateTimeHandler
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        public static DateTime CurrentTime => DateTime.Now;

        public static DateTime GetMaxDateTime()
        {
            return new DateTime(2100, 1, 1);
        }

        public static DateTime GetMinDateTime()
        {
            return new DateTime(1901, 1, 1);
        }

        /// <summary>
        /// 当前时间（没有秒）
        /// </summary>
        public static DateTime CurrentTimeWithoutSecond => CurrentTime.NoSecond();

        public const string ShortDateTime = "yyyy-MM-dd";

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public const string LongDateTimeStyle = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss fff
        /// </summary>
        public const string BigLongDateTimeStyle = "yyyy-MM-dd HH:mm:ss fff";

        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        public const string DateTimeNumberStyle = "yyyyMMddHHmmss";

        /// <summary>
        /// yyyyMMddHHmmssfff
        /// </summary>
        public const string DateTimeFullStyle = "yyyyMMddHHmmssfff";

        /// <summary>
        /// yyyy年MM月dd日 HH时mm分
        /// </summary>
        public const string ShortDateTimeChineseStyle = "yyyy年MM月dd日 HH时mm分";

        /// <summary>
        /// yyyy年MM月dd日
        /// </summary>
        public const string ShortDateChineseStyle = "yyyy年MM月dd日";

        public static DateTime MinTime => new DateTime(1900, 1, 1);

        /// <summary>
        /// 将Json序列化的时间由/Date(1304931520336+0800)/转为字符串
        /// </summary>
        public static string ConvertJsonAfter1970DateToDateString(Match m)
        {
            var dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            var result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 将Json序列化的时间由/Date(1304931520336+0800)/转为字符串
        /// </summary>
        public static string ConvertJsonBefore1970DateToDateString(Match m)
        {
            var dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(0 - long.Parse(m.Groups[1].Value));
            if (dt != DateTime.MinValue)
            {
                dt = dt.ToLocalTime();
            }
            var result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 获得二个输入日期的之间的每一天的日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<DateTime> GetDayListByDate(DateTime firstDate, DateTime secondDate)
        {
            var days = secondDate.Subtract(firstDate).Days;
            return GetDayList(days, firstDate).ToList();
        }

        /// <summary>
        /// 获得一个输入日期的当月的每一天的日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<DateTime> GetDayListByDate(DateTime date)
        {
            var days = DateTime.DaysInMonth(date.Year, date.Month);
            return GetDayList(days, date.AddDays(-date.Day)).ToList();
        }

        private static IEnumerable<DateTime> GetDayList(int number, DateTime date)
        {
            for (var i = 1; i <= number; i++)
            {
                yield return date.AddDays(i);
            }
        }

        /// <summary>
        /// 判断是否在时间范围内（包头不包尾）
        /// </summary>
        /// <param name="time">时间点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static bool IsBetween(DateTime time, DateTime startTime, DateTime endTime)
        {
            return time >= startTime && time < endTime;
        }

        /// <summary>
        /// 判断是否在时间范围内（包头包尾）
        /// </summary>
        /// <param name="time">时间点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static bool IsBetweenAndTial(DateTime time, DateTime startTime, DateTime endTime)
        {
            return time >= startTime && time <= endTime;
        }

        /// <summary>
        /// 计算患者生日（默认生日为1月1号）
        /// </summary>
        /// <param name="dateTime">当前日期</param>
        /// <param name="age">年龄</param>
        /// <param name="month">出生月份</param>
        /// <param name="day">出生日期</param>
        /// <returns></returns>
        public static DateTime CalculateBirthday(DateTime dateTime, int age, int month = 1, int day = 1)
        {
            var date = new DateTime(dateTime.Year - age, month, day);
            return date > MinTime ? date : MinTime;
        }

        /// <summary>
        /// 获取设备同步时间
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetDeviceSyncTime(int freq, DateTime dateTime)
        {
            var time = dateTime.Date.AddHours(dateTime.Hour);
            while (time <= dateTime)
            {
                time = time.AddMinutes(freq <= 0 ? 60 : freq);
            }

            return time;
        }

        #region 已弃用

        /// <summary>
        /// 根据指定时间点来计算当时年龄
        /// </summary>
        /// <param name="birthdayDate">出生日期</param>
        /// <param name="curDate">指定时间点</param>
        /// <param name="isString">是否包含“岁”等字眼</param>
        /// <returns>年龄结果</returns>
        [Obsolete]
        public static string CalculateAge(DateTime birthdayDate, DateTime curDate, bool isString = true)
        {
            try
            {
                //生日未赋值或生日大于要计算年龄的时间，直接返回
                if (birthdayDate == DateTime.MinValue || birthdayDate > curDate)
                {
                    return isString ? "" : "0";
                }
                //2010-08-23
                //≤28天（或1月以内） 按天 
                //1岁以内 按几月几天 
                //12周岁以内 按几岁几月（月份按四舍五入）
                int nYear = 0;
                int nMonth = 0;
                int nDay = 0;
                //
                CalculateAgeByYearAndMonth(birthdayDate, curDate, ref nYear, ref nMonth);
                //12周岁以上
                if (nYear >= 12)
                {
                    return isString ? nYear + "岁" : nYear.ToString();
                }
                //12周岁以内 1岁以上 按几岁几月（月份按四舍五入）
                else if (nYear >= 1)
                {
                    return isString ? nYear + "岁" + (nMonth > 0 ? nMonth + "月" : "") : MathHandler.DecimalNumber(nYear + nMonth / 12m, 2).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    CalculateAgeByMonthAndDay(birthdayDate, curDate, ref nMonth, ref nDay);
                    //全年总天数 = 一年中除去2月份的天数+当年二月份的天数
                    decimal allDayOfYear = 337 + DateTime.DaysInMonth(curDate.Year, 2);
                    //1岁以内 按几月几天 
                    if (nMonth > 0)
                    {
                        return isString ? nMonth + "月" + (nDay > 0 ? nDay + "天" : "") : MathHandler.DecimalNumber(nMonth / 12m + nDay / allDayOfYear, 2).ToString(CultureInfo.InvariantCulture);
                    }
                    //≤28天（或1月以内） 按天 
                    else
                    {
                        return isString ? nDay + "天" : MathHandler.DecimalNumber(nDay / allDayOfYear, 2).ToString(CultureInfo.InvariantCulture);
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 计算年龄（按X年X月）
        /// </summary>
        /// <param name="birthdayDate"></param>
        /// <param name="date"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        [Obsolete]
        private static void CalculateAgeByYearAndMonth(DateTime birthdayDate, DateTime date, ref int year, ref int month)
        {
            if (birthdayDate < date)
            {
                year = date.Year - birthdayDate.Year;
                month = date.Month - birthdayDate.Month;
                //天数是负数的时候，要减去一个月
                if (date.Day - birthdayDate.Day < 0)
                {
                    month -= 1;
                }
                if (month < 0)
                {
                    //减掉一年，补上十二个月
                    year -= 1;
                    month += 12;
                }
            }
        }

        /// <summary>
        /// 计算年龄（按X月X天）
        /// </summary>
        /// <param name="birthdayDate"></param>
        /// <param name="date"></param>
        /// <param name="month"></param>
        /// <param name="days"></param>
        [Obsolete]
        private static void CalculateAgeByMonthAndDay(DateTime birthdayDate, DateTime date, ref int month, ref int days)
        {
            if (birthdayDate < date)
            {
                month = (date.Year - birthdayDate.Year) * 12 - birthdayDate.Month + date.Month;
                days = date.Day - birthdayDate.Day;
                if (days < 0)
                {
                    //月份统计到上个月
                    month -= 1;
                    //上个月剩余天数
                    var addDays = DateTime.DaysInMonth(date.AddMonths(-1).Year, date.AddMonths(-1).Month) - birthdayDate.Day;
                    //本月已过天数+上个月剩余天数 addDays取正数是为了避免
                    days = date.Day + (addDays > 0 ? addDays : 0);
                }
            }
        }
        #endregion
    }
}
