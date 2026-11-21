using System.ComponentModel.DataAnnotations;

namespace SmartHire.ViewModels;

public class JobItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string JobType { get; set; } = "Full-Time";
    public string WorkMode { get; set; } = "Hybrid";
    public double MinimumCGPA { get; set; } = 7.0;
    public string SalaryRange { get; set; } = "₹ 6.0 - 9.0 LPA";
    public List<string> Skills { get; set; } = new();
    public string Description { get; set; } = string.Empty;
    public string Responsibilities { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public int Vacancies { get; set; } = 5;
    public string PostedAgo { get; set; } = "2 days ago";
    public string Deadline { get; set; } = "15 Dec 2026";
    public bool IsActive { get; set; } = true;
    public double AiMatchPercent { get; set; } = 85.0;
}

public class JobSearchFilterViewModel
{
    public string? Keyword { get; set; }
    public string? Location { get; set; }
    public string? JobType { get; set; }
    public string? WorkMode { get; set; }
    public double? MinCGPA { get; set; }
    public string? Skill { get; set; }
    public List<JobItemViewModel> Jobs { get; set; } = new();
    public int TotalResults => Jobs.Count;
}

public class JobApplyViewModel
{
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Full Name is required")]
    public string StudentName { get; set; } = "Raushan Kumar";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = "raushan@student.college.edu";

    [Range(0, 10)]
    public double CGPA { get; set; } = 8.7;

    public string CoverLetter { get; set; } = "Excited to apply for this role. I have extensive hands-on experience in full-stack .NET and enterprise database systems.";
    
    public string? ResumeFileName { get; set; }
    public double CalculatedAiScore { get; set; } = 92.0;
}
