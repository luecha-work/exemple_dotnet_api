using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class SingUpDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(
            15,
            ErrorMessage = "Your Password is limited to {2} t0 {1} characters.",
            MinimumLength = 5
        )]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public bool Active { get; set; } = true;
    }
}
