using System.ComponentModel.DataAnnotations;

namespace PraktikaSon2.Areas.Admin.ViewModels.Account
{
	public class LoginVM
	{
        public string UserNameorEmail { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public bool IsRemembered { get; set; }
    }
}
