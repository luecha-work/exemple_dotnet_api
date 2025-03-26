using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class BaseBook
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string? Isbn { get; set; }

        public int? PublicationYear { get; set; }

        public string? Category { get; set; }

        public string Status { get; set; } = null!;
    }
}