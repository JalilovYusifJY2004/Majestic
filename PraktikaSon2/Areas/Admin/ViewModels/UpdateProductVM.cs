using System.ComponentModel.DataAnnotations;

namespace PraktikaSon2.Areas.Admin.ViewModels
{
	public class UpdateProductVM
	{
		[Required]
		public string Name { get; set; }

		public IFormFile? Photo { get; set; }
		[Required]
		public int Price { get; set; }

		public string? Image { get; set; }
	}
}
