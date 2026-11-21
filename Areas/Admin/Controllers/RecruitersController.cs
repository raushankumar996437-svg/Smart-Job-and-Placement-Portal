using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHire.Data;

namespace SmartHire.Areas.Admin.Controllers;

[Area("Admin")]
public class RecruitersController : Controller
{
    private readonly ApplicationDbContext _context;

    public RecruitersController(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool CheckAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var recruiters = await _context.RecruiterProfiles
            .Include(r => r.Company)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return View(recruiters);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var recruiter = await _context.RecruiterProfiles.FindAsync(id);
        if (recruiter != null)
        {
            _context.RecruiterProfiles.Remove(recruiter);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Recruiter profile removed.";
        }

        return RedirectToAction(nameof(Index));
    }
}
