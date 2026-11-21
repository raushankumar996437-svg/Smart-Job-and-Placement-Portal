using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHire.Data;

namespace SmartHire.Areas.Admin.Controllers;

[Area("Admin")]
public class StudentsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StudentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool CheckAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var students = await _context.StudentProfiles
            .Include(s => s.User)
            .OrderByDescending(s => s.CGPA)
            .ToListAsync();

        // Get application counts per student
        var appCounts = await _context.Applications
            .GroupBy(a => a.StudentUserId)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        ViewBag.AppCounts = appCounts;
        return View(students);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var student = await _context.StudentProfiles
            .Include(s => s.User)
            .Include(s => s.StudentSkills).ThenInclude(ss => ss.Skill)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null) return NotFound();

        // Fetch all jobs this student applied to
        var applications = await _context.Applications
            .Include(a => a.Job).ThenInclude(j => j!.Company)
            .Where(a => a.StudentUserId == student.UserId)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();

        ViewBag.Applications = applications;
        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var student = await _context.StudentProfiles.FindAsync(id);
        if (student != null)
        {
            _context.StudentProfiles.Remove(student);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student profile deleted successfully from SQL Server.";
        }

        return RedirectToAction(nameof(Index));
    }
}
