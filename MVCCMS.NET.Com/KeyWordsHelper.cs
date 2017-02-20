using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCCMS.NET.Com
{
    public class KeyWordsHelper
    {

        //增加改查等数据返回jsommodel.msg需要的提示信息===============================================
        /// <summary>
        /// 数据删除成功
        /// </summary>
        public const string DeleteSuccess = "数据删除成功";
        /// <summary>
        /// 数据删除失败
        /// </summary>
        public const string DeleteFailure = "数据删除失败";
        /// <summary>
        /// 系统禁止删除此数据
        /// </summary>
        public const string DeleteProhibit = "系统禁止删除此数据";
        /// <summary>
        /// 此数据含有子类信息，禁止删除
        /// </summary>
        public const string DeleteIsHaveChildren = "此数据含有子类信息，禁止删除";
        /// <summary>
        /// 数据处理异常
        /// </summary>
        public const string DataHandleEx = "数据处理异常";
        /// <summary>
        /// 数据添加成功
        /// </summary>
        public const string CreateSuccess = "数据添加成功";
        /// <summary>
        /// 数据添加失败
        /// </summary>
        public const string CreateFailure = "数据添加失败";
        /// <summary>
        /// 系统禁止添加数据
        /// </summary>
        public const string CreateProhibit = "系统禁止添加数据";
        /// <summary>
        /// 数据编辑成功
        /// </summary>
        public const string EditSuccess = "数据编辑成功";
        /// <summary>
        /// 数据编辑失败
        /// </summary>
        public const string EditFailure = "数据编辑失败";
        /// <summary>
        /// 系统禁止编辑此数据
        /// </summary>
        public const string EditProhibit = "系统禁止编辑此数据";
        /// <summary>
        /// 数据已存在
        /// </summary>
        public const string DataIsHave = "数据已存在";
        /// <summary>
        /// 数据不存在
        /// </summary>
        public const string DataisNo = "数据不存在";
        /// <summary>
        /// 请提交必要的参数
        /// </summary>
        public const string DataParameterError = "请提交必要的参数";
        /// <summary>
        /// 数据处理成功
        /// </summary>
        public const string InsertOk = "数据处理成功！";
        /// <summary>
        /// 数据处理失败
        /// </summary>
        public const string InsertBad = "数据处理失败！";

        /// <summary>
        /// 缓存管理员登录信息
        /// </summary>
        public const string SessionManager = "SessionManager";


        //缓存数据
        /// <summary>
        /// 缓存已经排序后台导航
        /// </summary>
        public const string CacheFindNavSortList = "CacheFindNavSortList";
        /// <summary>
        /// 缓存未排序后台导航
        /// </summary>
        public const string CacheFindNavNoSortList = "CacheFindNavNoSortList";
        /// <summary>
        /// 缓存角色列表
        /// </summary>
        public const string CacheManagerRoleList = "CacheManagerRoleList";
        /// <summary>
        /// 缓存角色详细信息
        /// </summary>
        public const string CacheRoleValues = "CacheRoleValues";
        




    }
}
