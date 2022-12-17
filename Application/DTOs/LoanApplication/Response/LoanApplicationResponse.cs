using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.LoanApplication.Response
{
    public class LoanApplicationResponse
    {
        public int LoanTypeId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime LoanPeriod { get; set; }
        public int StatusId { get; set; }
    }
}
