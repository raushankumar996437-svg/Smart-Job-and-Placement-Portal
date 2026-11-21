using SmartHire.ViewModels;

namespace SmartHire.Services;

public interface IJobService
{
    List<JobItemViewModel> GetAllJobs();
    JobItemViewModel? GetJobById(int id);
    List<JobItemViewModel> SearchJobs(string? keyword, string? location, string? jobType, string? workMode, double? minCgpa);
    List<JobItemViewModel> GetFeaturedJobs();
}

public class JobService : IJobService
{
    private static readonly List<JobItemViewModel> _jobs = new()
    {
        new JobItemViewModel
        {
            Id = 1,
            Title = "Junior .NET Software Engineer",
            CompanyName = "CloudTech Systems",
            Location = "Bangalore, India",
            JobType = "Full-Time",
            WorkMode = "Hybrid",
            MinimumCGPA = 7.0,
            SalaryRange = "₹ 6.5 - 9.0 LPA",
            Skills = new() { "C#", "ASP.NET Core", "SQL Server", "EF Core", "REST APIs" },
            Description = "Join CloudTech Systems to architect and deploy resilient web backends using .NET Core and modern relational databases.",
            Responsibilities = "Develop high-throughput REST APIs, write transactional EF Core models, and integrate secure authentication workflows.",
            Requirements = "Sound knowledge of OOP, C#, ASP.NET Core MVC, SQL Server, and Git version control.",
            Vacancies = 8,
            PostedAgo = "1 day ago",
            Deadline = "28 Dec 2026",
            AiMatchPercent = 94.0
        },
        new JobItemViewModel
        {
            Id = 2,
            Title = "Full-Stack Web Developer",
            CompanyName = "NextGen Digital Labs",
            Location = "Hyderabad, India",
            JobType = "Full-Time",
            WorkMode = "Remote",
            MinimumCGPA = 7.5,
            SalaryRange = "₹ 7.0 - 10.5 LPA",
            Skills = new() { "C#", "ASP.NET Core", "JavaScript", "Bootstrap 5", "HTML5/CSS3" },
            Description = "Create responsive, accessible, interactive web portals connecting college graduates with corporate recruitment pipelines.",
            Responsibilities = "Craft clean Razor views, design responsive layouts, and connect endpoints with asynchronous service layers.",
            Requirements = "Proficiency in HTML5, CSS3, JavaScript, Bootstrap, and C# MVC razor controllers.",
            Vacancies = 5,
            PostedAgo = "2 days ago",
            Deadline = "20 Dec 2026",
            AiMatchPercent = 89.0
        },
        new JobItemViewModel
        {
            Id = 3,
            Title = "Associate Database Engineer",
            CompanyName = "DataMatrix Solutions",
            Location = "Pune, India",
            JobType = "Full-Time",
            WorkMode = "Hybrid",
            MinimumCGPA = 6.5,
            SalaryRange = "₹ 5.5 - 7.5 LPA",
            Skills = new() { "SQL Server", "SSMS 22", "T-SQL", "Database Design", "Indexing" },
            Description = "Manage relational database schemas, optimize queries, construct stored procedures, and enforce ACID guarantees.",
            Responsibilities = "Design normalized tables, configure foreign key cascading rules, tune slow queries, and monitor backups.",
            Requirements = "Solid foundation in relational databases, indexing concepts, SQL queries, and SSMS operations.",
            Vacancies = 4,
            PostedAgo = "3 days ago",
            Deadline = "31 Dec 2026",
            AiMatchPercent = 82.0
        },
        new JobItemViewModel
        {
            Id = 4,
            Title = "Frontend UI/UX Developer",
            CompanyName = "PixelCraft Technologies",
            Location = "Bangalore, India",
            JobType = "Internship",
            WorkMode = "On-Site",
            MinimumCGPA = 6.0,
            SalaryRange = "₹ 28,000 / month",
            Skills = new() { "Bootstrap 5", "JavaScript", "HTML5", "CSS3", "Responsive Design" },
            Description = "Work with our design teams to craft modern, futuristic cyber-styled web dashboards with high performance.",
            Responsibilities = "Convert Figma mockups into responsive HTML/CSS/JS components with glassmorphism and subtle 3D effects.",
            Requirements = "Passion for clean UI/UX, CSS transitions, cross-browser responsiveness, and component design.",
            Vacancies = 6,
            PostedAgo = "Just now",
            Deadline = "18 Dec 2026",
            AiMatchPercent = 78.0
        },
        new JobItemViewModel
        {
            Id = 5,
            Title = "Cloud & DevOps Trainee",
            CompanyName = "SkyCloud Infrastructure",
            Location = "Mumbai, India",
            JobType = "Full-Time",
            WorkMode = "On-Site",
            MinimumCGPA = 7.0,
            SalaryRange = "₹ 6.0 - 8.5 LPA",
            Skills = new() { "Docker", "CI/CD", "Linux", "Git", "Azure" },
            Description = "Automate software deployments, configure container clusters, and manage production CI/CD pipelines.",
            Responsibilities = "Support Docker builds, manage GitHub workflows, maintain test environments, and enforce zero-downtime releases.",
            Requirements = "Understanding of containerization, cloud fundamentals (Azure/AWS), shell scripting, and version control.",
            Vacancies = 3,
            PostedAgo = "4 days ago",
            Deadline = "05 Jan 2027",
            AiMatchPercent = 71.0
        },
        new JobItemViewModel
        {
            Id = 6,
            Title = "AI & Data Science Analyst",
            CompanyName = "NeuralMatrix AI",
            Location = "Gurugram, India",
            JobType = "Internship",
            WorkMode = "Remote",
            MinimumCGPA = 7.5,
            SalaryRange = "₹ 35,000 / month",
            Skills = new() { "Python", "Machine Learning", "SQL", "Pandas", "Data Analytics" },
            Description = "Leverage advanced analytics and rule-based recommendation algorithms to match job candidates with optimal openings.",
            Responsibilities = "Build predictive models, clean data pipelines, generate placement insight dashboards, and evaluate metrics.",
            Requirements = "Strong mathematical and statistical aptitude, Python data libraries, and database querying capabilities.",
            Vacancies = 4,
            PostedAgo = "5 days ago",
            Deadline = "25 Dec 2026",
            AiMatchPercent = 88.0
        }
    };

