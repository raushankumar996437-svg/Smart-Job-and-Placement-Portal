using Microsoft.AspNetCore.Identity;
using SmartHire.Models;

namespace SmartHire.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        await context.Database.EnsureCreatedAsync();

        // 1. Seed Roles
        string[] roles = { "Admin", "Student", "Recruiter" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Seed Default Admin User
        var adminEmail = "admin@smarthire.local";
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Chief Placement Officer",
                UserRole = "Admin",
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, "Admin@2026!");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // 3. Seed Default Skills
        if (!context.Skills.Any())
        {
            var skills = new[]
            {
                new Skill { Name = "C#", IsActive = true },
                new Skill { Name = "ASP.NET Core", IsActive = true },
                new Skill { Name = "SQL Server", IsActive = true },
                new Skill { Name = "Entity Framework Core", IsActive = true },
                new Skill { Name = "JavaScript", IsActive = true },
                new Skill { Name = "Bootstrap 5", IsActive = true },
                new Skill { Name = "Docker", IsActive = true },
                new Skill { Name = "Python", IsActive = true },
                new Skill { Name = "Git", IsActive = true }
            };
            await context.Skills.AddRangeAsync(skills);
            await context.SaveChangesAsync();
        }

        // 4. Seed Companies
        if (!context.Companies.Any())
        {
            var companies = new[]
            {
                new Company
                {
                    CompanyName = "CloudTech Systems Pvt Ltd",
                    OfficialEmail = "careers@cloudtech.com",
                    Website = "https://www.cloudtech.com",
                    Industry = "Cloud & Enterprise Software",
                    ApprovalStatus = "Approved",
                    Description = "Leading cloud-native enterprise application development company."
                },
                new Company
                {
                    CompanyName = "NextGen Digital Labs",
                    OfficialEmail = "hr@nextgendigital.com",
                    Website = "https://www.nextgendigital.com",
                    Industry = "Full-Stack Web & AI",
                    ApprovalStatus = "Approved",
                    Description = "Modern web solutions and digital transformation partner."
                },
                new Company
                {
                    CompanyName = "DataMatrix Solutions",
                    OfficialEmail = "placements@datamatrix.com",
                    Website = "https://www.datamatrix.com",
                    Industry = "Relational Data Infrastructure",
                    ApprovalStatus = "Approved",
                    Description = "Specializes in large-scale SQL Server databases, data lakes, and indexing."
                },
                new Company
                {
                    CompanyName = "Vanguard AI Technologies",
                    OfficialEmail = "contact@vanguardai.io",
                    Website = "https://vanguardai.io",
                    Industry = "Artificial Intelligence",
                    ApprovalStatus = "Pending", // Pending verification for admin testing!
                    Description = "Next-gen deep learning and natural language processing applications."
                }
            };
            await context.Companies.AddRangeAsync(companies);
            await context.SaveChangesAsync();
        }

        // 5. Seed Sample Students
        var student1Email = "raushan@student.college.edu";
        var student1 = await userManager.FindByEmailAsync(student1Email);
        if (student1 == null)
        {
            student1 = new ApplicationUser
            {
                UserName = student1Email,
                Email = student1Email,
                FullName = "Raushan Kumar",
                UserRole = "Student",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(student1, "Pass@123");
            await userManager.AddToRoleAsync(student1, "Student");

            context.StudentProfiles.Add(new StudentProfile
            {
                UserId = student1.Id,
                CollegeName = "National Institute of Technology",
                Degree = "B.Tech",
                Department = "Computer Science & Engineering",
                GraduationYear = 2026,
                CGPA = 8.7,
                LinkedInUrl = "https://linkedin.com/in/raushan-kumar",
                GitHubUrl = "https://github.com/raushan-kumar",
                ResumePath = "resume_raushan_v2.pdf",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        var student2Email = "aarav@student.college.edu";
        var student2 = await userManager.FindByEmailAsync(student2Email);
        if (student2 == null)
        {
            student2 = new ApplicationUser
            {
                UserName = student2Email,
                Email = student2Email,
                FullName = "Aarav Sharma",
                UserRole = "Student",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(student2, "Pass@123");
            await userManager.AddToRoleAsync(student2, "Student");

            context.StudentProfiles.Add(new StudentProfile
            {
                UserId = student2.Id,
                CollegeName = "IIT Delhi",
                Degree = "B.Tech",
                Department = "Information Technology",
                GraduationYear = 2026,
                CGPA = 8.9,
                ResumePath = "resume_aarav.pdf",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        var student3Email = "priya@student.college.edu";
        var student3 = await userManager.FindByEmailAsync(student3Email);
        if (student3 == null)
        {
            student3 = new ApplicationUser
            {
                UserName = student3Email,
                Email = student3Email,
                FullName = "Priya Patel",
                UserRole = "Student",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(student3, "Pass@123");
            await userManager.AddToRoleAsync(student3, "Student");

            context.StudentProfiles.Add(new StudentProfile
            {
                UserId = student3.Id,
                CollegeName = "BITS Pilani",
                Degree = "B.Tech",
                Department = "Electronics & Communication",
                GraduationYear = 2026,
                CGPA = 8.4,
                ResumePath = "resume_priya.pdf",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        // 6. Seed Sample Recruiter
        var recruiterEmail = "hr@cloudtech.com";
        var recruiterUser = await userManager.FindByEmailAsync(recruiterEmail);
        var cloudTech = context.Companies.FirstOrDefault(c => c.CompanyName.Contains("CloudTech"));
        if (recruiterUser == null && cloudTech != null)
        {
            recruiterUser = new ApplicationUser
            {
                UserName = recruiterEmail,
                Email = recruiterEmail,
                FullName = "Ananya Sharma",
                UserRole = "Recruiter",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(recruiterUser, "Pass@123");
            await userManager.AddToRoleAsync(recruiterUser, "Recruiter");

            context.RecruiterProfiles.Add(new RecruiterProfile
            {
                UserId = recruiterUser.Id,
                CompanyId = cloudTech.Id,
                ContactPerson = "Ananya Sharma",
                Designation = "Senior Talent Acquisition Specialist",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        // 7. Seed Jobs
        if (!context.Jobs.Any())
        {
            var c1 = context.Companies.First();
            var c2 = context.Companies.Skip(1).First();
            var jobs = new[]
            {
                new Job
                {
                    CompanyId = c1.Id,
                    Title = "Junior .NET Software Engineer",
                    Description = "Build enterprise web applications using C#, ASP.NET Core, EF Core, and SQL Server.",
                    Requirements = "Proficiency in C#, ASP.NET Core MVC, SQL Server, and Git.",
                    Responsibilities = "Develop secure REST APIs, design database schemas, and participate in sprint reviews.",
                    JobType = "Full-Time",
                    WorkMode = "Hybrid",
                    Location = "Bangalore",
                    MinimumCGPA = 7.0,
                    Vacancies = 8,
                    SalaryMin = 650000,
                    SalaryMax = 900000,
                    Deadline = DateTime.UtcNow.AddDays(30),
                    IsActive = true
                },
                new Job
                {
                    CompanyId = c1.Id,
                    Title = "Associate Database Administrator",
                    Description = "Design normalized SQL schemas, optimize indexing, and enforce ACID guarantees.",
                    Requirements = "Experience with SQL Server Management Studio (SSMS), T-SQL, and relational normalization.",
                    Responsibilities = "Monitor query performance, maintain database integrity, and run health diagnostics.",
                    JobType = "Full-Time",
                    WorkMode = "On-Site",
                    Location = "Pune",
                    MinimumCGPA = 6.5,
                    Vacancies = 4,
                    SalaryMin = 550000,
                    SalaryMax = 750000,
                    Deadline = DateTime.UtcNow.AddDays(25),
                    IsActive = true
                },
                new Job
                {
                    CompanyId = c2.Id,
                    Title = "Full-Stack Web Developer",
                    Description = "Develop interactive web portals with responsive UI, asynchronous services, and secure Identity authentication.",
                    Requirements = "Strong knowledge of Bootstrap 5, JavaScript, C# controllers, and SQL databases.",
                    Responsibilities = "Build user-friendly dashboards, implement responsive designs, and integrate APIs.",
                    JobType = "Full-Time",
                    WorkMode = "Remote",
                    Location = "Hyderabad",
                    MinimumCGPA = 7.5,
                    Vacancies = 5,
                    SalaryMin = 700000,
                    SalaryMax = 1050000,
                    Deadline = DateTime.UtcNow.AddDays(20),
                    IsActive = true
                }
            };
            await context.Jobs.AddRangeAsync(jobs);
            await context.SaveChangesAsync();
        }

        // 8. Seed Sample Applications (Track which student applied to what)
        if (!context.Applications.Any() && student1 != null && student2 != null)
        {
            var job1 = context.Jobs.First();
            var job2 = context.Jobs.Skip(1).First();

            var app1 = new Application
            {
                JobId = job1.Id,
                StudentUserId = student1.Id,
                Status = "InterviewScheduled",
                ResumePath = "resume_raushan_v2.pdf",
                CoverLetter = "Excited to apply for the Junior .NET Engineer role. I have built full-stack portals with ASP.NET Core MVC and SQL Server.",
                AppliedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };

            var app2 = new Application
            {
                JobId = job1.Id,
                StudentUserId = student2.Id,
                Status = "Shortlisted",
                ResumePath = "resume_aarav.pdf",
                CoverLetter = "Hands-on experience in backend architectures, microservices, and database optimization.",
                AppliedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };

            var app3 = new Application
            {
                JobId = job2.Id,
                StudentUserId = student1.Id,
                Status = "UnderReview",
                ResumePath = "resume_raushan_v2.pdf",
                CoverLetter = "Strong interest in relational database engineering and database tuning.",
                AppliedAt = DateTime.UtcNow.AddDays(-1)
            };

            context.Applications.AddRange(app1, app2, app3);
            await context.SaveChangesAsync();

            // Status histories
            context.ApplicationStatusHistories.AddRange(
                new ApplicationStatusHistory
                {
                    ApplicationId = app1.Id,
                    PreviousStatus = "Shortlisted",
                    NewStatus = "InterviewScheduled",
                    ChangedByUserId = "admin@smarthire.local",
                    Remarks = "Round 1 cleared with high score. Technical interview scheduled."
                },
                new ApplicationStatusHistory
                {
                    ApplicationId = app2.Id,
                    PreviousStatus = "UnderReview",
                    NewStatus = "Shortlisted",
                    ChangedByUserId = "admin@smarthire.local",
                    Remarks = "Shortlisted for high CGPA and .NET project experience."
                }
            );

            // Seed Interview
            context.Interviews.Add(new Interview
            {
                ApplicationId = app1.Id,
                ScheduledAt = DateTime.UtcNow.AddDays(4),
                InterviewMode = "Virtual (Google Meet)",
                MeetingLink = "https://meet.google.com/abc-defg-hij",
                Location = "Google Meet Virtual Session",
                Notes = "Technical assessment on C# OOP, EF Core, and database indexing.",
                Status = "Scheduled"
            });

            await context.SaveChangesAsync();
        }
    }
}
