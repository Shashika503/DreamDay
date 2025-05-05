namespace DreamDay.Models;

public class Wedding
{
    public int Id { get; set; }

    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Venue { get; set; }

    public string CoupleId { get; set; }
    public ApplicationUser Couple { get; set; }

    public string? PlannerId { get; set; }
    public ApplicationUser Planner { get; set; }

    public ICollection<Vendor> Vendors { get; set; }

    public ICollection<ChecklistItem> ChecklistItems { get; set; }
    public ICollection<Guest> Guests { get; set; }
    public ICollection<BudgetItem> BudgetItems { get; set; }
    public ICollection<TimelineEvent> TimelineEvents { get; set; }
}


