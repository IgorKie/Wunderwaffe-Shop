using System.ComponentModel.DataAnnotations;

namespace Wunderwaffe_Shop.Models
{
    public class CheckoutDto
    {
        [Required(ErrorMessage = "Adres jest wymagany.")]
        [MaxLength(300)]
        public string DeliveryAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
    }
}
