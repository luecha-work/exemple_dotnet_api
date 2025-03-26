using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class AuthenticationLocalDto: BaseAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [StringLength(
            15,
            ErrorMessage = "Your Password is limited to {2} t0 {1} characters.",
            MinimumLength = 8
        )]
        public string Password { get; init; }
    }
}
