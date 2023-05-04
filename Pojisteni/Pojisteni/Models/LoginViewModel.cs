using System.ComponentModel.DataAnnotations;

namespace Pojisteni.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; } = "";

        [Display(Name = "Zapamatovat")]
        public bool RememberMe { get; set; }
    }
}
