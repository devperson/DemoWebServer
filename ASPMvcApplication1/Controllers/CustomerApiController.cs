using ASPMvcApplication1.Models;
using HServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ASPMvcApplication1.Controllers
{
    public class CustomerApiController : ApiController
    {
        DataBaseContext context = new DataBaseContext();

        [HttpPost]
        public object Login([FromBody]Customer customer)
        {            
            try
            {
                var user = context.Customers.FirstOrDefault(c => c.UserName == customer.UserName && c.Password == customer.Password);
                if(user!=null)
                {
                    return new { Success = true, UserId = user.Id };
                }
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message, Success = false };
            }

            return new { Success = false };
        }

        [HttpPost]
        public object Register([FromBody]Customer customer)
        {
            try
            {
                if (context.Customers.All(c => c.UserName != customer.UserName))
                {
                    context.Customers.Add(customer);
                    context.SaveChanges();
                    return new { Success = true, UserId = customer.Id };
                }
                else
                {
                    return new { Success = true, Error = "User name already taken." };
                }
            }
            catch(Exception ex)
            {
                return new { Error = ex.Message, Success = false };
            }            
        }

        public object UpdateUserLocation([FromBody]int customerId, [FromBody]Position pos, [FromBody]string address)
        {
            var customer = context.Customers.FirstOrDefault(c => c.Id == customerId);
            customer.Latitude = pos.Latitude;
            customer.Longitude = pos.Longitude;
            customer.Address = address;
            context.SaveChanges();

            return new { Success = true };
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Menu> GetMenu()
        {
            var menus = context.Menus.ToList();
            foreach (var menu in menus)            
                menu.Image = this.ToAbsoluteUrl(string.Format("/Images/Products/{0}", menu.Image));            

            return menus;
        }

        [HttpPost]
        public object Order([FromBody]int customerId, List<OrderDetailModel> meals)
        {
            var order = new Order();
            try
            {

                var customer = context.Customers.First(c => c.Id == customerId);
                order.Customer = customer;
                //TODO get driver which is most closest by location to customer.
                order.Driver = context.Drivers.Where(d => d.IsApproved).ToList().MinBy(d => this.GetDistance(new Position(d.Latitude, d.Longitude), new Position(customer.Latitude, customer.Longitude)));

                foreach (var meal in meals)
                {
                    order.Details.Add(new OrderDetail { MenuId = meal.Id, Qty = meal.Quantity });   
                }                

                context.Orders.Add(order);
                context.SaveChanges();

                //TODO Send Push Notification to driver about assigned order
            }
            catch(Exception ex)
            {
                return new { Error = "Server error.", Success = false };
            }

            return new { Success = true, OrderId = order.Id, DriverId = order.Driver, DriverPosition = new Position(order.Driver.Latitude, order.Driver.Longitude) };            
        }


        [HttpGet]
        public object GetOrders(int customerId)
        {            
            try
            {
                var orders = context.Orders.Where(c => c.Id == customerId);

                var ordersModels = orders.Select(o => new OrderModel 
                { 
                    Id = o.Id, 
                    Date = o.Date,
                    IsDelivered = o.IsDelivered,
                    Meals = o.Details.Select(od => new OrderDetailModel 
                            { 
                                Id = od.MenuId,
                                Quantity = od.Qty 
                            }).ToList(),
                    Driver = new DriverModel
                    {
                        Id = o.DriverId,
                        Position = new Position(o.Driver.Latitude,o.Driver.Longitude)
                    }
                }).ToList();

                return ordersModels;
            }
            catch (Exception ex)
            {
                return new { Error = "Server error.", Success = false };
            }
        }    

        private string ToAbsoluteUrl(string relativeUrl)
        {
            var url = HttpContext.Current.Request.Url;
            var port = (url.AbsoluteUri.Contains("localhost") || url.AbsoluteUri.Contains("pc")) ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        private double GetDistance(Position From, Position To, char unit = 'K')
        {
            double rlat1 = Math.PI * From.Latitude / 180;
            double rlat2 = Math.PI * To.Latitude / 180;
            double theta = From.Longitude - To.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }

        
    }


    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<OrderDetailModel> Meals { get; set; }
        public bool IsDelivered { get; set; }
        public DriverModel Driver { get; set; }
        
    }
    public class OrderDetailModel
    {
        public int Id { get; set; } //MenuId
        public int Quantity { get; set; }
    }

    public class DriverModel
    {
        public int Id { get; set; }
        public Position Position { get; set; }
    }

    public class Position
    {
        public Position(double lat, double lon)
        {
            this.Latitude = lat;
            this.Longitude = lon;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}