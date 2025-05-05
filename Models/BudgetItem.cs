

namespace DreamDay.Models;


public class BudgetItem
{
    public int Id { get; set; }
    public string Category { get; set; }
    public decimal EstimatedCost { get; set; }
    public decimal ActualCost { get; set; }

    public int WeddingId { get; set; }
    public Wedding Wedding { get; set; }
}
