using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Dos.ORM;
using MVCCMS.NET.Com;

namespace MVCCMS.NET.Dal
{
    public class BaseRepository<T> where T : Entity
    {
        private static readonly DbSession db = new DbSession("LJWCMS.DataConn");

        public static DbSession Db
        {
            get
            {
                return db;
            }
        }

        #region 根据条件判断是否存在数据
        /// <summary>
        ///     根据条件判断是否存在数据
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<T , bool>> where)
        {
            return Db.Exists(where);
        }
        public bool Exists(Where where)
        {
            return Db.Exists<T>(where);
        }
        #endregion

        #region 取总数==================================================
        /// <summary>
        ///     取总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T , bool>> where)
        {
            return Db.From<T>().Where(where).Count();
        }

        /// <summary>
        ///     取总数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Count(Where<T> where)
        {
            return Db.From<T>().Where(where).Count();
        }
        #endregion

        #region 更新====================================================

        public int Update(T entity)
        {
            return Db.Update(entity);
        }

        public void Update(DbTrans context , T entity)
        {
            Db.Update(context , entity);
        }

        public int UpdateBatch(List<T> entitylist)
        {
            return Db.Update(entitylist);
        }

        public int Update(IEnumerable<T> entities)
        {
            var enumerable = entities as T[] ?? entities.ToArray();
            Db.Update(enumerable.ToArray());
            return 1;
        }
        public void Update(DbTrans context , IEnumerable<T> entities)
        {
            Db.Update(context , entities.ToArray());
        }

        public int Update(T entity , Expression<Func<T , bool>> where)
        {
            return Db.Update(entity , where);
        }

        #endregion

        #region 查询====================================================

        /// <summary>
        ///     根据条件查询的单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T QueryEnetity(Where<T> where)
        {
            return Db.From<T>().Where(where).First();
        }

        /// <summary>
        ///     Expression条件查询
        /// </summary>
        /// <param name="ep"></param>
        /// <returns></returns>
        public T QueryToEnetity(Expression<Func<T , bool>> ep)
        {
            return Db.From<T>().Where(ep).First();
        }

        /// <summary>
        ///     查询全部数据
        /// </summary>
        /// <returns></returns>
        public List<T> QueryToAllList()
        {
            return Db.From<T>().ToList();
        }
        /// <summary>
        /// 单表查询前几个
        /// </summary>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public List<T> QueryToList(int topNum)
        {
            return Db.From<T>().Top(topNum).ToList();
        }

        public List<T> QueryToList(Where<T> where)
        {
            return Db.From<T>().Where(where).ToList();
        }

        public List<T> QueryToList(Expression<Func<T , bool>> ep)
        {
            return Db.From<T>().Where(ep).ToList();
        }

        public List<T> QueryToList(Where<T> where , int topNum)
        {
            return Db.From<T>().Where(where).Top(topNum).ToList();
        }

        public List<T> QueryToList(Expression<Func<T , bool>> ep , int topNum)
        {
            return Db.From<T>().Where(ep).Top(topNum).ToList();
        }

        public List<T> QueryToList(Where<T> where , int topNum , Expression<Func<T , object>> orderBy , EnumHelper.OrderBy ascDesc)
        {
            if (ascDesc == EnumHelper.OrderBy.Desc)
            {
                return Db.From<T>().Where(where).Top(topNum).OrderByDescending(orderBy).ToList();
            }
            return Db.From<T>().Where(where).Top(topNum).OrderBy(orderBy).ToList();
        }

        public List<T> QueryToList(Expression<Func<T , bool>> ep , int topNum , Expression<Func<T , object>> orderBy , EnumHelper.OrderBy ascDesc)
        {
            if (ascDesc == EnumHelper.OrderBy.Desc)
            {
                return Db.From<T>().Where(ep).Top(topNum).OrderByDescending(orderBy).ToList();
            }
            return Db.From<T>().Where(ep).Top(topNum).OrderBy(orderBy).ToList();
        }

        /// <summary>
        ///     通用查询
        /// </summary>
        /// <returns></returns>
        public List<T> QueryToList(Expression<Func<T , bool>> where , Expression<Func<T , object>> orderBy)
        {
            return Db.From<T>().Where(where).OrderBy(orderBy).ToList();
        }

        /// <summary>
        ///     通用查询
        /// </summary>
        /// <returns></returns>
        public List<T> QueryToList(Expression<Func<T , bool>> where , Expression<Func<T , object>> orderBy , EnumHelper.OrderBy ascDesc)
        {
            if (ascDesc == EnumHelper.OrderBy.Desc)
            {
                return Db.From<T>().Where(where).OrderByDescending(orderBy).ToList();
            }
            return QueryToList(where , orderBy);
        }

