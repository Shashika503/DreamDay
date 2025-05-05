using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DreamDay.Models;

namespace DreamDay.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Wedding> Weddings { get; set; }
    public DbSet<Guest> Guests { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<BudgetItem> BudgetItems { get; set; }
    public DbSet<TimelineEvent> TimelineEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    builder.Entity<Wedding>()
        .HasOne(w => w.Couple)
        .WithMany(u => u.WeddingsAsCouple)
        .HasForeignKey(w => w.CoupleId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<Wedding>()
        .HasOne(w => w.Planner)
        .WithMany(u => u.WeddingsAsPlanner)
        .HasForeignKey(w => w.PlannerId)
        .OnDelete(DeleteBehavior.Restrict);

    builder.Entity<Wedding>()
        .HasMany(w => w.Vendors)
        .WithMany(v => v.Weddings)
        .UsingEntity(j => j.ToTable("WeddingVendors"));
}



}
