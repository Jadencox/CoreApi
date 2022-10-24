using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Admin.BusinessEntity.Tables;
using Microsoft.EntityFrameworkCore;


namespace Core.Admin.DataAccess
{
    public class UserManageDbContext : DbContext
    {
        public UserManageDbContext(DbContextOptions<UserManageDbContext> options) : base(options)
        {
        }

        public virtual DbSet<SysUser> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SysUser>(e => { e.HasKey(t => new { t.UserId }); });
            //builder.Entity<SysRoleAuthority>(e => { e.HasKey(t => new { t.AuthorityId, t.RoleId }); });
            //builder.Entity<NursingScheduleDict>().HasKey(t => new { t.WardCode, t.SerialNo });
            //builder.Entity<SysUserRole>().HasKey(t => new { t.UserId, t.RoleId });
            //builder.Entity<SysConfig>().HasKey(t => new { t.WardCode, t.Code });
        }
    }
}
