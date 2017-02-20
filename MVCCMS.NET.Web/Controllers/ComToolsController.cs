
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace MVCCMS.NET.Web.Controllers
{
    public class ComToolsController : Controller
    {
        private static readonly Logger MyLogger = LogManager.GetCurrentClassLogger();
        // GET: ComTools
        public static void AddtxtLog(Exception ex , string remark , string logtype)
        {
            if (logtype == "Error")
            {
                MyLogger.Error(ex , remark);
            }
            else if (logtype == "Info")
            {
                MyLogger.Info(ex , remark);
            }
            else if (logtype == "Debug")
            {
                MyLogger.Debug(ex , remark);
            }
        }
    }
}