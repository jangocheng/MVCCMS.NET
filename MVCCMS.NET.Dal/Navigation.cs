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
    public class Navigation : BaseRepository<MVCCMS.NET.Model.Navigation>
    {

        #region 更新父类和子类
        public bool UpdateParentAndChilds(Model.Navigation model)
        {
            var oldmodel = QueryToEnetity(p => p.Id == model.Id);
            if (oldmodel == null)
            {
                return false;
            }
            int oldParentId = oldmodel.ParentId;
            oldmodel.ParentId = model.ParentId;
            oldmodel.Name = model.Name;
            oldmodel.Title = model.Title;
            oldmodel.SubTitle = model.SubTitle;
            oldmodel.LinkUrl = model.LinkUrl;
            oldmodel.SortId = model.SortId;
            oldmodel.IsLock = model.IsLock;
            oldmodel.Remark = model.Remark;
            oldmodel.ActionType = model.ActionType;
            oldmodel.IconUrl = model.IconUrl;
            //var bl = Update(oldmodel);
            if (model.ParentId > 0 && oldParentId != model.ParentId)
            {
                var modelP = QueryToEnetity(p => p.Id == model.ParentId);
                oldmodel.ClassList = modelP.ClassList + oldmodel.Id + ",";
                oldmodel.ClassLayer = modelP.ClassLayer + 1;
            }
            if (model.ParentId == 0 && oldParentId != model.ParentId)
            {
                oldmodel.ClassLayer = 1;
                oldmodel.ClassList = "," + model.Id + ",";
            }
            var bl = (Update(oldmodel) > 0);
            if (oldParentId != model.ParentId) { UpdateChilds(oldmodel.Id); }
            if (bl)
            {
                var oldData = QueryToAllList().OrderBy(p => p.SortId).ToList();
                var newData = new List<Model.Navigation>();
                GList(oldData , newData , 0);
                CacheHelper.Set(KeyWordsHelper.CacheFindNavSortList , newData , 1440);
                CacheHelper.Set(KeyWordsHelper.CacheFindNavNoSortList , newData , 1440);
            }

            return bl;
        }
        private void UpdateChilds(int parentId)
        {
            //查找父节点信息
            var model = QueryToEnetity(p => p.Id == parentId);
            if (model != null)
            {
                //查找子节点
                var list = QueryToList(p => p.ParentId == parentId);
                foreach (var dr in list)
                {
                    //修改子节点的ID列表及深度
                    var id = dr.Id;
                    dr.ClassList = model.ClassList + id + ",";
                    dr.ClassLayer = model.ClassLayer + 1;
                    Update(dr);
                    //调用自身迭代
                    UpdateChilds(id); //带事务
                }
            }
        }
        #endregion

        #region 添加新类别
        public bool InsertAndUpdateCache(Model.Navigation model)
        {

            model.NavType = Com.EnumHelper.NavigationEnum.System.ToString();
            var id = Insert(model);
            var newmodel = QueryToEnetity(p => p.Id == id);
            if (newmodel.ParentId > 0)
            {
                var modelP = QueryToEnetity(p => p.Id == model.ParentId);
                newmodel.ClassList = modelP.ClassList + newmodel.Id + ",";
                newmodel.ClassLayer = modelP.ClassLayer + 1;
            }
            else
            {
                newmodel.ClassLayer = 1;
                newmodel.ClassList = "," + newmodel.Id + ",";
            }

            var outid = (Update(newmodel) > 0);
            var oldData = QueryToAllList().OrderBy(p => p.SortId).ToList();
            var newData = new List<Model.Navigation>();
            GList(oldData , newData , 0);
            CacheHelper.Set(KeyWordsHelper.CacheFindNavSortList , newData , 1440);
            CacheHelper.Set(KeyWordsHelper.CacheFindNavNoSortList , oldData , 1440);
            return outid;

        }
        #endregion

        #region 获取未排序导航
        public List<Model.Navigation> FindNoSortList()
        {
            if (CacheHelper.Get(KeyWordsHelper.CacheFindNavNoSortList) == null)
            {
                var list = QueryToAllList().OrderBy(p => p.SortId).ToList();
                CacheHelper.Set(KeyWordsHelper.CacheFindNavNoSortList , list , 1440);
                return list;
            }
            return CacheHelper.Get(KeyWordsHelper.CacheFindNavNoSortList) as List<Model.Navigation>;
        }
        #endregion

        #region 获取排序后导航

        /// <summary>
        /// 获取排序后导航
        /// </summary>
        /// <returns></returns>
        public List<Model.Navigation> FindSortList()
        {
            if (CacheHelper.Get(KeyWordsHelper.CacheFindNavSortList) == null)
            {
                var oldData = QueryToAllList().OrderBy(p => p.SortId).ToList();
                var newData = new List<Model.Navigation>();
                GList(oldData , newData , 0);
                CacheHelper.Set(KeyWordsHelper.CacheFindNavSortList , newData , 1440);
                return newData;
            }
            return CacheHelper.Get(KeyWordsHelper.CacheFindNavSortList) as List<Model.Navigation>;
        }

        private static void GList(List<Model.Navigation> oldList , List<Model.Navigation> newData , int parentId)
        {
            var model = oldList.Where(p => p.ParentId == parentId);
            foreach (var m in model)
            {
                newData.Add(m);
                GList(oldList , newData , m.Id);
            }
        }
        #endregion

        /// <summary>
        /// 删除导航和子类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteParentAndChilds(int id)
        {
            var bl = Delete(p => p.ClassList.Like("," + id + ","));
            var oldData = QueryToAllList().OrderBy(p => p.SortId).ToList();
            var newData = new List<Model.Navigation>();
            GList(oldData , newData , 0);
            CacheHelper.Set(KeyWordsHelper.CacheFindNavSortList , newData , 1440);
            CacheHelper.Set(KeyWordsHelper.CacheFindNavNoSortList , oldData , 1440);
            return bl > 0;
        }
    }
}
