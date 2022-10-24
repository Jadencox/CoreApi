using Jwt.Identity.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Jwt.Identity.EntityFrameworkCore
{
    public class JwtIdentityDbContext<TUser, TRole, TKey> : DbContext
        where TUser : JwtIdentityUser<TKey>
        where TRole : JwtIdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public JwtIdentityDbContext(DbContextOptions<JwtIdentityDbContext<TUser, TRole, TKey>> options) : base(options) { }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        protected JwtIdentityDbContext() { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TUser>(entity =>
            {
                entity.ToTable("MED_SYS_USER");
                entity.Property(e => e.Id).IsRequired().HasColumnType("varchar(36)").HasColumnName("USER_ID");
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50).HasColumnName("LOGIN_NAME");
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(50).HasColumnName("LOGIN_PWD");
                entity.Property(e => e.VersionNo).IsRequired().HasColumnType("int").HasColumnName("VERSION_NO");
                entity.Property(e => e.LastLoginTime).HasColumnName("LAST_LOGIN_TIME");
                entity.Property(e => e.NormalizedUserName).HasColumnName("USER_NAME");
                entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
                entity.Property(e => e.CreatedBy).HasColumnName("CREATOR");
                entity.Property(e => e.CreatedTime).HasColumnName("CREATE_TIME");
                entity.Property(e => e.LastUpdatedBy).HasColumnName("MODIFIER");
                entity.Property(e => e.LastUpdatedTime).HasColumnName("MODIFY_TIME");
            });

            builder.Entity<TRole>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasIndex(r => r.Name).IsUnique();
                entity.ToTable("MED_SYS_ROLE");
                entity.Property(r => r.Id).HasColumnName("ROLE_ID");
                entity.Property(r => r.Name).HasColumnName("ROLE_NAME");
                entity.Property(u => u.DomainId).HasColumnName("DOMAIN_ID");
                entity.Property(u => u.Description).HasColumnName("DESCRIPTION");
                entity.Property(u => u.Status).HasColumnName("STATUS");
                entity.Property(e => e.VersionNo).IsRequired().HasColumnType("int").HasColumnName("VERSION_NO");
                entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
                entity.Property(e => e.CreatedBy).HasColumnName("CREATOR");
                entity.Property(e => e.CreatedTime).HasColumnName("CREATE_TIME");
                entity.Property(e => e.LastUpdatedBy).HasColumnName("MODIFIER");
                entity.Property(e => e.LastUpdatedTime).HasColumnName("MODIFY_TIME");
            });

            builder.Entity<JwtIdentityUserRole<TKey>>(entity =>
            {
                entity.HasKey(r => new { r.UserId, r.RoleId });
                entity.ToTable("MED_SYS_USER_ROLE");
                entity.Property(e => e.UserId).HasColumnName("USER_ID");
                entity.Property(e => e.RoleId).HasColumnName("ROLE_ID");
                entity.Property(e => e.VersionNo).IsRequired().HasColumnType("int").HasColumnName("VERSION_NO");
                entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
                entity.Property(e => e.CreatedBy).HasColumnName("CREATOR");
                entity.Property(e => e.CreatedTime).HasColumnName("CREATE_TIME");
                entity.Property(e => e.LastUpdatedBy).HasColumnName("MODIFIER");
                entity.Property(e => e.LastUpdatedTime).HasColumnName("MODIFY_TIME");
            });

            base.OnModelCreating(builder);
        }
        public DbSet<TUser> Users { get; set; }

        public DbSet<JwtIdentityUserRole<TKey>> UserRole { get; set; }

        //public virtual DbSet<SysUserAudit> SysUserAudit { get; set; }

    }
}
