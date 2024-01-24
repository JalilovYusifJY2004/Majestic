using System.ComponentModel.DataAnnotations;

namespace PraktikaSon2.Areas.Admin.ViewModels
{
	public class CreateProductVM
	{
		
		public string Name { get; set; }
		
		public IFormFile Photo { get; set; }
	
		public int Price { get; set; }
	}
}
