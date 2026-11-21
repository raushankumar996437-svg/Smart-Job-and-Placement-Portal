using System.ComponentModel.DataAnnotations;

namespace SmartHire.ViewModels;

public class AdminLoginViewModel
{
    [Required(ErrorMessage = "Admin official email is required")]
    [EmailAddress]
    public string Email { get; set; } = "admin@smarthire.local";

    [Required(ErrorMessage = "Admin password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "Admin@2026!";

    public bool RememberMe { get; set; }
}

public class AdminRegisterViewModel
{
    [Required(ErrorMessage = "Administrator Full Name is required")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Official Institutional Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password")]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Institutional Department / Office is required")]
    public string Department { get; set; } = "Central Placement Cell";

    [Required(ErrorMessage = "Secret Master Admin Key is required to authorize admin accounts")]
    [Display(Name = "Admin Security Key")]
    public string SecurityKey { get; set; } = string.Empty; // Required key: "SmartAdmin2026!"
}
