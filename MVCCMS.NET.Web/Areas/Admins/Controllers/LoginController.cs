using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dos.Common;
using MVCCMS.NET.Com;
using MVCCMS.NET.Model;
using MVCCMS.NET.Web.Controllers;

namespace MVCCMS.NET.Web.Areas.Admins.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admins/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Description("登录提交")]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Manager model)
        {
            var jm = new JsonWithUIcallback();
            try
            {
                var bll = new Dal.Manager();
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    jm.message = "请输入用户名和密码！";
                    return Json(jm);
                }
                var md5Pwd = ToolsHelper.Md5(model.Password);
                var manager = bll.QueryToEnetity(p => p.Username == model.Username && p.Password == md5Pwd);
                if (manager == null)
                {
                    jm.message = "账户密码错误！";
                    return Json(jm);
                }
                //写入数据库日志
                var mlog = new ManagerLog
                {
                    UserId = manager.Id ,
                    UserName = manager.Username ,
                    ActionType = "Login" ,
                    AddTime = DateTime.Now ,
                    Remark = "后台管理登录" ,
                    UserIp = RequestHelper.GetIp()
                };
                new Dal.ManagerLog().Insert(mlog);

                //修改登录时间
                manager.LastLoginTime = DateTime.Now;
                bll.Update(manager);

                //写入session
                System.Web.HttpContext.Current.Session.Timeout = 20;
                System.Web.HttpContext.Current.Session[KeyWordsHelper.SessionManager] = manager;


                //写入cookie
                CookieHelper.Set("AdminName" , model.Username , 1200);
                CookieHelper.Set("AdminPwd" , md5Pwd , 1200);

                jm.statusCode = 200;
                jm.message = "登录成功!";
                jm.forward = "/Admins/Panel/";
                jm.closeCurrent = true;
                return Json(jm);
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , model.Username + "登录" , "Login");
                jm.message = "数据异常";
            }
            return Json(jm);
        }


        [Description("注销登录")]
        public ActionResult LoginMin()
        {
            return View();
        }

        [HttpPost]
        [Description("登录提交")]
        [ValidateAntiForgeryToken]
        public ActionResult LoginMin(Manager model)
        {
            var jm = new JsonWithUIcallback();
            try
            {
                var bll = new Dal.Manager();
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    jm.message = "请输入用户名和密码！";
                    return Json(jm);
                }
                var md5Pwd = ToolsHelper.Md5(model.Password);
                var manager = bll.QueryToEnetity(p => p.Username == model.Username && p.Password == md5Pwd);
                if (manager == null)
                {
                    jm.message = "账户密码错误！";
                    return Json(jm);
                }
                //写入数据库日志
                var mlog = new ManagerLog
                {
                    UserId = manager.Id ,
                    UserName = manager.Username ,
                    ActionType = "Login" ,
                    AddTime = DateTime.Now ,
                    Remark = "后台管理登录" ,
                    UserIp = RequestHelper.GetIp()
                };
                new Dal.ManagerLog().Insert(mlog);

                //修改登录时间
                manager.LastLoginTime = DateTime.Now;
                bll.Update(manager);

                //写入session
                System.Web.HttpContext.Current.Session.Timeout = 20;
                System.Web.HttpContext.Current.Session[KeyWordsHelper.SessionManager] = manager;


                //写入cookie
                CookieHelper.Set("AdminName" , model.Username , 1200);
                CookieHelper.Set("AdminPwd" , md5Pwd , 1200);

                jm.statusCode = 200;
                jm.message = "登录成功!";
                //jm.forward = "/Admins/Panel/";
                jm.closeCurrent = true;
                return Json(jm);
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , model.Username + "登录" , "Login");
                jm.message = "数据异常";
            }
            return Json(jm);
        }


        [Description("注销登录")]
        [HttpPost]
        public ActionResult LoginOut()
        {
            
            Session[KeyWordsHelper.SessionManager] = null;

            CookieHelper.Remove("AdminName");
            CookieHelper.Remove("AdminPwd");

            return RedirectToAction("Index");
            
        }

    }
}