using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class Coaches
    {
        public int CoachId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }
        public int YearsOfExperience { get; set; }
        public string Status { get; set; }

        public Coaches()
        {
        }

        public Coaches(string name, string lastName, string phone,
                       string email, string specialty,
                       int yearsOfExperience, string status)
        {
            Name = name;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Specialty = specialty;
            YearsOfExperience = yearsOfExperience;
            Status = status;
        }
    }
}
