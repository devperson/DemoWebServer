using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPMvcApplication1.Models
{
    public class DriverInventory
    {
        public DriverInventory()
        {            
        }
        public int Id { get; set; }
        public int DriverId { get; set; }
        public int MenuId { get; set; }        
        public int Count { get; set; }
        public DateTime? Date { get; set; }

        public Driver Driver { get; set; }
        public Menu Menu { get; set; }                
    }
}
