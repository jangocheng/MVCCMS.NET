﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//     Website: http://ITdos.com/Dos/ORM/Index.html
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using Dos.ORM;

namespace MVCCMS.NET.Model
{
    /// <summary>
    /// 实体类ManagerLog。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Table("ManagerLog")]
    [Serializable]
    public partial class ManagerLog : Entity
    {
        #region Model
        private int _Id;
        private int _UserId;
        private string _UserName;
        private string _ActionType;
        private string _Remark;
        private string _UserIp;
        private DateTime _AddTime;
        private string _ControllerName;
        private string _ActionName;

        /// <summary>
        /// 序列
        /// </summary>
        [DisplayName("序列")]
        public int Id
        {
            get { return _Id; }
            set
            {
                this.OnPropertyValueChange("Id");
                this._Id = value;
            }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("用户ID")]
        public int UserId
        {
            get { return _UserId; }
            set
            {
                this.OnPropertyValueChange("UserId");
                this._UserId = value;
            }
        }
        /// <summary>
        /// 操作员
        /// </summary>
        [DisplayName("操作员")]
        public string UserName
        {
            get { return _UserName; }
            set
            {
                this.OnPropertyValueChange("UserName");
                this._UserName = value;
            }
        }
        /// <summary>
        /// 操作类型
        /// </summary>
        [DisplayName("操作类型")]
        public string ActionType
        {
            get { return _ActionType; }
            set
            {
                this.OnPropertyValueChange("ActionType");
                this._ActionType = value;
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Remark
        {
            get { return _Remark; }
            set
            {
                this.OnPropertyValueChange("Remark");
                this._Remark = value;
            }
        }
        /// <summary>
        /// 用户IP
        /// </summary>
        [DisplayName("用户IP")]
        public string UserIp
        {
            get { return _UserIp; }
            set
            {
                this.OnPropertyValueChange("UserIp");
                this._UserIp = value;
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime AddTime
        {
            get { return _AddTime; }
            set
            {
                this.OnPropertyValueChange("AddTime");
                this._AddTime = value;
            }
        }
        /// <summary>
        /// 控制器名
        /// </summary>
        [DisplayName("控制器名")]
        public string ControllerName
        {
            get { return _ControllerName; }
            set
            {
                this.OnPropertyValueChange("ControllerName");
                this._ControllerName = value;
            }
        }
        /// <summary>
        /// 操作方法
        /// </summary>
        [DisplayName("操作方法")]
        public string ActionName
        {
            get { return _ActionName; }
            set
            {
                this.OnPropertyValueChange("ActionName");
                this._ActionName = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 获取实体中的主键列
        /// </summary>
        public override Field[] GetPrimaryKeyFields()
        {
            return new Field[] {
                _.Id,
            };
        }
        /// <summary>
        /// 获取实体中的标识列
        /// </summary>
        public override Field GetIdentityField()
        {
            return _.Id;
        }
        /// <summary>
        /// 获取列信息
        /// </summary>
        public override Field[] GetFields()
        {
            return new Field[] {
                _.Id,
                _.UserId,
                _.UserName,
                _.ActionType,
                _.Remark,
                _.UserIp,
                _.AddTime,
                _.ControllerName,
                _.ActionName,
            };
        }
        /// <summary>
        /// 获取值信息
        /// </summary>
        public override object[] GetValues()
        {
            return new object[] {
                this._Id,
                this._UserId,
                this._UserName,
                this._ActionType,
                this._Remark,
                this._UserIp,
                this._AddTime,
                this._ControllerName,
                this._ActionName,
            };
        }
        /// <summary>
        /// 是否是v1.10.5.6及以上版本实体。
        /// </summary>
        /// <returns></returns>
        public override bool V1_10_5_6_Plus()
        {
            return true;
        }
        #endregion

        #region _Field
        /// <summary>
        /// 字段信息
        /// </summary>
        public class _
        {
            /// <summary>
            /// * 
            /// </summary>
            public readonly static Field All = new Field("*" , "ManagerLog");
            /// <summary>
			/// 序列
			/// </summary>
			public readonly static Field Id = new Field("Id" , "ManagerLog" , "序列");
            /// <summary>
			/// 用户ID
			/// </summary>
			public readonly static Field UserId = new Field("UserId" , "ManagerLog" , "用户ID");
            /// <summary>
			/// 操作员
			/// </summary>
			public readonly static Field UserName = new Field("UserName" , "ManagerLog" , "操作员");
            /// <summary>
			/// 操作类型
			/// </summary>
			public readonly static Field ActionType = new Field("ActionType" , "ManagerLog" , "操作类型");
            /// <summary>
			/// 备注
			/// </summary>
			public readonly static Field Remark = new Field("Remark" , "ManagerLog" , "备注");
            /// <summary>
			/// 用户IP
			/// </summary>
			public readonly static Field UserIp = new Field("UserIp" , "ManagerLog" , "用户IP");
            /// <summary>
			/// 创建时间
			/// </summary>
			public readonly static Field AddTime = new Field("AddTime" , "ManagerLog" , "创建时间");
            /// <summary>
			/// 控制器名
			/// </summary>
			public readonly static Field ControllerName = new Field("ControllerName" , "ManagerLog" , "控制器名");
            /// <summary>
			/// 操作方法
			/// </summary>
			public readonly static Field ActionName = new Field("ActionName" , "ManagerLog" , "操作方法");
        }
        #endregion
    }
}