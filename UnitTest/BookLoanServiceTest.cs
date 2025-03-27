using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Service;
using Shared.DTOs;
using UnitTest.MockData;
using UnitTest.MockRepository;
using Xunit;

namespace UnitTest
{
    public class BookLoanServiceTest
    {
        private readonly BookLoanService _mockService;

        public BookLoanServiceTest()
        {
            var repositoryManagerMock = MockRepositoryManager.GetMock();
            var mapperMock = MapperMock.GetMapper();

            _mockService = new BookLoanService(repositoryManagerMock.Object, mapperMock);
        }
        
        [Fact]
        public async Task GetAllLoansAsync_ShouldReturnAllLoans()
        {
            // Act
            var result = await _mockService.GetAllLoansAsync();
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }
        
        [Fact]
        public async Task GetLoanByIdAsync_WithValidId_ShouldReturnLoan()
        {
            // Arrange
            int loanId = 1;
            
            // Act
            var result = await _mockService.GetLoanByIdAsync(loanId);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(loanId, result.Id);
            Assert.Equal(1, result.BookId);
            Assert.Equal(1, result.AccountId);
        }
        
        [Fact]
        public async Task GetLoanByIdAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            int invalidLoanId = 999;
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockService.GetLoanByIdAsync(invalidLoanId));
            Assert.Equal($"Loan with id: {invalidLoanId} doesn't exist.", exception.Message);
        }
        
        [Fact]
        public async Task GetLoansByUserIdAsync_ShouldReturnUserLoans()
        {
            // Arrange
            int accountId = 1;
            
            // Act
            var result = await _mockService.GetLoansByUserIdAsync(accountId);
            
            // Assert
            Assert.NotNull(result);
            Assert.All(result, loan => Assert.Equal(accountId, loan.AccountId));
        }
        
        [Fact]
        public async Task GetOverdueLoansAsync_ShouldReturnOverdueLoans()
        {
            // Act
            var result = await _mockService.GetOverdueLoansAsync();
            
            // Assert
            Assert.NotNull(result);
            var currentDate = DateTime.Now;
            Assert.All(result, loan => 
            {
                Assert.True(loan.DueDate < currentDate);
                Assert.Null(loan.ReturnDate);
            });
        }
        
        [Fact]
        public async Task CreateLoanAsync_WithValidData_ShouldCreateAndReturnLoan()
        {
            // Arrange
            var newLoan = new CreateBookLoanDto
            {
                AccountId = 3,
                BookId = 5
            };
            
            // Act
            var result = await _mockService.CreateLoanAsync(newLoan);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(newLoan.AccountId, result.AccountId);
            Assert.Equal(newLoan.BookId, result.BookId);
            Assert.NotNull(result.LoanDate);
            Assert.NotNull(result.DueDate);
            Assert.Null(result.ReturnDate);
            Assert.Equal(6, result.Id); // Based on your mock data, this should be the next ID
        }
        
        [Fact]
        public async Task CreateLoanAsync_WithAlreadyLoanedBook_ShouldThrowException()
        {
            // Arrange
            var newLoan = new CreateBookLoanDto
            {
                AccountId = 2,
                BookId = 1 // Book ID 1 is already loaned (see BookLoanData)
            };
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockService.CreateLoanAsync(newLoan));
            Assert.Equal($"Book with id: {newLoan.BookId} is already loaned.", exception.Message);
        }
        
        [Fact]
        public async Task UpdateLoanAsync_WithValidId_ShouldUpdateAndReturnLoan()
        {
            // Arrange
            int loanId = 1;
            var updateLoan = new UpdateBookLoanDto
            {
                DueDate = DateTime.Now.AddDays(30)
            };
            
            // Act
            var result = await _mockService.UpdateLoanAsync(loanId, updateLoan);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(loanId, result.Id);
            Assert.Equal(updateLoan.DueDate, result.DueDate);
        }
        
        [Fact]
        public async Task UpdateLoanAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            int invalidLoanId = 999;
            var updateLoan = new UpdateBookLoanDto
            {
                DueDate = DateTime.Now.AddDays(30)
            };
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockService.UpdateLoanAsync(invalidLoanId, updateLoan));
            Assert.Equal($"Loan with id: {invalidLoanId} doesn't exist.", exception.Message);
        }
        
        [Fact]
        public async Task ReturnBookAsync_WithValidId_ShouldReturnBook()
        {
            // Arrange
            int loanId = 1; // Active loan
            
            // Act
            var result = await _mockService.ReturnBookAsync(loanId);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(loanId, result.Id);
            Assert.NotNull(result.ReturnDate);
        }
        
        [Fact]
        public async Task ReturnBookAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            int invalidLoanId = 999;
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockService.ReturnBookAsync(invalidLoanId));
            Assert.Equal($"Loan with id: {invalidLoanId} doesn't exist.", exception.Message);
        }
        
        [Fact]
        public async Task ReturnBookAsync_WithAlreadyReturnedBook_ShouldThrowException()
        {
            // Arrange
            int loanId = 3; // Already returned book (see BookLoanData)
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockService.ReturnBookAsync(loanId));
            Assert.Equal($"Book from loan with id: {loanId} is already returned.", exception.Message);
        }
        
        [Fact]
        public async Task DeleteLoanAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            int loanId = 5;
            
            // Act
            var result = await _mockService.DeleteLoanAsync(loanId);
            
            // Assert
            Assert.True(result);
            
            // Verify it's deleted by trying to get it (should throw exception)
            await Assert.ThrowsAsync<Exception>(() => _mockService.GetLoanByIdAsync(loanId));
        }
        
        [Fact]
        public async Task DeleteLoanAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            int invalidLoanId = 999;
            
            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockService.DeleteLoanAsync(invalidLoanId));
            Assert.Equal($"Loan with id: {invalidLoanId} doesn't exist.", exception.Message);
        }
    }
}