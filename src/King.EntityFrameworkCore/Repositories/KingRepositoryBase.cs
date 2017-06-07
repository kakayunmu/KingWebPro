using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.IRepositories;
using King.Domain;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace King.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class KingRepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        /// <summary>
        /// 数据访问上下文对象
        /// </summary>
        protected readonly KingDBContext _dbContext;

        /// <summary>
        /// 通过构造函数注入得到数据上下文对象实例
        /// </summary>
        /// <param name="dbContext"></param>
        public KingRepositoryBase(KingDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 通过主键删除实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public virtual async Task Delete(TPrimaryKey id, bool autoSave = true)
        {
            _dbContext.Set<TEntity>().Remove(await Get(id));
            if (autoSave)
                await Save();
        }
        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="where">lambda表达式</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public virtual async Task Delete(Expression<Func<TEntity, bool>> where, bool autoSave = true)
        {
            _dbContext.Set<TEntity>().Where(where).ToList().ForEach(it => _dbContext.Set<TEntity>().Remove(it));
            if (autoSave)
                await Save();
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">要删除的实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        public virtual async Task Delete(TEntity entity, bool autoSave = true)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            if (autoSave)
                await Save();

        }
        /// <summary>
        /// 根据条件获取单个实体
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public virtual Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(_dbContext.Set<TEntity>().FirstOrDefault(predicate));
        }
        /// <summary>
        /// 根据主键获取实体
        /// </summary>
        /// <param name="id">实体主键</param>
        /// <returns></returns>
        public virtual Task<TEntity> Get(TPrimaryKey id)
        {
            return Task.FromResult(_dbContext.Set<TEntity>().FirstOrDefault(CreateEqualityExpressionForId(id)));
        }

        /// <summary>
        /// 获取实体集合
        /// </summary>
        /// <returns></returns>
        public virtual Task<List<TEntity>> GetAllList()
        {
            return Task.FromResult(_dbContext.Set<TEntity>().ToList());
        }

        /// <summary>
        /// 根据条件获取实体集合
        /// </summary>
        /// <param name="predicate">lambda表达式条件</param>
        /// <returns></returns>
        public virtual Task<List<TEntity>> GetAllList(Expression<Func<TEntity, bool>> predicate)
        {
            return Task.FromResult(_dbContext.Set<TEntity>().Where(predicate).ToList());
        }

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public virtual async Task<TEntity> Insert(TEntity entity, bool autoSave = true)
        {
            _dbContext.Set<TEntity>().Add(entity);
            if (autoSave)
                await Save();
            return entity;
        }
        /// <summary>
        /// 新增或更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public virtual async Task<TEntity> InsertOrUpdate(TEntity entity, bool autoSave = true)
        {
            var retEnt = await Get(entity.Id);
            if (retEnt != null)
                return await Update(entity, autoSave);
            return await Insert(entity, autoSave);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="startPage">页码</param>
        /// <param name="pageSize">单页数据数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="where">条件</param>
        /// <param name="order">排序</param>
        /// <returns></returns>
        public virtual Task<PageData<TEntity>> LoadPageList(int startPage, int pageSize, Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, object>> order)
        {
            var result = from p in _dbContext.Set<TEntity>()
                         select p;
            if (where != null)
                result = result.Where(where);
            if (order != null)
                result = result.OrderBy(order);
            else
                result = result.OrderBy(m => m.Id);            
            return Task.FromResult(new PageData<TEntity>
            {
                Rows = result.Skip((startPage - 1) * pageSize).Take(pageSize),
                Total = result.Count()
            });
        }
        /// <summary>
        /// 事务性保存
        /// </summary>
        public virtual async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="autoSave">是否立即执行保存</param>
        /// <returns></returns>
        public virtual async Task<TEntity> Update(TEntity entity, bool autoSave = true)
        {
            var obj = await Get(entity.Id);
            await EntityToEntity(entity, obj);
            if (autoSave)
                await Save();
            return entity;
        }
        private Task EntityToEntity<T>(T pTargetObjSrc, T pTargetObjDest)
        {
            foreach (var mItem in typeof(T).GetProperties())
            {
                mItem.SetValue(pTargetObjDest, mItem.GetValue(pTargetObjSrc, new object[] { }), null);
            }
            return Task.FromResult(0);
        }
        /// <summary>
        /// 根据主键构建判断表达式
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));
            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
                );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

    }

    /// <summary>
    /// 主键为Guid类型的仓储基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class KingRepositoryBase<TEntity> : KingRepositoryBase<TEntity, Guid> where TEntity : Entity
    {
        public KingRepositoryBase(KingDBContext dbContent) : base(dbContent)
        {

        }
    }
}
