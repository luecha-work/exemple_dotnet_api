using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class VerifyAccessTokenDto
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;
    }
}