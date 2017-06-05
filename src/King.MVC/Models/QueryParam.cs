using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace King.MVC.Models
{
    public class QueryParam
    {
        public int StartPage { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
        public string Sort { get; set; }
        public List<SearchFeld> SearchFelds { get; set; }
    }
    public class SearchFeld
    {
        public string Field { get; set; }
        public string Val { get; set; }
        public string SType { get; set; }
        public string GroupId { get; set; }
    }
}
