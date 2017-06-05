using System;

namespace King.Domain
{
    /// <summary>
    /// 泛型实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public abstract class Entity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual TPrimaryKey Id { get; set; }
    }

    /// <summary>
    /// 定义默认主键类型为Guid 的实体基类
    /// </summary>
    public abstract class Entity : Entity<Guid>
    {

    }
}
