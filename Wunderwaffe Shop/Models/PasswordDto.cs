using System.ComponentModel.DataAnnotations;

namespace Wunderwaffe_Shop.Models
{
	public class PasswordDto
	{
		[Required(ErrorMessage = "Podaj stare hasło"), MaxLength(100)]
		public string CurrentPassword { get; set; } = "";
		[Required(ErrorMessage = "Podanie nowego hasła jest wymagane"), MaxLength(100)]
		public string NewPassword { get; set; } = "";

		[Required(ErrorMessage = "Potwierdzenie hasła jest wymagane")]
		[Compare("NewPassword", ErrorMessage = "Hasła nie są identyczne")]
		public string ConfirmPassword { get; set; } = "";
	}
}
