using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility.Common
{
    /// <summary>
    /// 字符串扩展方法
    /// </summary>
    public static class StringExtensions
    {
        public static string Fill(this string template, params object[] arguments)
        {
            Guard.ArgumentNotNullOrWhiteSpace(template, nameof(template));
            return string.Format(CultureInfo.CurrentCulture, template, arguments);
        }

        /// <summary>
        /// 字符串忽略大小写相等（已判断是否null和WhiteSpace）
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool NotNullIgnoreCaseEqual(this string str1, string str2)
        {
            if (string.IsNullOrWhiteSpace(str1) || string.IsNullOrWhiteSpace(str2))
            {
                return false;
            }

            return string.Equals(str1, str2, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 字符串是否不相等（已判断是否null和WhiteSpace）
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool NotNullIgnoreCaseNotEqual(this string str1, string str2)
        {
            if (string.IsNullOrWhiteSpace(str1) || string.IsNullOrWhiteSpace(str2))
            {
                return false;
            }

            var dd = !string.Equals(str1, str2, StringComparison.CurrentCultureIgnoreCase);
            return dd;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Clone<T>(this T value) //where T : class
        {
            var valueJson = JsonConvert.SerializeObject(value);
            var rsult = JsonConvert.DeserializeObject<T>(valueJson);
            return rsult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return DateTime.MinValue;
            var ms = 0;
            if (str.NotNullContains(","))
            {
                //说明时间带毫秒
                var strList = str.RemoveEmptySplit(',');
                str = strList[0];
                ms = strList[1].ToInt32();
            }

            DateTime value;
            DateTime.TryParse(str.Trim(), out value);
            if (value != DateTime.MinValue)
            {
                value = value.AddMilliseconds(ms);
            }

            return value;
        }

        /// <summary>
        /// 字符串是否包含数据（已判断null以及为空情况）
        /// </summary>
        /// <param name="bigStr"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NotNullContains(this string bigStr, string str)
        {
            if (string.IsNullOrWhiteSpace(bigStr) || string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return bigStr.Contains(str);
        }

        /// <summary>
        /// 分割字符传
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splits"></param>
        /// <returns></returns>
        public static List<string> RemoveEmptySplit(this string str, params char[] splits)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return new List<string>();
            }

            var split = new[] { ',' };
            if (splits.Length > 0)
            {
                split = splits;
            }

            return str.Split(split, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// String转字典
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="groupSplits">组分割符</param>
        /// <param name="itemGroups">子项分隔符</param>
        /// <returns></returns>
        static public Dictionary<string, string> ParseStrToDictionary(this string str, char[] groupSplits, char[] itemGroups)
        {
            try
            {
                var strList = str.RemoveEmptySplit(groupSplits);
                Dictionary<string, string> dataDic = new Dictionary<string, string>();
                foreach (var itemStr in strList)
                {
                    var itemStrList = itemStr.RemoveEmptySplit(itemGroups);
                    if (itemStrList.Count == 2 && !dataDic.ContainsKey(itemStrList[0]))
                    {
                        dataDic.Add(itemStrList[0], itemStrList[1]);
                    }
                }
                return dataDic;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 分割字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="split"></param>
        /// <returns></returns>
        public static List<string> RemoveEmptySplit(this string str, string split)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return new List<string>();
            }

            return str.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// 字符串转Int32类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            int.TryParse(str.Trim(), out var value);
            return value;
        }

        /// <summary>
        /// 从字符串转成Decimal类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }

            decimal.TryParse(str.Trim(), out var value);
            return value;
        }

        /// <summary>
        /// 把字符串转成List类型，相当于new List{}
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> ToList(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return new List<string>();
            }

            return new List<string> { str };
        }

        /// <summary>
        /// 字符串是否不包含数据（已判断是否null和WhiteSpace）
        /// </summary>
        /// <param name="bigStr"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NotNullNotContains(this string bigStr, string str)
        {
            if (string.IsNullOrWhiteSpace(bigStr) || string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return !bigStr.Contains(str);
        }

        /// <summary>
        /// 查找数据所在索引
        /// </summary>
        /// <param name="str"></param>
        /// <param name="indexStr"></param>
        /// <returns></returns>
        public static int NotNullIndexOf(this string str, string indexStr)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return -1;
            }

            return str.IndexOf(indexStr, StringComparison.Ordinal);
        }

        public static bool NotNullStartsWith(this string str, string startsWith)
        {
            if (Guard.IsNullOrWhiteSpace(str, startsWith))
            {
                return false;
            }

            return str.StartsWith(startsWith);
        }

        public static string NotNullReplace(this string str, string beforeStr, string afterStr)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            if (string.IsNullOrEmpty(beforeStr))
            {
                return str;
            }

            return str.Replace(beforeStr, afterStr);
        }

        public static string NotNullReplaces(this string str, List<string> beforeStrs, string afterStr)
        {


            foreach (var beforeStr in beforeStrs)
            {
                str = str.NotNullReplace(beforeStr, afterStr);
            }

            return str;
        }

        /// <summary>
        /// 移除末尾字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimChars"></param>
        /// <returns></returns>
        public static string NotNullTrimEnd(this string str, params char[] trimChars)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            return str.TrimEnd(trimChars);
        }

        /// <summary>
        /// 转换成小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NotNullToLower(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            return str.ToLower();
        }

        /// <summary>
        /// 转换成大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NotNullToUpper(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            return str.ToUpper();
        }

        /// <summary>
        /// 列表组装SQL语句
        /// </summary>
        /// <param name="list">数据列表</param>
        /// <param name="split">分隔符</param>
        /// <param name="pre">数据前后分隔符</param>
        /// <returns></returns>
        public static string SQLAggregate(this List<string> list, char split = ',', string pre = "'")
        {
            if (list.Count <= 0)
            {
                return "''";
            }

            return list.Distinct().Aggregate("", (current, id) => current + pre + id + pre + split).TrimEnd(split);
        }
    }

    /// <summary>
    /// decimal?扩展方法
    /// </summary>
    public static class NullDecimalExtensions
    {
        public static decimal ToDecimal(this decimal? value)
        {
            if (!value.HasValue)
            {
                return 0;
            }

            return value.Value;
        }

        public static decimal? DecimalMinus(this decimal? a, decimal? b)
        {
            if (!a.HasValue && !b.HasValue)
            {
                return null;
            }

            return (a ?? 0) - (b ?? 0);
        }
    }

    /// <summary>
    /// DateTime?扩展方法
    /// </summary>
    public static class NullDateTimeExtensions
    {
        public static DateTime ToDateTime(this DateTime? time)
        {
            if (!time.HasValue)
            {
                return DateTime.MinValue;
            }

            return time.Value;
        }

        /// <summary>
        /// 是否最大值最小值
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsMinOrMax(this DateTime? time)
        {
            if (!time.HasValue)
            {
                return true;
            }

            if (time.Value.Date == DateTime.MaxValue.Date || time.Value == DateTime.MinValue)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 去除秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime? NoSecond(this DateTime? time)
        {
            return time?.AddSeconds(-time.Value.Second);
        }

        /// <summary>
        /// 判断是否在时间范围内（包头不包尾）
        /// </summary>
        /// <param name="time">时间点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime? time, DateTime startTime, DateTime endTime)
        {
            if (!time.HasValue)
            {
                return false;
            }

            return time.Value >= startTime && time.Value < endTime;
        }

        /// <summary>
        /// 判断是否在时间范围内（不包头不包尾）
        /// </summary>
        /// <param name="time"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool IsBetweenNoHeadAndNoTail(this DateTime time, DateTime start, DateTime end)
        {
            return time > start && time < end;
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime SqlMinValue(this DateTime datetime)
        {
            return new DateTime(1753, 1, 1);
        }

        /// <summary>
        /// 去除分（也去除秒）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime NoMinute(this DateTime time)
        {
            return time.NoSecond().AddMinutes(-time.Minute);
        }

        /// <summary>
        /// 去除秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime NoSecond(this DateTime time)
        {
            return time.AddSeconds(-time.Second);
        }

        /// <summary>
        /// 去除毫秒
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime NoMillisecond(this DateTime time)
        {
            return time.AddMilliseconds(-time.Millisecond);
        }

        /// <summary>
        /// 判断是否在时间范围内（包头不包尾）
        /// </summary>
        /// <param name="time">时间点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static bool IsBetween(this DateTime time, DateTime startTime, DateTime endTime)
        {
            return time >= startTime && time < endTime;
        }

        /// <summary>
        /// 判断是否在时间范围内（不包头包尾）
        /// </summary>
        /// <param name="time">时间点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static bool IsBetweenTail(this DateTime time, DateTime startTime, DateTime endTime)
        {
            return time > startTime && time <= endTime;
        }

        /// <summary>
        /// 判断是否在时间范围内（包头包尾）
        /// </summary>
        /// <param name="time">时间点</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public static bool IsBetweenHeadAndTail(this DateTime time, DateTime startTime, DateTime endTime)
        {
            return time >= startTime && time <= endTime;
        }

        /// <summary>
        /// 不重复添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="value">需要新增的值</param>
        public static void NoRepeatAdd(this List<DateTime> list, DateTime value)
        {
            if (list.Contains(value))
            {
                return;
            }

            list.Add(value);
        }
    }

    /// <summary>
    /// 字符串列表扩展方法
    /// </summary>
    public static class StringListExtensions
    {
        /// <summary>
        /// 列表是否包含数据（已判断是否null和WhiteSpace）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NotNullContains(this List<string> list, string str)
        {
            if (list == null || string.IsNullOrWhiteSpace(str))
            {
                return false;
            }

            return list.Contains(str.Trim());
        }

        /// <summary>
        /// 列表不包含数据（已判断是否null和WhiteSpace）
        /// </summary>
        /// <param name="list"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool NotNullNotContains(this List<string> list, string str)
        {
            if (string.IsNullOrWhiteSpace(str) || list.Contains(str))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 不重复添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="str">需要新增的值</param>
        public static List<string> NoRepeatAdd(this List<string> list, string str)
        {
            if (string.IsNullOrWhiteSpace(str) || list.Contains(str))
            {
                return list;
            }

            list.Add(str);
            return list;
        }

        /// <summary>
        /// 不重复添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="addList">需要新增的值</param>
        public static List<string> NoRepeatAdd(this List<string> list, List<string> addList)
        {
            if (addList == null || addList.Count <= 0)
            {
                return list;
            }

            foreach (var add in addList)
            {
                list.NoRepeatAdd(add);
            }

            return list;
        }
    }

    /// <summary>
    /// T列表扩展方法
    /// </summary>
    public static class TListExtensions
    {
        /// <summary>
        /// 获取列表第一个数据，没有则实例化
        /// </summary>
        /// <param name="list">数据列表</param>
        /// <returns></returns>
        public static T FirstOrInstance<T>(this List<T> list) where T : new()
        {
            if (list == null || list.Count <= 0)
            {
                return new T();
            }

            return list[0];
        }

        /// <summary>
        /// 不重复新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="predicate">判断表达式</param>
        public static void NoRepeatAdd<T>(this List<T> list, T item, Predicate<T> predicate)
        {
            if (list.Find(predicate) != null)
            {
                return;
            }

            list.Add(item);
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="list"></param>
        /// <param name="property"></param>
        /// <param name="split">连接符</param>
        /// <returns></returns>
        public static string ConcatenateText<TEntity, TProperty>(this List<TEntity> list, Expression<Func<TEntity, TProperty>> property, char split) where TEntity : class
        {
            var fieldName = GetPropertyName(property);
            var parentType = typeof(TEntity);
            //进行属性拷贝
            var prop = parentType.GetProperty(fieldName);
            if (prop == null)
            {
                return "";
            }

            var concatTxt = list.Aggregate("", delegate (string current, TEntity order)
            {
                var text = prop.GetValue(order);
                return $"{current} {text}{split}";
            }).TrimStart(' ');
            return !string.IsNullOrEmpty(concatTxt) && concatTxt.EndsWith(split.ToString()) ? concatTxt.Substring(0, concatTxt.Length - 1) : concatTxt;
        }

        private static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expr)
        {
            var rtn = "";
            if (expr.Body is UnaryExpression)
            {
                rtn = ((MemberExpression)((UnaryExpression)expr.Body).Operand).Member.Name;
            }
            else if (expr.Body is MemberExpression)
            {
                rtn = ((MemberExpression)expr.Body).Member.Name;
            }
            else if (expr.Body is ParameterExpression)
            {
                rtn = ((ParameterExpression)expr.Body).Type.Name;
            }

            return rtn;
        }
    }

    /// <summary>
    /// int扩展方法
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// 不重复添加
        /// </summary>
        /// <param name="list">列表</param>
        /// <param name="i">需要新增的值</param>
        public static List<int> NoRepeatAdd(this List<int> list, int i)
        {
            if (list.Contains(i))
            {
                return list;
            }

            list.Add(i);
            return list;
        }
    }

    /// <summary>
    /// int?扩展方法
    /// </summary>
    public static class NullIntExtensions
    {
        /// <summary>
        /// int?转Int32类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt32(this int? value)
        {
            if (!value.HasValue)
            {
                return 0;
            }

            return value.Value;
        }
    }

    /// <summary>
    /// 线程安全的键值对
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ThreadSafeDictionary<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        public TValue Add(TKey key, TValue value)
        {
            return AddOrUpdate(key, value, (sqlKey, s) => value);
        }

        public bool Remove(TKey key)
        {
            return TryRemove(key, out _);
        }
    }

    /// <summary>
    /// 线程安全的List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSafeList<T> : ConcurrentQueue<T>
    {
        public void Add(T item)
        {
            Enqueue(item);
        }

        private void Enqueue(T? item)
        {
            throw new NotImplementedException();
        }
    }

    public static class IDictionaryExtension
    {
        /// <summary>
        /// 获取字典的值
        /// </summary>
        /// <param name="dict">列表</param>
        /// <param name="key">Key</param>
        public static T2 NotNullGetValue<T, T2>(this IDictionary<T, T2> dict, T key)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            return default(T2);
        }
    }

    public static class TListExtension
    {
        public static T FirstOrInstant<T>(this List<T> list, Func<T, bool> func) where T : new()
        {
            var item = list.FirstOrDefault(func);
            if (item == null)
            {
                return new T();
            }

            return item;
        }
    }
}
