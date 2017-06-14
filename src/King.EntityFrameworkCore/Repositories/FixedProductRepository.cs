using King.Domain.IRepositories.WagesIRepositories;
using King.Domain.WagesEnities;
using System;
using System.Collections.Generic;
using System.Text;

namespace King.EntityFrameworkCore.Repositories
{
    public class FixedProductRepository : KingRepositoryBase<FixedProduct>, IFixedProductRepository
    {
        public FixedProductRepository(KingDBContext dbContent) : base(dbContent)
        {
        }
    }
}
