using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using King.Application.DepartmentApp.Dtos;
using King.Domain.IRepositories;
using AutoMapper;
using King.Domain.Entities;
using System.Linq;

namespace King.Application.DepartmentApp
{
    public class DepartmentAppService : IDepartmentAppService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentAppService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        public Task<List<DepartmentTreeDto>> ConvertL2T(List<DepartmentDto> departments)
        {
            var retLi = new List<DepartmentTreeDto>();
            foreach (var item in departments)
            {
                if (departments.Find(it => it.Id == item.ParentId) == null)
                {
                    retLi.Add(new DepartmentTreeDto()
                    {
                        Id = item.Id,
                        Code = item.Code,
                        ContactNumber = item.ContactNumber,
                        Name = item.Name,
                        ParentId = item.ParentId,
                        CreateTime = item.CreateTime,
                        CreateUserId = item.CreateUserId,
                        IsDeleted = item.IsDeleted,
                        Manager = item.Manager,
                        Reamrks = item.Reamrks,
                        Childs = DepartmentRec(item.Id, departments)
                    });
                }
            }
            return Task.FromResult(retLi);
        }

        private List<DepartmentTreeDto> DepartmentRec(Guid pId, List<DepartmentDto> departments)
        {
            var childs = departments.FindAll(m => m.ParentId == pId);
            List<DepartmentTreeDto> retLi = new List<DepartmentTreeDto>();
            foreach (var item in childs)
            {
                retLi.Add(new DepartmentTreeDto()
                {
                    Id = item.Id,
                    Code = item.Code,
                    ContactNumber = item.ContactNumber,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    CreateTime = item.CreateTime,
                    CreateUserId = item.CreateUserId,
                    IsDeleted = item.IsDeleted,
                    Manager = item.Manager,
                    Reamrks = item.Reamrks,
                    Childs = DepartmentRec(item.Id, departments)
                });
            }
            return retLi;
        }

        public async Task Delete(Guid id)
        {
            var department = await _departmentRepository.Get(id);
            department.IsDeleted = 1;
            await _departmentRepository.Update(department);
            //递归删除所有子类
            var childs = await _departmentRepository.GetAllList(it => it.ParentId == id);
            foreach (var item in childs)
            {
                await Delete(item.Id);
            }
        }

        public async Task DeleteBatch(List<Guid> ids)
        {
            foreach (var item in ids)
            {
                await Delete(item);
            }
        }

        public async Task<DepartmentDto> Get(Guid id)
        {
            return Mapper.Map<DepartmentDto>(await _departmentRepository.Get(id));
        }

        public async Task<List<DepartmentDto>> GetAll()
        {
            var departments = await _departmentRepository.GetAllList(d=>d.IsDeleted.Equals(0));
            return Mapper.Map<List<DepartmentDto>>(departments);
        }

        public async Task<PageData<DepartmentDto>> GetDepartmentByParent(Guid parentId, int startPage, int pageSize)
        {
            var departments = await _departmentRepository.LoadPageList(startPage,
                pageSize,
                it => it.ParentId == parentId && it.IsDeleted == 0,
                it => it.Name);
            var dataList = Mapper.Map<List<DepartmentDto>>(departments.Rows.ToList());
            return new PageData<DepartmentDto>()
            {
                Rows = dataList.AsQueryable<DepartmentDto>(),
                Total = departments.Total
            };
        }

        public async Task<DepartmentDto> InsertOrUpdate(DepartmentDto dot)
        {
            var department = await _departmentRepository.InsertOrUpdate(Mapper.Map<Department>(dot));
            return Mapper.Map<DepartmentDto>(department);
        }
    }
}
