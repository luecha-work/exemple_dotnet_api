using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace UnitTest.MockData
{
    public class BookLoanData
    {
        public static List<BookLoan> GetBookLoans()
        {
            return new List<BookLoan>
            {
                new BookLoan
                {
                    Id = 1,
                    AccountId = 1,
                    BookId = 1,
                    LoanDate = DateTime.Now.AddDays(-14),
                    DueDate = DateTime.Now.AddDays(7),
                    Status = "Active",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-14)
                },
                new BookLoan
                {
                    Id = 2,
                    AccountId = 2,
                    BookId = 3,
                    LoanDate = DateTime.Now.AddDays(-20),
                    DueDate = DateTime.Now.AddDays(-6),
                    Status = "Overdue",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-20),
                    UpdatedBy = "System",
                    UpdatedAt = DateTime.Now.AddDays(-6)
                },
                new BookLoan
                {
                    Id = 3,
                    AccountId = 1,
                    BookId = 2,
                    LoanDate = DateTime.Now.AddDays(-30),
                    DueDate = DateTime.Now.AddDays(-16),
                    ReturnDate = DateTime.Now.AddDays(-18),
                    Status = "Returned",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-30),
                    UpdatedBy = "System",
                    UpdatedAt = DateTime.Now.AddDays(-18)
                },
                new BookLoan
                {
                    Id = 4,
                    AccountId = 3,
                    BookId = 4,
                    LoanDate = DateTime.Now.AddDays(-10),
                    DueDate = DateTime.Now.AddDays(11),
                    Status = "Active",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-10)
                },
                new BookLoan
                {
                    Id = 5,
                    AccountId = 2,
                    BookId = 5,
                    LoanDate = DateTime.Now.AddDays(-25),
                    DueDate = DateTime.Now.AddDays(-11),
                    ReturnDate = DateTime.Now.AddDays(-12),
                    Status = "Returned",
                    CreatedBy = "System",
                    CreatedAt = DateTime.Now.AddDays(-25),
                    UpdatedBy = "System",
                    UpdatedAt = DateTime.Now.AddDays(-12)
                }
            };
        }
    }
}