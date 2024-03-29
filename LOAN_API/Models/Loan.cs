using System.ComponentModel.DataAnnotations.Schema;

namespace LOAN_API.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Column(TypeName = "varchar(255)")]
        public LoanType LoanType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int Period { get; set; }
        public LoanStatus Status { get; set; }

     
    }

    public enum LoanType
    {
        QuickLoan = 1,
        AutoLoan = 2,
        InstallmentLoan = 3
    }

    public enum LoanStatus
    {
        InProcess = 1,
        Approved = 2,
        Rejected = 3
    }
}
