using King.Domain.WagesEnities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace King.Application.WagesTemplateApp
{
    public interface IWagesTemplateAppService
    {
        Task<List<WagesTemplate>> GetAllByGroupId(Guid groupId);
        Task<Guid> BulkInsert(FileInfo fileInfo);
        Task<List<WagesTemplate2StaffCompare>> Compare(Guid groupId);
        Task WagesImport(Guid groupId, Guid execBy);
    }
}