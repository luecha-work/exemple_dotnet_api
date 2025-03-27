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
    public class BookServiceTest
    {
        private readonly BookService _mockService;

        public BookServiceTest()
        {
            var repositoryManagerMock = MockRepositoryManager.GetMock();
            var mapperMock = MapperMock.GetMapper();

            _mockService = new BookService(repositoryManagerMock.Object, mapperMock);
        }

        [Fact]
        public async Task GetAllBooksAsync_ShouldReturnAllBooks()
        {
            // Act
            var result = await _mockService.GetAllBooksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task GetBookByIdAsync_WithValidId_ShouldReturnBook()
        {
            // Arrange
            int bookId = 1;

            // Act
            var result = await _mockService.GetBookByIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result.Id);
            Assert.Equal("Clean Code", result.Title);
        }

        [Fact]
        public async Task GetBookByIdAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            int invalidBookId = 999;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _mockService.GetBookByIdAsync(invalidBookId));
            Assert.Equal($"Book with id: {invalidBookId} doesn't exist.", exception.Message);
        }

        [Fact]
        public async Task CreateBookAsync_ShouldCreateAndReturnNewBook()
        {
            // Arrange
            var newBook = new CreateBookDto
            {
                Title = "Test Driven Development",
                Author = "Kent Beck",
                Isbn = "9780321146533",
                PublicationYear = 2002,
                Category = "Programming",
                Status = "Available"
            };

            // Act
            var result = await _mockService.CreateBookAsync(newBook);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newBook.Title, result.Title);
            Assert.Equal(newBook.Author, result.Author);
            Assert.Equal(6, result.Id); // Based on your mock data, this should be the next ID
        }

        [Fact]
        public async Task UpdateBookAsync_WithValidId_ShouldUpdateAndReturnBook()
        {
            // Arrange
            int bookId = 2;
            var updateBook = new UpdateBookDto
            {
                Title = "Design Patterns: Updated Edition",
                Status = "Reserved"
            };

            // Act
            var result = await _mockService.UpdateBookAsync(bookId, updateBook);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateBook.Title, result.Title);
            Assert.Equal(updateBook.Status, result.Status);
            Assert.Equal("Erich Gamma", result.Author); // Author shouldn't change
        }

        [Fact]
        public async Task UpdateBookAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            int invalidBookId = 999;
            var updateBook = new UpdateBookDto { Title = "Updated Title" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _mockService.UpdateBookAsync(invalidBookId, updateBook));
            Assert.Equal($"Book with id: {invalidBookId} doesn't exist.", exception.Message);
        }

        [Fact]
        public async Task DeleteBookAsync_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            int bookId = 4; // Refactoring book - should be available to delete

            // Act
            var result = await _mockService.DeleteBookAsync(bookId);

            // Assert
            Assert.True(result);

            // Verify it's deleted by trying to get it (should throw exception)
            await Assert.ThrowsAsync<Exception>(() => _mockService.GetBookByIdAsync(bookId));
        }

        [Fact]
        public async Task DeleteBookAsync_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            int invalidBookId = 999;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _mockService.DeleteBookAsync(invalidBookId));
            Assert.Equal($"Book with id: {invalidBookId} doesn't exist.", exception.Message);
        }

        [Fact]
        public async Task SearchBooksAsync_WithMatchingTerm_ShouldReturnFilteredBooks()
        {
            // Arrange
            string searchTerm = "programming";

            // Act
            var result = await _mockService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Count()); // Should return all programming books
            Assert.All(result, book => Assert.Contains("Programming", book.Category));
        }

        [Fact]
        public async Task SearchBooksAsync_WithNoMatchingTerm_ShouldReturnEmptyList()
        {
            // Arrange
            string searchTerm = "nonexistent";

            // Act
            var result = await _mockService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task SearchBooksAsync_WithEmptyTerm_ShouldReturnAllBooks()
        {
            // Arrange
            string searchTerm = "";

            // Act
            var result = await _mockService.SearchBooksAsync(searchTerm);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Count()); // Should return all books
        }
    }
}