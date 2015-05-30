using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

[assembly: PreApplicationStartMethod(typeof(ASPMvcApplication1.RegisterHubs), "Start")]

namespace ASPMvcApplication1
{
    public static class RegisterHubs
    {
        public static void Start()
        {            
            RouteTable.Routes.MapHubs(new HubConfiguration());
        }
    }
}
