using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dos.Common;
using MVCCMS.NET.Com;

namespace MVCCMS.NET.Filter
{
    [AdminsFilter]
    public class AdminsBase : Controller
    {
        public static bool IsAdminLogin()
        {
            //如果Session为Null
            if (System.Web.HttpContext.Current.Session[KeyWordsHelper.SessionManager] != null)
            {
                return true;
            }
            //检查Cookies
            var adminname = CookieHelper.Get("AdminName");
            var adminpwd = CookieHelper.Get("AdminPwd");
            if (!string.IsNullOrEmpty(adminname) && !string.IsNullOrEmpty(adminpwd))
            {
                var model = new Dal.Manager().QueryToEnetity(p => p.Username == adminname && p.Password == adminpwd);
                if (model == null) return false;

                CookieHelper.Set("AdminName" , model.Username , 1200);
                CookieHelper.Set("AdminPwd" , model.Password , 1200);

                System.Web.HttpContext.Current.Session[KeyWordsHelper.SessionManager] = model;
                System.Web.HttpContext.Current.Session.Timeout = 20;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     获取管理员信息
        /// </summary>
        /// <returns></returns>
        protected static Model.Manager GetAdminsInfo()
        {
            if (IsAdminLogin())
            {
                var manager = System.Web.HttpContext.Current.Session[KeyWordsHelper.SessionManager] as Model.Manager;

                return manager;
            }
            return null;
        }


        public static bool GetRoleValuesById(int id , string controllerName , string actionName)
        {
            return new Dal.ManagerRoleValue().GetRoleValues().Any(p => p.RoleId == id && p.ControllerName == controllerName && p.ActionName == actionName);
        }

    }
}
