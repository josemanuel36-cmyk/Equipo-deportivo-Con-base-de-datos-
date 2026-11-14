using System;
using System.Collections.Generic;
using System.Text;

namespace Equipo_deportivo__Con_base_de_datos_
{
    public class Players
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public string Nationality { get; set; }
        public string Status { get; set; }

        public Players()
        {
        }

        public Players(string name, string lastName, DateTime birthDate,
                       string phone, string email, string document,
                       string nationality, string status)
        {
            Name = name;
            LastName = lastName;
            BirthDate = birthDate;
            Phone = phone;
            Email = email;
            Document = document;
            Nationality = nationality;
            Status = status;
        }
    }
}
