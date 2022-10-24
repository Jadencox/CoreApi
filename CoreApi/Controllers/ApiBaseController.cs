using Core.Utility.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [ApiController]
    [EnableCors("vueDomain")]
    public class ApiBaseController : ControllerBase
    {

        /// <summary>
        /// 封装返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">返回值</param>
        /// <returns></returns>
        protected ResultObj<T> Result<T>(T obj)
        {
            return Result(obj, ResultCode.Success, string.Empty);
        }

        /// <summary>
        /// 封装返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">返回值</param>
        /// <param name="retCode">返回提示类型</param>
        /// <param name="retMsg">返回提示内容</param>
        /// <returns></returns>
        protected ResultObj<T> Result<T>(T obj, ResultCode retCode, string retMsg)
        {
            if (string.IsNullOrEmpty(retMsg))
            {
                switch (retCode)
                {
                    case ResultCode.Success:
                        retMsg = "操作成功！";
                        break;
                    case ResultCode.ParamsNull:
                        retMsg = "缺少参数！";
                        break;
                }
            }
            return ResultObj<T>.GetResult(obj, retCode, retMsg);
        }
    }
}
