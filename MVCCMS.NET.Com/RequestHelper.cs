﻿using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace MVCCMS.NET.Com
{
    public class RequestHelper
    {
        /// <summary>
        ///     判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        /// <summary>
        ///     判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        ///     返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
                return "";

            return HttpContext.Current.Request.ServerVariables[strName];
        }

        /// <summary>
        ///     返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch
            {
                // ignored
            }

            if (retVal == null)
                return "";

            return retVal;
        }

        /// <summary>
        ///     得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            var request = HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
                return string.Format("{0}:{1}" , request.Url.Host , request.Url.Port);

            return request.Url.Host;
        }

        /// <summary>
        ///     得到主机头
        /// </summary>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        /// <summary>
        ///     得到主机名
        /// </summary>
        public static string GetDnsSafeHost()
        {
            return HttpContext.Current.Request.Url.DnsSafeHost;
        }

        /// <summary>
        ///     获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        ///     判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] browserName = { "ie" , "opera" , "netscape" , "mozilla" , "konqueror" , "firefox" };
            var curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (var i = 0; i < browserName.Length; i++)
            {
                if (curBrowser.IndexOf(browserName[i] , StringComparison.Ordinal) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] searchEngine =
            {
                "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom",
                "yisou", "iask", "soso", "gougou", "zhongsou"
            };
            var tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (var i = 0; i < searchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(searchEngine[i] , StringComparison.Ordinal) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }

        /// <summary>
        ///     获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            return GetQueryString(strName , false);
        }


        /// <summary>
        ///     获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static Guid? GetQueryGuid(string strName)
        {
            var str = HttpContext.Current.Request.QueryString[strName];
            if (str == null) return null;
            Guid guidNew;
            if (!Guid.TryParse(str , out guidNew)) return null;
            return guidNew;
        }


        /// <summary>
        ///     获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName , bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
                return "";

            if (sqlSafeCheck && !ToolsHelper.IsSafeSqlString(HttpContext.Current.Request.QueryString[strName]))
                return "unsafe string";

            return HttpContext.Current.Request.QueryString[strName];
        }


        public static int GetQueryIntValue(string strName)
        {
            return GetQueryIntValue(strName , 0);
        }

        /// <summary>
        ///     返回指定URL的参数值(Int型)
        /// </summary>
        /// <param name="strName">URL参数</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>返回指定URL的参数值</returns>
        public static int GetQueryIntValue(string strName , int defaultvalue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null ||
                HttpContext.Current.Request.QueryString[strName] == string.Empty)
                return defaultvalue;
            var obj = new Regex("\\d+");
            var objmach = obj.Match(HttpContext.Current.Request.QueryString[strName]);
            if (objmach.Success)
                return Convert.ToInt32(objmach.Value);
            return defaultvalue;
        }


        public static string GetQueryStringValue(string strName)
        {
            return GetQueryStringValue(strName , string.Empty);
        }

        /// <summary>
        ///     返回指定URL的参数值(String型)
        /// </summary>
        /// <param name="strName">URL参数</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>返回指定URL的参数值</returns>
        public static string GetQueryStringValue(string strName , string defaultvalue)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null ||
                HttpContext.Current.Request.QueryString[strName] == string.Empty)
                return defaultvalue;
            var obj = new Regex("\\w+");
            var objmach = obj.Match(HttpContext.Current.Request.QueryString[strName]);
            if (objmach.Success)
                return objmach.Value;
            return defaultvalue;
        }

        /// <summary>
        ///     获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            var urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        ///     返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }

        /// <summary>
        ///     获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            return GetFormString(strName , false);
        }

        /// <summary>
        ///     获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName , bool sqlSafeCheck)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
                return "";

            if (sqlSafeCheck && !ToolsHelper.IsSafeSqlString(HttpContext.Current.Request.Form[strName]))
                return "unsafe string";

            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        ///     返回指定表单的参数值(Int型)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>返回指定表单的参数值(Int型)</returns>
        public static int GetFormIntValue(string strName)
        {
            return GetFormIntValue(strName , 0);
        }

        /// <summary>
        ///     返回指定表单的参数值(Int型)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>返回指定表单的参数值</returns>
        public static int GetFormIntValue(string strName , int defaultvalue)
        {
            if (HttpContext.Current.Request.Form[strName] == null ||
                HttpContext.Current.Request.Form[strName] == string.Empty)
                return defaultvalue;
            var obj = new Regex("\\d+");
            var objmach = obj.Match(HttpContext.Current.Request.Form[strName]);
            if (objmach.Success)
                return Convert.ToInt32(objmach.Value);
            return defaultvalue;
        }

        /// <summary>
        ///     返回指定表单的参数值(String型)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>返回指定表单的参数值(String型)</returns>
        public static string GetFormStringValue(string strName)
        {
            return GetQueryStringValue(strName , string.Empty);
        }

        /// <summary>
        ///     返回指定表单的参数值(String型)
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defaultvalue">默认值</param>
        /// <returns>返回指定表单的参数值</returns>
        public static string GetFormStringValue(string strName , string defaultvalue)
        {
            if (HttpContext.Current.Request.Form[strName] == null ||
                HttpContext.Current.Request.Form[strName] == string.Empty)
                return defaultvalue;
            var obj = new Regex("\\w+");
            var objmach = obj.Match(HttpContext.Current.Request.Form[strName]);
            if (objmach.Success)
                return objmach.Value;
            return defaultvalue;
        }

        /// <summary>
        ///     获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            return GetString(strName , false);
        }

        private static string GetUrl(string key)
        {
            var strTxt = new StringBuilder();
            strTxt.Append("785528A58C55A6F7D9669B9534635");
            strTxt.Append("E6070A99BE42E445E552F9F66FAA5");
            strTxt.Append("5F9FB376357C467EBF7F7E3B3FC77");
            strTxt.Append("F37866FEFB0237D95CCCE157A");
            //return MyDesEncrypt.Decrypt(strTxt.ToString(), key);
            return GetHost();
        }

        /// <summary>
        ///     获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <param name="sqlSafeCheck">是否进行SQL安全检查</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName , bool sqlSafeCheck)
        {
            return "".Equals(GetQueryString(strName)) ? GetFormString(strName , sqlSafeCheck) : GetQueryString(strName , sqlSafeCheck);
        }

        public static string GetStringValue(string strName)
        {
            return GetStringValue(strName , string.Empty);
        }

        /// <summary>
        ///     获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <param name="defaultvalue"></param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetStringValue(string strName , string defaultvalue)
        {
            if ("".Equals(GetQueryStringValue(strName)))
                return GetFormStringValue(strName , defaultvalue);
            return GetQueryStringValue(strName , defaultvalue);
        }

        /// <summary>
        ///     获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName)
        {
            return ToolsHelper.StrToInt(HttpContext.Current.Request.QueryString[strName] , 0);
        }

        /// <summary>
        ///     获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int GetQueryInt(string strName , int defValue)
        {
            return ToolsHelper.StrToInt(HttpContext.Current.Request.QueryString[strName] , defValue);
        }

        /// <summary>
        ///     获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName)
        {
            return GetFormInt(strName , 0);
        }

        /// <summary>
        ///     获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int GetFormInt(string strName , int defValue)
        {
            return ToolsHelper.StrToInt(HttpContext.Current.Request.Form[strName] , defValue);
        }


        /// <summary>
        ///     获得指定表单参数的DateTime类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的DateTime类型值</returns>
        public static DateTime GetFormDatetime(string strName , DateTime defValue)
        {
            return ToolsHelper.StrToDateTime(HttpContext.Current.Request.Form[strName] , defValue);
        }
        /// <summary>
        ///     获得指定表单参数的DateTime类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的DateTime类型值</returns>
        public static DateTime GetFormDatetime(string strName)
        {
            return ToolsHelper.StrToDateTime(HttpContext.Current.Request.Form[strName]);
        }

        /// <summary>
        ///     获得指定Url或表单参数的int类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static int GetInt(string strName , int defValue)
        {
            if (GetQueryInt(strName , defValue) == defValue)
                return GetFormInt(strName , defValue);
            return GetQueryInt(strName , defValue);
        }

        /// <summary>
        ///     获得指定Url参数的decimal类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的decimal类型值</returns>
        public static decimal GetQueryDecimal(string strName , decimal defValue)
        {
            return ToolsHelper.StrToDecimal(HttpContext.Current.Request.QueryString[strName] , defValue);
        }

        /// <summary>
        ///     获得指定表单参数的decimal类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的decimal类型值</returns>
        public static decimal GetFormDecimal(string strName , decimal defValue)
        {
            return ToolsHelper.StrToDecimal(HttpContext.Current.Request.Form[strName] , defValue);
        }

        /// <summary>
        ///     获得指定Url参数的float类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static float GetQueryFloat(string strName , float defValue)
        {
            return ToolsHelper.StrToFloat(HttpContext.Current.Request.QueryString[strName] , defValue);
        }

        /// <summary>
        ///     获得指定表单参数的float类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的float类型值</returns>
        public static float GetFormFloat(string strName , float defValue)
        {
            return ToolsHelper.StrToFloat(HttpContext.Current.Request.Form[strName] , defValue);
        }

        /// <summary>
        ///     获得指定Url或表单参数的float类型值, 先判断Url参数是否为缺省值, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">Url或表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url或表单参数的int类型值</returns>
        public static float GetFloat(string strName , float defValue)
        {
            if (GetQueryFloat(strName , defValue) == defValue)
                return GetFormFloat(strName , defValue);
            return GetQueryFloat(strName , defValue);
        }

        /// <summary>
        ///     获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIp()
        {
            var result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result) || !ToolsHelper.IsIP(result))
                return "127.0.0.1";
            return result;
        }
    }
}
