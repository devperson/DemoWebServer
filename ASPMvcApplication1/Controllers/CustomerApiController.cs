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
                if (user != null)
                    return new { Success = true, UserId = user.Id };
                else
                    return new { Error = "User name or password is incorrect." };
            }
            catch (Exception ex)
            {
                return new { Error = ex.Message };
            }
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
                    //return Request.CreateResponse(HttpStatusCode.OK, new { Success = true, UserId = customer.Id });                    
                }
                else
                {
                    return new { Error = "User name already taken." };
                }
            }
            catch(Exception ex)
            {
                return new { Error = ex.Message };
            }            
        }
        
        [HttpPost]
        public object UpdateUserLocation([FromBody]UserLocationModel location)
        {
            var customer = context.Customers.FirstOrDefault(c => c.Id == location.UserId);
            customer.Latitude = location.Position.Latitude;
            customer.Longitude = location.Position.Longitude;
            customer.Address = location.Address;
            context.SaveChanges();

            return new { Success = true };
        }

        
        [HttpGet]
        public object GetMenu()
        {
            try
            {
                var menus = context.Menus.ToList();
                foreach (var menu in menus)
                    menu.Image = this.ToAbsoluteUrl(string.Format("/Images/Products/{0}", menu.Image));

                return new { Menu = menus, Success = true };
            }
            catch (Exception ex)
            {
                return new { Error = "Server error. " + ex.Message};
            }
        }


        [HttpPost]
        public object Order([FromBody]OrderModel orderModel)
        {
            var order = new Order();
            try
            {

                var customer = context.Customers.First(c => c.Id == orderModel.CustomerId);
                order.Customer = customer;
                //TODO get driver which is most closest by location to customer.

                //.Where(d => d.IsApproved)
                order.Driver = context.Drivers.ToList().MinBy(d => this.GetDistance(new Position(d.CurrentLatitude, d.CurrentLongitude), new Position(customer.Latitude, customer.Longitude)));

                foreach (var meal in orderModel.Details)
                {
                    order.Details.Add(new OrderDetail { MenuId = meal.Id, Qty = meal.Quantity });   
                }                

                context.Orders.Add(order);
                context.SaveChanges();

                //TODO Send Push Notification to driver about assigned order
            }
            catch(Exception ex)
            {
                return new { Error = "Server error. " + ex.Message};
            }

            return new { Success = true, OrderId = order.Id, DriverId = order.Driver.Id, Latitude = order.Driver.CurrentLatitude, Longitude = order.Driver.CurrentLongitude };            
        }


        [HttpGet]
        public object GetOrders(int customerId)
        {
            try
            {
                var orders = context.Orders.Include("Details").Include("Driver").Where(c => c.CustomerId == customerId).ToList();

                var ordersModels = orders.Select(o => new
                {
                    Id = o.Id,
                    Date = o.Date,
                    IsDelivered = o.IsDelivered,
                    Meals = o.Details.Select(od => new
                            {
                                Id = od.MenuId,
                                Quantity = od.Qty
                            }).ToList(),
                    Driver = new
                    {
                        Id = o.DriverId,
                        Lat = o.Driver.CurrentLatitude,
                        Lon = o.Driver.CurrentLongitude
                    }
                }).ToList();

                return new { Orders = ordersModels, Success = true };
            }
            catch (Exception ex)
            {
                return new { Error = "Server error. " + ex.Message };
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
        public int CustomerId { get; set; }
        public List<OrderDetailModel> Details { get; set; }
        //public int Id { get; set; }
        //public DateTime Date { get; set; }
        //public List<OrderDetailModel> Meals { get; set; }
        //public bool IsDelivered { get; set; }
        //public DriverModel Driver { get; set; }
        
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

    public class UserLocationModel
    {
        public int UserId { get; set; }
        public Position Position { get; set; }
        public string Address { get; set; }
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