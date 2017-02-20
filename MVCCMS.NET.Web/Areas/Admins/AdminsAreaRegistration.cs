using System.Web.Mvc;

namespace MVCCMS.NET.Web.Areas.Admins
{
    public class AdminsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admins";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "Admins_Login" ,
                "Admins" ,
                new { Controller = "Login" , action = "Index" , id = UrlParameter.Optional } ,
                new string[] { "MVCCMS.NET.Web.Areas.Admins.Controllers" }
            );



            context.MapRoute(
                "Admins_default" ,
                "Admins/{controller}/{action}/{id}" ,
                new { action = "Index" , id = UrlParameter.Optional } ,
                new string[] { "MVCCMS.NET.Web.Areas.Admins.Controllers" }
            );
        }
    }
}