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
    public class BookService : IBookService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public BookService(
            IRepositoryManager repositoryManager,
            IMapper mapper
        )
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
        {
            // Create new book entity
            var book = new Book
            {
                Title = createBookDto.Title,
                Author = createBookDto.Author,
                Isbn = createBookDto.Isbn,
                PublicationYear = createBookDto.PublicationYear,
                Category = createBookDto.Category,
                Status = "Available", // Default status
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Add to repository
            await _repositoryManager.BookRepository.CreateAsync(book);

            // Return mapped DTO
            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            // Check if the book exists
            var book = await _repositoryManager.BookRepository.FindOneByIdAsync(id);
            if (book == null)
                throw new Exception($"Book with id: {id} doesn't exist.");

            // Check if the book has active loans
            var activeLoans = await _repositoryManager.BookLoanRepository.FindByConditionAsync(
                l => l.BookId == id && l.ReturnDate == null);

            if (activeLoans.Any())
                throw new Exception($"Cannot delete book with id: {id} because it has active loans.");

            // Delete the book
            _repositoryManager.BookRepository.Delete(book);
            _repositoryManager.Commit();

            return true;
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
        {
            var books = await _repositoryManager.BookRepository.FindAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _repositoryManager.BookRepository.FindOneByIdAsync(id);
            if (book == null)
                throw new Exception($"Book with id: {id} doesn't exist.");

            return _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> SearchBooksAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllBooksAsync();

            searchTerm = searchTerm.ToLower();

            var books = await _repositoryManager.BookRepository.FindByConditionAsync(
                b => b.Title.ToLower().Contains(searchTerm) ||
                     b.Author.ToLower().Contains(searchTerm) ||
                     (b.Isbn != null && b.Isbn.ToLower().Contains(searchTerm)) ||
                     (b.Category != null && b.Category.ToLower().Contains(searchTerm))
            );

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
        {
            // Check if the book exists
            var book = await _repositoryManager.BookRepository.FindOneByIdAsync(id);
            if (book == null)
                throw new Exception($"Book with id: {id} doesn't exist.");

            // Update book properties
            book.Title = updateBookDto.Title ?? book.Title;
            book.Author = updateBookDto.Author ?? book.Author;
            book.Isbn = updateBookDto.Isbn ?? book.Isbn;
            book.PublicationYear = updateBookDto.PublicationYear ?? book.PublicationYear;
            book.Category = updateBookDto.Category ?? book.Category;
            book.Status = updateBookDto.Status ?? book.Status;
            book.UpdatedAt = DateTime.Now;

            // Update in repository
            _repositoryManager.BookRepository.Update(book);
            _repositoryManager.Commit();

            // Return updated book
            return _mapper.Map<BookDto>(book);
        }
    }
}