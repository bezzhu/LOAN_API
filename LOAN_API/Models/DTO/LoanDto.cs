namespace LOAN_API.Models.DTO
{
    public class LoanDto
    {
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int Period { get; set; }
    }
}
