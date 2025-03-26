using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SystemSession
    {
        public Guid Id { get; set; }
        public int AccountId { get; set; }
        public DateTime LoginAt { get; set; }
        public string? Platform { get; set; }
        public string? Os { get; set; }
        public string? Browser { get; set; }
        public string LoginIp { get; set; } = null!;
        public DateTime IssuedTime { get; set; }
        public DateTime ExpirationTime { get; set; }

        /// <summary>
        /// B (Blocked): Session ยังไม่ได้ใช้งาน
        /// A (Active): Session กำลังใช้งานอยู่
        /// E (Expired): Session หมดอายุแล้ว
        /// </summary>
        public string SessionStatus { get; set; } = null!;
        public string? Token { get; set; }
        public DateTime? RefreshTokenAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
