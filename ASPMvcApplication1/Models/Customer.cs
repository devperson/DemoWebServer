using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPMvcApplication1.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        //address
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
