using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCCMS.NET.Web.Models
{
    public class MVCCMSNETWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MVCCMSNETWebContext() : base("name=MVCCMSNETWebContext")
        {
        }

        public System.Data.Entity.DbSet<MVCCMS.NET.Model.ManagerLog> ManagerLogs { get; set; }

        public System.Data.Entity.DbSet<MVCCMS.NET.Model.Manager> Managers { get; set; }

        public System.Data.Entity.DbSet<MVCCMS.NET.Model.ManagerRole> ManagerRoles { get; set; }

        public System.Data.Entity.DbSet<MVCCMS.NET.Model.Navigation> Navigations { get; set; }
    }
}
