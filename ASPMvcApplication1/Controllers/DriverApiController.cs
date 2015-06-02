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
                else
                    return new { Error = "User name or password is incorrect." };
            }
            catch (Exception ex)
            {
                return new { Error = "Server error. " + ex.Message };
            }
            
        }

        // GET api/values
        [HttpPost]
        public object Register([FromBody]Driver driver)
        {
            try
            {
                var user = context.Drivers.FirstOrDefault(d => d.UserName == driver.UserName);
                if (user != null)
                    return new { Error = "User name '" + driver.UserName + "' already registered." };

                context.Drivers.Add(driver);
                context.SaveChanges();

                return new { Success = true, DriverId = driver.Id };
            }
            catch (Exception ex)
            {
                return new { Error = "Server error. " + ex.Message};
            }            
        }

        [HttpPost]
        public object UpdateDriverLocation([FromBody]UserLocationModel location)
        {
            var driver = context.Drivers.FirstOrDefault(c => c.Id == location.UserId);
            driver.CurrentLatitude = location.Position.Latitude;
            driver.CurrentLongitude = location.Position.Longitude;
            driver.CurrentAddress = location.Address;
            context.SaveChanges();

            return new { Success = true };
        }

        [HttpGet]
        public object GetOrders(int driverId, int lastOrderId)
        {
            try
            {
                var orders = context.Orders.Include("Customer").Include("Details").Include("Details.Menu").Where(o => o.DriverId == driverId && !o.IsDelivered && o.Id > lastOrderId).ToList();
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
                        Id = o.CustomerId,
                        FirstName = o.Customer.FirstName,
                        LastName = o.Customer.LastName,
                        Phone = o.Customer.Phone,
                        Gender = o.Customer.Gender,
                        Address = new
                        {
                            AddressText = o.Customer.Address,
                            Lat = o.Customer.Latitude,
                            Lon = o.Customer.Longitude                            
                        }
                    }
                }).ToList();

                return new { Orders = ordersModel, Success = true };
            }
            catch (Exception ex)
            {
                return new { Error = "Server error. " + ex.Message};
            }  
        }

        [HttpGet]
        public object GetInventory(int driverId)
        {
            try
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

                return new { Inventories = invModels, Success = true };
            }
            catch (Exception ex)
            {
                return new { Error = "Server error. " + ex.Message};
            }
        }



        [HttpPut]
        public object CompleteOrder([FromBody]int orderId)
        {
            context.Orders.FirstOrDefault(o => o.Id == orderId).IsDelivered = true;
            context.SaveChanges();

            return new { Success = true };
        }
    }
}