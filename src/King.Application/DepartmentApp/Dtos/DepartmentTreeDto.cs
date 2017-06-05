using System;
using System.Collections.Generic;
using System.Text;

namespace King.Application.DepartmentApp.Dtos
{
    /// <summary>
    /// 树形结果的部门dto
    /// </summary>
    public class DepartmentTreeDto : DepartmentDto
    {
        public List<DepartmentTreeDto> Childs { get; set; }
    }
}
