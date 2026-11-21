using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHire.Models;

public class StudentProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

    [Required]
    [StringLength(150)]
    public string CollegeName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Degree { get; set; } = "B.Tech";

    [Required]
    [StringLength(100)]
    public string Department { get; set; } = "Computer Science";

    public int GraduationYear { get; set; } = 2026;

    [Column(TypeName = "decimal(4, 2)")]
    public double CGPA { get; set; } = 8.5;

    [StringLength(255)]
    public string? LinkedInUrl { get; set; }

    [StringLength(255)]
    public string? GitHubUrl { get; set; }

    [StringLength(500)]
    public string? ResumePath { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<StudentSkill> StudentSkills { get; set; } = new List<StudentSkill>();
}

public class Company
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string OfficialEmail { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Website { get; set; }

    [StringLength(100)]
    public string Industry { get; set; } = "IT";

    public string? Description { get; set; }

    [StringLength(50)]
    public string ApprovalStatus { get; set; } = "Approved"; // "Pending", "Approved", "Rejected"

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RecruiterProfile> Recruiters { get; set; } = new List<RecruiterProfile>();
    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}

public class RecruiterProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

    public int CompanyId { get; set; }
    public virtual Company? Company { get; set; }

    [Required]
    [StringLength(100)]
    public string ContactPerson { get; set; } = string.Empty;

    [StringLength(100)]
    public string Designation { get; set; } = "HR";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Skill
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public virtual ICollection<StudentSkill> StudentSkills { get; set; } = new List<StudentSkill>();
    public virtual ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
}

public class StudentSkill
{
    public int StudentProfileId { get; set; }
    public virtual StudentProfile? StudentProfile { get; set; }

    public int SkillId { get; set; }
    public virtual Skill? Skill { get; set; }
}

public class Job
{
    [Key]
    public int Id { get; set; }

    public int CompanyId { get; set; }
    public virtual Company? Company { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public string? Requirements { get; set; }
    public string? Responsibilities { get; set; }

    [StringLength(50)]
    public string JobType { get; set; } = "Full-Time";

    [StringLength(50)]
    public string WorkMode { get; set; } = "Hybrid";

    [StringLength(100)]
    public string Location { get; set; } = "Bangalore";

    [Column(TypeName = "decimal(4, 2)")]
    public double MinimumCGPA { get; set; } = 7.0;

    public int Vacancies { get; set; } = 5;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SalaryMin { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? SalaryMax { get; set; }

    public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    public DateTime? Deadline { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<JobSkill> JobSkills { get; set; } = new List<JobSkill>();
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
}

public class JobSkill
{
    public int JobId { get; set; }
    public virtual Job? Job { get; set; }

    public int SkillId { get; set; }
    public virtual Skill? Skill { get; set; }
}

public class Application
{
    [Key]
    public int Id { get; set; }

    public int JobId { get; set; }
    public virtual Job? Job { get; set; }

    [Required]
    public string StudentUserId { get; set; } = string.Empty;

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Applied"; // "Applied", "UnderReview", "Shortlisted", "InterviewScheduled", "Selected", "Rejected", "Withdrawn"

    [StringLength(500)]
    public string? ResumePath { get; set; }

    public string? CoverLetter { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ApplicationStatusHistory> StatusHistories { get; set; } = new List<ApplicationStatusHistory>();
    public virtual ICollection<Interview> Interviews { get; set; } = new List<Interview>();
}

public class ApplicationStatusHistory
{
    [Key]
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    [StringLength(50)]
    public string PreviousStatus { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string NewStatus { get; set; } = string.Empty;

    public string? ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    public string? Remarks { get; set; }
}

public class Interview
{
    [Key]
    public int Id { get; set; }

    public int ApplicationId { get; set; }
    public virtual Application? Application { get; set; }

    public DateTime ScheduledAt { get; set; }

    [StringLength(50)]
    public string InterviewMode { get; set; } = "Virtual";

    [StringLength(500)]
    public string? MeetingLink { get; set; }

    [StringLength(200)]
    public string? Location { get; set; }

    public string? Notes { get; set; }

    [StringLength(50)]
    public string Status { get; set; } = "Scheduled";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Notification
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class AuditLog
{
    [Key]
    public int Id { get; set; }

    public string? UserId { get; set; }

    [Required]
    [StringLength(100)]
    public string Action { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string EntityType { get; set; } = string.Empty;

    public string? EntityId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Details { get; set; }
}
