using LOAN_API.Models.DTO;
using LOAN_API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using LOAN_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace LOAN_API.Services
{
    public interface IAccoutantService
    {
        Task<ActionResult<IEnumerable<Loan>>> GetLoansAsync(int userId);
        Task UpdateLoanAsync(int loanId, LoanDto loanDto);
        Task DeleteLoanAsync(int loanId);
        Task BlockUserAsync(int userId);
    }
    public class AccoutantService : IAccoutantService
    {
        private readonly UserDbContext _context;
        public AccoutantService(UserDbContext context)
        {
            _context = context;
        }
        public async Task BlockUserAsync(int userId)
        {

            var user = await _context.Users.FindAsync(userId) ?? throw new Exception("User not found");

            user.IsBlocked = true;
            await _context.SaveChangesAsync(); 

        }

        public async Task DeleteLoanAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId) ?? throw new Exception("Loan not found");

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<IEnumerable<Loan>>> GetLoansAsync(int userId)
        {
            
            var loans = await _context.Loans
            .Where(l => l.UserId == userId)
            .ToListAsync();

            if (loans == null || loans.Count == 0)
            {
                throw new Exception("Loan not found");
            }

            return loans;

        }

        public async Task UpdateLoanAsync(int loanId, LoanDto updatedLoanDto)
        {
            
            var loan = await _context.Loans.FindAsync(loanId) ?? throw new Exception("Loan not found");
            loan.LoanType = updatedLoanDto.LoanType;
            loan.Amount = updatedLoanDto.Amount;
            loan.Currency = updatedLoanDto.Currency;
            loan.Period = updatedLoanDto.Period;

            _context.Update(loan);
            await _context.SaveChangesAsync();
           
            
        }

        
    }

}
