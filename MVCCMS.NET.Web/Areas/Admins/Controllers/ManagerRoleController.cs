using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Dos.ORM;
using MVCCMS.NET.Com;
using MVCCMS.NET.Filter;
using MVCCMS.NET.Model;
using MVCCMS.NET.Web.Controllers;

namespace MVCCMS.NET.Web.Areas.Admins.Controllers
{
    [Description("角色权限")]
    public class ManagerRoleController : AdminsBase
    {
        private readonly Dal.ManagerRole _bll = new Dal.ManagerRole();
        private readonly Dal.Manager _bllManager = new Dal.Manager();

        #region 列表读取============================================================
        // GET: Admins/ManagerRole/Index
        [Description("列表")]
        public ActionResult Index()
        {
            var pageCurrent = RequestHelper.GetFormInt("pageCurrent" , 1);
            var pageSize = RequestHelper.GetFormInt("pageSize" , 30);
            int total;
            var where = new Where<ManagerRole>();
            //获取排序字段
            var orderField = RequestHelper.GetFormString("orderField");
            Expression<Func<ManagerRole , object>> orderEx;
            switch (orderField)
            {
                case "Id":
                    orderEx = p => p.Id;
                    break;
                case "RoleName":
                    orderEx = p => p.RoleName;
                    break;
                case "IsSystem":
                    orderEx = p => p.IsSystem;
                    break;
                case "SortId":
                    orderEx = p => p.SortId;
                    break;
                case "DepartmentID":
                    orderEx = p => p.DepartmentID;
                    break;
                default:
                    orderEx = p => p.Id;
                    break;
            }
            //设置方式
            var orderDirection = RequestHelper.GetFormString("orderDirection");
            EnumHelper.OrderBy orderby = EnumHelper.OrderBy.Asc;
            if (orderDirection == "desc")
            {
                orderby = EnumHelper.OrderBy.Desc;
            }
            //查询筛选

            var Id = RequestHelper.GetFormInt("Id" , 0);
            if (Id > 0)
            {
                where.And(p => p.Id == Id);
            }
            ViewBag.Id = Id;

            var RoleName = RequestHelper.GetFormString("RoleName");
            if (!string.IsNullOrEmpty(RoleName))
            {
                where.And(p => p.RoleName == RoleName);
            }
            ViewBag.RoleName = RoleName;

            var IsSystem = RequestHelper.GetFormString("IsSystem");
            if (!string.IsNullOrEmpty(IsSystem) && IsSystem == "true")
            {
                where.And(p => p.IsSystem == true);
            }
            else if (!string.IsNullOrEmpty(IsSystem) && IsSystem == "false")
            {
                where.And(p => p.IsSystem == false);
            }
            ViewBag.IsSystem = IsSystem;

            var SortId = RequestHelper.GetFormInt("SortId" , 0);
            if (SortId > 0)
            {
                where.And(p => p.SortId == SortId);
            }
            ViewBag.SortId = SortId;

            var DepartmentID = RequestHelper.GetFormInt("DepartmentID" , 0);
            if (DepartmentID > 0)
            {
                where.And(p => p.DepartmentID == DepartmentID);
            }
            ViewBag.DepartmentID = DepartmentID;

            //获取数据
            var list = _bll.QueryPageList(pageCurrent , pageSize , out total , where , orderEx , orderby);

            //缓存数据
            ViewBag.pageCurrent = pageCurrent;
            ViewBag.pageSize = pageSize;
            ViewBag.total = total;

            ViewBag.orderDirection = orderDirection;
            ViewBag.orderField = orderField;

            return View(list);

        }
        #endregion

        #region 创建界面============================================================
        // GET: Admins/ManagerRole/Create
        [Description("创建")]
        public ActionResult Create()
        {
            //自定义内容
            return View();
        }

        #endregion

        #region 创建提交============================================================
        // POST: Admins/ManagerRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Description("创建提交")]
        public ActionResult Create(ManagerRole model , string[] roles , string[] navs)
        {
            var jm = new JsonWithUIcallback();
            try
            {

                if (ModelState.IsValid)
                {
                    //其他修改
                    var resultid = _bll.Insert(model);

                    if (resultid > 0)
                    {
                        //获取具体权限内容
                        var rolelist = new List<Model.ManagerRoleValue>();
                        if (roles.Length > 0)
                        {
                            rolelist.AddRange(
                                roles.Select(role => role.Split('|')).Select(arrrole => new ManagerRoleValue
                                {
                                    RoleId = resultid ,
                                    ControllerName = arrrole[0] ,
                                    ActionName = arrrole[1]
                                }));
                        }
                        var bllrolevalue = new Dal.ManagerRoleValue();
                        bllrolevalue.InsertBatch(rolelist);

                        var navlist = new List<NavigationRoleValue>();
                        if (navs.Length > 0)
                        {
                            navlist.AddRange(
                                navs.Select(t => new NavigationRoleValue { RoleId = resultid , ActionName = t }));
                        }
                        var bllnavvalue = new Dal.NavigationRoleValue();
                        bllnavvalue.InsertBatch(navlist);
                    }

                    var bl = resultid > 0;
                    jm.statusCode = bl ? 200 : 300;
                    jm.message = (bl ? KeyWordsHelper.CreateSuccess : KeyWordsHelper.CreateFailure);
                    jm.closeCurrent = bl;
                    jm.tabid = bl ? "ManagerRoleList" : "";
                }
                else
                {
                    jm.message = KeyWordsHelper.DataParameterError;
                }
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , "创建提交" , EnumHelper.Nlog.Error.ToString());
                jm.statusCode = 300;
                jm.message = ex.ToString();
                jm.closeCurrent = true;

            }
            return Json(jm);
        }
        #endregion

