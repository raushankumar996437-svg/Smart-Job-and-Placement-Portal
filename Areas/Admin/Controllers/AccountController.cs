using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Models;
using SmartHire.ViewModels;

namespace SmartHire.Areas.Admin.Controllers;

[Area("Admin")]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new AdminLoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(AdminLoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (isPasswordValid)
            {
                await _signInManager.SignInAsync(user, model.RememberMe);
                HttpContext.Session.SetString("UserRole", "Admin");
                HttpContext.Session.SetString("UserEmail", user.Email ?? "");
                HttpContext.Session.SetString("UserName", user.FullName ?? "Admin Officer");

                TempData["SuccessMessage"] = $"Administrator session activated. Welcome, {user.FullName}!";
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        ModelState.AddModelError("", "Invalid administrator credentials or unauthorized email.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View(new AdminRegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(AdminRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Validate Master Admin Security Key
        if (model.SecurityKey != "SmartAdmin2026!")
        {
            ModelState.AddModelError("SecurityKey", "Invalid Admin Security Key. You need authorization from the university dean to create an administrator account.");
            return View(model);
        }

        var existing = await _userManager.FindByEmailAsync(model.Email);
        if (existing != null)
        {
            ModelState.AddModelError("Email", "An account with this email address already exists.");
            return View(model);
        }

        var adminUser = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            UserRole = "Admin",
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(adminUser, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(adminUser, "Admin");
            await _signInManager.SignInAsync(adminUser, isPersistent: false);
            
            HttpContext.Session.SetString("UserRole", "Admin");
            HttpContext.Session.SetString("UserEmail", adminUser.Email);
            HttpContext.Session.SetString("UserName", adminUser.FullName);

            TempData["SuccessMessage"] = "New administrator registered and verified in SQL Server!";
            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        HttpContext.Session.Clear();
        TempData["InfoMessage"] = "Admin session terminated safely.";
        return RedirectToAction("Index", "Home", new { area = "" });
    }
}
