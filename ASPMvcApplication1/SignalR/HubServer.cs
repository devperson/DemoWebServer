using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Diagnostics;
using ASPMvcApplication1.Models;

namespace ASPMvcApplication1
{
    public class HubServer : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();       
        public void Notify_NewDriverLocation(MsgData data)
        {
            foreach (var to in data.To)
            {
                foreach (var connectionId in _connections.GetConnections(to))
                {
                    Clients.Client(connectionId).NewDriverLocation(data.Data);
                }
            }     
        }

        public void Notify_NewOrderPosted(MsgData data)
        {
            foreach (var to in data.To)
            {
                foreach (var connectionId in _connections.GetConnections(to))
                {
                    Clients.Client(connectionId).NewOrderPosted(data.Data);
                }
            }
        }

        public void Notify_OrderCompleted(MsgData data)
        {
            foreach (var to in data.To)
            {
                foreach (var connectionId in _connections.GetConnections(to))
                {
                    Clients.Client(connectionId).OrderCompleted(data.Data);
                }
            }
        }

        public override Task OnConnected()
        {
            string name = Context.QueryString["name"];
            Debug.WriteLine(string.Format("{0} Connected", name));
            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.QueryString["name"];
            Debug.WriteLine(string.Format("{0} Disconnected", name));
            if (!string.IsNullOrEmpty(name))
                _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }        
    }
}