using System;
using System.Collections.Generic;
using System.Text;
using King.Domain.Entities;
using King.Domain.IRepositories;

namespace King.EntityFrameworkCore.Repositories
{
    public class DepartmentRepository : KingRepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(KingDBContext dbContent) : base(dbContent)
        {
        }
    }
}
