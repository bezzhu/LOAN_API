using LOAN_API.Data;
using LOAN_API.Models.DTO;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LOAN_API.Models;
using Microsoft.AspNetCore.Http;

namespace LOAN_API.Services
{
    public interface IUserService
    {
        Task<ActionResult<User>> GetInfoAsync();
        Task AddLoanAsync(LoanDto loanDto);
        Task UpdateLoanAsync(int loanId, LoanDto loanDto);
        Task DeleteLoanAsync(int loanId);
    }
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
   
        public async Task<ActionResult<User>> GetInfoAsync()
        {
            var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User
                                    .FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var userEntity = await _context.Users
                                        .Include(u => u.Loans)
                                        .FirstOrDefaultAsync(u => u.Id == currentUserId); 

            if (userEntity == null)
            {
                return new NotFoundResult();
            }       

                return userEntity;
        }
        public async Task AddLoanAsync( LoanDto loanDto)
        {
            try
            {
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User
                                        .FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _context.Users.FindAsync(currentUserId);

                if (user == null || user.IsBlocked)
                {
                    throw new Exception("User is blocked");
                }

                var newLoan = new Loan
                {
                    UserId = currentUserId,
                    LoanType = loanDto.LoanType,
                    Amount = loanDto.Amount,
                    Currency = loanDto.Currency,
                    Period = loanDto.Period,
                    Status = LoanStatus.InProcess
                };

                _context.Loans.Add(newLoan);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateLoanAsync(int loanId, LoanDto loanDto)
        {
            try
            {
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User
                                    .FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var loan = await _context.Loans.FindAsync(loanId) ?? throw new Exception("Loan not found");

                if (loan.UserId != currentUserId)
                {
                    throw new Exception("No permission to update this loan");
                }
                if (loan.Status != LoanStatus.InProcess)
                {
                    throw new Exception($"Cannot update a loan with status: {Enum.GetName(typeof(LoanStatus), loan.Status)}");
                }

                loan.LoanType = loanDto.LoanType;
                loan.Amount = loanDto.Amount;
                loan.Currency = loanDto.Currency;
                loan.Period = loanDto.Period;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async Task DeleteLoanAsync(int loanId)
        {
            try
            {
                var currentUserId = int.Parse(_httpContextAccessor.HttpContext.User
                                        .FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var loan = await _context.Loans.FindAsync(loanId) ?? throw new Exception("Loan not found");

                if (loan.UserId != currentUserId)
                {
                    throw new Exception("No permission to delete this loan");
                }

                if (loan.Status != LoanStatus.InProcess)
                {
                    throw new Exception($"Cannot delete a loan with status: {Enum.GetName(typeof(LoanStatus), loan.Status)}");
                }

                _context.Loans.Remove(loan);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}

