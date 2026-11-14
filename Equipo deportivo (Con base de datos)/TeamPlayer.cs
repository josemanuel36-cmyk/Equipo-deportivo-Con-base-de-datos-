using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class TeamPlayer
    {
        public int RegistrationId { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }
        public int PositionId { get; set; }
        public int? JerseyNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public TeamPlayer()
        {
        }

        public TeamPlayer(int teamId, int playerId, int positionId,
                          int? jerseyNumber, DateTime startDate,
                          DateTime? endDate)
        {
            TeamId = teamId;
            PlayerId = playerId;
            PositionId = positionId;
            JerseyNumber = jerseyNumber;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
