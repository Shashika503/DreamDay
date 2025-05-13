namespace DreamDay.Models;

public class ChecklistItem
{
    public int Id { get; set; }
    public string Task { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }

    public int WeddingId { get; set; }
    public Wedding? Wedding { get; set; }
}
