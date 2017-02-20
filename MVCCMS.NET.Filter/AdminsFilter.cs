using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Dos.Common;
using MVCCMS.NET.Com;
using MVCCMS.NET.Model;

namespace MVCCMS.NET.Filter
{
    public class AdminsFilter : AuthorizeAttribute
    {
        /// <summary>
        ///     重写OnAuthorization
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //判断当前方法是否跳过过滤器
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(NoFilter) , true)
                                    ||
                                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                                        typeof(NoFilter) , true);
            if (skipAuthorization)
            {
                return;
            }

            //获取请求路径
            var rData = filterContext.HttpContext.Request.RequestContext.RouteData;
            var controller = rData.Values["controller"].ToString();
            var action = rData.Values["action"].ToString();
            var bllogin = AdminsBase.IsAdminLogin();
            //判断管理员是否登录
            if (bllogin)
            {
                var manager = filterContext.HttpContext.Session[KeyWordsHelper.SessionManager] as Model.Manager;
                if (manager != null && manager.RoleId > 1)
                {
                    var bl = AdminsBase.GetRoleValuesById(manager.RoleId , controller , action);
                    if (!bl)
                    {
                        filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Panel" , action = "NohavaRole" , area = "Admins" }));
                    }
                }
            }else if (controller== "Panel" && action== "Index")
            {
                filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login" , action = "Index" , area = "Admins" }));
            }
            else
            {
                filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Panel" , action = "LoginTimeout" , area = "Admins" }));
            }
            //通过验证
        }
    }
}
