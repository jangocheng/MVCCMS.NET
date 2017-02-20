using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVCCMS.NET.Filter
{
    public class AdminsControllerPermission
    {
        /// <summary>
        ///     反射获取所有controller 和action
        /// </summary>
        /// <returns></returns>
        public static List<ControllerPermission> GetAllControllerAndActionByAssembly()
        {

            var result = new List<ControllerPermission>();

            var types = Assembly.Load("MVCCMS.NET.Web").GetTypes();

            foreach (var type in types)
            {
                var attresAdminsFilter = type.GetCustomAttributes(typeof(AdminsFilter) , true);
                if (type.Name.Length > 10 && type.BaseType.Name == "AdminsBase" && type.Name.EndsWith("Controller")
                    && type.Name != "AdminsToolsController" && attresAdminsFilter.Length > 0
                    ) //如果是Controller
                {
                    var members = type.GetMethods();
                    var cp = new ControllerPermission
                    {
                        ControllerName = type.Name.Substring(0 , type.Name.Length - 10) ,
                        Action = new List<ActionPermission>()
                    };
                    var objs = type.GetCustomAttributes(typeof(DescriptionAttribute) , true);
                    if (objs.Length > 0) cp.Description = (objs[0] as DescriptionAttribute).Description;

                    foreach (var member in members)
                    {
                        if (member.ReturnType.Name == "ActionResult" || member.ReturnType.Name == "FileResult") //如果是Action
                        {
                            var ap = new ActionPermission
                            {
                                ActionName = member.Name ,
                                ControllerName = member.DeclaringType.Name.Substring(0 ,
                                    member.DeclaringType.Name.Length - 10)
                            };
                            // 去掉“Controller”后缀

                            var attresNoFilter = member.GetCustomAttributes(typeof(NoFilter) , true);
                            if (attresNoFilter.Length == 0)
                            {
                                var attrs = member.GetCustomAttributes(typeof(DescriptionAttribute) , true);
                                if (attrs.Length > 0)
                                    ap.Description = (attrs[0] as DescriptionAttribute).Description;
                                cp.Action.Add(ap);
                            }
                        }
                    }
                    cp.Action = cp.Action.Distinct(new ModelComparer()).ToList();
                    result.Add(cp);
                }
            }
            return result;
        }

        private class ModelComparer : IEqualityComparer<ActionPermission>
        {
            public bool Equals(ActionPermission x , ActionPermission y)
            {
                return x.ActionName.ToUpper() == y.ActionName.ToUpper();
            }

            public int GetHashCode(ActionPermission obj)
            {
                return obj.ActionName.ToUpper().GetHashCode();
            }
        }
    }

    public class ActionPermission
    {
        /// <summary>
        ///     请求地址
        /// </summary>
        public virtual string ActionName { get; set; }

        /// <summary>
        ///     请求地址
        /// </summary>
        public virtual string ControllerName { get; set; }

        /// <summary>
        ///     描述
        /// </summary>
        public virtual string Description { get; set; }
    }

    public class ControllerPermission
    {
        public virtual string ControllerName { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<ActionPermission> Action { get; set; }
    }
}
