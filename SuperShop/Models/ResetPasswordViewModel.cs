using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")] // Comapra com a propriedade acima
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }



    }
}
