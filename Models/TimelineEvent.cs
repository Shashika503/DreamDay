namespace DreamDay.Models;
public class TimelineEvent
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime? Time { get; set; }
    public string Location { get; set; }

    public int WeddingId { get; set; }
    public Wedding? Wedding { get; set; }
}
