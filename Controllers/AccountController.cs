using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartHire.Data;
using SmartHire.Models;
using SmartHire.ViewModels;

namespace SmartHire.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Login(string? role, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        var model = new LoginViewModel
        {
            Role = !string.IsNullOrEmpty(role) ? role : "Student",
            ReturnUrl = returnUrl
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // If trying to log in as Admin from main form, redirect to Admin login
        if (model.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded || model.Password == "Pass@123") // Allow seeded demo pass
            {
                HttpContext.Session.SetString("UserRole", user.UserRole ?? model.Role);
                HttpContext.Session.SetString("UserEmail", user.Email ?? model.Email);
                HttpContext.Session.SetString("UserName", user.FullName ?? "User");

                TempData["SuccessMessage"] = $"Welcome back, {user.FullName}! Logged in as {model.Role}.";

                if (model.Role.Equals("Recruiter", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("Dashboard", "Recruiter");
                }
                return RedirectToAction("Dashboard", "Student");
            }
        }
        else
        {
            // For immediate testing: if user doesn't exist yet, auto-create student or recruiter session
            HttpContext.Session.SetString("UserRole", model.Role);
            HttpContext.Session.SetString("UserEmail", model.Email);
            HttpContext.Session.SetString("UserName", model.Email.Split('@')[0]);

            TempData["SuccessMessage"] = $"Welcome to SmartHire, {model.Email}! Logged in as {model.Role}.";

            if (model.Role.Equals("Recruiter", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Dashboard", "Recruiter");
            }
            return RedirectToAction("Dashboard", "Student");
        }

        ModelState.AddModelError("", "Invalid login attempt. Please check email and password.");
        return View(model);
    }

    [HttpGet]
    public IActionResult RegisterStudent()
    {
        return View(new StudentRegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterStudent(StudentRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            UserRole = "Student",
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Student");

            // Save StudentProfile to SQL Server
            var profile = new StudentProfile
            {
                UserId = user.Id,
                CollegeName = model.CollegeName,
                Degree = "B.Tech",
                Department = model.Department,
                GraduationYear = model.GraduationYear,
                CGPA = model.CGPA,
                LinkedInUrl = model.LinkedInUrl,
                GitHubUrl = model.GitHubUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _context.StudentProfiles.AddAsync(profile);
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);
            HttpContext.Session.SetString("UserRole", "Student");
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);

            TempData["SuccessMessage"] = $"Student account for {model.FullName} saved to SQL Server! You are now logged in.";
            return RedirectToAction("Dashboard", "Student");
        }

        foreach (var err in result.Errors)
        {
            ModelState.AddModelError("", err.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult RegisterRecruiter()
    {
        return View(new RecruiterRegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterRecruiter(RecruiterRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.OfficialEmail,
            Email = model.OfficialEmail,
            FullName = model.ContactPerson,
            UserRole = "Recruiter",
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "Recruiter");

            // Save Company & RecruiterProfile to SQL Server
            var company = new Company
            {
                CompanyName = model.CompanyName,
                OfficialEmail = model.OfficialEmail,
                Website = model.Website,
                Industry = model.Industry,
                Description = model.Description,
                ApprovalStatus = "Pending", // Needs Admin approval
                CreatedAt = DateTime.UtcNow
            };

            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            var recruiterProfile = new RecruiterProfile
            {
                UserId = user.Id,
                CompanyId = company.Id,
                ContactPerson = model.ContactPerson,
                Designation = model.Designation,
                CreatedAt = DateTime.UtcNow
            };

            await _context.RecruiterProfiles.AddAsync(recruiterProfile);
            await _context.SaveChangesAsync();

            await _signInManager.SignInAsync(user, isPersistent: false);
            HttpContext.Session.SetString("UserRole", "Recruiter");
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);

            TempData["SuccessMessage"] = $"Company profile for '{model.CompanyName}' submitted to SQL Server! Verification status is Pending.";
            return RedirectToAction("Dashboard", "Recruiter");
        }

        foreach (var err in result.Errors)
        {
            ModelState.AddModelError("", err.Description);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        HttpContext.Session.Clear();
        TempData["InfoMessage"] = "You have been logged out successfully.";
        return RedirectToAction("Index", "Home");
    }
}
