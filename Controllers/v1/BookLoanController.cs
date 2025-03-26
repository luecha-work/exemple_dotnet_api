using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOs;

namespace Controllers.v1
{
    [Route("api/v{version:apiVersion}/book_loan")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    public class BookLoanController : ControllerBase
    {
        private readonly IServiceManager _service;
        private readonly ILogger<BookLoanController> _logger;

        public BookLoanController(IServiceManager service, ILogger<BookLoanController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get all book loans
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _service.BookLoanService.GetAllLoansAsync();

            return Ok(loans);
        }

        /// <summary>
        /// Get loan by id
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLoanById(int id)
        {
            var loan = await _service.BookLoanService.GetLoanByIdAsync(id);

            return Ok(loan);
        }

        /// <summary>
        /// Get loans by user id
        /// </summary>
        [HttpGet("user/{accountId:int}")]
        public async Task<IActionResult> GetLoansByUserId(int accountId)
        {
            var loans = await _service.BookLoanService.GetLoansByUserIdAsync(accountId);

            return Ok(loans);
        }

        /// <summary>
        /// Get overdue loans
        /// </summary>
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueLoans()
        {
            var overdueLoans = await _service.BookLoanService.GetOverdueLoansAsync();

            return Ok(overdueLoans);
        }

        /// <summary>
        /// Create a new book loan
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateLoan([FromBody] CreateBookLoanDto createLoanDto)
        {
            var createdLoan = await _service.BookLoanService.CreateLoanAsync(createLoanDto);

            return CreatedAtAction(nameof(GetLoanById), new { id = createdLoan.Id }, createdLoan);
        }

        /// <summary>
        /// Update an existing loan
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateLoan(int id, [FromBody] UpdateBookLoanDto updateLoanDto)
        {
            var updatedLoan = await _service.BookLoanService.UpdateLoanAsync(id, updateLoanDto);

            return Ok(updatedLoan);
        }

        /// <summary>
        /// Return a borrowed book
        /// </summary>
        [HttpPost("{id:int}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var returnedLoan = await _service.BookLoanService.ReturnBookAsync(id);

            return Ok(returnedLoan);
        }

        /// <summary>
        /// Delete a loan
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var result = await _service.BookLoanService.DeleteLoanAsync(id);

            return NoContent();
        }
    }
}