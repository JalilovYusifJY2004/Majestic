using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PraktikaSon.Areas.Admin.ViewModels.Setting;
using PraktikaSon.DAL;
using PraktikaSon.Models;

namespace PraktikaSon.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SettingController : Controller
	{
		private readonly AppDbContext _context;

		public SettingController(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			List<Setting> settings = await _context.Settings.ToListAsync();
			return View(settings);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CreateSettingVM settingVM)
		{
			if (!ModelState.IsValid)
			{
				return View(settingVM);
			}
			bool result = await _context.Settings.AnyAsync(s => s.Key.Trim().ToLower() == settingVM.Key.Trim().ToLower());
			if (result)
			{
				ModelState.AddModelError("Key", "Key is exists");
				return View(settingVM);
			}
			Setting setting = new Setting
			{
				Key = settingVM.Key,
				Value = settingVM.Value
			};
			await _context.Settings.AddAsync(setting);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Update(int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}
			Setting setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
			if (setting == null)
			{
				return NotFound();
			}
			UpdateSettingVM settingVM = new UpdateSettingVM
			{
				Key = setting.Key,
				Value = setting.Value
			};
			return View(settingVM);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, UpdateSettingVM settingVM)
		{
			if (!ModelState.IsValid)
			{
				return View(settingVM);
			}
			Setting setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
			if (setting == null)
			{
				return NotFound();
			}
			bool result = await _context.Settings.AnyAsync(s => s.Key.Trim().ToLower() == settingVM.Key.Trim().ToLower() && s.Id != id);
			if (result)
			{
				ModelState.AddModelError("Key", "Key is exists");
				return View(settingVM);
			}
			setting.Key = settingVM.Key;
			setting.Value = settingVM.Value;
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0)
			{
				return BadRequest();
			}
			Setting setting = await _context.Settings.FirstOrDefaultAsync(s => s.Id == id);
			if (setting == null)
			{
				return NotFound();
			}
			_context.Settings.Remove(setting);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}
	}
}
