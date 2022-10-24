using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility.Common
{
    public class ResultObj<T>
    {
        public ResultCode RetCode { get; set; }

        public string RetMsg { get; set; }
        public T RetObj { get; set; }

        public static ResultObj<T> GetResult(T retObj, ResultCode retCode, string retMsg)
        {
            return new ResultObj<T>() { RetCode = retCode, RetMsg = retMsg, RetObj = retObj == null ? default(T) : retObj };
        }


    }

    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = 20,

        /// <summary>
        /// 参数为空
        /// </summary>
        ParamsNull = 30,

        /// <summary>
        /// 
        /// </summary>
        NoAccess = 100,

        /// <summary>
        /// Token失效
        /// </summary>
        ErrorToken = 101,

        /// <summary>
        /// Session失效
        /// </summary>
        ErrorSession = 102,

        /// <summary>
        /// 没有权限
        /// </summary>
        ErrorAuth = 110,

        /// <summary>
        /// 报错（不弹框）
        /// </summary>
        Exception = 200,

        /// <summary>
        /// IO报错（不弹框）
        /// </summary>
        IoException = 300,

        /// <summary>
        /// 业务报错
        /// </summary>
        BizException = 400,

        /// <summary>
        /// Authentication failed
        /// </summary>
        AuthenticationFailed = 600

    }
}
