using CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CL.Interfaces
{
     /// <summary>
    /// EF操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : MappingBase<T>
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        bool Add(T model);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        bool Update(T model);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filterExpression"></param>
        /// <returns></returns>
        bool BatchUpdate(ICollection<T> list, Expression<Func<T, bool>> deleteFilterExpression);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool BatchUpdate(ICollection<T> list);

        /// <summary>
        /// 批量更新,不使用事务
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool BatchUpdateNoTransaction(ICollection<T> list);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="list"></param>
        /// <param name="deleteFilterExpression"></param>
        /// <returns></returns>
        bool BatchInsert(ICollection<T> list, Expression<Func<T, bool>> deleteFilterExpression);

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="tableName"></param>
        /// <param name="sbcMappings"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool BatchInsert(System.Data.IDataReader reader, string tableName, System.Data.SqlClient.SqlBulkCopyColumnMapping[] sbcMappings, string sql);



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        bool Delete(T model);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deleteFilterExpression"></param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> deleteFilterExpression);


        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        bool of_ExceSql(string sql);
        /// <summary>
        /// 返回全部
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> All();
        /// <summary>
        /// 条件筛选查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        IEnumerable<T> Filter(Expression<Func<T, bool>> filter, string orderBy = null, bool ascending = true);
        /// <summary>
        /// 条件筛选分页查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="total">返回记录数</param>
        /// <param name="index">页号从0开始</param>
        /// <param name="size">分页大小，默认为20</param>
        /// <param name="orderBy">排序列</param>
        /// <param name="ascending">默认升序</param>
        /// <returns></returns>
        IEnumerable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 20, string orderBy = null, bool ascending = true);
        /// <summary>
        /// 条件筛选分页查询
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="total">返回记录数</param>
        /// <param name="index">页号从0开始</param>
        /// <param name="size">分页大小，默认为20</param>
        /// <param name="jtSort">jqtable排序参数</param>
        /// <returns></returns>
        IEnumerable<T> Filter(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 20, string jtSorting = "");
        /// <summary>
        /// 返回查询对象（尚未执行查询）
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="ascending"></param>
        /// <returns></returns>
        IQueryable<T> FilterNoList(Expression<Func<T, bool>> filter, string orderBy = null, bool ascending = true);



        /// <summary>
        /// 主键查询
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        T GetByKey(params Object[] keys);
        /// <summary>
        /// 根据条件返回单一实体
        /// 查无数据返回null
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy">排序列</param>
        /// <param name="ascending">默认升序</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> filter, string orderBy = null, bool ascending = true);

        /// <summary>
        /// 执行Sql命令
        /// 除非必要,否则不建意使用
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        System.Data.DataTable of_GetDataTable(string sql);
    }
}
