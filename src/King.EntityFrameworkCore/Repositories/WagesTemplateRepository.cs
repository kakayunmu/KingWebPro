using King.Domain.IRepositories.WagesIRepositories;
using King.Domain.WagesEnities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace King.EntityFrameworkCore.Repositories
{
    public class WagesTemplateRepository : KingRepositoryBase<WagesTemplate>, IWagesTemplateRepository
    {
        public WagesTemplateRepository(KingDBContext dbContent) : base(dbContent)
        {
        }

        public async Task BulkInsert(List<WagesTemplate> dataList)
        {
            _dbContext.WagesTemplates.AddRange(dataList.ToArray());
            await Save();
        }

        public Task<List<WagesTemplate2StaffCompare>> Compare(Guid groupId)
        {
            var query = from wt in _dbContext.WagesTemplates
                        join sf in _dbContext.Staffs on wt.IDNumber equals sf.IDNumber into temp
                        from sf in temp.DefaultIfEmpty()
                        where wt.GroupId.Equals(groupId)
                        select new WagesTemplate2StaffCompare()
                        {
                            Id = wt.Id,
                            Amount = wt.Amount,
                            CreateTime = wt.CreateTime,
                            GroupId = wt.GroupId,
                            IDNumber = wt.IDNumber,
                            MobileNumber = wt.MobileNumber,
                            StaffName = wt.StaffName,
                            IsMapping = sf != null ? 1 : 2,
                            Message = (sf == null ? "未找到员工，本次导入新建" : string.Format("找到已存在员工。信息：{0} 手机：{1} 身份证：{2}", sf.Name, sf.MobileNumber, sf.IDNumber))
                        };
            return Task.FromResult(query.ToList<WagesTemplate2StaffCompare>());
        }

        public async Task WagesImport(Guid groupId, Guid execBy)
        {
            var query = from wt in _dbContext.WagesTemplates
                        join sf in _dbContext.Staffs
                        on wt.IDNumber
                        equals sf.IDNumber into temp
                        from sf in temp.DefaultIfEmpty()
                        where wt.GroupId.Equals(groupId)
                        select new
                        {
                            Id = wt.Id,
                            IDNumber = wt.IDNumber,
                            MobileNumber = wt.MobileNumber,
                            StaffName = wt.StaffName,
                            Amount = wt.Amount,
                            IsMapping = sf != null ? 1 : 2,
                            StaffId = sf != null ? sf.Id : Guid.Empty
                        };


            foreach (var item in query)
            {
                //更新员工活期总额
                var staffObj = _dbContext.Staffs.FirstOrDefault(it => it.Id.Equals(item.StaffId));
                if (staffObj == null)
                {
                    staffObj = new Staff()
                    {
                        Id = Guid.NewGuid(),
                        CreateTime = DateTime.Now,
                        CurrentAmount = 0,
                        FixedAmount = 0,
                        HeadImg = "img / user3 - 160x160.jpg",
                        IDNumber = item.IDNumber,
                        IsDel = 0,
                        MobileNumber = item.MobileNumber,
                        Name = item.StaffName,
                        Password = King.Utility.Security.Encryption.Md5WithSalt("KingWeb", "123456")
                    };
                    _dbContext.Staffs.Add(staffObj);
                }
                staffObj.CurrentAmount += item.Amount;
                //添加活期存款记录
                _dbContext.CurrentDeposits.Add(new CurrentDeposit
                {
                    Id = Guid.NewGuid(),
                    Amount = item.Amount,
                    CreateTime = DateTime.Now,
                    MType = 1,
                    Remarks = DateTime.Now.ToString("yyyy年MM月 工资发放"),
                    StaffId = staffObj.Id
                });

            }
            //增加工资导入记录
            _dbContext.WagesImportRecord.Add(new WagesImportRecord()
            {
                Id = Guid.NewGuid(),
                CreateBy = execBy,
                CreateTime = DateTime.Now,
                GrpupId = groupId
            });
            await Save();
        }
    }
}
