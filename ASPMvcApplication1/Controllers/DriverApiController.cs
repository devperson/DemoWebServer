using ASPMvcApplication1.Models;
using HServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ASPMvcApplication1.Controllers
{
    public class DriverApiController : ApiController
    {
        DataBaseContext context = new DataBaseContext();

        // GET api/values
        [HttpPost]
        public object Register([FromBody]Driver driver)
        {
            try
            {
                context.Drivers.Add(driver);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new { Error = "Server error.", Success = false };
            }
            return new { Success = true };  
        }

        [HttpGet]
        public IEnumerable<Order> GetOrders(int driverId, int lastOrderId)
        {
            return context.Orders.Include("Menu").Include("Customer").Where(o => o.DriverId == driverId && !o.IsDelivered && o.Id > lastOrderId);
        }

        [HttpGet]
        public IEnumerable<DriverInventory> GetInventory(int driverId)
        {
            return context.DriverInventories.Include("Menu").Where(d => d.DriverId == driverId);
        }

        [HttpPut]
        public void CompleteOrder([FromBody]int orderId)
        {
            context.Orders.FirstOrDefault(o => o.Id == orderId).IsDelivered = true;
            context.SaveChanges();
        }
    }
}