    public List<JobItemViewModel> GetAllJobs() => _jobs;

    public JobItemViewModel? GetJobById(int id) => _jobs.FirstOrDefault(j => j.Id == id);

    public List<JobItemViewModel> SearchJobs(string? keyword, string? location, string? jobType, string? workMode, double? minCgpa)
    {
        var query = _jobs.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var kw = keyword.Trim().ToLower();
            query = query.Where(j =>
                j.Title.ToLower().Contains(kw) ||
                j.CompanyName.ToLower().Contains(kw) ||
                j.Skills.Any(s => s.ToLower().Contains(kw)) ||
                j.Description.ToLower().Contains(kw));
        }

        if (!string.IsNullOrWhiteSpace(location))
        {
            var loc = location.Trim().ToLower();
            query = query.Where(j => j.Location.ToLower().Contains(loc));
        }

        if (!string.IsNullOrWhiteSpace(jobType) && jobType != "All")
        {
            query = query.Where(j => j.JobType.Equals(jobType, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(workMode) && workMode != "All")
        {
            query = query.Where(j => j.WorkMode.Equals(workMode, StringComparison.OrdinalIgnoreCase));
        }

        if (minCgpa.HasValue)
        {
            query = query.Where(j => j.MinimumCGPA <= minCgpa.Value);
        }

        return query.ToList();
    }

    public List<JobItemViewModel> GetFeaturedJobs() => _jobs.Take(3).ToList();
}
