namespace PraktikaSon.Areas.Admin.ViewModels.Team
{
    public class UpdateTeamVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
