using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class Seasons
    {
        public int SeasonId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public Seasons()
        {
        }

        public Seasons(string name, DateTime startDate,
                       DateTime endDate, string status)
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }
    }
}
