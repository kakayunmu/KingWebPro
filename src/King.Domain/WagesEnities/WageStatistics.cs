using System;
using System.Collections.Generic;
using System.Text;

namespace King.Domain.WagesEnities
{
    public class WageStatistics
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public string MobileNumber { get; set; }
        public string IDNumber { get; set; }
        public string CurrDate { get; set; }
        public decimal CurrAmount { get; set; }
        public decimal WageAmount { get; set; }
        public decimal FixAmount { get; set; }
    }
}
