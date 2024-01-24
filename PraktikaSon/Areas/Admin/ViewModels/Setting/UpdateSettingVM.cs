using System.ComponentModel.DataAnnotations;

namespace PraktikaSon.Areas.Admin.ViewModels.Setting
{
	public class UpdateSettingVM
	{
		[Required]
		public string Key { get; set; }
		[Required]
		public string Value { get; set; }
	}
}
