using Microsoft.AspNetCore.Mvc;
using SmartHire.Services;
using SmartHire.ViewModels;

namespace SmartHire.Controllers;

public class AiController : Controller
{
    private readonly IAiCareerService _aiService;
    private readonly IJobService _jobService;

    public AiController(IAiCareerService aiService, IJobService jobService)
    {
        _aiService = aiService;
        _jobService = jobService;
    }

    [HttpGet]
    public IActionResult Assistant()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role))
        {
            TempData["InfoMessage"] = "Please sign in to access the AI Career Copilot & ATS Resume Scorer.";
            return RedirectToAction("Login", "Account", new { returnUrl = "/Ai/Assistant" });
        }

        var request = new AiAnalysisRequestViewModel();
        var result = _aiService.AnalyzeCandidate(request, _jobService.GetAllJobs());

        ViewBag.Request = request;
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Assistant(AiAnalysisRequestViewModel request)
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role))
        {
            return RedirectToAction("Login", "Account");
        }

        var result = _aiService.AnalyzeCandidate(request, _jobService.GetAllJobs());
        ViewBag.Request = request;
        TempData["AiUpdated"] = "AI Analysis regenerated with updated skills and parameters!";
        return View(result);
    }
}
