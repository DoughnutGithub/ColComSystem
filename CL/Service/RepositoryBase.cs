using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Interfaces;
using CL.DB;
using CL.Models;
using System.Data.Entity;
using System.Reflection;
using System.Linq.Expressions;
using EntityFramework.Extensions;

namespace CL.Service
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryBase<T> : IRepository<T> where T : MappingBase<T>
    {


        private EFHelper _db;
        private readonly IDbSet<T> _dbSet;
        public IDbSet<T> DbSet { get { return _dbSet; } }
        protected IDatabaseFactory DatabaseFactory { get; private set; }
        protected EFHelper DB { get { return _db ?? (_db = DatabaseFactory.Get()); } }
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="dbFactory"></param>
        public RepositoryBase(IDatabaseFactory dbFactory)
        {
            DatabaseFactory = dbFactory;
            _dbSet = DB.Set<T>();
        }
        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="model"></param>
        public virtual bool Add(T model)
        {
            try
            {
                _dbSet.Add(model);
                _db.Commit();
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Trace.TraceInformation("Property:{0} Error:{1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
                throw dbEx;

            }
            catch (Exception ex)
            {

                throw new Exception(CL.Common.Util.GetModelMessage<T>(model), ex);


            }
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="model"></param>
        public virtual bool Delete(T model)
        {
            try
            {
                _dbSet.Remove(model);
                _db.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(CL.Common.Util.GetModelMessage<T>(model), ex);
            }
        }

        public virtual bool Delete(Expression<Func<T, bool>> deleteFilterExpression)
        {
            try
            {
                _dbSet.Where(deleteFilterExpression).Delete();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public virtual T GetByKey(params object[] keys)
        {
            return _dbSet.Find(keys);
        }

        public virtual T Find(Expression<Func<T, bool>> filter, string orderBy = null, bool ascending = true)
        {
            if (orderBy == null)
                return _dbSet.FirstOrDefault(filter);
            else
            {
                var _resetSet = filter != null ? _dbSet.Where(filter) : _dbSet;
                return _resetSet.OrderBy(orderBy, ascending).FirstOrDefault();
            }

        }

        public virtual IEnumerable<T> All()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<T> Filter(Expression<Func<T, bool>> filter, string orderBy = null, bool ascending = true)
        {
            if (orderBy == null)
                return _dbSet.Where(filter).ToList();
            else
                return _dbSet.Where(filter).OrderBy(orderBy, ascending).ToList();
        }

        public virtual IEnumerable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 20, string jtSorting = "")
        {
            String[] array = Common.StringHelper.of_SplitString(jtSorting, " ");
            return Filter(filter, out total, index, size, array.Length == 2 ? array[0] : null, (array.Length == 2 && array[1].Equals("asc", StringComparison.CurrentCultureIgnoreCase)) ? true : false);
        }

        public virtual IEnumerable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 20, string orderBy = null, bool ascending = true)
        {
            int skipCount = index * size;
            var _resetSet = filter != null ? _dbSet.Where(filter) : _dbSet;
            total = _resetSet.Count();
            if (orderBy != null)
            {
                _resetSet = _resetSet.OrderBy(orderBy, ascending);
            }
            if (size > 0)
                _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);

            return _resetSet.ToList();
        }
        public IQueryable<T> FilterNoList(Expression<Func<T, bool>> filter, string orderBy = null, bool ascending = true)
        {
            if (orderBy == null)
                return _dbSet.Where(filter);
            else
                return _dbSet.Where(filter).OrderBy(orderBy, ascending);
        }
        public virtual bool Update(T model)
        {
            try
            {
                _dbSet.Attach(model);
                _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                _db.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(CL.Common.Util.GetModelMessage<T>(model), ex);
            }
        }

        public virtual bool BatchUpdate(ICollection<T> list)
        {
            return BatchUpdate(list, null);
        }

        public virtual bool BatchUpdate(ICollection<T> list, Expression<Func<T, bool>> deleteFilterExpression)
        {
            CL.DB.MyConfiguration.SuspendExecutionStrategy = true;
            bool reval = false;
            using (var ts = _db.Database.BeginTransaction())
            {
                try
                {
                    if (deleteFilterExpression != null)
                        _dbSet.Where(deleteFilterExpression).Delete();
                    foreach (T model in list)
                    {
                        switch (model.DALStatus)
                        {
                            case Types.DALType.delete:
                                _dbSet.Remove(model);
                                break;
                            case Types.DALType.insert:
                                _dbSet.Add(model);
                                break;
                            case Types.DALType.update:
                                _dbSet.Attach(model);
                                _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                                break;
                        }
                    }
                    _db.Commit();
                    ts.Commit();
                    reval = true;
                }

                catch (Exception ex)
                {
                    var updateEx = ex.InnerException as System.Data.Entity.Core.UpdateException;
                    ts.Rollback();
                    if (updateEx != null && updateEx.StateEntries.Count > 0)
                        throw new Exception(CL.Common.Util.GetModelMessage1(updateEx.StateEntries[0].Entity), ex);
                    else
                        throw ex;



                }

            }
            CL.DB.MyConfiguration.SuspendExecutionStrategy = false;
            return reval;


        }



        public virtual bool BatchUpdateNoTransaction(ICollection<T> list)
        {
            try
            {
                foreach (T model in list)
                {
                    switch (model.DALStatus)
                    {
                        case Types.DALType.delete:
                            _dbSet.Remove(model);
                            break;
                        case Types.DALType.insert:
                            _dbSet.Add(model);
                            break;
                        case Types.DALType.update:
                            _dbSet.Attach(model);
                            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                            break;
                    }
                }
                _db.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public virtual bool BatchInsert(ICollection<T> list, Expression<Func<T, bool>> deleteFilterExpression)
        {
            var dr = list.AsDataReader();
            bool reval = false;
            System.Data.SqlClient.SqlConnection connection = (System.Data.SqlClient.SqlConnection)_db.Database.Connection;
            System.Data.SqlClient.SqlTransaction tran = null;
            try
            {

                if (deleteFilterExpression != null)
                    _dbSet.Where(deleteFilterExpression).Delete();
                _db.Commit();
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                tran = connection.BeginTransaction();
                using (var sbc = new System.Data.SqlClient.SqlBulkCopy(connection, System.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                {
                    sbc.DestinationTableName = typeof(T).GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.TableAttribute>().Name;

                    PropertyInfo[] pi = CL.Common.StaticCache<Type, PropertyInfo[]>.Get(typeof(T), p => p.GetProperties());
                    foreach (PropertyInfo p in pi)
                    {
                        System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute nma = p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute>();
                        if (nma != null) continue;

                        System.ComponentModel.DataAnnotations.Schema.ColumnAttribute ca = p.GetCustomAttribute<System.ComponentModel.DataAnnotations.Schema.ColumnAttribute>();
                        if (ca != null)
                        {
                            sbc.ColumnMappings.Add(new System.Data.SqlClient.SqlBulkCopyColumnMapping(p.Name, ca.Name));
                        }
                        else
                            sbc.ColumnMappings.Add(new System.Data.SqlClient.SqlBulkCopyColumnMapping(p.Name, p.Name));
                    }
                    sbc.WriteToServer(dr);

                }

                tran.Commit();
                reval = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();

            }
            finally
            {
                if (tran != null) tran.Dispose();
                dr.Close(); dr.Dispose();
                if (connection.State == System.Data.ConnectionState.Open) connection.Close();
            }
            return reval;

        }

        public bool BatchInsert(System.Data.IDataReader reader, string tableName, System.Data.SqlClient.SqlBulkCopyColumnMapping[] sbcMappings, string sql)
        {
            bool reval = false;
            System.Data.SqlClient.SqlConnection connection = (System.Data.SqlClient.SqlConnection)_db.Database.Connection;
            System.Data.SqlClient.SqlTransaction tran = null;
            try
            {
                if (!String.IsNullOrEmpty(sql))
                    this.of_ExceSql(sql);
                _db.Commit();
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();
                tran = connection.BeginTransaction();
                using (var sbc = new System.Data.SqlClient.SqlBulkCopy(connection, System.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                {
                    sbc.DestinationTableName = tableName;
                    foreach (System.Data.SqlClient.SqlBulkCopyColumnMapping sbcMapping in sbcMappings)
                    {
                        sbc.ColumnMappings.Add(sbcMapping);
                    }
                    sbc.BulkCopyTimeout = 1800;
                    sbc.WriteToServer(reader);
                }
                tran.Commit();
                reval = true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                if (tran != null) tran.Dispose();
                reader.Close(); reader.Dispose();
                if (connection.State == System.Data.ConnectionState.Open) connection.Close();
            }
            return reval;
        }

        public bool of_ExceSql(string sql)
        {
            bool result = false;
            try
            {
                _db.Database.CommandTimeout = _db.Database.Connection.ConnectionTimeout;
                _db.Database.ExecuteSqlCommand(sql);
                result = true;
            }
            catch (Exception ex)
            {
                throw new Exception(sql, ex);
            }
            return result;

        }



        public System.Data.DataTable of_GetDataTable(string sql)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            var connection = ((System.Data.SqlClient.SqlConnection)DB.Database.Connection);
            if (connection != null && connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, connection))
            {
                try
                {
                    da.Fill(dt);
                }
                catch
                { }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }




    }

    public class RepositoryBase
    {
        private EFHelper _db;
        protected IDatabaseFactory DatabaseFactory { get; private set; }
        public EFHelper DB { get { return _db ?? (_db = DatabaseFactory.Get()); } }

        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="dbFactory"></param>
        public RepositoryBase(IDatabaseFactory dbFactory)
        {
            DatabaseFactory = dbFactory;
        }

        public System.Data.DataTable of_GetDataTable(string sql)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            var connection = ((System.Data.SqlClient.SqlConnection)DB.Database.Connection);
            if (connection != null && connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, connection))
            {
                try
                {
                    da.Fill(dt);
                }
                catch
                { }
                finally
                {
                    connection.Close();
                }
            }
            return dt;
        }

    }

    public static class LinqExpress
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending) where T : class
        {
            Type type = typeof(T);

            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
                throw new ArgumentException("propertyName", "Not Exist");

            ParameterExpression param = Expression.Parameter(type, "p");
            Expression propertyAccessExpression = Expression.MakeMemberAccess(param, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccessExpression, param);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
