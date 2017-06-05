using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.Entities;
using King.Domain.IRepositories;

namespace King.EntityFrameworkCore.Repositories
{
    public class MenuRepository : KingRepositoryBase<Menu>, IMenuRepository
    {
        public MenuRepository(KingDBContext dbContent) : base(dbContent)
        {
        }
    }
}
