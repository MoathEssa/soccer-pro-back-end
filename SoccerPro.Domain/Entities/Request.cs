using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Domain.Entities;

public class Request
{
    public int RequestId { get; set; }
    public RequestType RequestType { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? ProcessedByUserId { get; set; }

    // Navigation properties
    public Player Player { get; set; } = null!;
    public Team Team { get; set; } = null!;
}