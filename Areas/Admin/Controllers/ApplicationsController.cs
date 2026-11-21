using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartHire.Data;
using SmartHire.Models;

namespace SmartHire.Areas.Admin.Controllers;

[Area("Admin")]
public class ApplicationsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ApplicationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool CheckAdmin() => HttpContext.Session.GetString("UserRole") == "Admin";

    [HttpGet]
    public async Task<IActionResult> Index(string? statusFilter, int? jobId)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var query = _context.Applications
            .Include(a => a.Job).ThenInclude(j => j!.Company)
            .Include(a => a.Interviews)
            .AsQueryable();

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "All")
        {
            query = query.Where(a => a.Status == statusFilter);
        }

        if (jobId.HasValue && jobId > 0)
        {
            query = query.Where(a => a.JobId == jobId);
        }

        var applications = await query.OrderByDescending(a => a.AppliedAt).ToListAsync();

        // Fetch student profiles to match userIds
        var studentUserIds = applications.Select(a => a.StudentUserId).Distinct().ToList();
        var students = await _context.StudentProfiles
            .Include(s => s.User)
            .Where(s => studentUserIds.Contains(s.UserId))
            .ToDictionaryAsync(s => s.UserId, s => s);

        ViewBag.StudentProfiles = students;
        ViewBag.StatusFilter = statusFilter;
        ViewBag.Jobs = await _context.Jobs.ToListAsync();

        return View(applications);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string newStatus, string? remarks)
    {
        if (!CheckAdmin()) return RedirectToAction("Login", "Account", new { area = "Admin" });

        var app = await _context.Applications.FindAsync(id);
        if (app != null)
        {
            var oldStatus = app.Status;
            app.Status = newStatus;
            app.UpdatedAt = DateTime.UtcNow;

            // Audit history
            _context.ApplicationStatusHistories.Add(new ApplicationStatusHistory
            {
                ApplicationId = app.Id,
                PreviousStatus = oldStatus,
                NewStatus = newStatus,
                ChangedByUserId = HttpContext.Session.GetString("UserEmail") ?? "admin@smarthire.local",
                ChangedAt = DateTime.UtcNow,
                Remarks = remarks ?? $"Status manually modified by Administrator to '{newStatus}'"
            });

            // If scheduled, create sample interview round
            if (newStatus == "InterviewScheduled" && !await _context.Interviews.AnyAsync(i => i.ApplicationId == app.Id))
            {
                _context.Interviews.Add(new Interview
                {
                    ApplicationId = app.Id,
                    ScheduledAt = DateTime.UtcNow.AddDays(3),
                    InterviewMode = "Virtual (Google Meet)",
                    MeetingLink = "https://meet.google.com/adm-hire-call",
                    Notes = remarks ?? "Admin scheduled technical placement round.",
                    Status = "Scheduled"
                });
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Application #{id} status updated from '{oldStatus}' to '{newStatus}'. Audit trail logged!";
        }

        return RedirectToAction(nameof(Index));
    }
}
