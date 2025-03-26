using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public partial class Role : IdentityRole<int>
    {
        public required string RoleCode { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<AccountRoles> AccountRoles { get; set; } =
            new List<AccountRoles>();
    }
}
