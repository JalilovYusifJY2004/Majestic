using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PraktikaSon2.Areas.Admin.ViewModels.Account;
using PraktikaSon2.Models;
using PraktikaSon2.Utilities.Enum;

namespace PraktikaSon2.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM register)
		{
			if (ModelState.IsValid)
			{
				return View(register);
			}
			AppUser user = new AppUser
			{
				Name = register.Name,
				Email = register.Email,
				Surname = register.Surname,
				UserName = register.UserName,
			};
			IdentityResult result = await _userManager.CreateAsync(user, register.Password);
			if (!result.Succeeded)
			{
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
				return View(register);
			}
			await _signInManager.SignInAsync(user, false);
			return RedirectToAction("Index", "Home", new { Area = "" });
		}
		public IActionResult Login()
		{
			return View();
		}
		public async Task<IActionResult> Login(LoginVM loginVM)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameorEmail);
			if (user == null)
			{
				user = await _userManager.FindByNameAsync(loginVM.UserNameorEmail);
				if (user == null)
				{
					ModelState.AddModelError(string.Empty, "istifadecisi tapilmadi");
					return View();
				}
			}
			var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemembered, true);
			if (!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "ugursuz");
				return View();
			}
			if (result.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "blok");
				return View();
			}
			return RedirectToAction("Index", "Home", new { Area = "" });
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
		public async Task<IActionResult> CreateRoles()
		{
			foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
			{
				if (!(await _roleManager.RoleExistsAsync(role.ToString())))
				{
					await _roleManager.CreateAsync(new IdentityRole
					{
						Name = role.ToString(),
					});
				}
			}
			return RedirectToAction("Index", "Home", new { Area = "" });
		}
	}
}
