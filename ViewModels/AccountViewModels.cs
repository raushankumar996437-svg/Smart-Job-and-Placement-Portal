using System.ComponentModel.DataAnnotations;

namespace SmartHire.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember my session")]
    public bool RememberMe { get; set; } = false;

    [Display(Name = "Portal Role")]
    public string Role { get; set; } = "Student";

    public string? ReturnUrl { get; set; }
}

public class StudentRegisterViewModel
{
    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Student Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid academic/personal email")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "College / University name is required")]
    [Display(Name = "College / Institute")]
    public string CollegeName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required")]
    public string Department { get; set; } = "Computer Science & Engineering";

    [Required(ErrorMessage = "Graduation Year is required")]
    [Range(2023, 2030, ErrorMessage = "Graduation year must be between 2023 and 2030")]
    [Display(Name = "Graduation Year")]
    public int GraduationYear { get; set; } = 2026;

    [Required(ErrorMessage = "CGPA is required")]
    [Range(0.0, 10.0, ErrorMessage = "CGPA must be between 0.0 and 10.0")]
    [Display(Name = "Current CGPA")]
    public double CGPA { get; set; } = 8.5;

    [Required(ErrorMessage = "Enter your core technical skills")]
    [Display(Name = "Technical Skills (comma separated)")]
    public string TechnicalSkills { get; set; } = "C#, ASP.NET Core, SQL Server, JavaScript, Python";

    [Url(ErrorMessage = "Enter a valid LinkedIn URL")]
    [Display(Name = "LinkedIn Profile")]
    public string? LinkedInUrl { get; set; }

    [Url(ErrorMessage = "Enter a valid GitHub URL")]
    [Display(Name = "GitHub Profile")]
    public string? GitHubUrl { get; set; }
}

public class RecruiterRegisterViewModel
{
    [Required(ErrorMessage = "Contact person name is required")]
    [Display(Name = "HR / Recruiter Full Name")]
    public string ContactPerson { get; set; } = string.Empty;

    [Required(ErrorMessage = "Designation is required")]
    [Display(Name = "Official Designation")]
    public string Designation { get; set; } = "Talent Acquisition Specialist";

    [Required(ErrorMessage = "Company name is required")]
    [Display(Name = "Company / Organization Name")]
    public string CompanyName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Official corporate email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid corporate email address")]
    [Display(Name = "Official Corporate Email")]
    public string OfficialEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Company website is required")]
    [Url(ErrorMessage = "Enter a valid URL (e.g. https://company.com)")]
    [Display(Name = "Company Website")]
    public string Website { get; set; } = string.Empty;

    [Required(ErrorMessage = "Industry sector is required")]
    public string Industry { get; set; } = "Information Technology / Software";

    [Display(Name = "Company Size")]
    public string CompanySize { get; set; } = "50-250 Employees";

    [Required(ErrorMessage = "Company overview is required")]
    [Display(Name = "About Company")]
    public string Description { get; set; } = string.Empty;
}
