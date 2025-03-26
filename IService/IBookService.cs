using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.DTOs;

namespace IService
{
    public interface IBookService
    {
        // Create
        Task<BookDto> CreateBookAsync(CreateBookDto createBookDto);
        
        // Read
        Task<BookDto> GetBookByIdAsync(int id);
        Task<IEnumerable<BookDto>> GetAllBooksAsync();
        Task<IEnumerable<BookDto>> SearchBooksAsync(string searchTerm);
        
        // Update
        Task<BookDto> UpdateBookAsync(int id, UpdateBookDto updateBookDto);
        
        // Delete
        Task<bool> DeleteBookAsync(int id);
    }
}