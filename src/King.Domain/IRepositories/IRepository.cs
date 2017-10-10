using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace King.Domain.IRepositories
{
    /// <summary>
    /// 仓储接口定义
    /// </summary>
    public interface IRepository
    {
    }
    /// <summary>
    /// 泛型仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IRepository<TEntity, TPrimaryKey> : IRepository where TEntity : Entity<TPrimaryKey>
    {
        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllList();
        /// <summary>
        /// 根据lambda表达式获取实体集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAllList(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> Get(TPrimaryKey id);
        /// <summary>
        /// 根据lambda表达式条件获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        Task<TEntity> Insert(TEntity entity, bool autoSave = true);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        Task<TEntity> Update(TEntity entity, bool autoSave = true);
        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        Task<TEntity> InsertOrUpdate(TEntity entity, bool autoSave = true);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        Task Delete(TEntity entity, bool autoSave = true);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        Task Delete(TPrimaryKey id, bool autoSave = true);
        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        /// <param name="autoSave">是否立即执行保存</param>
        Task Delete(Expression<Func<TEntity, bool>> where, bool autoSave = true);

        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="startPage">起始页</param>
        /// <param name="pageSize">页面条目</param>
        /// <param name="rowCount">数据总行</param>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        Task<PageData<TEntity>> LoadPageList(int startPage, int pageSize, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order);
        /// <summary>
        /// 立即执行保存
        /// </summary>
        Task Save();
    }
    /// <summary>
    /// 默认Guid 主键类型仓储接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IRepository<TEntity, Guid> where TEntity : Entity
    {

    }

    /// <summary>
    /// 查询数据实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T>
    {
        /// <summary>
        /// 查询的数据
        /// </summary>
        public IQueryable<T> Rows { get; set; }
        /// <summary>
        /// 总数据数
        /// </summary>
        public int Total { get; set; }
    }
    public class PageData2<T>
    {
        /// <summary>
        /// 查询的数据
        /// </summary>
        public List<T> Rows { get; set; }
        /// <summary>
        /// 总数据数
        /// </summary>
        public Int64 Total { get; set; }
    }

}
