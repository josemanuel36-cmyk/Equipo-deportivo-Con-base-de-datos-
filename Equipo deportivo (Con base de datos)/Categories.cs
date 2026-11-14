using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class Categories
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }

        public Categories()
        {
        }

        public Categories(string name, int minimumAge, int maximumAge)
        {
            Name = name;
            MinimumAge = minimumAge;
            MaximumAge = maximumAge;
        }
    }
}
