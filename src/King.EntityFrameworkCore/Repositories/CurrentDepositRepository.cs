using King.Domain.IRepositories.WagesIRepositories;
using King.Domain.WagesEnities;
using System;
using System.Collections.Generic;
using System.Text;

namespace King.EntityFrameworkCore.Repositories
{
    public class CurrentDepositRepository : KingRepositoryBase<CurrentDeposit>, ICurrentDepositRepository
    {
        public CurrentDepositRepository(KingDBContext dbContent) : base(dbContent)
        {
        }
    }
}
