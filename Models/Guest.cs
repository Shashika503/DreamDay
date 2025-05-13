namespace DreamDay.Models;
public class Guest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string RSVPStatus { get; set; }

    public int WeddingId { get; set; }
    public Wedding? Wedding { get; set; }
}
