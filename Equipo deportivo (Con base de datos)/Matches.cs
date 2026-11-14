using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class Matches
    {
        public int MatchId { get; set; }
        public int SeasonId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public DateTime MatchDate { get; set; }
        public string Location { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
        public string Status { get; set; }

        public Matches()
        {
        }

        public Matches(int seasonId, int homeTeamId, int awayTeamId,
                       DateTime matchDate, string location,
                       int? homeGoals, int? awayGoals, string status)
        {
            SeasonId = seasonId;
            HomeTeamId = homeTeamId;
            AwayTeamId = awayTeamId;
            MatchDate = matchDate;
            Location = location;
            HomeGoals = homeGoals;
            AwayGoals = awayGoals;
            Status = status;
        }
    }
}
