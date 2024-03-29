using LOAN_API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LOAN_API.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet <User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options): base(options)
        {

        }
        public UserDbContext()
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Loan>()
                .Property(e => e.LoanType)
                .HasConversion(
                    v => v.ToString(),
                    v => (LoanType)Enum.Parse(typeof(LoanType), v));

            modelBuilder.Entity<Loan>()
            .Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (LoanStatus)Enum.Parse(typeof(LoanStatus), v));
        }
    }
}
