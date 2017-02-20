using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCCMS.NET.Com
{
    public class EnumHelper
    {
        /// <summary>
        /// 排序方式
        /// </summary>
        public enum OrderBy
        {
            Asc,
            Desc
        }

        /// <summary>
        /// Nlog日志排序
        /// </summary>
        public enum Nlog
        {
            Error,
            Info,
            Debug
        }

        /// <summary>
        /// 系统导航菜单类别枚举
        /// </summary>
        public enum NavigationEnum
        {
            /// <summary>
            /// 系统后台菜单
            /// </summary>
            System,
            /// <summary>
            /// 会员中心导航
            /// </summary>
            Users,
            /// <summary>
            /// 网站主导航
            /// </summary>
            WebSite
        }

    }
}
