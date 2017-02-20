using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;

namespace MVCCMS.NET.Filter
{
    public class UserBase : Controller
    {
        protected static readonly Logger MyLogger = LogManager.GetCurrentClassLogger();
        #region 数据库/文本日志

        protected static void AddtxtLog(Exception ex , string remark , string logtype)
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

        #endregion

    }
}
