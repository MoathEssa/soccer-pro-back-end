using SoccerPro.Domain.Entities.Enums;

namespace SoccerPro.Application.DTOs.SharedDTOs;

public class FieldDTO
{
    public int FieldId { get; set; }
    public string Number { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int AudienceCapacity { get; set; }
    public FieldStatus Status { get; set; }
}