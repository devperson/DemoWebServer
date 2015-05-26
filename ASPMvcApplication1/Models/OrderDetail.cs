using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPMvcApplication1.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public int MenuId { get; set; }        
        public int Qty { get; set; }

        public Order Order { get; set; }
        public Menu Menu { get; set; }
    }
}
