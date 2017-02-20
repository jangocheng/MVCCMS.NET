using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc;
using Dos.ORM;
using MVCCMS.NET.Com;
using MVCCMS.NET.Dal;
using MVCCMS.NET.Filter;
using MVCCMS.NET.Model;
using MVCCMS.NET.Web.Controllers;
using Manager = MVCCMS.NET.Model.Manager;

namespace MVCCMS.NET.Web.Areas.Admins.Controllers
{
    [Description("管理员账户")]
    public class ManagerController : AdminsBase
    {

        private readonly Dal.Manager _bll = new Dal.Manager();




        #region 列表读取============================================================
        [Description("列表")]
        public ActionResult Index()
        {
            var pageCurrent = RequestHelper.GetFormInt("pageCurrent" , 1);
            var pageSize = RequestHelper.GetFormInt("pageSize" , 30);
            var total = 0;
            var where = new Where<Manager>();
            //获取排序字段
            var orderField = RequestHelper.GetFormString("orderField");
            Expression<Func<Manager , object>> orderEx;
            switch (orderField)
            {
                case "Id":
                    orderEx = p => p.Id;
                    break;
                case "Username":
                    orderEx = p => p.Username;
                    break;
                case "Password":
                    orderEx = p => p.Password;
                    break;
                case "Nickname":
                    orderEx = p => p.Nickname;
                    break;
                case "Description":
                    orderEx = p => p.Description;
                    break;
                case "IsSystem":
                    orderEx = p => p.IsSystem;
                    break;
                case "RoleId":
                    orderEx = p => p.RoleId;
                    break;
                case "AddTime":
                    orderEx = p => p.AddTime;
                    break;
                case "LastLoginTime":
                    orderEx = p => p.LastLoginTime;
                    break;
                case "IsLock":
                    orderEx = p => p.IsLock;
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

            var Username = RequestHelper.GetFormString("Username");
            if (!string.IsNullOrEmpty(Username))
            {
                where.And(p => p.Username == Username);
            }
            ViewBag.Username = Username;

            var Password = RequestHelper.GetFormString("Password");
            if (!string.IsNullOrEmpty(Password))
            {
                where.And(p => p.Password == Password);
            }
            ViewBag.Password = Password;

            var Nickname = RequestHelper.GetFormString("Nickname");
            if (!string.IsNullOrEmpty(Nickname))
            {
                where.And(p => p.Nickname == Nickname);
            }
            ViewBag.Nickname = Nickname;

            var Description = RequestHelper.GetFormString("Description");
            if (!string.IsNullOrEmpty(Description))
            {
                where.And(p => p.Description == Description);
            }
            ViewBag.Description = Description;

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

            var RoleId = RequestHelper.GetFormInt("RoleId" , 0);
            if (RoleId > 0)
            {
                where.And(p => p.RoleId == RoleId);
            }
            ViewBag.RoleId = RoleId;

            var AddTime = RequestHelper.GetFormString("AddTime");
            if (!string.IsNullOrEmpty(AddTime))
            {
                var dtAddTime = ToolsHelper.StrToDateTime(AddTime);
                where.And(p => p.AddTime > dtAddTime);
            }
            ViewBag.AddTime = AddTime;

            var LastLoginTime = RequestHelper.GetFormString("LastLoginTime");
            if (!string.IsNullOrEmpty(LastLoginTime))
            {
                var dtLastLoginTime = ToolsHelper.StrToDateTime(LastLoginTime);
                where.And(p => p.AddTime > dtLastLoginTime);
            }
            ViewBag.LastLoginTime = LastLoginTime;

            var IsLock = RequestHelper.GetFormString("IsLock");
            if (!string.IsNullOrEmpty(IsLock) && IsLock == "true")
            {
                where.And(p => p.IsLock == true);
            }
            else if (!string.IsNullOrEmpty(IsLock) && IsLock == "false")
            {
                where.And(p => p.IsLock == false);
            }
            ViewBag.IsLock = IsLock;


            //获取数据
            var list = _bll.QueryPageList(pageCurrent , pageSize , out total , where , orderEx , orderby);

            //缓存数据
            ViewBag.pageCurrent = pageCurrent;
            ViewBag.pageSize = pageSize;
            ViewBag.total = total;

            ViewBag.orderDirection = orderDirection;
            ViewBag.orderField = orderField;

            ViewBag.ManagerRoleList = new Dal.ManagerRole().FindList();

            return View(list);

        }
        #endregion

        #region 创建界面============================================================
        [Description("创建")]
        public ActionResult Create()
        {
            //自定义内容
            ViewBag.ManagerRoleList = new Dal.ManagerRole().FindList();
            return View();
        }

        #endregion

        #region 创建提交============================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Description("创建提交")]
        public ActionResult Create(Manager model)
        {
            var jm = new JsonWithUIcallback();
            try
            {

                if (ModelState.IsValid)
                {
                    model.Password = ToolsHelper.Md5(model.Password);
                    model.AddTime = DateTime.Now;

                    var bl = _bll.Insert(model) > 0;
                    jm.statusCode = bl ? 200 : 300;
                    jm.message = (bl ? KeyWordsHelper.CreateSuccess : KeyWordsHelper.CreateFailure);
                    jm.closeCurrent = bl;
                    jm.tabid = bl ? "ManagerList" : "";
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
                ViewBag.ManagerRoleList = new Dal.ManagerRole().FindList();

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Description("编辑提交")]
        public ActionResult Edit(Manager model)
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
                if (!string.IsNullOrEmpty(model.Password))
                {
                    var pwd = ToolsHelper.Md5(model.Password);
                    oldModel.Password = pwd;
                }
                oldModel.Nickname = model.Nickname;
                oldModel.RoleId = model.RoleId;
                oldModel.IsLock = model.IsLock;
                oldModel.IsSystem = model.IsSystem;
                oldModel.Description = model.Description;
                //事物处理过程结束
                var bl = _bll.Update(oldModel) > 0;
                jm.statusCode = bl ? 200 : 300;
                jm.message = bl ? KeyWordsHelper.EditSuccess : KeyWordsHelper.EditFailure;
                jm.closeCurrent = bl;
                jm.tabid = bl ? "ManagerList" : "";
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
        [HttpPost]
        [Description("删除")]
        public ActionResult Delete(int? id)
        {
            var jm = new JsonWithUIcallback();
            try
            {
                var model = _bll.QueryToEnetity(p => p.Id == id);
                if (model == null)
                {
                    jm.message = KeyWordsHelper.DataisNo;
                }
                else if (model.Username == "admin")
                {
                    jm.message = "系统初始管理员禁止删除";
                }
                else
                {
                    var bl = _bll.Delete(model) > 0;
                    jm.statusCode = bl ? 200 : 300;
                    jm.message = bl ? KeyWordsHelper.DeleteSuccess : KeyWordsHelper.DeleteFailure;
                }
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , "删除" , EnumHelper.Nlog.Error.ToString());
                jm.message = KeyWordsHelper.DataHandleEx;
            }
            return Json(jm);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="delids"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public ActionResult BatchDelete(string delids)
        {
            var jm = new JsonWithUIcallback();
            try
            {
                if (string.IsNullOrEmpty(delids))
                {
                    jm.message = KeyWordsHelper.DataParameterError;
                }
                else
                {
                    var idintarr = ToolsHelper.StringToIntArray(delids);
                    var bl = _bll.Delete(p => p.Id.In(idintarr) && p.Username != "admin") > 0;
                    jm.statusCode = bl ? 200 : 300;
                    jm.message = bl ? KeyWordsHelper.DeleteSuccess : KeyWordsHelper.DeleteFailure;
                }
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , "批量删除" , EnumHelper.Nlog.Error.ToString());
                jm.message = KeyWordsHelper.DataHandleEx;
            }
            return Json(jm);
        }

        #endregion

        #region 预览数据============================================================
        [Description("预览数据")]
        public ActionResult Details(int id)
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
                return View(model);
            }
            catch (Exception ex)
            {
                ComToolsController.AddtxtLog(ex , "预览数据" , EnumHelper.Nlog.Error.ToString());
                jm.statusCode = 300;
                jm.message = ex.ToString();
                jm.closeCurrent = true;
            }
            return Json(jm);
        }
        #endregion

        #region 扩展方法============================================================

        #endregion

    }
}