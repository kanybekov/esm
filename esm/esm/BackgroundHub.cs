using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace esm
{
    public class BackgroundHub : Hub
    {
        public void request(string user, string mes)
        {
            Clients.All.hello(user, mes);
        }

        public void getTask()
        {
            //Clients.All.broadcast();
        }

        public void getFunc()
        {
            //Clients.All.broadcast();
        }
    }
}