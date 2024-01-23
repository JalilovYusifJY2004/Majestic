using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Praktikaimt.Areas.Admin.ViewModels;
using Praktikaimt.DAL;
using Praktikaimt.Models;
using Praktikaimt.Utilities.Extension;

namespace Praktikaimt.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Department> departments = await _context.Departments.ToListAsync();
        
            return View(departments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentVM deparmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(deparmentVM);
            }
            bool result = await _context.Departments.AnyAsync(d => d.Name.Trim().ToLower() == deparmentVM.Name.Trim().ToLower());
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda movcuddur");

                return View(deparmentVM);
            }
            if (!deparmentVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Name", "sekil tipi uygun deyill");
                return View(deparmentVM);
            }
            if (!deparmentVM.Photo.ValidateSize(2 * 1024))
            {
                ModelState.AddModelError("Photo", "Sekil tipi uygun deyil");
                return View(deparmentVM);
            }
            string filename = await deparmentVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");
            Department department = new Department
            {
                Image = filename,
                Name = deparmentVM.Name,
                Description = deparmentVM.Description,
            };
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            UpdateDepartmentVM departmentVM = new UpdateDepartmentVM
            {
                Name = department.Name,
                Description = department.Description,
                Image = department.Image
            };
            return View(departmentVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateDepartmentVM departmentVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmentVM);
            }
            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            bool result = await _context.Departments.AnyAsync(d => d.Name.Trim().ToLower() == departmentVM.Name.Trim().ToLower() && d.Id != id);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda movcuddur");

                return View(departmentVM);
            }
            if (departmentVM.Photo is not null)
            {
                if (!departmentVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Name", "sekil tipi uygun deyill");
                    return View(departmentVM);
                }
                if (!departmentVM.Photo.ValidateSize(2 * 1024))
                {
                    ModelState.AddModelError("Photo", "Sekil tipi uygun deyil");
                    return View(departmentVM);
                }
                string newImage = await departmentVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");
                department.Image.DeleteFile(_env.WebRootPath, "assets", "images");
                department.Image = newImage;

            }
            department.Name = departmentVM.Name;
            department.Description = departmentVM.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id<=0)
            {
                return BadRequest();
            }
            Department department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
            {
                return NotFound();
            }
            department.Image.DeleteFile(_env.WebRootPath, "assets", "images");

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        

    }
}
