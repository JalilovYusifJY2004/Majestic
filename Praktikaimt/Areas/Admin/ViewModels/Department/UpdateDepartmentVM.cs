namespace Praktikaimt.Areas.Admin.ViewModels;

public class UpdateDepartmentVM
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public IFormFile? Photo { get; set; }
}
