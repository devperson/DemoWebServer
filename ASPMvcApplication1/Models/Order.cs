using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPMvcApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int MenuId { get; set; }
        public int DriverId { get; set; }
        public int Quantity { get; set; }

        public bool IsDelivered { get; set; }

        public Customer Customer { get; set; }
        public Menu Menu { get; set; }
        public Driver Driver { get; set; }
    }
}
