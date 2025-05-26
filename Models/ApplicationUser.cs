using Microsoft.AspNetCore.Identity;

namespace DreamDay.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }

    

    // Navigation
    public ICollection<Wedding>? WeddingsAsCouple { get; set; }
    public ICollection<Wedding>? WeddingsAsPlanner { get; set; }
}