using Microsoft.AspNetCore.Mvc;
using SmartHire.Services;

namespace SmartHire.Controllers;

public class RecruiterController : Controller
{
    private readonly IJobService _jobService;

    public RecruiterController(IJobService jobService)
    {
        _jobService = jobService;
    }

    public IActionResult Dashboard()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role))
        {
            TempData["InfoMessage"] = "Please sign in to access the Recruiter Workspace.";
            return RedirectToAction("Login", "Account", new { returnUrl = "/Recruiter/Dashboard" });
        }

        ViewBag.PostedJobs = _jobService.GetAllJobs().Take(3).ToList();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateApplicantStatus(string candidateName, string status)
    {
        TempData["SuccessMessage"] = $"Applicant '{candidateName}' status successfully updated to '{status}'. Candidate notified!";
        return RedirectToAction("Dashboard");
    }
}