        #region 编辑展示============================================================
        // GET: Admins/ManagerRole/Edit
        [Description("编辑")]
        public ActionResult Edit(int id)
        {
            var jm = new JsonWithUIcallback();
            try
            {
                var model = _bll.QueryToEnetity(p => p.Id == id);
                if (model == null)
                {
                    jm.statusCode = 300;
                    jm.message = "不存在此信息";
                    jm.closeCurrent = true;
                    return Json(jm);
                }
                ViewBag.rolesvalues = new Dal.ManagerRoleValue().QueryToList(p => p.RoleId == id);
                ViewBag.MyNavValueList = new Dal.NavigationRoleValue().QueryToList(p => p.RoleId == id);

                return View(model);
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , "编辑" , EnumHelper.Nlog.Error.ToString());
                jm.statusCode = 300;
                jm.message = ex.ToString();
                jm.closeCurrent = true;
            }
            return Json(jm);
        }

        #endregion

        #region 编辑提交============================================================
        // POST: Admins/ManagerRole/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Description("编辑提交")]
        public ActionResult Edit(ManagerRole model , string[] roles , string[] navs)
        {
            var jm = new JsonWithUIcallback();
            try
            {
                if (!ModelState.IsValid)
                {
                    jm.message = KeyWordsHelper.DataParameterError;
                    return Json(jm);
                }
                var oldModel = _bll.QueryToEnetity(p => p.Id == model.Id);
                if (oldModel == null)
                {
                    jm.statusCode = 300;
                    jm.message = "不存在此信息";
                    jm.closeCurrent = true;
                    return Json(jm);
                }
                //事物处理过程开始

                oldModel.DepartmentID = model.DepartmentID;
                oldModel.IsSystem = model.IsSystem;
                oldModel.RoleName = model.RoleName;
                oldModel.SortId = model.SortId;

                //获取具体权限内容
                var rolelist = new List<Model.ManagerRoleValue>();
                if (roles.Length > 0)
                {
                    rolelist.AddRange(roles.Select(role => role.Split('|')).Select(arrrole => new ManagerRoleValue
                    {
                        RoleId = oldModel.Id ,
                        ControllerName = arrrole[0] ,
                        ActionName = arrrole[1]
                    }));
                }
                //获取具体栏目显示
                var navlist = new List<NavigationRoleValue>();
                if (navs !=null && navs.Length > 0)
                {
                    navlist.AddRange(navs.Select(t => new NavigationRoleValue { RoleId = oldModel.Id , ActionName = t }));
                }
                var resultbl = _bll.Update(oldModel) > 0;
                if (resultbl)
                {
                    var bllrolevalue = new Dal.ManagerRoleValue();
                    bllrolevalue.Delete(p => p.RoleId == oldModel.Id);
                    bllrolevalue.InsertBatch(rolelist);
                    var bllnavvalue = new Dal.NavigationRoleValue();
                    bllnavvalue.Delete(p => p.RoleId == oldModel.Id);
                    bllnavvalue.InsertBatch(navlist);
                }
                //事物处理过程结束
                var bl = resultbl;
                jm.statusCode = bl ? 200 : 300;
                jm.message = bl ? KeyWordsHelper.EditSuccess : KeyWordsHelper.EditFailure;
                jm.closeCurrent = bl;
                jm.tabid = bl ? "ManagerRoleList" : "";
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , "创建提交" , EnumHelper.Nlog.Error.ToString());
                jm.statusCode = 300;
                jm.message = ex.ToString();
                jm.closeCurrent = true;
            }
            return Json(jm);
        }
        #endregion

        #region 删除数据============================================================
        // POST: Admins/ManagerRole/Delete/10
        [HttpPost]
        [Description("删除")]
        public ActionResult Delete(int id)
        {
            var jm = new JsonWithUIcallback();
            try
            {
                var model = _bll.QueryToEnetity(p => p.Id == id);
                if (model == null)
                {
                    jm.message = KeyWordsHelper.DataisNo;
                }
                else if (model.Id == 1)
                {
                    jm.message = "此项目禁止删除";
                }
                else if (_bllManager.Exists(p => p.RoleId == model.Id))
                {
                    jm.message = "存在关联的管理员账户信息！禁止删除!";
                }
                else
                {
                    var bl = _bll.Delete(model) > 0;
                    jm.statusCode = bl ? 200 : 300;
                    jm.message = bl ? KeyWordsHelper.DeleteSuccess : KeyWordsHelper.DeleteFailure;
                    //jm.tabid = bl ? "ManagerRoleList" : "";
                }
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , "删除" , EnumHelper.Nlog.Error.ToString());
                jm.message = KeyWordsHelper.DataHandleEx;
            }
            return Json(jm);
        }
        #endregion


        #region 扩展方法============================================================

        #endregion
    }
}
