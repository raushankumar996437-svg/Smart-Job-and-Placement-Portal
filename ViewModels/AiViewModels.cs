namespace SmartHire.ViewModels;

public class AiAnalysisRequestViewModel
{
    public string TargetRole { get; set; } = "Junior .NET Full-Stack Engineer";
    public string StudentSkills { get; set; } = "C#, ASP.NET Core, SQL Server, Entity Framework Core, HTML5, CSS3, JavaScript, Git, REST APIs";
    public string ResumeSummary { get; set; } = "Final year Computer Science student with practical projects in ASP.NET Core MVC, SQL Server database design, and role-based authentication.";
    public double StudentCGPA { get; set; } = 8.6;
}

public class AiAnalysisResultViewModel
{
    public double OverallMatchScore { get; set; }
    public double AtsReadinessScore { get; set; }
    public string MatchCategory { get; set; } = "Strong Match";
    public List<string> MatchedSkills { get; set; } = new();
    public List<string> MissingSkills { get; set; } = new();
    public List<string> AiRecommendations { get; set; } = new();
    public List<string> PredictedInterviewQuestions { get; set; } = new();
    public List<JobItemViewModel> RecommendedJobs { get; set; } = new();
}
