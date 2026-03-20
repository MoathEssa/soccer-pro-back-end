using SoccerPro.Domain.Entities.Enums;
using SoccerPro.Domain.Enums;

namespace SoccerPro.Domain.Entities
{
    public class JoinTeamForFirstTimeRequest
    {
        public int RequestId { get; set; }

        public int UserId { get; set; }
        public int DepartmentId { get; set; }

        public int TeamId { get; set; }

        public string? Notes { get; set; }

        public PlayerPosition PlayerPosition { get; set; }

        public PlayerRole PlayerRole { get; set; }


        public PlayerType PlayerType { get; set; }
        public Request Request { get; set; } = new();
    }
}
