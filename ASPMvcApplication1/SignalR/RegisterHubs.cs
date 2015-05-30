using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

//[assembly: PreApplicationStartMethod(typeof(ASPMvcApplication1.RegisterHubs), "Start")]

[assembly: OwinStartup(typeof(ASPMvcApplication1.RegisterHubs))]
namespace ASPMvcApplication1
{
    //public static class RegisterHubs
    //{
    //    public static void Start()
    //    {            
    //        RouteTable.Routes.MapHubs(new HubConfiguration());
    //    }
    //}

    public class RegisterHubs
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
