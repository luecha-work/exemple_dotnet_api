using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTOs;

namespace IService
{
    public interface IBookLoanService
    {
        // Create
        Task<BookLoanDto> CreateLoanAsync(CreateBookLoanDto createLoanDto);
        
        // Read
        Task<BookLoanDto> GetLoanByIdAsync(int id);
        Task<IEnumerable<BookLoanDto>> GetAllLoansAsync();
        Task<IEnumerable<BookLoanDto>> GetLoansByUserIdAsync(int accountId);
        Task<IEnumerable<BookLoanDto>> GetOverdueLoansAsync();
        
        // Update
        Task<BookLoanDto> UpdateLoanAsync(int id, UpdateBookLoanDto updateLoanDto);
        Task<BookLoanDto> ReturnBookAsync(int loanId);
        
        // Delete
        Task<bool> DeleteLoanAsync(int id);
    }
}