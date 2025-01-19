using System.ComponentModel.DataAnnotations;

namespace Wunderwaffe_Shop.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Imie jest wymagane"), MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Nazwisko jest wymagane"), MaxLength(100)]
        public string LastName { get; set; } = "";

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = "";

        [Phone(ErrorMessage = "Format telefony jest błędny"), MaxLength(11)]
        public string? PhoneNumber { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; } = "";

        [Required, MaxLength(100)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne")]
        public string ConfirmPassword { get; set; } = "";
    }
}
