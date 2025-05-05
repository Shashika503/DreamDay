namespace DreamDay.Models;
public class Vendor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public decimal PriceRange { get; set; }

     // ✅ Add inverse navigation
    public ICollection<Wedding> Weddings { get; set; } = new List<Wedding>();
}
