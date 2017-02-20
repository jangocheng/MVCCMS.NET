
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVCCMS.NET.Com;
using MVCCMS.NET.Dal;
using MVCCMS.NET.Filter;
using MVCCMS.NET.Model;

namespace MVCCMS.NET.Web.Areas.Admins.Controllers
{
    public class AdminsToolsController : AdminsBase
    {
        #region 获取后台登录后导航信by树菜单
        //[HttpPost]
        public static string GetAdminsNavListbyTree()
        {
            //var manager = GetAdminsInfo();
            //if (manager == null)
            //{
            //    return Content("");
            //}
            var managerRole = new Dal.ManagerRole().QueryToEnetity(p => p.Id == 1);
            if (managerRole == null)
            {
                return "";
            }
            var listnav = new Dal.Navigation().QueryToList(p => p.NavType == EnumHelper.NavigationEnum.System.ToString() , p => p.SortId);
            var roleList = new Dal.NavigationRoleValue().QueryToList(p => p.RoleId == managerRole.Id);

            var sb = new StringBuilder();
            var htmlStr = GetNavlistStrbyTree(sb , listnav , 0 , managerRole.Id , roleList);
            return htmlStr;
        }

        private static string GetNavlistStrbyTree(StringBuilder sb , List<Model.Navigation> oldData , int parentId , int roleType ,
            List<Model.NavigationRoleValue> ls)
        {
            var model = oldData.Where(p => p.ParentId == parentId).ToList();
            var count = 0;
            foreach (var item in model)
            {
                count++;
                //检查是否显示在界面上====================
                var isActionPass = item.IsLock;
                //检查管理员权限==========================
                if (isActionPass && roleType > 1)
                {
                    var bl = ls.Any(p => p.ActionName == item.Name && p.RoleId == roleType);
                    if (!bl)
                    {
                        isActionPass = false;
                    }
                }
                //如果没有该权限则不显示
                if (!isActionPass)
                {
                    continue;
                }
                //如果是顶级导航
                if (parentId == 0)
                {
                    if (count == 1)
                    {
                        sb.Append("<li class=\"active\">\n");
                    }
                    else
                    {
                        sb.Append("<li>\n");
                    }
                    sb.Append("<a href=\"javascript:;\" data-toggle=\"slidebar\">");
                    if (!string.IsNullOrEmpty(item.IconUrl))
                    {
                        sb.Append("<i class=\"" + item.IconUrl + "\"></i> ");
                    }
                    else
                    {
                        sb.Append("<i class=\"fa fa fa-folder\"></i> ");
                    }

                    sb.Append(item.Title + "</a>\n");
                    sb.Append("<div class=\"items hide\" data-noinit=\"true\">\n");
                    sb.Append("<ul id=\"bjui-hnav-tree" + count + "\" class=\"ztree ztree_main\" data-toggle=\"ztree\" data-on-click=\"MainMenuClick\" data-expand-all=\"true\" ");
                    if (!string.IsNullOrEmpty(item.IconUrl))
                    {
                        sb.Append(" data-faicon=\"" + item.IconUrl + "\">\n");
                    }
                    else
                    {
                        sb.Append(" >\n");
                    }
                    //调用自身迭代
                    GetNavlistStrbyTree(sb , oldData , item.Id , roleType , ls);
                    sb.Append("</ul>\n");
                    sb.Append("</div>\n");
                    sb.Append("</li>\n");
                }
                else //下级导航
                {
                    sb.Append("<li data-id=\"" + item.Id + "\" data-pid=\"" + item.ParentId + "\" ");
                    if (!string.IsNullOrEmpty(item.LinkUrl))
                    {
                        sb.Append(" data-url=\"" + item.LinkUrl + "\" ");
                        sb.Append(" data-tabid=\"" + item.Name + "\" ");
                        if (!string.IsNullOrEmpty(item.IconUrl))
                        {
                            sb.Append("  data-faicon=\"" + item.IconUrl + "\" ");
                        }
                        else
                        {
                            sb.Append("  data-faicon=\"fa fa-caret-right\" ");
                        }
                    }
                    else
                    {
                        sb.Append(" data-faicon-close=\"fa fa-folder\" ");
                        sb.Append(" data-faicon=\"fa fa-folder-open\" ");
                    }

                    sb.Append(">" + item.Title + "</li>\n");
                    //调用自身迭代
                    GetNavlistStrbyTree(sb , oldData , item.Id , roleType , ls);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 获取后台登录后导航信息by列表
        public static string GetAdminsNavListbyList()
        {
            //var manager = GetAdminsInfo();
            //if (manager == null)
            //{
            //    return Content("");
            //}
            var managerRole = new Dal.ManagerRole().QueryToEnetity(p => p.Id == 1);
            if (managerRole == null)
            {
                return "";
            }
            var listnav = new Dal.Navigation().QueryToList(p => p.NavType == EnumHelper.NavigationEnum.System.ToString() , p => p.SortId);
            var roleList = new Dal.NavigationRoleValue().QueryToList(p => p.RoleId == managerRole.Id);

            var sb = new StringBuilder();
            var htmlStr = GetNavlistStrbyList(sb , listnav , 0 , managerRole.Id , roleList);
            return htmlStr;
        }

        private static string GetNavlistStrbyList(StringBuilder sb , List<Model.Navigation> oldData , int parentId , int roleType ,
            List<Model.NavigationRoleValue> ls)
        {
            var model = oldData.Where(p => p.ParentId == parentId).ToList();
            var count = 0;
            foreach (var item in model)
            {
                count++;
                //检查是否显示在界面上====================
                var isActionPass = item.IsLock;
                //检查管理员权限==========================
                if (isActionPass && roleType > 1)
                {
                    var bl = ls.Any(p => p.ActionName == item.Name && p.RoleId == roleType);
                    if (!bl)
                    {
                        isActionPass = false;
                    }
                }
                //如果没有该权限则不显示
                if (!isActionPass)
                {
                    continue;
                }
                //如果是顶级导航
                if (parentId == 0)
                {
                    if (count == 1)
                    {
                        sb.Append("<li class=\"active\">\n");
                    }
                    else
                    {
                        sb.Append("<li>\n");
                    }
                    sb.Append("<a href=\"javascript:;\" data-toggle=\"slidebar\">");
                    if (!string.IsNullOrEmpty(item.IconUrl))
                    {
                        sb.Append("<i class=\"" + item.IconUrl + "\"></i> ");
                    }
                    else
                    {
                        sb.Append("<i class=\"fa fa-check-square-o\"></i> ");
                    }

                    sb.Append(item.Title + "</a>\n");
                    sb.Append("<div class=\"items hide\" data-noinit=\"true\">\n");
                    //调用自身迭代
                    GetNavlistStrbyList(sb , oldData , item.Id , roleType , ls);
                    sb.Append("</div>\n");
                    sb.Append("</li>\n");
                }
                else if (oldData.Any(p => p.ParentId == item.Id))
                {
                    sb.Append("<ul class=\"menu-items\" data-faicon=\"table\"  data-tit=\"" + item.Title + "\">");
                    //调用自身迭代
                    GetNavlistStrbyList(sb , oldData , item.Id , roleType , ls);
                    sb.Append("</ul>");
                }
                else if (item.ClassLayer == 2 && oldData.All(p => p.ParentId != item.Id))
                {
                    sb.Append("<ul class=\"menu-items\" data-faicon=\"table\">");
                    //调用自身迭代
                    sb.Append("<li><a href=\"" + item.LinkUrl + "\" data-options=\"{id:'" + item.Name + "', ");
                    if (!string.IsNullOrEmpty(item.IconUrl))
                    {
                        sb.Append("  faicon:'" + item.IconUrl + "'}");
                    }
                    else
                    {
                        sb.Append("  faicon:'caret-right'}");
                    }
                    sb.Append(" \">" + item.Title + "</a></li>");
                    sb.Append("</ul>");
                }
                else //下级导航
                {
                    sb.Append("<li><a href=\"" + item.LinkUrl + "\" data-options=\"{id:'" + item.Name + "', ");
                    if (!string.IsNullOrEmpty(item.IconUrl))
                    {
                        sb.Append("  faicon:'" + item.IconUrl + "'}");
                    }
                    else
                    {
                        sb.Append("  faicon:'caret-right'}");
                    }
                    sb.Append(" \">" + item.Title + "</a></li>");
                    GetNavlistStrbyList(sb , oldData , item.Id , roleType , ls);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 获取管理员角色对应的名称
        public static string GetManagerRoleName(int id)
        {
            var name = new Dal.ManagerRole().FindList().Find(p => p.Id == id);
            if (name != null)
            {
                return name.RoleName;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 判断后台用户名是否存在
        /// <summary>
        /// 判断后台用户名是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerifyUserName()
        {
            var ok = new JsonWithNiceValidaorOk();
            var error = new JsonWithNiceValidaorError();
            var username = RequestHelper.GetFormString("Username");
            if (string.IsNullOrEmpty(username))
            {
                error.error = "请填写用户名";
                return Json(error);
            }
            var bll = new Dal.Manager().Exists(p => p.Username == username);
            if (bll)
            {
                error.error = "用户名已存在，请重新输入";
                return Json(error);
            }
            return Json(ok);
        }

        #endregion

        #region 判断导航名称是否存在
        /// <summary>
        /// 判断导航名称是否存在
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerifyNavigationsName()
        {
            var ok = new JsonWithNiceValidaorOk();
            var error = new JsonWithNiceValidaorError();

            var navname = RequestHelper.GetFormString("Name");

            if (string.IsNullOrEmpty(navname))
            {
                error.error = "该识别码不可为空！";
                return Json(error);

            }
            if (new Dal.Navigation().Exists(p => p.Name == navname))
            {
                error.error = "该识别码已被占用，请更换！";
                return Json(error);
            }
            else
            {
                ok.ok = "该识别码可使用!";
                return Json(ok);
            }
        }
        #endregion

        #region 获取所有导航列表
        public static List<Model.Navigation> GetNavigationList()
        {
            return new Dal.Navigation().FindSortList();
        }
        #endregion

        #region 反射获取Admins目录下Controller和Action
        public static List<ControllerPermission> GetControllerPermission()
        {
            return AdminsControllerPermission.GetAllControllerAndActionByAssembly();
        }

        #endregion

    }
}