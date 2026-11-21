using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHire.Data;

namespace SmartHire.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Enforce Admin session/auth
        var role = HttpContext.Session.GetString("UserRole");
        if (role != "Admin")
        {
            TempData["InfoMessage"] = "Please log in with Administrator credentials to access the Admin Console.";
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        ViewBag.TotalStudents = await _context.StudentProfiles.CountAsync();
        ViewBag.TotalCompanies = await _context.Companies.CountAsync();
        ViewBag.TotalJobs = await _context.Jobs.CountAsync();
        ViewBag.TotalApplications = await _context.Applications.CountAsync();

        ViewBag.CompaniesList = await _context.Companies.Take(5).ToListAsync();
        ViewBag.JobsList = await _context.Jobs.Include(j => j.Company).Take(5).ToListAsync();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleCompanyStatus(int companyId, string status)
    {
        var company = await _context.Companies.FindAsync(companyId);
        if (company != null)
        {
            company.ApprovalStatus = status;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Company '{company.CompanyName}' status updated to '{status}'.";
        }
        return RedirectToAction("Index");
    }
}
