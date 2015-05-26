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

        [HttpPost]
        public object Login([FromBody]Driver driver)
        {
            try
            {
                var user = context.Drivers.FirstOrDefault(d => d.UserName == driver.UserName && d.Password == driver.Password);
                if (user != null)
                {
                    return new { Success = true, DriverId = user.Id };
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, Success = false };
            }

            return new { Success = false };
        }

        // GET api/values
        [HttpPost]
        public object Register([FromBody]Driver driver)
        {
            try
            {
                context.Drivers.Add(driver);
                context.SaveChanges();

                return new { Success = true, DriverId = driver.Id };
            }
            catch (Exception ex)
            {
                return new { Error = "Server error.", Success = false };
            }            
        }

        [HttpGet]
        public object GetOrders(int driverId, int lastOrderId)
        {
            var orders = context.Orders.Include("Customer").Include("Details").Include("Menu").Where(o => o.DriverId == driverId && !o.IsDelivered && o.Id > lastOrderId);
            var ordersModel = orders.Select(o => new 
            { 
                Id = o.Id, 
                Date = o.Date, 
                IsDelivered = o.IsDelivered, 
                Meals = o.Details.Select(d => new 
                { 
                    Name = d.Menu.Name, 
                    Description = d.Menu.Description, 
                    Image = d.Menu.Image, 
                    Price = d.Menu.Price, 
                    Quantity = d.Qty 
                }).ToList(),
                User = new
                {
                    FirstName = o.Customer.FirstName,
                    LastName = o.Customer.LastName,
                    Phone = o.Customer.Phone,
                    Gender = o.Customer.Gender,
                    Address = new { 
                        AddressText = o.Customer.Address,
                        Position = new Position(o.Customer.Latitude, o.Customer.Longitude)
                    }
                }
            }).ToList();

            return ordersModel;
        }

        [HttpGet]
        public object GetInventory(int driverId)
        {
            var inventory = context.DriverInventories.Include("Menu").Where(d => d.DriverId == driverId).ToList();
            var invModels = inventory.Select(d => new
            {
                Name = d.Menu.Name,
                Description = d.Menu.Description,
                Image = d.Menu.Image,
                Price = d.Menu.Price,
                Quantity = d.Count
            }).ToList();

            return invModels;
        }

        [HttpPut]
        public void CompleteOrder([FromBody]int orderId)
        {
            context.Orders.FirstOrDefault(o => o.Id == orderId).IsDelivered = true;
            context.SaveChanges();
        }
    }
}