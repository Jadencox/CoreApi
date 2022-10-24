using Core.Authentication.Token;
using Core.Security.Authentication.Token.Configuration;
using CoreApi.Extensions;
using Jwt.Identity.Core;
using Jwt.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var Configuration = builder.Configuration;

#region Cache

builder.Services.AddDistributedMemoryCache();
//services.AddMemoryCache().AddMemoryCacheFactory().AddDoCareCacheManagement();

//services.AddDistributedRedisCache(option =>
//{
//    option.Configuration = "localhost";
//    option.InstanceName = "redisDb";
//}).AddDistributedCacheFactory().AddDoCareCacheManagement();

builder.Services.AddRedisCache();

#endregion end cache

#region Jwt Identity

builder.Services.Configure<JwtProviderOption>(Configuration.GetSection("JwtProviderOption"))
    .AddAuthenticationJwt();

builder.Services.AddDbContext<JwtIdentityDbContext<JwtIdentityUser, JwtIdentityRole, string>>(options =>
    options.UseOracle11(Configuration));

builder.Services.AddJwtIdentity<JwtIdentityUser, JwtIdentityRole, JwtIdentityDbContext<JwtIdentityUser, JwtIdentityRole, string>>();

#region Add Authorization

//builder.Services.AddDbContext<AuthorizationActionDbContext>((serviceProvider, options) =>
//    options.UseOracle11(Configuration, serviceProvider));

//builder.Services.AddAuthorization(option => option.AddAuthorizationActionBasedPolicies(services))
//    .AddAuthorizationAction(buildAction => buildAction.AddEntityFrameworkStore());

//builder.Services.AddAuthorizationAction();


#endregion

#endregion

#region Add Cors for cross site access

builder.Services.AddCors(options => options.AddPolicy("vueDomain", builder => builder.AllowAnyOrigin().AllowAnyMethod().WithHeaders("Accept", "Content-Type", "Origin", "Authorization", "Referer", "User-Agent").AllowAnyHeader()));

#endregion

#region Add DbContext

builder.Services.AddDbContexts(Configuration);

#endregion

builder.Services.AddSession();


var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("vueDomain");

app.UseAuthorization();

app.UseMiddlewareJwt<JwtIdentityUser, JwtIdentityRole>();

app.MapControllers();

app.Run();

