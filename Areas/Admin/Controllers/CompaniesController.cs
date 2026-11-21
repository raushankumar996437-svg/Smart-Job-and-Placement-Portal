using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHire.Data;
using SmartHire.Models;

namespace SmartHire.Areas.Admin.Controllers;

[Area("Admin")]
public class CompaniesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CompaniesController(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool CheckAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var companies = await _context.Companies
            .Include(c => c.Jobs)
            .Include(c => c.Recruiters)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        return View(companies);
    }

    [HttpGet]
    public IActionResult Create()
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });
        return View(new Company());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Company company)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        if (!ModelState.IsValid) return View(company);

        company.CreatedAt = DateTime.UtcNow;
        company.ApprovalStatus = "Approved"; // Admin created is auto-approved

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Company '{company.CompanyName}' successfully added and approved!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var company = await _context.Companies.FindAsync(id);
        if (company == null) return NotFound();

        return View(company);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Company company)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        if (id != company.Id) return NotFound();

        if (!ModelState.IsValid) return View(company);

        var existing = await _context.Companies.FindAsync(id);
        if (existing == null) return NotFound();

        existing.CompanyName = company.CompanyName;
        existing.OfficialEmail = company.OfficialEmail;
        existing.Website = company.Website;
        existing.Industry = company.Industry;
        existing.Description = company.Description;
        existing.ApprovalStatus = company.ApprovalStatus;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Company '{company.CompanyName}' details updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id, string status)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var company = await _context.Companies.FindAsync(id);
        if (company != null)
        {
            company.ApprovalStatus = status;
            company.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Company '{company.CompanyName}' status updated to '{status}'.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var company = await _context.Companies
            .Include(c => c.Jobs)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (company != null)
        {
            // Remove associated jobs first if any
            if (company.Jobs.Any())
            {
                _context.Jobs.RemoveRange(company.Jobs);
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Company '{company.CompanyName}' and associated jobs deleted permanently.";
        }

        return RedirectToAction(nameof(Index));
    }
}
