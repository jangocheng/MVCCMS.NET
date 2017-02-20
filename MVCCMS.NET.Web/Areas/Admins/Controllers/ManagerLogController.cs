using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Mvc;
using Dos.ORM;
using MVCCMS.NET.Com;
using MVCCMS.NET.Filter;
using MVCCMS.NET.Model;
using MVCCMS.NET.Web.Controllers;

namespace MVCCMS.NET.Web.Areas.Admins.Controllers
{
    [Description("操作日志")]
    public class ManagerLogController : AdminsBase
    {
        private readonly Dal.ManagerLog _bll = new Dal.ManagerLog();

        #region 列表读取============================================================
        // GET: Admins/ManagerLog/Index
        [Description("列表")]
        public ActionResult Index()
        {
            var pageCurrent = RequestHelper.GetFormInt("pageCurrent" , 1);
            var pageSize = RequestHelper.GetFormInt("pageSize" , 30);
            int total;
            var where = new Where<ManagerLog>();
            //获取排序字段
            var orderField = RequestHelper.GetFormString("orderField");
            Expression<Func<ManagerLog , object>> orderEx;
            switch (orderField)
            {
                case "Id":
                    orderEx = p => p.Id;
                    break;
                case "UserId":
                    orderEx = p => p.UserId;
                    break;
                case "UserName":
                    orderEx = p => p.UserName;
                    break;
                case "ActionType":
                    orderEx = p => p.ActionType;
                    break;
                case "Remark":
                    orderEx = p => p.Remark;
                    break;
                case "UserIp":
                    orderEx = p => p.UserIp;
                    break;
                case "AddTime":
                    orderEx = p => p.AddTime;
                    break;
                case "ControllerName":
                    orderEx = p => p.ControllerName;
                    break;
                case "ActionName":
                    orderEx = p => p.ActionName;
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

            var UserId = RequestHelper.GetFormInt("UserId" , 0);
            if (UserId > 0)
            {
                where.And(p => p.UserId == UserId);
            }
            ViewBag.UserId = UserId;

            var UserName = RequestHelper.GetFormString("UserName");
            if (!string.IsNullOrEmpty(UserName))
            {
                where.And(p => p.UserName == UserName);
            }
            ViewBag.UserName = UserName;

            var ActionType = RequestHelper.GetFormString("ActionType");
            if (!string.IsNullOrEmpty(ActionType))
            {
                where.And(p => p.ActionType == ActionType);
            }
            ViewBag.ActionType = ActionType;

            var Remark = RequestHelper.GetFormString("Remark");
            if (!string.IsNullOrEmpty(Remark))
            {
                where.And(p => p.Remark == Remark);
            }
            ViewBag.Remark = Remark;

            var UserIp = RequestHelper.GetFormString("UserIp");
            if (!string.IsNullOrEmpty(UserIp))
            {
                where.And(p => p.UserIp == UserIp);
            }
            ViewBag.UserIp = UserIp;

            var AddTime = RequestHelper.GetFormString("AddTime");
            if (!string.IsNullOrEmpty(AddTime))
            {
                var dtAddTime = ToolsHelper.StrToDateTime(AddTime);
                where.And(p => p.AddTime > dtAddTime);
            }
            ViewBag.AddTime = AddTime;

            var ControllerName = RequestHelper.GetFormString("ControllerName");
            if (!string.IsNullOrEmpty(ControllerName))
            {
                where.And(p => p.ControllerName == ControllerName);
            }
            ViewBag.ControllerName = ControllerName;

            var ActionName = RequestHelper.GetFormString("ActionName");
            if (!string.IsNullOrEmpty(ActionName))
            {
                where.And(p => p.ActionName == ActionName);
            }
            ViewBag.ActionName = ActionName;

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

        #region 删除数据============================================================
        // POST: Admins/ManagerLog/Delete/10
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
        #endregion

        #region 批量删除============================================================
        // POST: Admins/ManagerLog/BatchDelete/10,11,20
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
                    var bl = _bll.Delete(p => p.Id.In(idintarr) && p.Id != 1) > 0;
                    jm.statusCode = bl ? 200 : 300;
                    jm.message = bl ? KeyWordsHelper.DeleteSuccess : KeyWordsHelper.DeleteFailure;
                    jm.tabid = bl ? "ManagerLogList" : "";
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
        // GET: Admins/ManagerLog/Details/10
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
