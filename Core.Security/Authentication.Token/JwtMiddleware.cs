using Core.Authentication.Token;
using Core.Authorization.Core;
using Core.Security.Authentication.Token.Configuration;
using Core.Utility.Common;
using Jwt.Identity.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Authentication.Token
{
    public class JwtMiddleware<TUser, TRole>
        where TUser : JwtIdentityUser<string>, new()
        where TRole : JwtIdentityRole<string>, new()
    {
        private readonly RequestDelegate _next;
        private JwtProviderOption _JwtProviderOption;

        public JwtMiddleware(RequestDelegate next,
            IOptions<JwtProviderOption> options)
        {
            _next = next;
            _JwtProviderOption = options.Value;

        }
        public async Task Invoke(HttpContext httpContext,
            JwtUserStoreBase<TUser, TRole, string> userStore,
            //AuthorizationActionManager actionManager,
            IJwtIdentityTokenManager<JwtIdentityUser, JwtIdentityRole> jwtIdentityTokenManager)
        {
            Guard.ArgumentNotNull(httpContext, "httpContext");

            if (!httpContext.Request.Path.Equals(_JwtProviderOption.Path, StringComparison.Ordinal))
                await _next(httpContext);
            else if (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType)
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("请用FORM提交!");
            }
            else
            {
                var username = httpContext.Request.Form["username"];
                var password = httpContext.Request.Form["password"];
                var isbase64 = httpContext.Request.Form["isbase64"];
                try
                {
                    var result = new ResultObj<string>();
                    password = CovertPasswordByBase64(password, isbase64);
                    var maxErrorTimes = _JwtProviderOption.MaxErrorTimes >= 1 ? _JwtProviderOption.MaxErrorTimes : 0;
                    var isValid = await userStore.LoginWithUserNamePassword(httpContext, username, password, maxErrorTimes);
                    if (isValid.Succeeded)
                    {
                        //var tokenResult = GenerateToken(username, userStore, actionManager);
                        var tokenResult = jwtIdentityTokenManager.GenerateToken(username);
                        var tokenObj = JsonConvert.SerializeObject(tokenResult);
                        result = new ResultObj<string> { RetCode = ResultCode.Success, RetMsg = isValid.ToString(), RetObj = tokenObj };
                    }
                    else
                    {
                        //httpContext.Response.StatusCode = 401;
                        result = new ResultObj<string> { RetCode = ResultCode.AuthenticationFailed, RetMsg = isValid.ToString() };
                    }

                    var resultJson = JsonConvert.SerializeObject(result);
                    httpContext.Response.Headers["Access-Control-Allow-Methods"] = "POST, GET, OPTIONS, DELETE";
                    httpContext.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Accept, Origin, Authorization, Referer, User-Agent";
                    httpContext.Response.Headers["Charset"] = "UTF-8";
                    await httpContext.Response.WriteAsync(resultJson);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //await _next(httpContext);
        }

        private string CovertPasswordByBase64(StringValues password, StringValues isbase64)
        {
            if (string.IsNullOrEmpty(isbase64))
                return password;
            if (!bool.TryParse(isbase64, out var flag))
                return password;
            if (!flag)
                return password;
            var bytes = Convert.FromBase64String(password);
            return UTF8Encoding.UTF8.GetString(bytes);
        }
    }
}
