using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PraktikaSon.Areas.Admin.ViewModels.Account;
using PraktikaSon.Models;
using PraktikaSon.Utilities.Enum;

namespace PraktikaSon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _usermanager = usermanager;
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
                return View (registerVM);
            }
            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.UserName,
                Email = registerVM.Email,
            };
            IdentityResult result= await _usermanager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return View(registerVM);
            }
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home" ,new { Area = "" });
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
            AppUser user = await _usermanager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if (user is null)
            {
                user = await _usermanager.FindByNameAsync(loginVM.UserNameOrEmail);
                if (user is null)
                {
                    ModelState.AddModelError(String.Empty, "Username tapilmadi");
                    return View(loginVM);
                }
                var result= await _signInManager.PasswordSignInAsync(user,loginVM.Password,loginVM.IsRemenbered,true);
                if(result.IsLockedOut)
                {
                    ModelState.AddModelError(String.Empty, "300 saniye  say sora yoxla");
                    return View(loginVM);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError(String.Empty, "UserName or password sehvdi");
                    return View(loginVM);
                }
            }


            return RedirectToAction("Index", "Home", new {Area=""} );
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
