using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class BaseBookLoanDto
    {
        public int AccountId { get; set; }

        public int BookId { get; set; }

        public DateTime? LoanDate { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public string Status { get; set; } = null!;
    }
}