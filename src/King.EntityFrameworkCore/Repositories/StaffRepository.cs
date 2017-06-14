using King.Domain.IRepositories.WagesIRepositories;
using King.Domain.WagesEnities;
using System;
using System.Collections.Generic;
using System.Text;

namespace King.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// 员工Repository
    /// </summary>
    public class StaffRepository : KingRepositoryBase<Staff>,IStaffRepository
    {
        public StaffRepository(KingDBContext dbContent) : base(dbContent)
        {
        }
    }
}
