namespace Praktikaimt.Areas.Admin.ViewModels;

public class CreateDepartmentVM
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IFormFile Photo { get; set; }
}
