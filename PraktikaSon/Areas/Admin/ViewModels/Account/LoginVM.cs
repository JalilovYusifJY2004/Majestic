using System.ComponentModel.DataAnnotations;

namespace PraktikaSon.Areas.Admin.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string UserNameOrEmail  { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemenbered { get; set;}
    }
    }

