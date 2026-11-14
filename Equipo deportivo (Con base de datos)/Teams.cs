using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class Teams
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public DateTime? FoundationDate { get; set; }
        public int SportId { get; set; }
        public int CategoryId { get; set; }
        public string Status { get; set; }

        public Teams()
        {
        }

        public Teams(string name, string city, DateTime? foundationDate,
                     int sportId, int categoryId, string status)
        {
            Name = name;
            City = city;
            FoundationDate = foundationDate;
            SportId = sportId;
            CategoryId = categoryId;
            Status = status;
        }
    }
}
