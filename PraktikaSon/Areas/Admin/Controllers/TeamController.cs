using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PraktikaSon.Areas.Admin.ViewModels.Team;
using PraktikaSon.DAL;
using PraktikaSon.Models;
using PraktikaSon.Utilities.Extension;

namespace PraktikaSon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.ToListAsync();
            return View(teams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTeamVM createTeamVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createTeamVM);
            }
            bool result = await _context.Teams.AnyAsync(t => t.Name.Trim().ToLower() == createTeamVM.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "Name is exists");
            }
            if (!createTeamVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "Photo Type unavailable");
                return View(createTeamVM);
            }
            if (!createTeamVM.Photo.ValidateSize(2 * 1024))
            {
                ModelState.AddModelError("Photo", "Photo size max 2mb");
                return View(createTeamVM);
            }
            string filename = await createTeamVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "featured-cars");
            Team team = new Team
            {
                Image = filename,
                Name = createTeamVM.Name,
                Description = createTeamVM.Description,
            };
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            UpdateTeamVM updateTeamVM = new UpdateTeamVM
            {
                Image = team.Image,
                Name = team.Name,
                Description = team.Description,
            };
            return View(updateTeamVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateTeamVM updateTeamVM)
        {
            if (!ModelState.IsValid)
            {
                return View(updateTeamVM);
            }
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            bool result = await _context.Teams.AnyAsync(t => t.Name.Trim().ToLower() == updateTeamVM.Name.Trim().ToLower()&&t.Id!=id);
            if (result)
            {
                ModelState.AddModelError("Name", "Name is exists");
                return View(updateTeamVM);
            }
            if (updateTeamVM.Photo is not null)
            {
                if (!updateTeamVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "Photo Type unavailable");
                    return View(updateTeamVM);
                }
                if (!updateTeamVM.Photo.ValidateSize(2 * 1024))
                {
                    ModelState.AddModelError("Photo", "Photo size max 2mb");
                    return View(updateTeamVM);
                }
                string newImage = await updateTeamVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "featured-cars");
                team.Image.DeleteFile(_env.WebRootPath, "assets", "images", "featured-cars");
                team.Image = newImage;
            }
            team.Name = updateTeamVM.Name;
            team.Description = updateTeamVM.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            Team team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            team.Image.DeleteFile(_env.WebRootPath, "assets", "images", "featured-cars");
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
