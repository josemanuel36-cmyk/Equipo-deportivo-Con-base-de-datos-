using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class Sports
    {
        public int SportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

        public Sports()
        {
        }

        public Sports(string name, string description, bool status)
        {
            Name = name;
            Description = description;
            Status = status;
        }
    }
}
