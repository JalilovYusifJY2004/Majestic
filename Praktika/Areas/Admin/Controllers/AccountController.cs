using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Praktika.Areas.Admin.ViewModels.Account;
using Praktika.Models;
using Praktika.Utilities.Enum;

namespace Praktika.Areas.Admin.Controllers
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
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
                Surname = registerVM.SurName
            };
            IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);
            {
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(registerVM);
                }
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home", new { Area = "" });
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameorEmail);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameorEmail);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "tappilmadi");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemembered, true);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "NAme or password ynalis");
                    return View(loginVM);
                }
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "blok xois");
                    return View(loginVM);
                }
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


