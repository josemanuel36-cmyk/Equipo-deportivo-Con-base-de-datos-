using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class Positions
    {
        public int PositionId { get; set; }
        public string Name { get; set; }
        public int SportId { get; set; }

        public Positions()
        {
        }

        public Positions(string name, int sportId)
        {
            Name = name;
            SportId = sportId;
        }
    }
}