        public List<T> QueryPageList(int pageIndex , int pageSize , out int totalRecord , Where<T> where , Expression<Func<T , object>> orderBy , EnumHelper.OrderBy ascDesc)
        {
            totalRecord = Db.From<T>().Where(where).Count();
            if (ascDesc == EnumHelper.OrderBy.Desc)
            {
                return Db.From<T>().Where(where).OrderByDescending(orderBy).Page(pageSize , pageIndex).ToList();
            }
            return Db.From<T>().Where(where).OrderBy(orderBy).Page(pageSize , pageIndex).ToList();
        }
        #endregion

        #region 添加

        public int Insert(T entity)
        {
            var id = Db.Insert(entity);
            return id;
        }

        /// <summary>
        ///     插入单个实体
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Insert(DbTrans context , T entity)
        {
            Db.Insert(context , entity);
        }

        public int InsertBatch(List<T> entitylist)
        {
            var count = Db.Insert(entitylist);
            return count;
        }

        public void Insert(DbTrans context , IEnumerable<T> entities)
        {
            Db.Insert(context , entities.ToArray());
        }

        #endregion

        #region 删除数据

        public int Delete(T entity)
        {
            return Db.Delete(entity);
        }
        public int Delete(DbTrans trans , T entity)
        {
            return Db.Delete(trans , entity);
        }
        public int Delete(Where where)
        {
            return Db.Delete<T>(where);
        }
        public int Delete(Expression<Func<T , bool>> where)
        {
            return Db.Delete(where);
        }
        public int Delete(DbTrans trans , Expression<Func<T , bool>> where)
        {
            return Db.Delete(trans , where);
        }


        /// <summary>
        ///     删除多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<T> entities)
        {
            return Db.Delete(entities);
        }

        /// <summary>
        ///     删除单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteById(Guid? id)
        {
            if (id == null)
            {
                return 0;
            }
            return Db.Delete<T>(id.Value);
        }

        public int DeleteById(int? id)
        {
            if (id == null)
            {
                return 0;
            }
            return Db.Delete<T>(id.Value);
        }

        #endregion

        #region 执行sql

        public IDataReader ExecuteSQlToDataReader(string sql)
        {
            var outData = Db.FromSql(sql).ToDataReader();
            return outData;
        }

        public IDataReader ExecuteSQlToDataReader(string sql , DbParameter par)
        {
            var outData = Db.FromSql(sql).AddParameter(par).ToDataReader();
            return outData;
        }

        public DataSet ExecuteSQlToDataSet(string sql)
        {
            var outData = Db.FromSql(sql).ToDataSet();
            return outData;
        }

        public DataSet ExecuteSQlToDataSet(string sql , DbParameter par)
        {
            var outData = Db.FromSql(sql).AddParameter(par).ToDataSet();
            return outData;
        }

        public DataTable ExecuteSQlToDataTable(string sql)
        {
            var outData = Db.FromSql(sql).ToDataTable();
            return outData;
        }

        public DataTable ExecuteSQlToDataTable(string sql , DbParameter par)
        {
            var outData = Db.FromSql(sql).AddParameter(par).ToDataTable();
            return outData;
        }

        public T ExecuteSQlToFirst(string sql)
        {
            var outData = Db.FromSql(sql).ToFirst<T>();
            return outData;
        }

        public T ExecuteSQlToFirst(string sql , DbParameter par)
        {
            var outData = Db.FromSql(sql).AddParameter(par).ToFirst<T>();
            return outData;
        }

        public T ExecuteSQlToFirstDefault(string sql)
        {
            var outData = Db.FromSql(sql).ToFirstDefault<T>();
            return outData;
        }

        public T ExecuteSQlToFirstDefault(string sql , DbParameter par)
        {
            var outData = Db.FromSql(sql).AddParameter(par).ToFirstDefault<T>();
            return outData;
        }

        public List<T> ExecuteSQlToList(string sql)
        {
            var outData = Db.FromSql(sql).ToList<T>();
            return outData;
        }

        public List<T> ExecuteSQlToList(string sql , DbParameter par)
        {
            var outData = Db.FromSql(sql).AddParameter(par).ToList<T>();
            return outData;
        }

        public object ExecuteSQlToScalar(string sql)
        {
            var outData = Db.FromSql(sql).ToScalar();
            return outData;
        }

        public object ExecuteSQlToScalar(string sql , DbParameter par)
        {
            var outData = Db.FromSql(sql).AddParameter(par).ToScalar();
            return outData;
        }

        public T ExecuteSQlToScalarEneity(string sql)
        {
            var outData = Db.FromSql(sql).ToScalar<T>();
            return outData;
        }

        public T ExecuteSQlToScalarEneity(string sql , DbParameter par)
        {
            var outData = Db.FromSql(sql).AddParameter(par).ToScalar<T>();
            return outData;
        }

        #endregion


    }
}
