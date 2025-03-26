using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities;
using IRepository;
using IService;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace Service
{
    public class BookLoanService : IBookLoanService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        // private BookLoan _bruteForce;

        public BookLoanService(
            IRepositoryManager repositoryManager,
            IMapper mapper
        )
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<BookLoanDto> CreateLoanAsync(CreateBookLoanDto createLoanDto)
        {
            // Check if the book exists
            var book = await _repositoryManager.BookRepository.FindOneByIdAsync(createLoanDto.BookId);
            if (book == null)
                throw new Exception($"Book with id: {createLoanDto.BookId} doesn't exist.");

            // Check if the user exists
            var user = await _repositoryManager.AccountRepository.FindOneByIdAsync(createLoanDto.AccountId);
            if (user == null)
                throw new Exception($"User with id: {createLoanDto.AccountId} doesn't exist.");

            // Check if the book is already loaned
            var existingLoan = await _repositoryManager.BookLoanRepository.FindByConditionAsync(
                l => l.BookId == createLoanDto.BookId && l.ReturnDate == null);

            if (existingLoan.Any())
                throw new Exception($"Book with id: {createLoanDto.BookId} is already loaned.");

            // Create new loan
            var bookLoan = new BookLoan
            {
                BookId = createLoanDto.BookId,
                AccountId = createLoanDto.AccountId,
                LoanDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14), // Default loan period is 14 days
                ReturnDate = null,
            };

            _repositoryManager.BookLoanRepository.Create(bookLoan);
            _repositoryManager.Commit();

            // Map DTO
            var bookLoanDto = _mapper.Map<BookLoanDto>(bookLoan);
            return bookLoanDto;
        }

        public async Task<bool> DeleteLoanAsync(int id)
        {
            var loan = await _repositoryManager.BookLoanRepository.FindOneByIdAsync(id);
            if (loan == null)
                throw new Exception($"Loan with id: {id} doesn't exist.");

            _repositoryManager.BookLoanRepository.Delete(loan);
            _repositoryManager.Commit();

            return true;
        }

        public async Task<IEnumerable<BookLoanDto>> GetAllLoansAsync()
        {
            var loans = await _repositoryManager.BookLoanRepository.FindAllAsync();

            var bookLoansDto = _mapper.Map<IEnumerable<BookLoanDto>>(loans);
            return bookLoansDto;
        }

        public async Task<BookLoanDto> GetLoanByIdAsync(int id)
        {
            var loan = await _repositoryManager.BookLoanRepository.FindOneByIdAsync(id);
            if (loan == null)
                throw new Exception($"Loan with id: {id} doesn't exist.");

            var bookLoanDto = _mapper.Map<BookLoanDto>(loan);

            return bookLoanDto;
        }

        public async Task<IEnumerable<BookLoanDto>> GetLoansByUserIdAsync(int accountId)
        {
            var loans = (await _repositoryManager.BookLoanRepository.FindByConditionAsync(
                l => l.AccountId == accountId)).ToListAsync();

            var bookLoansDto = _mapper.Map<IEnumerable<BookLoanDto>>(loans);
            return bookLoansDto;
        }

        public async Task<IEnumerable<BookLoanDto>> GetOverdueLoansAsync()
        {
            var currentDate = DateTime.Now;
            var loans = await _repositoryManager.BookLoanRepository.FindByConditionAsync(
                l => l.DueDate < currentDate && l.ReturnDate == null);

            var bookLoansDto = _mapper.Map<IEnumerable<BookLoanDto>>(loans);

            return bookLoansDto;
        }

        public async Task<BookLoanDto> ReturnBookAsync(int loanId)
        {
            var loans = await _repositoryManager.BookLoanRepository.FindByConditionAsync(
                l => l.Id == loanId);
            var loan = await loans.AsTracking()
                .FirstOrDefaultAsync();
            if (loan == null)
                throw new Exception($"Loan with id: {loanId} doesn't exist.");

            if (loan.ReturnDate != null)
                throw new Exception($"Book from loan with id: {loanId} is already returned.");

            loan.ReturnDate = DateTime.Now;
            _repositoryManager.Commit();

            var bookLoanDto = _mapper.Map<BookLoanDto>(loan);

            return bookLoanDto;
        }

        public async Task<BookLoanDto> UpdateLoanAsync(int id, UpdateBookLoanDto updateLoanDto)
        {
            var loans = await _repositoryManager.BookLoanRepository.FindByConditionAsync(l => l.Id == id);

            var loan = await loans.AsTracking()
                .FirstOrDefaultAsync();
            if (loan == null)
                throw new Exception($"Loan with id: {id} doesn't exist.");

            // Update loan properties
            // Check if DueDate is provided (not null)
            if (updateLoanDto.DueDate != null)
                loan.DueDate = updateLoanDto.DueDate;

            _repositoryManager.Commit();

            var bookLoanDto = _mapper.Map<BookLoanDto>(loan);

            return bookLoanDto;
        }
    }
}