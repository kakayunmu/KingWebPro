using System;
using System.Collections.Generic;
using System.Text;

namespace King.Application.MenuApp.Dtos
{
    /// <summary>
    /// 树形结果的功能Dto
    /// </summary>
    public class MenuTreeDto : MenuDto
    {
        public bool Active { get; set; }
        public List<MenuTreeDto> Childs { get; set; }
    }
}
