using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;

namespace Controllers.v1
{
    [Route("api/v{version:apiVersion}/book")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class BookController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly ILogger<BookController> _logger;

        public BookController(IServiceManager service, ILogger<BookController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/v1/book
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAllBooks()
        {
            try
            {
                var books = await _service.BookService.GetAllBooksAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetAllBooks)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/v1/book/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BookDto>> GetBookById(int id)
        {
            try
            {
                var book = await _service.BookService.GetBookByIdAsync(id);
                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetBookById)}: {ex.Message}");
                
                if (ex.Message.Contains("doesn't exist"))
                    return NotFound(ex.Message);
                    
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/v1/book/search?searchTerm=value
        [HttpGet("search")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BookDto>>> SearchBooks([FromQuery] string searchTerm)
        {
            try
            {
                var books = await _service.BookService.SearchBooksAsync(searchTerm);
                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(SearchBooks)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/v1/book
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] CreateBookDto createBookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdBook = await _service.BookService.CreateBookAsync(createBookDto);
                return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CreateBook)}: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/v1/book/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<BookDto>> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedBook = await _service.BookService.UpdateBookAsync(id, updateBookDto);
                return Ok(updatedBook);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(UpdateBook)}: {ex.Message}");
                
                if (ex.Message.Contains("doesn't exist"))
                    return NotFound(ex.Message);
                    
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/v1/book/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                await _service.BookService.DeleteBookAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeleteBook)}: {ex.Message}");
                
                if (ex.Message.Contains("doesn't exist"))
                    return NotFound(ex.Message);
                
                if (ex.Message.Contains("has active loans"))
                    return BadRequest(ex.Message);
                    
                return StatusCode(500, "Internal server error");
            }
        }
    }
}