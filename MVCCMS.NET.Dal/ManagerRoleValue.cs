using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dos.Common;
using MVCCMS.NET.Com;

namespace MVCCMS.NET.Dal
{
    public class ManagerRoleValue : BaseRepository<MVCCMS.NET.Model.ManagerRoleValue>
    {

        public List<Model.ManagerRoleValue> GetRoleValues()
        {
            if (CacheHelper.Get(KeyWordsHelper.CacheRoleValues) == null)
            {
                CacheHelper.Set(KeyWordsHelper.CacheRoleValues , QueryToAllList());
            }
            return CacheHelper.Get(KeyWordsHelper.CacheRoleValues) as List<Model.ManagerRoleValue>;
        }


        public new int InsertBatch(List<Model.ManagerRoleValue> entitylist)
        {
            var id = Db.Insert(entitylist);
            CacheHelper.Set(KeyWordsHelper.CacheRoleValues , QueryToAllList());
            return id;
        }
    }
}
