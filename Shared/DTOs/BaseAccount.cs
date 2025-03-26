using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class BaseAccount
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileImageName { get; set; }
    }
}
