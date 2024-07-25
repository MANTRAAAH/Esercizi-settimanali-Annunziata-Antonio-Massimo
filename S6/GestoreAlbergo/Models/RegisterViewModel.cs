using System.ComponentModel.DataAnnotations;

namespace GestoreAlbergo.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Le password non coincidono.")]
        public string ConfirmPassword { get; set; }
    }

}
