using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using King.Domain.WagesEnities;
using OfficeOpenXml;
using King.Domain.IRepositories.WagesIRepositories;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.RegularExpressions;

namespace King.Application.WagesTemplateApp
{
    public class WagesTemplateAppService : IWagesTemplateAppService
    {

        private IWagesTemplateRepository _repository;
        private ILogger<WagesTemplateAppService> _logger;
        public WagesTemplateAppService(IWagesTemplateRepository repository, ILogger<WagesTemplateAppService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<Guid> BulkInsert(FileInfo fileInfo)
        {
            var groupId = Guid.NewGuid();
            List<WagesTemplate> li = new List<WagesTemplate>();
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                if (rowCount < 2)
                {
                    return groupId;
                }
                for (int row = 2; row <= rowCount; row++)
                {
                    var wagesTemplate = new WagesTemplate()
                    {
                        Id = Guid.NewGuid(),
                        CreateTime = DateTime.Now,
                        GroupId = groupId
                    };
                    object staffName = worksheet.Cells[row, 1].Value;
                    if (staffName != null)
                    {
                        wagesTemplate.StaffName = staffName.ToString().Trim();
                    }
                    object idNumber = worksheet.Cells[row, 2].Value;
                    if (idNumber != null)
                    {
                        wagesTemplate.IDNumber = idNumber.ToString().Trim();
                    }
                    object mobileNumber = worksheet.Cells[row, 3].Value;
                    if (mobileNumber != null)
                    {
                        wagesTemplate.MobileNumber = mobileNumber.ToString().Trim();
                    }
                    decimal amount = 0;
                    if (worksheet.Cells[row, 4].Value != null)
                    {
                        decimal.TryParse(worksheet.Cells[row, 4].Value.ToString(), out amount);
                    }
                    wagesTemplate.Amount = amount;
                    li.Add(wagesTemplate);
                }
            }
            await _repository.BulkInsert(li);
            fileInfo.Delete();
            return groupId;
        }

        public async Task<List<WagesTemplate2StaffCompare>> Compare(Guid groupId)
        {
            var list = await _repository.Compare(groupId);
            list.FindAll(it => string.IsNullOrEmpty(it.IDNumber)).ForEach(it =>
            {
                it.IsMapping = 3;
                it.Message = "导入数据“身份证号”字段不能为空！";
            });
            list.FindAll(it => it.IDNumber != null && !Regex.IsMatch(it.IDNumber, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$"))
                .ForEach(it =>
                {
                    it.IsMapping = 3;
                    it.Message = "身份证无效";
                });
            var query = from l in list
                        where l.IsMapping != 3
                        group l by l.IDNumber into g
                        where g.Count() > 1
                        select g;
            foreach (var item in query)
            {
                list.FindAll(l => l.IDNumber != null && l.IDNumber.Equals(item.Key)).ForEach(it =>
                    {
                        it.IsMapping = 3;
                        it.Message = "本次导入存在重复人员";
                    });
            }
            list.FindAll(it => it.IsMapping != 3 ).ForEach(it =>
            {
                it.IsMapping = 3;
                it.Message = "导入金额不正确。";
            });
            var empQuery = from li in list
                           where li.IsMapping == 2 && (string.IsNullOrEmpty(li.StaffName) || string.IsNullOrEmpty(li.IDNumber) || string.IsNullOrEmpty(li.MobileNumber))
                           select li;
            foreach (var item in empQuery)
            {
                item.IsMapping = 3;
                item.Message = "未匹配到员工，系统需要创建员工，但是导入的员工信息不完整，无法完成创建。";
            }
            return list;
        }

        public async Task<List<WagesTemplate>> GetAllByGroupId(Guid groupId)
        {
            return await _repository.GetAllList(wt => wt.GroupId == groupId);
        }

        public async Task WagesImport(Guid groupId, Guid execBy)
        {
            await _repository.WagesImport(groupId, execBy);
        }
    }
}
