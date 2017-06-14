using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using King.Application.StaffApp.Dtos;
using King.Domain.IRepositories.WagesIRepositories;
using Microsoft.Extensions.Logging;
using AutoMapper;
using King.Domain.WagesEnities;
using King.Domain.IRepositories;
using System.Linq;

namespace King.Application.StaffApp
{
    public class StaffAppService : IStaffAppService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly ILogger<StaffAppService> _logger;
        public StaffAppService(IStaffRepository staffRepository, ILogger<StaffAppService> logger)
        {
            _staffRepository = staffRepository;
            _logger = logger;
        }
        public async Task<bool> CheckStaff(string IDNumber)
        {
            var staffs = await _staffRepository.GetAllList(s => s.IDNumber == IDNumber);
            if (staffs != null && staffs.Count > 0)
            {
                return false;
            }
            return true;
        }

        public async Task Delete(Guid id)
        {
            var staff = await _staffRepository.Get(id);
            staff.IsDel = 1;
            await _staffRepository.Update(staff);
        }

        public async Task<bool> DeleteBatch(List<Guid> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    await Delete(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<StaffDto> Get(Guid id)
        {
            return Mapper.Map<StaffDto>(await _staffRepository.Get(id));
        }

        public async Task<PageData<StaffDto>> GetAllStaff(int startPage, int pageSize)
        {
            var staff = await _staffRepository.LoadPageList(startPage, pageSize, s => s.IsDel == 0, s => s.CreateTime);
            var dataList = Mapper.Map<List<StaffDto>>(staff.Rows.ToList());
            return new PageData<StaffDto>()
            {
                Rows = dataList.AsQueryable<StaffDto>(),
                Total = staff.Total
            };
        }

        public async Task<StaffDto> InsertOrUpdate(StaffDto dto)
        {
            var staff = await _staffRepository.InsertOrUpdate(Mapper.Map<Staff>(dto));
            return Mapper.Map<StaffDto>(staff);
        }
    }
}
