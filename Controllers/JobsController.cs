using Microsoft.AspNetCore.Mvc;
using SmartHire.Services;
using SmartHire.ViewModels;

namespace SmartHire.Controllers;

public class JobsController : Controller
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public IActionResult Index(string? keyword, string? location, string? jobType, string? workMode, double? minCgpa)
    {
        // Enforce Login Requirement: Internal tab accessible only after login/register
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserRole")))
        {
            TempData["InfoMessage"] = "Please sign in or register to browse campus job openings.";
            return RedirectToAction("Login", "Account", new { returnUrl = "/Jobs" });
        }

        var jobs = _jobService.SearchJobs(keyword, location, jobType, workMode, minCgpa);
        var model = new JobSearchFilterViewModel
        {
            Keyword = keyword,
            Location = location,
            JobType = jobType,
            WorkMode = workMode,
            MinCGPA = minCgpa,
            Jobs = jobs
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserRole")))
        {
            TempData["InfoMessage"] = "Please sign in to view job details.";
            return RedirectToAction("Login", "Account", new { returnUrl = $"/Jobs/Details/{id}" });
        }

        var job = _jobService.GetJobById(id);
        if (job == null)
        {
            return NotFound();
        }
        return View(job);
    }

    [HttpGet]
    public IActionResult Apply(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserRole")))
        {
            TempData["InfoMessage"] = "Please sign in as a student to apply for this job.";
            return RedirectToAction("Login", "Account", new { returnUrl = $"/Jobs/Apply/{id}" });
        }

        var job = _jobService.GetJobById(id);
        if (job == null)
        {
            return NotFound();
        }

        var userName = HttpContext.Session.GetString("UserName") ?? "Raushan Kumar";
        var userEmail = HttpContext.Session.GetString("UserEmail") ?? "raushan@student.college.edu";

        var model = new JobApplyViewModel
        {
            JobId = job.Id,
            JobTitle = job.Title,
            CompanyName = job.CompanyName,
            StudentName = userName,
            Email = userEmail,
            CalculatedAiScore = job.AiMatchPercent
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Apply(JobApplyViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        TempData["SuccessMessage"] = $"Application for '{model.JobTitle}' at {model.CompanyName} submitted successfully! Your application status is now 'Under Review'.";
        return RedirectToAction("Dashboard", "Student");
    }
}
