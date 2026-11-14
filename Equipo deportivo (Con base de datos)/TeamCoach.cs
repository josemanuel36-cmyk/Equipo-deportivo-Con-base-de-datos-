using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class TeamCoach
    {
        public int AssignmentId { get; set; }
        public int TeamId { get; set; }
        public int CoachId { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public TeamCoach()
        {
        }

        public TeamCoach(int teamId, int coachId, string position,
                         DateTime startDate, DateTime? endDate)
        {
            TeamId = teamId;
            CoachId = coachId;
            Position = position;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
