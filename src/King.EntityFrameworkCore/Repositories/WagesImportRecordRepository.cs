using King.Domain.IRepositories.WagesIRepositories;
using King.Domain.WagesEnities;
using System;
using System.Collections.Generic;
using System.Text;

namespace King.EntityFrameworkCore.Repositories
{
    public class WagesImportRecordRepository : KingRepositoryBase<WagesImportRecord>,IWagesImportRecordRepository
    {
        public WagesImportRecordRepository(KingDBContext dbContent) : base(dbContent)
        {
        }
    }
}
