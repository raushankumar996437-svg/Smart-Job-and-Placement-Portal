using Microsoft.AspNetCore.Identity;

namespace SmartHire.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string UserRole { get; set; } = "Student"; // "Student", "Recruiter", "Admin"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
