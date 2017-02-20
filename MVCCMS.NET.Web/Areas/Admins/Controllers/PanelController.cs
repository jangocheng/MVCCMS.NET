using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dos.Common;
using MVCCMS.NET.Com;
using MVCCMS.NET.Filter;
using MVCCMS.NET.Model;

namespace MVCCMS.NET.Web.Areas.Admins.Controllers
{
    [Description("控制面板")]
    public class PanelController : AdminsBase
    {
        // GET: Admins/Panel
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [Description("面板")]
        public ActionResult Index()
        {
            ViewBag.Manager = GetAdminsInfo();

            return View();
        }

        [Description("我的主页")]
        public ActionResult Layout()
        {
            return View();
        }

        [Description("修改密码")]
        public ActionResult ChangePwd()
        {
            return View();
        }

        [HttpPost]
        [Description("修改密码")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePwd(string password)
        {
            var jm = new JsonWithUIcallback();
            var manager = GetAdminsInfo();
            manager.Password = ToolsHelper.Md5(password);
            var bl = new Dal.Manager().Update(manager) > 0;
            jm.statusCode = bl ? 200 : 300;
            jm.message = "修改成功";
            jm.closeCurrent = bl;

            //重置账户密码
            Session[KeyWordsHelper.SessionManager] = null;
            CookieHelper.Remove("AdminName");
            CookieHelper.Remove("AdminPwd");

            return Json(jm);
        }


        [NoFilter]
        [Description("无权限提醒")]
        public ActionResult NohavaRole()
        {
            var jm = new JsonWithUIcallback
            {
                statusCode = 300 ,
                message = "您无此功能权限"
            };

            return Json(jm , JsonRequestBehavior.AllowGet);
        }
        [NoFilter]
        [Description("无权限提醒")]
        public ActionResult LoginTimeout()
        {
            var jm = new JsonWithUIcallback
            {
                statusCode = 301 ,
                message = "后台登录会话超时!"
            };

            return Json(jm , JsonRequestBehavior.AllowGet);
        }
    }
}