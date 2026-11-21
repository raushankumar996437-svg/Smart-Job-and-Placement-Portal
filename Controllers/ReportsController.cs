using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace SmartHire.Controllers;

public class ReportsController : Controller
{
    public IActionResult Analytics()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role))
        {
            TempData["InfoMessage"] = "Please sign in to view Institutional Placement Analytics & Reports.";
            return RedirectToAction("Login", "Account", new { returnUrl = "/Reports/Analytics" });
        }

        return View();
    }

    [HttpGet]
    public IActionResult ExportCsv()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (string.IsNullOrEmpty(role))
        {
            return RedirectToAction("Login", "Account");
        }

        var csv = new StringBuilder();
        csv.AppendLine("StudentId,FullName,Department,GraduationYear,CGPA,PlacedCompany,JobRole,PackageLPA,Status");
        csv.AppendLine("STU101,Aarav Sharma,Computer Science,2026,8.9,CloudTech Systems,Junior .NET Engineer,8.5,Selected");
        csv.AppendLine("STU102,Raushan Kumar,Information Technology,2026,8.7,NextGen Digital,Full-Stack Developer,9.2,Selected");
        csv.AppendLine("STU103,Priya Patel,Electronics & Comm,2026,8.4,DataMatrix Solutions,Database Engineer,7.0,Selected");
        csv.AppendLine("STU104,Sneha Rao,Computer Science,2026,9.1,NeuralMatrix AI,AI Analyst,12.0,Selected");

        var bytes = Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", $"SmartHire_Placement_Report_{DateTime.Now:yyyyMMdd}.csv");
    }
}
