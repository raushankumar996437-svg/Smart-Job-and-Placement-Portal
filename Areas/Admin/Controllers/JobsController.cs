using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHire.Data;
using SmartHire.Models;

namespace SmartHire.Areas.Admin.Controllers;

[Area("Admin")]
public class JobsController : Controller
{
    private readonly ApplicationDbContext _context;

    public JobsController(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool CheckAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var jobs = await _context.Jobs
            .Include(j => j.Company)
            .Include(j => j.Applications)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();

        return View(jobs);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        ViewBag.Companies = await _context.Companies.Where(c => c.ApprovalStatus == "Approved").ToListAsync();
        return View(new Job());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Job job)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        if (!ModelState.IsValid)
        {
            ViewBag.Companies = await _context.Companies.Where(c => c.ApprovalStatus == "Approved").ToListAsync();
            return View(job);
        }

        job.CreatedAt = DateTime.UtcNow;
        job.PostedAt = DateTime.UtcNow;
        job.IsActive = true;

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Job opening '{job.Title}' published successfully to SQL Server!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return NotFound();

        ViewBag.Companies = await _context.Companies.Where(c => c.ApprovalStatus == "Approved").ToListAsync();
        return View(job);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Job job)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        if (id != job.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewBag.Companies = await _context.Companies.Where(c => c.ApprovalStatus == "Approved").ToListAsync();
            return View(job);
        }

        var existing = await _context.Jobs.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Title = job.Title;
        existing.CompanyId = job.CompanyId;
        existing.Description = job.Description;
        existing.Requirements = job.Requirements;
        existing.Responsibilities = job.Responsibilities;
        existing.JobType = job.JobType;
        existing.WorkMode = job.WorkMode;
        existing.Location = job.Location;
        existing.MinimumCGPA = job.MinimumCGPA;
        existing.Vacancies = job.Vacancies;
        existing.SalaryMin = job.SalaryMin;
        existing.SalaryMax = job.SalaryMax;
        existing.Deadline = job.Deadline;
        existing.IsActive = job.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Job '{job.Title}' updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var job = await _context.Jobs.FindAsync(id);
        if (job != null)
        {
            job.IsActive = !job.IsActive;
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Job '{job.Title}' status changed to {(job.IsActive ? "Active" : "Closed")}.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var job = await _context.Jobs.FindAsync(id);
        if (job != null)
        {
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Job '{job.Title}' removed permanently.";
        }

        return RedirectToAction(nameof(Index));
    }
}
