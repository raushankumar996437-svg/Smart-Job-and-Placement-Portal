using Microsoft.AspNetCore.Mvc;
using SmartHire.Services;

namespace SmartHire.Controllers;

public class StudentController : Controller
{
    private readonly IJobService _jobService;

    public StudentController(IJobService jobService)
    {
        _jobService = jobService;
    }

    public IActionResult Dashboard()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role))
        {
            TempData["InfoMessage"] = "Please sign in to access your Student Dashboard.";
            return RedirectToAction("Login", "Account", new { returnUrl = "/Student/Dashboard" });
        }

        ViewBag.RecommendedJobs = _jobService.GetFeaturedJobs();
        return View();
    }
}
