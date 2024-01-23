using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Praktikaimt.Areas.Admin.ViewModels.Account;
using Praktikaimt.Models;
using Praktikaimt.Utilities.Enum;

namespace Praktikaimt.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,RoleManager<IdentityRole>roleManager)
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
                SurName = registerVM.SurName,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };
            IdentityResult result= await _userManager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded) 
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    
                }
                return View(registerVM);
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            AppUser user = await _userManager.FindByEmailAsync(loginVM.UserNameorEmail);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameorEmail);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Tapilmadi");
                    return View(loginVM);
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsRemembered, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "password or username incorrect");
                return View(loginVM);
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "blok");
                return View(loginVM);
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
