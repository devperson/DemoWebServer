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
        public object Register([FromBody]Customer customer)
        {
            try
            {
                context.Customers.Add(customer);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                return new { Error = "Server error.", Success = false };
            }
            return new { Success = true };         
        }

        // GET api/values
        public IEnumerable<Menu> GetMenu()
        {
            var menus = context.Menus;
            foreach (var menu in menus)            
                menu.Image = this.ToAbsoluteUrl(string.Format("/Images/{0}", menu.Image));            

            return menus;
        }

        [HttpPost]
        public object Order([FromBody]int menuId, [FromBody]int customerId)
        {
            try
            {
                var order = new Order();
                order.CustomerId = customerId;
                order.MenuId = menuId;
                var customer = context.Customers.First(c=>c.Id==customerId);
                //TODO get driver which is most closest by location to customer.
                order.Driver = context.Drivers.ToList().MinBy(d => this.GetDistance(new Position(d.Latitude, d.Longitude), new Position(customer.Latitude, customer.Longitude)));
                context.Orders.Add(order);
                context.SaveChanges();

                //TODO Send Push Notification to driver about assigned order
            }
            catch(Exception ex)
            {
                return new { Error = "Server error.", Success = false };
            }

            return new { Success = true };            
        }      

        public string ToAbsoluteUrl(string relativeUrl)
        {
            var url = HttpContext.Current.Request.Url;
            var port = (url.AbsoluteUri.Contains("localhost") || url.AbsoluteUri.Contains("pc")) ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        public double GetDistance(Position From, Position To, char unit = 'K')
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
}