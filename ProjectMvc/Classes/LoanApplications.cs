using System;

namespace ProjectMvc.Classes
{
    public class LoanApplications
    {
        public int Id { get; set; }
        public int LoanTypeId { get; set; }
        public string LoanType { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime LoanPeriod { get; set; }
        public string LoanPeriodStr { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
    }
}
