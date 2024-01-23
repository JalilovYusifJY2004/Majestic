using System.ComponentModel.DataAnnotations;

namespace Praktika.Areas.Admin.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string UserNameorEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsRemembered { get; set; }
    }
}
