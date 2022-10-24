using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Common.DataAccess
{
    public class CommonFieldsBase
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        [Column("TRANSACTION_ID")]
        [JsonIgnore]
        public string TransactionId { get; set; }

        /// <summary>
        /// 数据版本号
        /// </summary>
        [Column("VERSION_NO")]
        //[ConcurrencyCheck] 开启并发验证
        public int VersionNo { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [NotMapped]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Column("CREATOR")]
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("CREATE_TIME", TypeName = "DATE")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        [NotMapped]
        public string ModifierName { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Column("MODIFIER")]
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Column("MODIFY_TIME", TypeName = "DATE")]
        public DateTime? ModifyTime { get; set; }
    }

    /// <summary>
    /// T扩展方法
    /// </summary>
    public static class TExtensions
    {
        /// <summary>
        /// 实体复制
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static TChild AutoCopy<TParent, TChild>(this TParent parent) where TChild : new()
        {
            var child = new TChild();
            parent.AutoCopy(child);
            return child;
        }

        /// <summary>
        /// 实体复制
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static TChild AutoCopy<TParent, TChild>(this TParent parent, TChild child) where TChild : new()
        {
            if (parent == null)
            {
                return child;
            }

            #region 反射复制

            //var parentProperties = CacheHandler.GetObjectProperties(parent);
            //var childProperties = CacheHandler.GetObjectProperties(child);
            //foreach (var parentProp in parentProperties)
            //{
            //    var findChildProp = childProperties.FirstOrDefault(e => e.Name == parentProp.Name);
            //    if (findChildProp == null)
            //    {
            //        continue;
            //    }

            //    if (!findChildProp.CanWrite)
            //    {
            //        continue;
            //    }

            //    var value = ReflectionHandler.PropertyFastGetValue(parentProp, parent);
            //    if (value == null)
            //    {
            //        continue;
            //    }

            //    if (findChildProp.PropertyType == typeof(DateTime))
            //    {
            //        if (!DateTime.TryParse(value.ToString().Trim(), out var temp))
            //        {
            //            value = temp.ToString(CultureInfo.InvariantCulture);
            //        }
            //    }

            //    ReflectionHandler.PropertyFastSetValue(findChildProp, child, ReflectionHandler.ChangeType(value, findChildProp.PropertyType));
            //}

            #endregion 反射复制

            EntityCloneHandler<TParent, TChild>.Clone(parent, child);

            return child;
        }

        /// <summary>
        /// 表达式树创建
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        public static class TransExpV2<TIn, TOut>
        {
            private static readonly Func<TIn, TOut> _cache = GetFunc();

            private static Func<TIn, TOut> GetFunc()
            {
                ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
                List<MemberBinding> memberBindingList = new List<MemberBinding>();
                foreach (var item in typeof(TOut).GetProperties())
                {
                    if (!item.CanWrite)
                    {
                        continue;
                    }

                    var inProperty = typeof(TIn).GetProperty(item.Name);
                    if (inProperty == null)
                    {
                        continue;
                    }

                    MemberExpression property = Expression.Property(parameterExpression, inProperty);
                    MemberBinding memberBinding = Expression.Bind(item, property);
                    memberBindingList.Add(memberBinding);
                }

                MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
                //var ssss = Expression.MemberInit(Expression.)
                Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });
                return lambda.Compile();
            }

            public static TOut Trans(TIn tIn)
            {
                return _cache(tIn);
            }
        }

        /// <summary>
        /// 表达式树复制
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        private static class EntityCloneHandler<TIn, TOut> where TOut : new()
        {
            /// <summary>
            /// 初始化
            /// </summary>
            static EntityCloneHandler()
            {
                _cloneMethod = CreateCloneMethod();
            }

            /// <summary>
            /// 克隆函数
            /// </summary>
            private static readonly Action<TIn, TOut> _cloneMethod;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            private static Action<TIn, TOut> CreateCloneMethod()
            {
                var sourceType = typeof(TIn);
                var targetType = typeof(TOut);
                var tTarget = Expression.Parameter(targetType, "TTarget");
                var tSource = Expression.Parameter(sourceType, "TSource");
                var expression = new List<BinaryExpression>(16);
                foreach (var field in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!field.CanWrite)
                    {
                        continue;
                    }

                    var inProperty = typeof(TIn).GetProperty(field.Name);
                    if (inProperty == null)
                    {
                        continue;
                    }

                    var originalMember = Expression.Property(tSource, inProperty);
                    var newMember = Expression.Property(tTarget, field);

                    var setValue = Expression.Assign(newMember, originalMember);
                    expression.Add(setValue);
                }

                var body = Expression.Block(typeof(void), expression);
                return Expression.Lambda<Action<TIn, TOut>>(body, tSource, tTarget).Compile();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="instance"></param>
            /// <returns></returns>
            public static TOut Clone(TIn instance)
            {
                var newIns = new TOut();
                _cloneMethod(instance, newIns);
                return newIns;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="newIns"></param>
            /// <returns></returns>
            public static TOut Clone(TIn instance, TOut newIns)
            {
                _cloneMethod(instance, newIns);
                return newIns;
            }
        }

        ///// <summary>
        ///// 设置属性值
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <typeparam name="TProperty"></typeparam>
        ///// <param name="entity"></param>
        ///// <param name="property"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public static TEntity AddPropertyValue<TEntity, TProperty>(this TEntity entity, Expression<Func<TEntity, TProperty>> property, TProperty value) where TEntity : class
        //{
        //    var fieldName = EntityHandler.GetPropertyName(property);

        //    var parentType = typeof(TEntity);

        //    //进行属性拷贝
        //    var prop = parentType.GetProperty(fieldName);
        //    prop.SetValue(entity, value, null);

        //    return entity;
        //}

        //public static TEntity AddPropertyValue<TEntity, TProperty>(this TEntity entity, Expression<Func<TEntity, TProperty>> property, Expression<Func<TEntity, TProperty>> property1) where TEntity : class
        //{
        //    var fieldName = EntityHandler.GetPropertyName(property);

        //    var parentType = typeof(TEntity);

        //    //进行属性拷贝
        //    var prop = parentType.GetProperty(fieldName);
        //    prop.SetValue(entity, property1.Compile()(entity), null);

        //    return entity;
        //}

        /// <summary>
        /// 列表复制
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parents"></param>
        /// <returns></returns>
        public static List<TChild> AutoCopys<TParent, TChild>(this List<TParent> parents) where TChild : new()
        {
            return parents.Select(AutoCopy<TParent, TChild>).ToList();
        }

        /// <summary>
        /// 列表复制
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="parents"></param>
        /// <returns></returns>
        public static IEnumerable<TChild> AutoCopys<TParent, TChild>(this IEnumerable<TParent> parents) where TChild : new()
        {
            return parents.Select(AutoCopy<TParent, TChild>);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FastPropertyComparer<T> : IEqualityComparer<T>
    {
        private readonly List<Func<T, object>> _getPropertyValueFuncList = new List<Func<T, object>>();

        public FastPropertyComparer<T> Init<TParam>(Expression<Func<T, TParam>> expression)
        {
            switch (expression.Body)
            {
                //创建实体
                case MemberInitExpression expressionBody:
                    {
                        var binds = expressionBody.Bindings.ToList();

                        foreach (var property in binds)
                        {
                            var propertyName = property.Member.Name;
                            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                            if (propertyInfo == null)
                            {
                                throw new ArgumentException($"{propertyName} is not a property of type {typeof(T)}.");
                            }

                            var expPara = Expression.Parameter(typeof(T), "obj");
                            var me = Expression.Property(expPara, propertyInfo);
                            _getPropertyValueFuncList.Add(Expression.Lambda<Func<T, object>>(Expression.Convert(me, typeof(object)), expPara).Compile());
                        }

                        break;
                    }

                //匿名类
                case NewExpression expressionBody:
                    {
                        foreach (var property in expressionBody.Members)
                        {
                            var propertyName = property.Name;
                            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
                            if (propertyInfo == null)
                            {
                                throw new ArgumentException($"{propertyName} is not a property of type {typeof(T)}.");
                            }

                            var expPara = Expression.Parameter(typeof(T), "obj");
                            var me = Expression.Property(expPara, propertyInfo);
                            _getPropertyValueFuncList.Add(Expression.Lambda<Func<T, object>>(Expression.Convert(me, typeof(object)), expPara).Compile());
                        }

                        break;
                    }
            }

            return this;
        }

        #region IEqualityComparer<T> Members

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(T x, T y)
        {
            var returnValue = true;

            foreach (var func in _getPropertyValueFuncList)
            {
                var xValue = func(x);
                var yValue = func(y);

                if (xValue == null)
                {
                    returnValue = returnValue && yValue == null;
                    continue;
                }

                returnValue = returnValue && xValue.Equals(yValue);
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(T obj)
        {
            var returnValue = 0;

            foreach (var func in _getPropertyValueFuncList)
            {
                var propertyValue = func(obj);

                if (propertyValue == null)
                {
                    returnValue += 0;
                    continue;
                }

                returnValue += propertyValue.GetHashCode();
            }
            return returnValue;
        }

        #endregion
    }
}
