using King.Application.FixedProductApp.Dtos;
using King.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace King.Application.FixedProductApp
{
    public interface IFixedProductAppService
    {
        Task<bool> InsertOrUpdate(FixedProductDto dto);
        Task<PageData<FixedProductDto>> GetFixedProductByPage(int startPage, int pageSize);
        Task DeleteBatch(List<Guid> ids);
        Task Delete(Guid id);
        Task<FixedProductDto> Get(Guid id);
       
    }
}
