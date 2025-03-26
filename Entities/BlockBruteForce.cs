using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class BlockBruteForce
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public int Count { get; set; }

        /// <summary>
        /// L (Locked): ถูกล็อก
        /// U (UnLock): ไม่ล็อก
        /// </summary>
        public string Status { get; set; } = null!;
        public DateTime? LockedTime { get; set; }
        public DateTime? UnLockTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
