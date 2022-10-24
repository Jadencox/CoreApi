using Core.Admin.BusinessEntity.Tables;
using Core.Admin.DataAccess;
using Core.Caching.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public UserManageDbContext _DbContext;

        public IServiceProvider _ServiceProvider;

        public TestController(UserManageDbContext DbContext, IServiceProvider ServiceProvider) 
        {
            _DbContext = DbContext;
            _ServiceProvider = ServiceProvider;
        }

        [HttpGet]
        [Route("GetUsers")]
        public List<SysUser> GetUsers() 
        { 
            var query = _DbContext.User.ToList();
            return query;
        }

        [HttpGet]
        [Route("RedisTest")]
        public string RedisTest() 
        {
            var RedisCache = _ServiceProvider.GetService<IRedisCache>();
            //RedisCache.Add("key1", "测试key");
            //var a = RedisCache.Get("key1");
            return "OK";
        }
    }
}
