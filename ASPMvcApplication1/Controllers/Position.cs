using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASPMvcApplication1.Controllers
{
    public class Position
    {
        public Position(double lat,double lon)
        {
            this.Latitude = lat;
            this.Longitude = lon;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
