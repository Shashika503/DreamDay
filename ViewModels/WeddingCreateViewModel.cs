using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DreamDay.Models;
using Microsoft.AspNetCore.Identity;

namespace DreamDay.ViewModels;

public class WeddingCreateViewModel
{
    [Required]

    public int? Id { get; set; }
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

