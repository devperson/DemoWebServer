using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPMvcApplication1.Models
{
    public class Order
    {
        public Order()
        {
            this.Details = new List<OrderDetail>();
            this.Date = DateTime.Now;
        }
        public int Id { get; set; }
        public int CustomerId { get; set; }        
        public int DriverId { get; set; }        
        public bool IsDelivered { get; set; }
        public DateTime Date { get; set; }

        public List<OrderDetail> Details { get; set; }       
        public Customer Customer { get; set; }        
        public Driver Driver { get; set; }
    }
}
