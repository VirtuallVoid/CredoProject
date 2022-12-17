using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class LoanApplications : BaseEntity
    {
        public string LoanType { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime LoanPeriod { get; set; }
        public string Status { get; set; }
    }
}
