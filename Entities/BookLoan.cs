using System;
using System.Collections.Generic;

namespace Entities;

public partial class BookLoan
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public int BookId { get; set; }

    public DateTime? LoanDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string Status { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Book Book { get; set; } = null!;
}
