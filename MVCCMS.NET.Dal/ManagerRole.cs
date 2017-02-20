using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dos.Common;
using Dos.ORM;
using MVCCMS.NET.Com;

namespace MVCCMS.NET.Dal
{
    public class ManagerRole : BaseRepository<MVCCMS.NET.Model.ManagerRole>
    {
        public List<Model.ManagerRole> FindList()
        {
            if (CacheHelper.Get(KeyWordsHelper.CacheManagerRoleList) != null) { 
                return CacheHelper.Get(KeyWordsHelper.CacheManagerRoleList) as List<Model.ManagerRole>;
            }
            UpdateCacheManagerRoleList();
            return CacheHelper.Get(KeyWordsHelper.CacheManagerRoleList) as List<Model.ManagerRole>;
        }

        public new int Insert(Model.ManagerRole T)
        {
            var count = Db.Insert(T);
            UpdateCacheManagerRoleList();
            return count;
        }

        public new int Update(Model.ManagerRole entity)
        {
            var count = Db.Update(entity);
            UpdateCacheManagerRoleList();
            return count;
        }

        public new int Delete(Model.ManagerRole entity)
        {
            int count;
            using (DbTrans trans = Db.BeginTransaction())
            {
                count = Db.Delete(trans , entity);
                Db.Delete<Model.ManagerRoleValue>(trans , p=>p.RoleId == entity.Id);
                Db.Delete<Model.NavigationRoleValue>(trans , p => p.RoleId == entity.Id);
                trans.Commit();
            }
            UpdateCacheManagerRoleList();
            return count;
        }

        public new int Delete(IEnumerable<Model.ManagerRole> entity)
        {
            var count = Db.Delete(entity);
            UpdateCacheManagerRoleList();
            return count;
        }
        /// <summary>
        ///     更新cache
        /// </summary>
        private void UpdateCacheManagerRoleList()
        {
            var list = QueryToAllList().OrderBy(p => p.SortId).ToList();
            CacheHelper.Set(KeyWordsHelper.CacheManagerRoleList , list);
        }
    }
}
