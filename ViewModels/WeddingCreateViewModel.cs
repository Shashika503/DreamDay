using DreamDay.Models;
using System.ComponentModel.DataAnnotations;

public class WeddingCreateViewModel
{
    [Required]
    public string Title { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? Date { get; set; }

    [Required]
    public string PlannerId { get; set; }

    [Required(ErrorMessage = "Please select at least one vendor.")]
    public List<int> SelectedVendorIds { get; set; } = new();

    public List<ApplicationUser>? AvailablePlanners { get; set; }
    public List<Vendor>? AvailableVendors { get; set; }
}

