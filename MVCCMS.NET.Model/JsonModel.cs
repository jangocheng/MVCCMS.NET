using System;

namespace MVCCMS.NET.Model
{
    /// <summary>
    /// UI回调Json实体
    /// </summary>
    public class JsonWithUIcallback
    {
        private int _statusCode = 300;
        private string _message = "系统错误提示";

        /// <summary>
        /// 状态码(ok = 200, error = 300, timeout = 301)，必选。可以在BJUI.init时配置三个参数的默认值。
        /// </summary>
        public int statusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        /// <summary>
        /// 可选。信息内容。
        /// </summary>
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 可选。待刷新navtab id，多个id以英文逗号分隔开，当前的navtab id不需要填写，填写后可能会导致当前navtab重复刷新
        /// </summary>
        public string tabid { get; set; }
        /// <summary>
        /// 可选。待刷新dialog id，多个id以英文逗号分隔开，请不要填写当前的dialog id，要控制刷新当前dialog，请设置dialog中表单的reload参数。
        /// </summary>
        public string dialogid { get; set; }
        /// <summary>
        /// 可选。待刷新div id，多个id以英文逗号分隔开，请不要填写当前的div id，要控制刷新当前div，请设置该div中表单的reload参数。
        /// </summary>
        public string divid { get; set; }
        /// <summary>
        /// 可选。是否关闭当前窗口(navtab或dialog)。
        /// </summary>
        public Boolean closeCurrent { get; set; }
        /// <summary>
        /// 可选。跳转到某个url。
        /// </summary>
        public string forward { get; set; }
        /// <summary>
        /// 可选。跳转url前的确认提示信息。
        /// </summary>
        public string forwardConfirm { get; set; }
    }

    /// <summary>
    /// NiceValidaor验证返回成功信息
    /// </summary>
    public class JsonWithNiceValidaorOk
    {
        private string _ok = "可用";

        /// <summary>
        /// 成功
        /// </summary>
        public string ok
        {
            get { return _ok; }
            set { _ok = value; }
        }
    }
    /// <summary>
    /// NiceValidaor验证返回失败信息
    /// </summary>
    public class JsonWithNiceValidaorError
    {
        private string _error = "不可用，请重新输入";

        /// <summary>
        /// 失败
        /// </summary>
        public string error
        {
            get { return _error; }
            set { _error = value; }
        }
    }
}
