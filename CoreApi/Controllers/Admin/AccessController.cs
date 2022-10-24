using Core.Utility.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers.Admin
{
    public class AccessController : UserManageControllerBase
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="username">登录名</param>
        /// <param name="password">密码</param>
        /// <returns>Token：令牌内容; ExpiryTime: 过期时间; Expiration: 保持时间（天）</returns>
        /// <response code="200">成功，正常返回令牌</response>
        /// <response code="401">登录失败</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ResultObj<string>> Login([FromForm] string username, [FromForm] string password) => await Task.FromResult(Result("1", ResultCode.Success, string.Empty));
    }
}
