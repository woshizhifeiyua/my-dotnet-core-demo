using DemoDALUser.Model;
using DemoFarmWork.Enum;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using My.CoreDemo.UserService.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace My.CoreDemo.UserService.Service
{
    public class BaseService : IBaseService
    {
        protected DbContext  Context { get; private set; }

        protected IDbContextFactory _IDbContextFactory { get; private set; }

        public BaseService(IDbContextFactory dbContextFactory)
        {
            _IDbContextFactory = dbContextFactory;
        }

        #region Query
        public T Find<T>(int id, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Rdad) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            return this.Context.Set<T>().Find(id);
        }

        /// <summary>
        /// 不应该暴露给上端使用者，尽量少用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Set<T>( ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Rdad) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            return this.Context.Set<T>();
        }

        /// <summary>
        /// 这才是合理的做法，上端给条件，这里查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Rdad) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            return this.Context.Set<T>().Where<T>(funcWhere);
        }

        public PageResult<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Rdad) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            IQueryable<T> list = this.Context.Set<T>();
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderby);
            }
            else
            {
                list = list.OrderByDescending(funcOrderby);
            }
            PageResult<T> result = new PageResult<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = this.Context.Set<T>().Count(funcWhere)
            };
            return result;
        }
        #endregion

        #region Insert
        /// <summary>
        /// 即使保存  不需要再Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            if (connDbContextEnum == ConnDbContextEnumType.Rdad)
                throw new Exception("插入数据需要 ");
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            this.Context.Set<T>().Add(t);
            this.Commit();//写在这里  就不需要单独commit  不写就需要
            return t;
        }

        public IEnumerable<T> Insert<T>(IEnumerable<T> tList, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            this.Context.Set<T>().AddRange(tList);
            this.Commit();//一个链接  多个sql
            return tList;
        }
        #endregion

        #region Update
        /// <summary>
        /// 是没有实现查询，直接更新的,需要Attach和State
        /// 
        /// 如果是已经在context，只能再封装一个(在具体的service)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Update<T>(T t, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            if (t == null) throw new Exception("t is null");

            this.Context.Set<T>().Attach(t);//将数据附加到上下文，支持实体修改和新实体，重置为UnChanged
            this.Context.Entry<T>(t).State = EntityState.Modified;
            this.Commit();//保存 然后重置为UnChanged
        }

        public void Update<T>(IEnumerable<T> tList, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            foreach (var t in tList)
            {
                this.Context.Set<T>().Attach(t);
                this.Context.Entry<T>(t).State = EntityState.Modified;
            }
            this.Commit();
        }

        #endregion

        #region Delete
        /// <summary>
        /// 先附加 再删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Delete<T>(T t, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            if (t == null) throw new Exception("t is null");
            this.Context.Set<T>().Attach(t);
            this.Context.Set<T>().Remove(t);
            this.Commit();
        }

        /// <summary>
        /// 还可以增加非即时commit版本的，
        /// 做成protected
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        public void Delete<T>(int Id, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            T t = this.Find<T>(Id);//也可以附加
            if (t == null) throw new Exception("t is null");
            this.Context.Set<T>().Remove(t);
            this.Commit();
        }

        public void Delete<T>(IEnumerable<T> tList, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            foreach (var t in tList)
            {
                this.Context.Set<T>().Attach(t);
            }
            this.Context.Set<T>().RemoveRange(tList);
            this.Commit();
        }
        #endregion

        #region Other
        public void Commit()
        {

            this.Context.SaveChanges();
        }

        public IQueryable<T> ExcuteQuery<T>(string sql, SqlParameter[] parameters, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            Context = _IDbContextFactory.CreateContext(connDbContextEnum);
            return this.Context.Set<T>().FromSqlRaw(sql, parameters).AsQueryable();
        }

        public void Excute<T>(string sql, SqlParameter[] parameters, ConnDbContextEnumType connDbContextEnum = ConnDbContextEnumType.Write) where T : class
        {
            IDbContextTransaction trans = null;
            try
            {
                Context = _IDbContextFactory.CreateContext(connDbContextEnum);
                trans = this.Context.Database.BeginTransaction();
                this.Context.Database.ExecuteSqlRaw(sql, parameters);
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
        }

        public virtual void Dispose()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }
        #endregion
    }
}
