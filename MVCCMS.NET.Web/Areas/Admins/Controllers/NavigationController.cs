using System;
using System.ComponentModel;
using System.Web.Mvc;
using Dos.ORM;
using MVCCMS.NET.Com;
using MVCCMS.NET.Filter;
using MVCCMS.NET.Model;
using MVCCMS.NET.Web.Controllers;

namespace MVCCMS.NET.Web.Areas.Admins.Controllers
{
    [Description("后台导航")]
    public class NavigationController : AdminsBase
    {
        private readonly Dal.Navigation _bll = new Dal.Navigation();

        #region 列表读取============================================================
        // GET: Admins/Navigation/Index
        [Description("列表")]
        public ActionResult Index()
        {
            var list = _bll.FindSortList();

            return View(list);


        }
        #endregion

        #region 创建界面============================================================
        // GET: Admins/Navigation/Create
        [Description("创建")]
        public ActionResult Create()
        {
            ViewBag.ShowNav = _bll.FindSortList();
            ViewBag.ControllerAndAction = AdminsControllerPermission.GetAllControllerAndActionByAssembly();

            //自定义内容
            return View();
        }

        #endregion

        #region 创建提交============================================================
        // POST: Admins/Navigation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Description("创建提交")]
        public ActionResult Create(Navigation model)
        {
            var jm = new JsonWithUIcallback();
            try
            {

                if (ModelState.IsValid)
                {
                    //其他修改
                    var bl = _bll.InsertAndUpdateCache(model);
                    jm.statusCode = bl ? 200 : 300;
                    jm.message = (bl ? KeyWordsHelper.CreateSuccess : KeyWordsHelper.CreateFailure);
                    jm.closeCurrent = bl;
                    jm.tabid = bl ? "NavigationList" : "";
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
        // GET: Admins/Navigation/Edit
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

                ViewBag.ShowNav = _bll.FindSortList();
                ViewBag.ControllerAndAction = AdminsControllerPermission.GetAllControllerAndActionByAssembly();


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
        // POST: Admins/Navigation/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Description("编辑提交")]
        public ActionResult Edit(Navigation model)
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


                //事物处理过程结束
                var bl = _bll.UpdateParentAndChilds(model);
                jm.statusCode = bl ? 200 : 300;
                jm.message = bl ? KeyWordsHelper.EditSuccess : KeyWordsHelper.EditFailure;
                jm.closeCurrent = bl;
                jm.tabid = bl ? "NavigationList" : "";
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
        // POST: Admins/Navigation/Delete/10
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
                else if (model.IsSystem == true || model.NavType == "System")
                {
                    jm.message = "系统内置项目禁止删除!";
                }
                else
                {
                    var bl = _bll.DeleteParentAndChilds(id);
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

        #region 扩展方法============================================================

        #endregion
    }
}
