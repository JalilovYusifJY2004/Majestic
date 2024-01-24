namespace PraktikaSon.Areas.Admin.ViewModels.Team
{
    public class CreateTeamVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Photo { get; set; }
    }
}
