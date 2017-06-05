using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace King.Application.MenuApp.Dtos
{
    public class MenuDto
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int SerialNumber { get; set; }
        [Required(ErrorMessage = "功能名称不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "功能编码不能为空")]
        public string Code { get; set; }
        public string Url { get; set; }
        [Required(ErrorMessage = "菜单类型不能为空")]
        public int Type { get; set; }
        public string Icon { get; set; }
        public string Remarks { get; set; }
    }

    
}
