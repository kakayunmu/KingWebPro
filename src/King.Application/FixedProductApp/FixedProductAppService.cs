using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using King.Application.FixedProductApp.Dtos;
using King.Domain.IRepositories;
using King.Domain.IRepositories.WagesIRepositories;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Linq;
using King.Domain.WagesEnities;

namespace King.Application.FixedProductApp
{
    public class FixedProductAppService : IFixedProductAppService
    {
        private readonly IFixedProductRepository _repository;
        private readonly ILogger<FixedProductAppService> _logger;

        public FixedProductAppService(IFixedProductRepository repository, ILogger<FixedProductAppService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Delete(Guid id)
        {
            var fixedProduct = await _repository.Get(id);
            fixedProduct.IsDel = 1;
            await _repository.Update(fixedProduct);
        }

        public async Task DeleteBatch(List<Guid> ids)
        {
            foreach (var item in ids)
            {
                await Delete(item);
            }
        }

        public async Task<FixedProductDto> Get(Guid id)
        {
            return Mapper.Map<FixedProductDto>(await _repository.Get(id));
        }

        public async Task<PageData<FixedProductDto>> GetFixedProductByPage(int startPage, int pageSize)
        {
            var fixedProducts = await _repository.LoadPageList(startPage, pageSize, f => f.IsDel == 0, f => f.DataState);
            var dataList = Mapper.Map<List<FixedProductDto>>(fixedProducts.Rows.ToList());
            return new PageData<FixedProductDto>()
            {
                Rows = dataList.AsQueryable<FixedProductDto>(),
                Total = fixedProducts.Total
            };
        }

        public async Task<bool> InsertOrUpdate(FixedProductDto dto)
        {
            try
            {
                var fp = await _repository.InsertOrUpdate(Mapper.Map<FixedProduct>(dto));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

        }
    }
}
