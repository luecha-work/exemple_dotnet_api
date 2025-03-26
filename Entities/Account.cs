using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public partial class Account : IdentityUser<int>
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool Active { get; set; }

        public string? Title { get; set; }

        public string? Language { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<AccountRoles> AccountRoles { get; set; } = new List<AccountRoles>();
        public virtual ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
    }
}
