using System.ComponentModel.DataAnnotations;

namespace Praktikaimt.Areas.Admin.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MaxLength(25, ErrorMessage = "max length")]
        public string Name { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "max length")]
        public string SurName { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "max length")]
        public string UserName { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "max length")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
