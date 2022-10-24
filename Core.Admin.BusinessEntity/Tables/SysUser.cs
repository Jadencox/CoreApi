using Core.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Admin.BusinessEntity.Tables
{
    [Table("MED_SYS_USER")]
    public class SysUser : CommonFieldsBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Column("USER_ID", TypeName = "NVARCHAR2(36)")]
        [Description("用户ID")]
        [Key]
        public string UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Column("USER_NAME", TypeName = "NVARCHAR2(36)")]
        [Description("用户姓名")]
        public string UserName { get; set; }

        /// <summary>
        /// 雇员ID
        /// </summary>
        [Column("EMPLOYEE_ID", TypeName = "NVARCHAR2(36)")]
        [Description("雇员ID")]
        public string? EmployeeId { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        [Column("LOGIN_NAME", TypeName = "NVARCHAR2(36)")]
        [Description("登录名称")]
        public string LoginName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Column("LOGIN_PWD", TypeName = "NVARCHAR2(36)")]
        [Description("登录密码")]
        public string LoginPwd { get; set; }

        /// <summary>
        /// 1*:护士类；2*医生类
        /// </summary>
        [Column("USER_TYPE", TypeName = "NVARCHAR2(10)")]
        [Description("1*:护士类；2*医生类")]
        public string UserType { get; set; }

        /// <summary>
        /// 启用状态(USER_STATUS_TYPE类型),对应MED_GENERAL_DICT表中"USER_STATUS_TYPE"的TYPE，CODE为3表示删除
        /// </summary>
        [Column("STATUS", TypeName = "NVARCHAR2(10)")]
        [Description("启用状态(USER_STATUS_TYPE类型)")]
        public string Status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("MEMO", TypeName = "NVARCHAR2(100)")]
        [Description("备注")]
        public string? Memo { get; set; }

        /// <summary>
        /// 组织代码
        /// </summary>
        [Column("WARD_CODE", TypeName = "NVARCHAR2(1000)")]
        [Description("组织代码")]
        public string WardCode { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Column("PHOTO", TypeName = "NVARCHAR2(100)")]
        [Description("头像")]
        public string? Photo { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Column("LAST_LOGIN_TIME", TypeName = "DATE")]
        [Description("最后登录时间")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 职级(USER_RANK_TYPE)
        /// </summary>
        [Column("RANK", TypeName = "NVARCHAR2(30)")]
        [Description("职级(USER_RANK_TYPE)")]
        public string? Rank { get; set; }

        /// <summary>
        /// 岗位(USER_JOB_TYPE)
        /// </summary>
        [Column("JOB", TypeName = "NVARCHAR2(30)")]
        [Description("岗位(USER_JOB_TYPE)")]
        public string? Job { get; set; }

        /// <summary>
        /// 带教老师
        /// </summary>
        [Column("TEACHER", TypeName = "NVARCHAR2(30)")]
        [Description("带教老师")]
        public string? Teacher { get; set; }

        /// <summary>
        /// 工作时长(上系统前)，单位：小时
        /// </summary>
        [Column("WORK_TIMES")]
        [Description("工作时长(上系统前)，单位：小时")]
        public decimal? WorkTimes { get; set; }

        /// <summary>
        /// 休息时长(上系统前),单位：天
        /// </summary>
        [Column("REST_TIMES")]
        [Description("休息时长(上系统前),单位：天")]
        public decimal? RestTimes { get; set; }

        /// <summary>
        /// 保留字段1
        /// </summary>
        [Column("RESERVED01", TypeName = "NVARCHAR2(200)")]
        [Description("保留字段1")]
        public string? Reserved01 { get; set; }

        /// <summary>
        /// 保留字段2
        /// </summary>
        [Column("RESERVED02", TypeName = "NVARCHAR2(200)")]
        [Description("保留字段2")]
        public string? Reserved02 { get; set; }
    }
}
